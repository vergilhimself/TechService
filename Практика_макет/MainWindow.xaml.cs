using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Практика_макет
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("auth window");
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void Открыть_регистрацию(object sender, RoutedEventArgs e)
        {
            Регистрация регистрация = new Регистрация();
            регистрация.Show();
            this.Close();
        }

        public void Открыть_список_заявок(object sender, RoutedEventArgs e)
        {
            bool success = DbUse.Auth(Login.Text, Password.Password);
            
                if (success)
                {
                    
                    // Пользователь найден, открыть новое окно
                    Список_заявок регистрация = new Список_заявок();
                    регистрация.Show();
                    this.Close();
                }
                else
                {
                    // Пользователь не найден, показать сообщение об ошибке
                    MessageBox.Show("Неверный логин или пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
    }

