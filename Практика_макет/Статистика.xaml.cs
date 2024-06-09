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
            AddData();
        }
        private void Открыть_список_заявок(object sender, RoutedEventArgs e)
        {
            Список_заявок список_заявок = new Список_заявок();
            список_заявок.Show();
            this.Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        private void AddData()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                CountRequests.Text = dbContext.RequestsDetailsStatus.Where(rds => rds.RequestStatusID == 2).Count().ToString();

                var timeslist = dbContext.RequestsDetails
                    .Where(rd => rd.CompletionDate != null)
                    .ToList();

                // Вычисление среднего времени
                if (timeslist.Any())
                {
                    var averageTime = timeslist
                        .Select(rd => (rd.CompletionDate.Value - rd.StartDate).TotalDays)
                        .Average();

                    AverageTime.Text = averageTime.ToString();
                }
                else
                {
                    AverageTime.Text = "Нет данных для вычисления среднего времени.";
                    
                }

                RepairParts.Items.Clear();

                foreach (var rp in dbContext.RequestsDetails.Where(rd => rd.RepairParts != "" && rd.RepairParts != null).ToList())
                    RepairParts.Items.Add(rp.RepairParts);
            }
        }
    }
}
