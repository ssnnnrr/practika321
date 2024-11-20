using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public partial class EditDepartmentWindow : Window
    {
        public event Action DepartmentUpdated;

        private DepartmentPage _parentPage;
        private Department _department;

        public EditDepartmentWindow(DepartmentPage parentPage, Department department)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _department = department;
            LoadDepartmentData();
            LoadFaculties();
        }

        private void LoadDepartmentData()
        {
            CodeTextBox.Text = _department.code;
            NameTextBox.Text = _department.name;
        }

        private void LoadFaculties()
        {
            List<Faculty> faculties = DatabaseManager.GetFaculty();
            FacultyIdComboBox.ItemsSource = faculties;
            FacultyIdComboBox.SelectedValue = _department.id_faculty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string code = CodeTextBox.Text;
            string name = NameTextBox.Text;
            int facultyId = ((Faculty)FacultyIdComboBox.SelectedItem)?.id_faculty ?? 0;

            if (string.IsNullOrEmpty(name) || facultyId == 0)
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _department.name = name;
            _department.id_faculty = facultyId;

            DatabaseManager.UpdateDepartment(_department);
            _parentPage.LoadDepartmentData();
            DepartmentUpdated?.Invoke();
            this.Close();
        }
    }
}