using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public partial class AddDepartmentWindow : Window
    {
        private string _userRole;
        private DepartmentPage _parentPage;

        public AddDepartmentWindow(DepartmentPage parentPage, string userRole)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _userRole = userRole;
            LoadData();
        }

        private void LoadData()
        {
            List<Faculty> faculties = DatabaseManager.GetFaculty();
            FacultyIdComboBox.ItemsSource = faculties;
        }

        public Department GetDepartment()
        {
            string code = CodeTextBox.Text;
            string name = NameTextBox.Text;
            int facultyId = ((Faculty)FacultyIdComboBox.SelectedItem)?.id_faculty ?? 0;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || facultyId == 0)
            {
                throw new InvalidOperationException("Пожалуйста, заполните все поля корректно.");
            }

            return new Department
            {
                code = code,
                name = name,
                id_faculty = facultyId
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Department newDepartment = GetDepartment();
                DatabaseManager.AddDepartment(newDepartment, _userRole);
                MessageBox.Show("Кафедра успешно добавлена.");
                _parentPage.LoadDepartmentData();
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}