using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTeacher.Models;

namespace WebTeacher.Data
{
    //Třída, starající se o metody, které se používají na více stránkách
    public static class RecordMatcher
    {
        //Metoda vracející seznam otázek, resp. odpovědí z určených záznamů
        public static List<string> FindInRecords(List<Record> records, bool findQuestion = true)
        {
            List<string> returnList = new List<string>();
            if (findQuestion)
            {
                foreach (var record in records)
                {
                    returnList.Add(record.Question);
                }
            }
            else
            {
                foreach (var record in records)
                {
                    returnList.Add(record.Answer);
                }
            }
            return returnList;
        }

        //Metoda na sestavení stringu, ukládajícího se do TempData 
        public static string JoinInTemp(string[] array, string head)
        {
            string returner = "#" + head + "#";
            for (int i = 0; i < array.Length; i++)
            {
                returner += ("/" + array[i]);
            }
            return returner;
        }

        //Metoda, která rozloží string z TempData na seznam otázek/odpovědí a další informaci
        public static string[] SplitFromTemp(string temp, out string last)
        {
            last = temp.Split('#')[1].Trim('#');
            string splitter = temp.Split('#')[2].Trim('#');
            List<string> grossOut = splitter.Split('/').ToList<string>();
            grossOut.Remove("");
            string[] returner = grossOut.ToArray();
            return returner;
        }

        //Metoda, která rozloží string z TempData na seznam otázek/odpovědí
        public static string[] SplitFromTemp(string temp)
        {
            string splitter = temp.Split('#')[2].Trim('#');
            List<string> grossOut = splitter.Split('/').ToList<string>();
            grossOut.Remove("");
            string[] returner = grossOut.ToArray();
            return returner;
        }

        //Vytvoří slovník z otázek a odpovědí
        public static Dictionary<string, string> MakeDict(List<string> questions, List<string> answers)
        {
            var returner = new Dictionary<string, string>();

            //Kontrola vstupů
            if (!questions.Any())
            {
                returner["Error"] = "Empty";
            }
            else if (questions.Count != answers.Count)
            {
                returner["Error"] = "Not matching";
            }
            //Sestavení slovníku
            else
            {
                for (int i = 0; i < questions.Count; i++)
                {
                    returner[questions[i]] = answers[i];
                }
            }
            return returner;
        }

        //Metoda, která oboduje uživatelovu odpověď podle vzdálenosti od správné odpovědi
        //Nechtěl jsem, aby bylo třeba mít 100% shodný vstup, ale aby existovala určitá tolerance
        public static double GetLevenshteinScore(string rightAnswer, string testAnswer)
        {
            //Získání kratšího a delšího ze stringů
            int max = rightAnswer.Length >= testAnswer.Length ? rightAnswer.Length : testAnswer.Length;
            int min = rightAnswer.Length < testAnswer.Length ? rightAnswer.Length : testAnswer.Length;

            //Pokud je jeden z nich prázdný, nemůže se jednat o správnou odpověď
            if(min == 0)
            {
                return 0;
            }

            //Vzdálenost je (přinejmenším) rozdíl délek stringů
            double score = max - min;
            for(int i = 0; i < min; i++)
            {
                //Pokud není na stejném indexu stejné písmeno, jedná se o chybu
                if(rightAnswer[i] != testAnswer[i])
                {
                    score++;
                }
            }
            //Výpočet a vrácení
            return (max - score) / max;
        }
    }
}
