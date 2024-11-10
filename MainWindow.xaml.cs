using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace KickboksTimerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int countdown;
        private bool isRunning = false;
        private int roundCount;
        private int currentRound = 1;
        private bool isBreak = false;
        private int roundDuration;
        private int breakDuration;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Prompt user for number of rounds, round duration, and break duration
            roundCount = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Kaç round?", "Round Sayısı", "3"));
            roundDuration = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Round süresi (saniye)?", "Round Süresi", "60"));
            breakDuration = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Dinlenme süresi (saniye)?", "Dinlenme Süresi", "10"));
            countdown = roundDuration;
            RoundText.Text = $"Round: {currentRound}/{roundCount}";
            TimerText.Text = countdown.ToString();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (countdown > 0)
            {
                countdown--;
                TimerText.Text = countdown.ToString();
                UpdateProgressBar();

                // Son 10 saniye için renk değişikliği
                if (countdown <= 10)
                {
                    CircularProgress.Stroke = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    CircularProgress.Stroke = new SolidColorBrush(Colors.Green);
                }
            }
            else
            {
                timer.Stop();
                if (isBreak)
                {
                    currentRound++;
                    if (currentRound > roundCount)
                    {
                        TimerText.Text = "Tüm Roundlar Tamamlandı!";
                        StartButton.Content = "Başlat";
                        StartButton.IsEnabled = true;
                        StopButton.IsEnabled = false;
                        isRunning = false;

                        // Ana grid'i gizle, CompletionGrid'i göster
                        MainGrid.Visibility = Visibility.Collapsed;
                        CompletionGrid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        countdown = roundDuration;
                        TimerText.Text = countdown.ToString();
                        RoundText.Text = $"Round: {currentRound}/{roundCount}";
                        isBreak = false;
                        timer.Start();
                    }
                }
                else
                {
                    TimerText.Text = "Ara!";
                    countdown = breakDuration;
                    isBreak = true;
                    timer.Start();
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                timer.Start();
                isRunning = true;
                StartButton.Content = "Devam Et";
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                timer.Stop();
                isRunning = false;
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                StartButton.Content = "Devam Et";
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isRunning = false;
            currentRound = 1;
            isBreak = false;
            countdown = roundDuration;
            TimerText.Text = countdown.ToString();
            RoundText.Text = $"Round: {currentRound}/{roundCount}";
            StartButton.Content = "Başlat";
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void UpdateProgressBar()
        {
            double progress = (double)countdown / (isBreak ? breakDuration : roundDuration);
            double angle = 360 * progress;

            double radians = (angle - 90) * Math.PI / 180;

            double x = 100 + 100 * Math.Cos(radians);
            double y = 100 + 100 * Math.Sin(radians);

            ArcSegment arcSegment = (ArcSegment)((PathGeometry)CircularProgress.Data).Figures[0].Segments[0];
            arcSegment.Point = new Point(x, y);
            arcSegment.IsLargeArc = progress >= 0.5;
        }

        private void NewRoundButton_Click(object sender, RoutedEventArgs e)
        {
            // CompletionGrid'i gizle, ana grid'i göster
            CompletionGrid.Visibility = Visibility.Collapsed;
            MainGrid.Visibility = Visibility.Visible;

            // Kullanıcıdan yeniden değerleri al
            roundCount = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Kaç round?", "Round Sayısı", "3"));
            roundDuration = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Round süresi (saniye)?", "Round Süresi", "60"));
            breakDuration = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Dinlenme süresi (saniye)?", "Dinlenme Süresi", "10"));
            countdown = roundDuration;
            currentRound = 1;
            isBreak = false;
            TimerText.Text = countdown.ToString();
            RoundText.Text = $"Round: {currentRound}/{roundCount}";
            StartButton.Content = "Başlat";
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }
    }
}
