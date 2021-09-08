using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTeacher.Data;
using WebTeacher.Models;


namespace WebTeacher.Pages
{
    public class SelectModel : PageModel
    {
        private readonly MyContext _context;
        public List<Lesson> Lessons { get; set; }
        public SelectModel(MyContext context)
        {
            _context = context;
        }
        public async Task OnGetAsync()
        {
            //Dotaz na databázi
            Lessons = await _context.Lessons.ToListAsync();
            TempData.Clear();
        }
    }
}
