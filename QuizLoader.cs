using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Threading.Channels;

namespace QuizGameProject
{
    class QuizLoader
    {
        public static List<QuizQuestions> LoadQuestions(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("The specified file does not exist: " + path);
                    return null;
                }
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<QuizQuestions>>(json)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to read questions and answers file. Please check Path, " + ex.Message);
                return null;
                
            }
        }
    }
}
