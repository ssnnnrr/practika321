using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public static class Role
    {
        public const string Инженер = "инженер";
        public const string ЗавКафедрой = "зав. кафедрой";
        public const string Преподаватель = "преподаватель";
    }

    public partial class AuthWindow : Window
    {
        private DatabaseManager dbManager;

        public AuthWindow()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void QrButtonClick(object sender, RoutedEventArgs e)
        {
            Qr qrWindow = new Qr();
            qrWindow.Show();
            this.Close();
        }

        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(passwordBox.Password, out int tabNumber))
            {
                MessageBox.Show("Пожалуйста, введите корректный пароль.");
                return;
            }

            var (userRole, department) = DatabaseManager.GetRoleAndDepartmentByTabNumber(tabNumber);
            if (userRole != null)
            {
                MessageBox.Show($"Успешный вход! Роль: {userRole}");

                MainAppWindow mainAppWindow = new MainAppWindow(userRole, department);
                mainAppWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный пароль.");
            }
        }
    }
}