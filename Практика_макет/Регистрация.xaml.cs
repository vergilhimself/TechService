using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;

namespace Практика_макет
{
    /// <summary>
    /// Логика взаимодействия для Регистрация.xaml
    /// </summary>
    public partial class Регистрация : Window
    {
        public Регистрация()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Открыть_авторизацию(object sender, RoutedEventArgs e)
        {
            MainWindow авторизация = new MainWindow();
            авторизация.Show();
            this.Close();
        }
        private async void Открыть_список_заявок(object sender, RoutedEventArgs e)
        {
            int typeID = 1; // Значение по умолчанию или для случая, если ни один RadioButton не выбран

            // Определение выбранной роли
            if (Менеджер.IsChecked == true)
            {
                typeID = 1; // Здесь можно использовать любые другие уникальные значения для каждой роли
            }
            else if (Мастер.IsChecked == true)
            {
                typeID = 2;
            }
            else if (Оператор.IsChecked == true)
            {
                typeID = 3;
            }
            else if (Заказчик.IsChecked == true)
            {
                typeID = 4;
            }

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(FullName.Text) || string.IsNullOrWhiteSpace(Phone.Text) ||
                string.IsNullOrWhiteSpace(Login.Text) || string.IsNullOrWhiteSpace(Password.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            // Проверка валидации номера телефона
            if (!ValidatePhoneNumber(Phone.Text))
            {
                MessageBox.Show("Пожалуйста, введите корректный номер телефона.");
                return;
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                // Проверяем существует ли уже пользователь с таким логином
                bool isLoginExists = await dbContext.UserCredentials.AnyAsync(uc => uc.Login == Login.Text);

                if (isLoginExists)
                {
                    // Если логин уже существует, показываем сообщение пользователю или выполняем другие действия
                    MessageBox.Show("Такой логин уже существует. Пожалуйста, выберите другой логин.");
                    return;
                }

                // Создаем нового пользователя
                var user = new User
                {
                    FullName = FullName.Text,
                    Phone = Phone.Text
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                // Создаем учетные данные пользователя
                var userCredential = new UserCredential
                {
                    UserID = user.UserID,
                    Login = Login.Text,
                    Password = Password.Text
                };

                dbContext.UserCredentials.Add(userCredential);
                await dbContext.SaveChangesAsync();

                var userType = new UserType
                {
                    UserID = user.UserID,
                    TypeID = typeID
                };

                dbContext.UserTypes.Add(userType);
                await dbContext.SaveChangesAsync();

                EnteredUser.Instance.CurrentUser = user;
                EnteredUser.Instance.CurrentType = typeID;
            }

            // После успешного сохранения нового пользователя и его учетных данных, открываем список заявок      
            Список_заявок список_заявок = new Список_заявок();
            список_заявок.Show();
            this.Close();
        }

        // Метод для простой валидации номера телефона
        private bool ValidatePhoneNumber(string phoneNumber)
        {
            // Проверяем, что номер содержит только цифры и имеет длину не менее 7 символов
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 7;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        
    }
}
