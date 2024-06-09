using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZXing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using ZXing.QrCode;
namespace Практика_макет
{
    /// <summary>
    /// Логика взаимодействия для Создание_редоктирование_заявки.xaml
    /// </summary>
    public partial class Создание_редоктирование_заявки : Window
    {
        private RequestDetail _requestDetail;

        User currentUser = EnteredUser.Instance.CurrentUser;
        int currentType = EnteredUser.Instance.CurrentType;
        public Создание_редоктирование_заявки(RequestDetail requestDetail)
        {
            InitializeComponent();
            GenerateQRCode();
            _requestDetail = requestDetail;
            DataContext = _requestDetail;
            AddData();
            commentPanel.Visibility = Visibility.Hidden;
            switch (currentType)
            {
                case 1:
                    //менеджер меняет дату окончания и мастера
                    EndDate.IsEnabled = true;
                    Master.IsEnabled = true;
                    break;
                case 2:
                    //мастер добавляет комментарии
                    commentPanel.Visibility = Visibility.Visible;
                    break;
                case 3:
                    //оператор меняет статус 
                    Status.IsEnabled = true;
                    break;
                case 4:
                    //заказчик меняет модель, тип техники и описание
                    TechModel.IsEnabled = true;
                    TechType.IsEnabled = true;
                    Description.IsEnabled = true;
                    break;

            }
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика удаления заявки
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика сохранения изменений
        }

        private void AssignMasterButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика назначения мастера и изменения статуса
        }


        private void SaveRequestDetails()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                var requestDetails = dbContext.RequestsDetails
                    .Include(rd => rd.Requests)
                        .ThenInclude(r => r.Client)
                    .Include(rd => rd.Requests)
                        .ThenInclude(r => r.Master)
                    .Include(rd => rd.RequestsDetailsStatus)
                        .ThenInclude(rds => rds.RequestStatus)
                    .Include(rd => rd.RequestsDetailsTech)
                        .ThenInclude(rdt => rdt.HomeTech)
                    .FirstOrDefault(rd => rd.RequestID == _requestDetail.RequestID);

                if (requestDetails != null)
                {
                    // Обновляем данные
                    if (MasterComboBox.SelectedValue != null)
                    {
                        int selectedMasterID = (int)MasterComboBox.SelectedValue;
                        var selectedMaster = dbContext.Users.FirstOrDefault(m => m.UserID == selectedMasterID);
                        if (selectedMaster != null)
                        {
                            requestDetails.Requests.MasterID = selectedMasterID;
                            requestDetails.Requests.Master = selectedMaster;
                        }
                    }
                    requestDetails.problemDescryption = Description.Text;
                    var requestTech = requestDetails.RequestsDetailsTech.FirstOrDefault();
                    if (requestTech != null)
                    {
                        requestTech.HomeTech.HomeTechType = TechType.Text;
                        requestTech.HomeTech.HomeTechModel = TechModel.Text;
                    }

                    // Обновляем статус заявки
                    var selectedStatus = (ComboBoxItem)Status.SelectedItem;
                    if (selectedStatus != null)
                    {
                        string statusContent = selectedStatus.Content.ToString();
                        var status = dbContext.RequestStatus.FirstOrDefault(rs => rs.requestStatus == statusContent);
                        if (status != null)
                        {
                            var currentStatus = requestDetails.RequestsDetailsStatus.FirstOrDefault();
                            if (currentStatus != null)
                            {
                                dbContext.RequestsDetailsStatus.Remove(currentStatus);
                                dbContext.SaveChanges();

                                var newStatus = new RequestsDetailsStatus
                                {
                                    RequestID = requestDetails.RequestID,
                                    RequestStatusID = status.RequestStatusID
                                };
                                dbContext.RequestsDetailsStatus.Add(newStatus);
                            }
                        }
                    }

                    // Обновляем дату завершения
                    if (DateTime.TryParse(EndDate.Text, out DateTime completionDate))
                    {
                        requestDetails.CompletionDate = completionDate;
                    }
                    else
                    {
                        requestDetails.CompletionDate = null; // Если текстовое поле пустое или недопустимое значение, устанавливаем null
                    }

                    // Сохраняем изменения
                    dbContext.SaveChanges();
                    AddData();
                    MessageBox.Show("Изменения приняты");
                }
            }
        }




        public void AddData()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                RequestDetail requestDetails = dbContext.RequestsDetails
                     .Include(rd => rd.Requests)
                         .ThenInclude(r => r.Client)
                     .Include(rd => rd.Requests)
                         .ThenInclude(r => r.Master)
                     .Include(rd => rd.RequestsDetailsStatus)
                         .ThenInclude(rds => rds.RequestStatus)
                     .Include(rd => rd.RequestsDetailsTech)
                         .ThenInclude(rdt => rdt.HomeTech)
                     .FirstOrDefault(rd => rd.RequestID == _requestDetail.RequestID);

                if (requestDetails != null)
                {
                    RequestId.Content = "№ заявки: " + requestDetails.RequestID;
                    FullName.Text = requestDetails.Requests.Client.FullName;
                    Phone.Text = requestDetails.Requests.Client.Phone;
                    CreateDate.Text = requestDetails.StartDate.ToString();
                    Status.Text = requestDetails.RequestStatus;
                    Master.Text = requestDetails.Requests.Master?.FullName ?? "Не назначен";
                    TechType.Text = requestDetails.HomeTechType;
                    TechModel.Text = requestDetails.HomeTechModel;
                    Description.Text = requestDetails.problemDescryption;

                    // Получаем всех мастеров и устанавливаем их в качестве источника данных для ComboBox
                    var masters = dbContext.Users
                                           .Where(u => u.UserTypes.Any(ut => ut.TypeID == 2))
                                           .ToList();
                    MasterComboBox.ItemsSource = masters;

                    // Устанавливаем текущего мастера в ComboBox
                    MasterComboBox.SelectedValue = requestDetails.Requests.MasterID;

                    // Получаем все комментарии для текущей заявки
                    var comments = dbContext.Comments
                                            .Where(c => c.requestID == requestDetails.Requests.RequestID)
                                            .ToList();

                    // Устанавливаем список комментариев в качестве источника данных для ListBox
                    CommentsListBox.ItemsSource = comments;

                    // Отображаем дату завершения
                    EndDate.Text = requestDetails.CompletionDate?.ToString("dd.MM.yyyy") ?? "Не завершено";
                }
            }
        }


        private  void GenerateQRCode()
        {

            string googleFormUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdHb7SsdLn9oGX9lh4Uri-ICDk82hmQMc258UpER61cougZOw/viewform?usp=sf_link";

            var options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Height = 200,
                Width = 200
            };
            var writer = new ZXing.Windows.Compatibility.BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;

            var result = writer.Write(googleFormUrl);


            var bitmap = new BitmapImage();
            using (var stream = new System.IO.MemoryStream())
            {
                result.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }
            QrCodeImage.Source = bitmap;
        }

        private void new_comment_click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(newcomment.Text))
            {
                using (ApplicationDbContext dbContext = new ApplicationDbContext())
                {
                    var newComment = new Comment
                    {
                        Message = newcomment.Text,
                        UserID = currentUser.UserID,
                        requestID = _requestDetail.RequestID
                    };

                    dbContext.Comments.Add(newComment);
                    dbContext.SaveChanges();

                    // Обновляем список комментариев после добавления нового
                    var comments = dbContext.Comments
                                    .Where(c => c.requestID == _requestDetail.RequestID)
                                    .ToList();
                    CommentsListBox.ItemsSource = comments;

                    // Очищаем поле ввода комментария
                    newcomment.Clear();
                }
            }
            else
            {
                MessageBox.Show("Комментарий не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void Открыть_список_заявок(object sender, RoutedEventArgs e)
        {
            Список_заявок список_заявок = new Список_заявок();
            список_заявок.Show();
            this.Close();
        }

        private void SaveRequestDetails(object sender, RoutedEventArgs e)
        {
            SaveRequestDetails();
        }
    }
}

