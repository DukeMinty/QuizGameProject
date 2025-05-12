using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using static System.Formats.Asn1.AsnWriter;

namespace QuizGameProject
{
    class HighScore
    {
        public int highScore {get; set;}
        public static HighScore LoadHighScore(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("The specified file does not exist: " + path);
                    return null;
                }
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<HighScore>(json)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to high score file. Please check Path, " + ex.Message);
                return null;

            }
        }
        public static void SaveHighScore(int numCorrect, string path)
        {
            try
            {
                HighScore currentHighScore = LoadHighScore(path);

                if (numCorrect > currentHighScore.highScore)
                {
                    currentHighScore.highScore = numCorrect;
                    string json = JsonSerializer.Serialize(currentHighScore);
                    File.WriteAllText(path, json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save high score, " + ex.Message);
            }
        }

  
    }
  
}
