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
    public class InfoModel : PageModel
    {
        private readonly MyContext _context;
        public Lesson CurrentLesson { get; set; }

        public InfoModel(MyContext context) 
        {
            _context = context;
        }
        public async Task OnGetAsync(int Id)
        {
            CurrentLesson = await _context.Lessons
                .Include(lesson => lesson.Records)
                .AsNoTracking()
                .FirstOrDefaultAsync(lesson => lesson.LessonId == Id);
        }
    }
}
