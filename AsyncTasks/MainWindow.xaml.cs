using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncTasks
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Стартиране на асинхронното броене
        private async void btnStartCount_Click(object sender, RoutedEventArgs e)
        {
            btnStartCount.IsEnabled = false; // Деактивиране на бутона за стартиране
            btnCancel.IsEnabled = true;     // Активиране на бутона за отмяна
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await CounterAsync(_cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                txtCountResult.Text = "Task canceled.";
            }
            catch (Exception ex)
            {
                txtCountResult.Text = $"Error: {ex.Message}";
            }
            finally
            {
                btnStartCount.IsEnabled = true; // Реактивиране на бутона за стартиране
                btnCancel.IsEnabled = false;    // Деактивиране на бутона за отмяна
            }
        }

        // Асинхронно броене
        private async Task CounterAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i <= 100; i++)
            {
                cancellationToken.ThrowIfCancellationRequested(); // Проверка за отмяна

                txtCountResult.Text = i.ToString(); // Ъпдейт на UI
                await Task.Delay(100, cancellationToken); // Асинхронна пауза
            }
        }

        // Отмяна на задачата
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel(); // Изпращане на сигнал за отмяна
        }
    }
}