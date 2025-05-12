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
        private List<QuizQuestions> questions;
        private int currentQuestionIndex = 0;
        private int numCorrect = 0;

        private bool isPerGameTimer = true;
        private int perQuestionTime = 0;

        private string questionNumText;
        private string overallPercentageText;

        private bool questionsLoaded;

        private CancellationTokenSource gameTimerCts;
        private CancellationTokenSource questionTimerCts;

        //This window is called if a timer is set
        public Window1(bool timerStatus, int timerLength, int quizChoice)
        {
            InitializeComponent();
            if (timerStatus)
            { 
                StartGameTimer(timerLength);
            }
            else
            {
                isPerGameTimer = false;
                perQuestionTime = timerLength;
            }
            LoadQuestions(quizChoice);
            DisplayCurrentQuestion();
        }

        //This window is called if no timer is set
        public Window1(int quizChoice)
        {
            InitializeComponent();
            LoadQuestions(quizChoice);
            DisplayCurrentQuestion();
        }

        private void LoadQuestions(int quizChoice)
        {
            try
            {
                string filePath = string.Empty;
                switch (quizChoice)
                {
                    case 1:
                        filePath = "Questions/MizzouQuestions.json";
                        GameTitle.Text = "Mizzou Trivia";
                        break;
                    case 2:
                        filePath = "Questions/GeographyQuestions.json";
                        GameTitle.Text = "Geography Trivia";
                        break;
                    case 3:
                        filePath = "Questions/ProgrammingQuestions.json";
                        GameTitle.Text = "Programming Trivia";
                        break;
                    case 4:
                        filePath = "Questions/MovieQuestions.json";
                        GameTitle.Text = "Movie Trivia";
                        break;
                    default:
                        break;
                }
                questions = QuizLoader.LoadQuestions(filePath);
                if (questions == null)
                {
                    MessageBox.Show("Unable to load questions. PLease check question file or Try Again!");
                }
                else
                {
                    Shuffle(questions);
                    foreach (var question in questions)
                    {
                        if (question.Answers == null)
                        {
                            MessageBox.Show("Unable to load Answers. PLease check Answers file or Try Again!");
                        }
                        Shuffle(question.Answers);
                    }
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

                QuestionNum.Text = $"{currentQuestionIndex + 1} / {questions.Count}";

                var currentQuestion = questions[currentQuestionIndex];
                QuestionText.Text = currentQuestion.Question;

                AnswerButton1.Content = currentQuestion.Answers[0].Text;
                AnswerButton1.Tag = currentQuestion.Answers[0].IsCorrect;

                AnswerButton2.Content = currentQuestion.Answers[1].Text;
                AnswerButton2.Tag = currentQuestion.Answers[1].IsCorrect;

                AnswerButton3.Content = currentQuestion.Answers[2].Text;
                AnswerButton3.Tag = currentQuestion.Answers[2].IsCorrect;

                AnswerButton4.Content = currentQuestion.Answers[3].Text;
                AnswerButton4.Tag = currentQuestion.Answers[3].IsCorrect;

                if (!isPerGameTimer)
                {
                    StartPerQuestionTimer(perQuestionTime);
                }
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
                questionTimerCts?.Cancel();

                // Disable all buttons
                EnableDisableButtons();

                QuestionText.Text = ((bool)button.Tag ? "Correct" : "Incorrect");

                ProgressGame((bool)button.Tag, true);

            }
        }
        private void EnableDisableButtons()
        {
            AnswerButton1.IsEnabled = !AnswerButton1.IsEnabled;
            AnswerButton2.IsEnabled = !AnswerButton2.IsEnabled;
            AnswerButton3.IsEnabled = !AnswerButton3.IsEnabled;
            AnswerButton4.IsEnabled = !AnswerButton4.IsEnabled;
        }

        private async void ProgressGame(bool choice, bool gameTimerStillGoing)
        {
            if (choice)
            {
                numCorrect++;
            }

            ScoreCounter.Text = $"{(numCorrect * 100) / (currentQuestionIndex + 1)}%";

            await Task.Delay(2000);

            currentQuestionIndex++;

            if (currentQuestionIndex < questions.Count && gameTimerStillGoing)
            {
                DisplayCurrentQuestion();
                EnableDisableButtons();
            }
            else if(currentQuestionIndex >= questions.Count || !gameTimerStillGoing)
            {
                questionNumText = $"{currentQuestionIndex} / {questions.Count}";
                overallPercentageText = $"{(numCorrect * 100) / questions.Count}%";

                Window3 window3 = new Window3(questionNumText, ScoreCounter.Text, overallPercentageText);
                window3.Show();
                this.Close();
            }
        }

        private async Task StartGameTimer(int timerLength)
        {

            //Aaron -

            //GPT gave me the idea of a CancellationTokenSource, allowing for the timer
            //to be cancelled during the game without causing exceptions.

            gameTimerCts = new CancellationTokenSource();
            var token = gameTimerCts.Token;

            for (int i = timerLength; i >= 0; i--)
            {
                ProgressTimer(i, timerLength);
                try
                {
                    await Task.Delay(1000, token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                if (i == 0)
                {
                    EnableDisableButtons();
                    ProgressGame(false, false);
                    return;
                }
            }
        }

        private async void StartPerQuestionTimer(int seconds)
        {
            questionTimerCts?.Cancel();
            questionTimerCts = new CancellationTokenSource();
            var token = questionTimerCts.Token;

            for (int i = seconds; i >= 0; i--)
            {
                ProgressTimer(i, seconds);
                try
                {
                    await Task.Delay(1000, token);
                }
                catch(TaskCanceledException)
                {
                    return;
                }
                if(i == 0)
                {
                    EnableDisableButtons();
                    ProgressGame(false, true);
                    return;
                }
            }
        }

        private void ProgressTimer(int timeLeft, int timerLength)
        {
            Timer.Text = $"Time: {timeLeft}";

            if (timeLeft <= timerLength * 0.1)
            {
                Timer.Foreground = Brushes.Red;
            }
            else if (timeLeft <= timerLength * 0.3)
            {
                Timer.Foreground = Brushes.Orange;
            }
            else
            {
                Timer.Foreground = Brushes.Green;
            }
        }
    }
}
