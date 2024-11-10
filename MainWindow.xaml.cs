using System;
using System.Windows;
using System.Windows.Threading;

namespace KickboksTimerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int countdown = 60; // Zamanlayıcı başlangıç süresi (60 saniye)
        private bool isRunning = false;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (countdown > 0)
            {
                countdown--;
                TimerText.Text = countdown.ToString();

                // Son 10 saniye için renk değişikliği
                if (countdown <= 10)
                {
                    TimerText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                }
            }
            else
            {
                timer.Stop();
                TimerText.Text = "Süre Bitti!";
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                timer.Start();
                isRunning = true;
                StartButton.Content = "Devam Et";
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isRunning = false;
            countdown = 60;
            TimerText.Text = countdown.ToString();
            StartButton.Content = "Başlat";
        }
    }
}
