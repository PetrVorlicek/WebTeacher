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
    public class ViewModel : PageModel
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
        private string[] TestQuestions { get; set; }

        public ViewModel(MyContext context)
        {
            _context = context;
        }
        public async Task OnGetAsync(int Id)
        {
            //Dotaz na datab�zi
            var currentLesson = await _context.Lessons
                .Include(lesson => lesson.Records)
                .AsNoTracking()
                .FirstOrDefaultAsync(lesson => lesson.LessonId == Id);
            TempData.Clear();

            var records = currentLesson.Records;
            int count = records.Count;

            //P��prava ot�zek
            TestQuestions = RecordMatcher.FindInRecords(records, true).ToArray();
            //P��prava stringu ot�zek do TempData
            QuestionsTemp = RecordMatcher.JoinInTemp(TestQuestions, Id.ToString());
            TestAnswers = new string[count];

            ViewData["Questions"] = TestQuestions;
        }

        public IActionResult OnPost()
        {
            //Posl�n� ot�zek a odpov�d� do TempData
            Questions = QuestionsTemp;
            Result = RecordMatcher.JoinInTemp(TestAnswers, "View");
            return new RedirectToPageResult("Result");
        }

    }
}
