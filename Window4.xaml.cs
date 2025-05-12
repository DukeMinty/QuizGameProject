using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuizGameProject
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {

        private bool timerExists;
        private int buttonChoice;

        private bool timerStatus;
        private int timerLength;
        public Window4()
        {
            InitializeComponent();
        }

        public Window4(bool timerStatus, int timerLength)
        {
            InitializeComponent();

            timerExists = true;

            this.timerStatus = timerStatus;
            this.timerLength = timerLength;
            
            HighScore mizzouHighScore = HighScore.LoadHighScore("HighScore/MizzouHighscore.json");
            HighScore geoHighScore = HighScore.LoadHighScore("HighScore/GeographyHighscore.json");
            HighScore programmingHighScore = HighScore.LoadHighScore("HighScore/ProgrammingHighscore.json");
            HighScore movieHighScore = HighScore.LoadHighScore("HighScore/MovieHighscore.json");

            if(mizzouHighScore.highScore == 20)
            {
                MizzouButton.Content = "⭐ Mizzou ⭐";
                MizzouButton.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            }
            if (geoHighScore.highScore == 20)
            {
                GeoButton.Content = "⭐ US Geography ⭐";
                GeoButton.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            }
            if (programmingHighScore.highScore == 20)
            {
                ProgrammingButton.Content = "⭐ C# Programming ⭐";
                ProgrammingButton.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            }
            if (movieHighScore.highScore == 20)
            {
                MovieButton.Content = "⭐ Movies ⭐";
                MovieButton.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Name == "MizzouButton")
                {
                    buttonChoice = 1;
                }
                else if (button.Name == "GeoButton")
                {
                    buttonChoice = 2;
                }
                else if (button.Name == "ProgrammingButton")
                {
                    buttonChoice = 3;
                }
                else if (button.Name == "MovieButton")
                {
                    buttonChoice = 4;
                }
            }
            if (timerExists)
            {
                Window1 window1 = new Window1(timerStatus, timerLength, buttonChoice);
                window1.Show();
                this.Close();
            }
            else
            {
                Window1 window1 = new Window1(buttonChoice);
                window1.Show();
                this.Close();
            }
        }
    }
}
