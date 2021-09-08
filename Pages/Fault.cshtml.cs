using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebTeacher.Pages.Select
{
    public class FaultModel : PageModel
    {
        public string Error { get; set; }
        public void OnGet(string error)
        {
            //Podle chyby je zobrazena rùzná chybová zpráva
            Error = error;
        }
    }
}
