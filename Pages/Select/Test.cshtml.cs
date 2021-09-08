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
    public class TestModel : PageModel
    {
        private readonly MyContext _context;
        [TempData]
        public string Result { get; set; } = null;
        [TempData]
        public string Questions { get; set; } = null;
        [BindProperty]
        public string[] TestAnswers { get; set; }
        [BindProperty]
        public string QuestionsTemp { get; set; }
        public Lesson CurrentLesson { get; set; }
        public List<TestQuestion> TestQuestions { get; set; }


        public TestModel(MyContext context)
        {
            _context = context;
        }
        public async Task OnGetAsync(int Id)
        {    
            //Dotaz na databázi
            CurrentLesson = await _context.Lessons
                .Include(lesson => lesson.Records)
                .AsNoTracking()
                .FirstOrDefaultAsync(lesson => lesson.LessonId == Id);
            var records = CurrentLesson.Records;
            int count = records.Count;

            //Seznam otázek a odpovìdí
            List<string> questions = RecordMatcher.FindInRecords(records, true);
            List<string> answers = RecordMatcher.FindInRecords(records, false);

            //Vytvoøení testu
            TestQuestions = TestQuestion.CreateTest(questions, answers);
            //Pøíprava otázek 
            QuestionsTemp = RecordMatcher.JoinInTemp(questions.ToArray(), Id.ToString());

            TestAnswers = new string[count];
        }

        public IActionResult OnPost()
        {
            //Odeslání otázek a uživatelových odpovìdí do TempData
            TempData["Questions"] = QuestionsTemp;
            Result = RecordMatcher.JoinInTemp(TestAnswers, "Test");
            return new RedirectToPageResult("Result");
        }
    }
}
