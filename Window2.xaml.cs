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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            TimerOnOffButton.Tag = true;
            TimerStatusButton.Tag = true;
        }

        private void Quit_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)TimerOnOffButton.Tag)
                {
                    if (!int.TryParse(TimerTextBox.Text, out int timerSeconds))
                    {
                        MessageBox.Show("Please enter a valid number for the timer.");
                        return;
                    }
                    if (timerSeconds <= 4)
                    {
                        MessageBox.Show("Please ensure your timer is set to at least 5 seconds.");
                        return;
                    }
                    if(timerSeconds > 999)
                    {
                        MessageBox.Show("Timer value is too large.");
                        return;
                    }
                    Window4 window4 = new Window4((bool)TimerStatusButton.Tag, timerSeconds);
                    window4.Show();
                    this.Close();
                }
                else
                {
                    Window4 window4 = new Window4();
                    window4.Show();
                    this.Close();
                    //Window1 window1 = new Window1();
                    //window1.Show();
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to go to Quiz Game" + ex.Message);
            }
        }

        private void TimerOnOff_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)TimerOnOffButton.Tag)
            {
                TimerOnOffButton.Content = "Timer: Off";
                TimerOnOffButton.Tag = false;
                TimerStatusButton.IsEnabled = false;
                TimerTextBox.IsEnabled = false;
            }
            else
            {
                TimerOnOffButton.Content = "Timer: On";
                TimerOnOffButton.Tag = true;
                TimerStatusButton.IsEnabled = true;
                TimerTextBox.IsEnabled = true;
            }
        }

        private void TimerStatus_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)TimerStatusButton.Tag)
            {
                TimerStatusButton.Content = "Per Question";
                TimerStatusButton.Tag = false;
            }
            else
            {
                TimerStatusButton.Content = "Per Game";
                TimerStatusButton.Tag = true;
            }
        }
    }
}
