using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace Практика_макет
{
    /// <summary>
    /// Логика взаимодействия для Список_заявок.xaml
    /// </summary>
    public partial class Список_заявок : Window
    {
        public ObservableCollection<RequestDetail> Requests { get; set; } = new ObservableCollection<RequestDetail>();

        public Список_заявок()
        {
            InitializeComponent();
            DataContext = this; // Устанавливаем текущий объект как контекст данных
            LoadRequests();

        }

        private void Открыть_статистику(object sender, RoutedEventArgs e)
        {
            Статистика статистика = new Статистика();
            статистика.Show();
            this.Close();
        }

        public void LoadRequests()
        {
            // Загружаем заявки для текущего пользователя
            List<RequestDetail> requests = DbUse.GetRequestsByUserRole(EnteredUser.Instance.CurrentType);
            Requests.Clear();
            // Обновляем коллекцию данных Requests
            foreach (var request in requests)
            {
                Requests.Add(request);
            }


        }


        



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            // Фильтруем список заявок по тексту поиска
            var filteredRequests = Requests.Where(request =>
                request.problemDescryption.ToLower().Contains(searchText) ||
                request.RequestStatus.ToLower().Contains(searchText) ||
                request.HomeTechType.ToLower().Contains(searchText) ||
                request.HomeTechModel.ToLower().Contains(searchText)
            ).ToList();

            // Обновляем отображаемый список заявок
            RequestsListBox.ItemsSource = filteredRequests;
        }

        private void OpenRequestDetail(RequestDetail requestDetail)
        {
            var window = new Создание_редоктирование_заявки(requestDetail);
            window.Show();
            this.Close();
        }

        private void RequestsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedRequestDetail = (RequestDetail)e.AddedItems[0];
                OpenRequestDetail(selectedRequestDetail);
                ((ListBox)sender).SelectedItem = null; // Сброс выбора, чтобы можно было снова выбрать тот же элемент
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void createnewrequest(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // Создаем новый объект RequestDetail с необходимыми начальными данными
                var newRequestDetail = new RequestDetail
                {
                    StartDate = DateTime.Now,
                    problemDescryption = "Новая заявка",
                    CompletionDate = null,
                    RepairParts = "",
                    
                };
                dbContext.RequestsDetails.Add(newRequestDetail);
                dbContext.SaveChanges();
                // Создаем новый объект RequestDetailsStatus
                RequestsDetailsStatus newRequestDetailsStatus = new RequestsDetailsStatus
                {
                    RequestID = newRequestDetail.RequestID,
                    RequestStatusID = 3 // Здесь initialStatusID - это идентификатор начального статуса
                };
                dbContext.RequestsDetailsStatus.Add(newRequestDetailsStatus); // Добавляем объект RequestDetailsStatus в список

                // Создаем новый объект RequestDetailTech
                RequestDetailTech newRequestDetailTech = new RequestDetailTech
                {
                    RequestID = newRequestDetail.RequestID,
                    TechID = 1 // Здесь initialTechID - это идентификатор начального типа техники
                };
                dbContext.RequestsDetailsTech.Add(newRequestDetailTech); // Добавляем объект RequestDetailTech в список

                // Создаем новую заявку и клиента (для простоты используем текущего пользователя как клиента)
                var newRequest = new Request
                {
                    ClientID = EnteredUser.Instance.CurrentUser.UserID,
                    RequestDetail = newRequestDetail
                };

                dbContext.Requests.Add(newRequest);

                dbContext.SaveChanges();

                // Обновляем ID новой заявки после сохранения
                newRequestDetail.RequestID = newRequest.RequestID;
            

            // Открываем окно редактирования для новой заявки
            var editWindow = new Создание_редоктирование_заявки(newRequestDetail);
                editWindow.Show();
                this.Close();
            }
        }

    }
}
