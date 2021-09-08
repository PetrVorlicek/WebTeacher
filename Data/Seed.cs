using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTeacher.Models;
using WebTeacher.Data;

namespace WebTeacher.Data
{
    //Tato třída naplňuje prázdnou databázi
    public static class Seed
    {
        public static void SeedDb()
        {
            using (var context = new MyContext())
            {
                if (!context.Lessons.Any())
                {
                    try
                    {
                        
                        var lessons = new List<Lesson>()
                    {
                        new Lesson{Name = "Restaurace"},
                        new Lesson{Name = "Letiště"},
                        new Lesson{Name = "Město"}
                    };
                        var recordsRes = new List<Record>()
                    {
                        new Record{Question = "Food",
                                   Answer = "Jídlo",},
                        new Record{Question = "Drink",
                                   Answer = "Pití",},
                        new Record{Question = "Tip",
                                   Answer = "Zpropitné",},
                        new Record{Question = "Breakfast",
                                   Answer = "Snídaně",},
                        new Record{Question = "Dinner",
                                   Answer = "Večeře",}

                    };
                        foreach (var record in recordsRes)
                        {
                            lessons[0].Records.Add(record);
                        }

                        var recordsLet = new List<Record>()
                    {
                        new Record{Question = "Departure",
                                   Answer = "Odlet",},
                        new Record{Question = "Arrival",
                                   Answer = "Přílet",},
                        new Record{Question = "Luggage",
                                   Answer = "Zavazadlo",},
                        new Record{Question = "Airplane",
                                   Answer = "Letadlo",},
                        new Record{Question = "Travel",
                                   Answer = "Cestovat",}
                    };
                        foreach (var record in recordsLet)
                        {
                            lessons[1].Records.Add(record);
                        }

                        var recordsMes = new List<Record>()
                    {
                        new Record{Question = "Town square",
                                   Answer = "Náměstí",},
                        new Record{Question = "Accommodation",
                                   Answer = "Ubytování",},
                        new Record{Question = "Station",
                                   Answer = "Nádraží",},
                        new Record{Question = "Monument",
                                   Answer = "Památka",},
                        new Record{Question = "Hospital",
                                   Answer = "Nemocnice",}
                    };
                        foreach (var record in recordsMes)
                        {
                            lessons[2].Records.Add(record);
                        }

                        foreach (var lesson in lessons)
                        {
                            context.Add(lesson);
                        }
                        context.SaveChanges();

                    }
                    catch
                    {
                        Console.WriteLine("An error occured during seed!");
                    }
                }
            }
        }
    }
}
