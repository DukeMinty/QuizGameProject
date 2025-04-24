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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private List<QuizQuestions> _questions;
        private int _currentQuestionIndex = 0;
        public Window1()
        {
            InitializeComponent();
            LoadQuestions();
            DisplayCurrentQuestion();
        }

        private void LoadQuestions()
        {
            _questions = QuizLoader.LoadQuestions("Questions/testQuestions.json");
        }

        private void DisplayCurrentQuestion()
        {
            QuestionNum.Text = $"{_currentQuestionIndex + 1} / {_questions.Count}";

            var currentQuestion = _questions[_currentQuestionIndex];
            QuestionText.Text = currentQuestion.Question;

            AnswerButton1.Content = currentQuestion.Answers[0].Text;
            AnswerButton1.Tag = currentQuestion.Answers[0].IsCorrect;

            AnswerButton2.Content = currentQuestion.Answers[1].Text;
            AnswerButton2.Tag = currentQuestion.Answers[1].IsCorrect;

            AnswerButton3.Content = currentQuestion.Answers[2].Text;
            AnswerButton3.Tag = currentQuestion.Answers[2].IsCorrect;

            AnswerButton4.Content = currentQuestion.Answers[3].Text;
            AnswerButton4.Tag = currentQuestion.Answers[3].IsCorrect;
        }
    }
}
