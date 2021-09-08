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

        //Zjišuje, zdali se uivatel nedostal na stránku jinak ne zamıšlenım zpùsobem
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
                //Získání otázek a odpovìdí z TempData z pøedchozího formuláøe, a ID lekce a typu testu
                string[] questions = RecordMatcher.SplitFromTemp(Questions, out string idString);
                string[] answers = RecordMatcher.SplitFromTemp(Result, out string last);

                //Získání ID lekce z pøedchozího formuláøe
                ID = Int32.Parse(idString);

                //Dotaz na databázi
                var currentLesson = await _context.Lessons
                    .Include(lesson => lesson.Records)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(lesson => lesson.LessonId == ID);

                var records = currentLesson.Records;

                //Získání slovníku správnıch otázek-odpovìdí
                var rightQuestions = RecordMatcher.FindInRecords(records);
                var rightAnswers = RecordMatcher.FindInRecords(records, false);
                var Dict = RecordMatcher.MakeDict(rightQuestions, rightAnswers);

                //Odpovìdi a jejich formátování zobrazené na stránce
                CleanResult = new List<string>();
                ResultClass = new List<string>();

                //Hodnocení testu podle typu testu
                if(last == "Test")
                {
                    for (int i = 0; i < Dict.Count; i++)
                    {

                        if(Dict[questions[i]] == answers[i])
                        {
                            CleanResult.Add($"Odpovìï {answers[i]} na otázku: {questions[i]} je správnì.");
                            ResultClass.Add("text-success");
                        }
                        else
                        {
                            CleanResult.Add($"Odpovìï {answers[i]} na otázku: {questions[i]} je špatnì. Správná odpovìï je: {Dict[questions[i]]}.");
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
                            CleanResult.Add($"Odpovìï {answers[i]} na otázku: {questions[i]} je správnì.");
                            ResultClass.Add("text-success");
                        }
                        else if (levScore >= 0.65)
                        {
                            CleanResult.Add($"Odpovìï {myAns} na otázku: {questions[i]} byla akceptována. Správná odpovìï je: {rightAns}");
                            ResultClass.Add("text-warning");
                        }
                        else
                        {
                            CleanResult.Add($"Odpovìï {myAns} na otázku: {questions[i]} nebyla akceptována. Pøíliš se liší od správné odpovìdi: {rightAns}.");
                            ResultClass.Add("text-danger");
                        }
                    }
                }
                //Pokud jsou obì TempData pøítomna, aplikace pøedpokládá e se uivatel dostal na stránku chtìnım zpùsobem a zobrazí obsah
                InvalidAccess = false;
            }
            else
            {
                //Ukáe chybovou hlášku
                TempData.Clear();
                InvalidAccess = true;
            }           
        }
    }
}
