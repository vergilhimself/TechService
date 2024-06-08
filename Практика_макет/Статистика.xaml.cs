using System.Windows;

namespace Практика_макет
{
    /// <summary>
    /// Логика взаимодействия для Статистика.xaml
    /// </summary>
    public partial class Статистика : Window
    {
        public Статистика()
        {
            InitializeComponent();
        }
        private void Открыть_список_заявок(object sender, RoutedEventArgs e)
        {
            Список_заявок список_заявок = new Список_заявок();
            список_заявок.Show();
            this.Close();
        }

    }
}
