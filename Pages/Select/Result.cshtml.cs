using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTeacher.Data;
using WebTeacher.Models;

namespace WebTeacher.Pages.Select
{
    public class ResultModel : PageModel
    {
        [TempData]
        public string Result { get; set; }
        [TempData]
        public string Questions { get; set; }

        //Zji��uje, zdali se u�ivatel nedostal na str�nku jinak ne� zam��len�m zp�sobem
        public bool InvalidAccess { get; set; }

        public List<string> CleanResult { get; set; }
        public List<string> ResultClass { get; set; }

        public int ID { get; set; }

        private readonly MyContext _context;
        public ResultModel(MyContext context)
        {
            _context = context;
        }
        public async Task OnGetAsync()
        {
            //Test hodnot v TempData
            var checkQuestions = !string.IsNullOrEmpty(Result);
            var checkResult = !string.IsNullOrEmpty(Questions);

            if (checkResult && checkQuestions)
            {
                //Z�sk�n� ot�zek a odpov�d� z TempData z p�edchoz�ho formul��e, a ID lekce a typu testu
                string[] questions = RecordMatcher.SplitFromTemp(Questions, out string idString);
                string[] answers = RecordMatcher.SplitFromTemp(Result, out string last);

                //Z�sk�n� ID lekce z p�edchoz�ho formul��e
                ID = Int32.Parse(idString);

                //Dotaz na datab�zi
                var currentLesson = await _context.Lessons
                    .Include(lesson => lesson.Records)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(lesson => lesson.LessonId == ID);

                var records = currentLesson.Records;

                //Z�sk�n� slovn�ku spr�vn�ch ot�zek-odpov�d�
                var rightQuestions = RecordMatcher.FindInRecords(records);
                var rightAnswers = RecordMatcher.FindInRecords(records, false);
                var Dict = RecordMatcher.MakeDict(rightQuestions, rightAnswers);

                //Odpov�di a jejich form�tov�n� zobrazen� na str�nce
                CleanResult = new List<string>();
                ResultClass = new List<string>();

                //Hodnocen� testu podle typu testu
                if(last == "Test")
                {
                    for (int i = 0; i < Dict.Count; i++)
                    {

                        if(Dict[questions[i]] == answers[i])
                        {
                            CleanResult.Add($"Odpov�� {answers[i]} na ot�zku: {questions[i]} je spr�vn�.");
                            ResultClass.Add("text-success");
                        }
                        else
                        {
                            CleanResult.Add($"Odpov�� {answers[i]} na ot�zku: {questions[i]} je �patn�. Spr�vn� odpov�� je: {Dict[questions[i]]}.");
                            ResultClass.Add("text-danger");
                        }
                    }
                }
                else if(last == "View")
                {
                    for (int i = 0; i < Dict.Count; i++)
                    {
                        string rightAns = Dict[questions[i]];
                        string myAns = answers[i];
                        double levScore = RecordMatcher.GetLevenshteinScore(rightAns, myAns);

                        if (levScore == 1)
                        {
                            CleanResult.Add($"Odpov�� {answers[i]} na ot�zku: {questions[i]} je spr�vn�.");
                            ResultClass.Add("text-success");
                        }
                        else if (levScore >= 0.65)
                        {
                            CleanResult.Add($"Odpov�� {myAns} na ot�zku: {questions[i]} byla akceptov�na. Spr�vn� odpov�� je: {rightAns}");
                            ResultClass.Add("text-warning");
                        }
                        else
                        {
                            CleanResult.Add($"Odpov�� {myAns} na ot�zku: {questions[i]} nebyla akceptov�na. P��li� se li�� od spr�vn� odpov�di: {rightAns}.");
                            ResultClass.Add("text-danger");
                        }
                    }
                }
                //Pokud jsou ob� TempData p��tomna, aplikace p�edpokl�d� �e se u�ivatel dostal na str�nku cht�n�m zp�sobem a zobraz� obsah
                InvalidAccess = false;
            }
            else
            {
                //Uk�e chybovou hl�ku
                TempData.Clear();
                InvalidAccess = true;
            }           
        }
    }
}
