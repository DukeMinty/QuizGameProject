using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace QuizGameProject
{
    class QuizLoader
    {
        public static List<QuizQuestions> LoadQuestions(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<QuizQuestions>>(json)!;
        }
    }
}
