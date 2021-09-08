using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTeacher.Data
{
    //Třída testové otázky
    public class TestQuestion
    {
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public TestQuestion(string question, List<string> answers)
        {
            Question = question;
            Answers = answers;
        }


        //Vytváří testové otázky ze seznamu otázek a odpovědí
        public static List<TestQuestion> CreateTest(List<string> questions, List<string> answers)
        {
            var test = new List<TestQuestion>();
            Random r = new Random();
            //Metoda nemíchá pořadí otázek
            if (questions.Any())
            {
                
                for (int i = 0; i < questions.Count; i++)
                {
                    string question = questions[i];
                    string rightAnswer = answers[i];

                    //Metoda odebírá náhodně ze seznamu špatných odpovědí, dokud není požadovaný počet špatných odpovědí
                    List<string> testAnswers = new List<string>(answers);
                    testAnswers.RemoveAt(i);
                    while (testAnswers.Count > 3)
                    {
                        testAnswers.RemoveAt(r.Next(testAnswers.Count));
                    }

                    //Metoda vrátí správnou odpověď na náhodné místo do seznamu odpovědí na otázku
                    int rightIndex = r.Next(4);
                    testAnswers.Insert(rightIndex, rightAnswer);

                    test.Add(new TestQuestion(question, testAnswers));
                }
            }
            return test;
        }
    }
}
