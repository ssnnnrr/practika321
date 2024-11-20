using System;
using System.Windows;
using practika.BD;

namespace practika
{
    public partial class AddTeacherWindow : Window
    {
        private TeacherPage _parentPage;

        public AddTeacherWindow(TeacherPage parentPage)
        {
            InitializeComponent();
            _parentPage = parentPage;
        }

        public Teacher GetTeacher()
        {
            if (!int.TryParse(TabNumTextBox.Text, out int tabNum))
            {
                throw new ArgumentException("Табельный номер должен быть целым числом.");
            }

            string rank = RankTextBox.Text;
            string degree = DegreeTextBox.Text;

            return new Teacher
            {
                tab_num = tabNum,
                rank = rank,
                degree = degree
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Teacher newTeacher = GetTeacher();
                DatabaseManager.AddTeacher(newTeacher);
                _parentPage.LoadTeacherData();
                MessageBox.Show("Преподаватель успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении преподавателя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}