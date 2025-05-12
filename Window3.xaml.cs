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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3(string questionsAnswered, string percentCorrect, string overallCorrect)
        {
            InitializeComponent();

            QuestionsAnsweredText.Text = $"Questions answered: {questionsAnswered}";
            PercentageCorrectText.Text = $"Percent correct: {percentCorrect}";
            OverallPercentageCorrectText.Text = $"Overall correct: {overallCorrect}";
        }

        private void Main_Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
            this.Close();
        }
    }
}
