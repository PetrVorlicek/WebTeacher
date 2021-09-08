using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTeacher.Data;
using WebTeacher.Models;

namespace WebTeacher.Pages
{
    public class AddModel : PageModel
    {
        private readonly MyContext _context;
        public bool IsSelected { get; set; }
        public int Count { get; set; }
        [BindProperty]
        public List<Record> Records { get; set; }
        [BindProperty]
        public string LessonName { get; set; }
        public AddModel(MyContext context)
        {
            _context = context;
        }
        public void OnGet(string count)
        {
            //Vybere zobrazovaný obsah
            IsSelected = true;
            switch (count)
            {
                //zobrazí okénka pro záznamy
                case "5":
                    Count = 5;
                    break;
                case "10":
                    Count = 10;
                    break;
                case "15":
                    Count = 15;
                    break;
                case "20":
                    Count = 20;
                    break;
                //zobrazí tlaèítka pro výbìr poètu záznamù
                default:
                    IsSelected = false;
                    break;
            }
        }

        public IActionResult OnPost()
        {
            //Kontrola uživatelských vstupù
            bool recordFailure = false;
            bool nameFailure = false;
            if(string.IsNullOrEmpty(LessonName))
            {
                nameFailure = true;
            }
            foreach(var record in Records)
            {
                if (string.IsNullOrEmpty(record.Question) || string.IsNullOrEmpty(record.Answer))
                {
                    recordFailure = true;
                }
            }

            //Pokud uživatel nezadal platný vstup, je pøesmìrován na stránku chyby
            if(nameFailure)
            {
                return RedirectToPage("Fault",new {error = "Name"});
            }
            else if(recordFailure)
            {
                return RedirectToPage("Fault", new { error = "Records" });
            }
            //Jinak je jeho lekce uložena
            else
            {
                Lesson newLesson = new Lesson() { Name = LessonName };
                newLesson.Records.AddRange(Records);
                _context.Add(newLesson);
                _context.SaveChanges();
                return RedirectToPage("Select/Index");
            }
        }
    }
}
