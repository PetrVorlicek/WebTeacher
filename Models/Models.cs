using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

//Modely, ukládané do databáze
namespace WebTeacher.Models
{
    //Třída záznamu odpověď-otázka
    public class Record
    {
        public int RecordId { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }

    //Třída lekce - název lekce a seznam záznamů
    public class Lesson
    {
        public int LessonId { get; set; }
        public string Name { get; set; }
        public List<Record> Records { get; } = new List<Record>();
    }
}
