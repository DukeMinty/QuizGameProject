using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private int numCorrect = 0;
        private int percentageCorrect = 0;

        private int _gameTimeSeconds = 60;
        private CancellationTokenSource _gameTimerCts;

        //This window is called if a timer is set
        public Window1(bool timerOn, bool timerStatus, int timerLength)
        {
            InitializeComponent();
            StartGameTimer();
            LoadQuestions();
            DisplayCurrentQuestion();
        }

        //This window is called if no timer is set
        public Window1()
        {
            InitializeComponent();
            LoadQuestions();
            DisplayCurrentQuestion();
        }

        private void LoadQuestions()
        {
            try
            {
                _questions = QuizLoader.LoadQuestions("Questions/testQuestions.json");
                if (_questions == null)
                {
                    MessageBox.Show("Unable to load questions. PLease check question file or Try Again!");
                    return;
                }

                Shuffle(_questions);
                foreach (var question in _questions)
                {
                    if (question.Answers == null)
                    {
                        MessageBox.Show("Unable to load Answers. PLease check Answers file or Try Again!");
                        return;
                    }
                    Shuffle(question.Answers);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Close();
            }
        }

        private void DisplayCurrentQuestion()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Unable to show Questions and Answers. Please Try again");
            }
        }
        public static void Shuffle<t>(List<t> list)
        {
            if (list == null)
            {
                MessageBox.Show("Something went wrong");
                return;
            }
            try
            {
                Random rand = new Random();
                int i = list.Count;
                while (i > 0)
                {
                    i--;
                    int n = rand.Next(i + 1);
                    t value = list[n];
                    list[n] = list[i];
                    list[i] = value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Shuffle not working: " + ex.Message);
            }
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Disable all buttons
                EnableDisableButtons();

                QuestionText.Text = ((bool)button.Tag ? "Correct" : "Incorrect");

                ProgressGame((bool)button.Tag);

            }
        }
        private void EnableDisableButtons()
        {
            AnswerButton1.IsEnabled = !AnswerButton1.IsEnabled;
            AnswerButton2.IsEnabled = !AnswerButton2.IsEnabled;
            AnswerButton3.IsEnabled = !AnswerButton3.IsEnabled;
            AnswerButton4.IsEnabled = !AnswerButton4.IsEnabled;
        }

        private async void ProgressGame(bool choice)
        {
            if (choice)
            {
                numCorrect++;
            }

            ScoreCounter.Text = $"{(numCorrect * 100) / (_currentQuestionIndex + 1)}%";

            await Task.Delay(2000);

            _currentQuestionIndex++;

            if (_currentQuestionIndex < _questions.Count)
            {
                DisplayCurrentQuestion();
                EnableDisableButtons();
                //EndGame();
            }
        }

        private async Task StartGameTimer()
        {

            //Aaron -

            //GPT gave me the idea of a CancellationTokenSource, allowing for the timer
            //to be cancelled during the game without causing exceptions.

            _gameTimerCts = new CancellationTokenSource();
            var token = _gameTimerCts.Token;

            for (int i = _gameTimeSeconds; i >= 0; i--)
            {
                Timer.Text = $"{i}";

                try
                {
                    await Task.Delay(1000, token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }

        //private void EndGame()
        //{
        //    _gameTimerCts?.Cancel();
        //    AnswerButton1.IsEnabled = false;
        //    AnswerButton2.IsEnabled = false;
        //    AnswerButton3.IsEnabled = false;
        //    AnswerButton4.IsEnabled = false;

        //    QuestionText.Text = $"Final Score: {numCorrect} / {_questions.Count} ({percentageCorrect}%)";
        //}
    }
}
