using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTeacher.Models;

namespace WebTeacher.Data
{
    //Třída kontextu, která se stará o komunikaci s databází
    public class MyContext : DbContext
    {
        public DbSet<Record> Records { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source = database.db");
    }
}
