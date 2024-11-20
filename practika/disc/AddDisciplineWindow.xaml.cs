using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public partial class AddDisciplineWindow : Window
    {
        private DisciplinePage _parentPage;
        private string _userRole; 
        private string _userDepartmentCode; 

        public AddDisciplineWindow(DisciplinePage parentPage, string userRole, string userDepartmentCode)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _userRole = userRole;
            _userDepartmentCode = userDepartmentCode;
            InitializeControls();
        }

        private void InitializeControls()
        {
            if (_userRole == "Заведующий кафедрой")
            {
                ExecutorTextBox.IsEnabled = false;
                ExecutorTextBox.Text = _userDepartmentCode;
            }
        }

        public Discipline GetDiscipline()
        {
            if (!int.TryParse(KodTextBox.Text, out int kod) || kod == 0)
            {
                throw new InvalidOperationException("Код должен быть корректным числом.");
            }

            string name = NameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Наименование не может быть пустым.");
            }

            if (!int.TryParse(VolumeTextBox.Text, out int volume) || volume == 0)
            {
                throw new InvalidOperationException("Объем часов должен быть корректным числом.");
            }

            string executor = ExecutorTextBox.Text;
            if (string.IsNullOrWhiteSpace(executor))
            {
                throw new InvalidOperationException("Исполнитель не может быть пустым.");
            }

            if (_userRole == "Заведующий кафедрой" && executor.ToString() != _userDepartmentCode)
            {
                throw new InvalidOperationException("Заведующий кафедрой может добавлять новые данные только с тем же шифром, что и у него самого.");
            }

            return new Discipline
            {
                kod = kod,
                name = name,
                volume = volume,
                executor = executor
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Discipline newDiscipline = GetDiscipline();
                DatabaseManager.AddDiscipline(newDiscipline);
                MessageBox.Show("Дисциплина успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _parentPage.LoadDisciplineData();
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении дисциплины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}