using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public partial class AddExamWindow : Window
    {
        private ExamPage _parentPage;

        public AddExamWindow(ExamPage parentPage)
        {
            InitializeComponent();
            _parentPage = parentPage;
            LoadData();
        }

        private void LoadData()
        {
            List<Teacher> teachers = DatabaseManager.GetTeacher();
            if (teachers == null || teachers.Count == 0)
            {
                MessageBox.Show("No teachers found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TabNumComboBox.ItemsSource = teachers;
                TabNumComboBox.DisplayMemberPath = "tab_num"; 
                if (TabNumComboBox.Items.Count > 0) TabNumComboBox.SelectedIndex = 0;
            }

            List<Employee> employees = DatabaseManager.GetEmployee("преподаватель");
            if (employees == null || employees.Count == 0)
            {
                MessageBox.Show("No employees found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                KodComboBox.ItemsSource = employees;
                KodComboBox.DisplayMemberPath = "tab_number"; 
                if (KodComboBox.Items.Count > 0) KodComboBox.SelectedIndex = 0;
            }

            List<Student> registrations = DatabaseManager.GetStudent();
            if (registrations == null || registrations.Count == 0)
            {
                MessageBox.Show("No students found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                RegNumComboBox.ItemsSource = registrations;
                RegNumComboBox.DisplayMemberPath = "reg_num"; 
                if (RegNumComboBox.Items.Count > 0) RegNumComboBox.SelectedIndex = 0;
            }
        }

        public Exam GetExam()
        {
            DateTime date = DatePicker.SelectedDate ?? DateTime.Now;
            int tabNum = ((Teacher)TabNumComboBox.SelectedItem)?.tab_num ?? 0;
            int kod = ((Employee)KodComboBox.SelectedItem)?.tab_number ?? 0;
            int regNum = ((Student)RegNumComboBox.SelectedItem)?.reg_num ?? 0;
            string auditorium = AuditoriumTextBox.Text;
            int estimation = int.TryParse(EstimationTextBox.Text, out int est) ? est : 0;

            if (tabNum == 0 || kod == 0 || regNum == 0 || string.IsNullOrWhiteSpace(auditorium) || estimation == 0)
            {
                throw new InvalidOperationException("Все поля должны быть заполнены корректно.");
            }

            if (estimation < 1 || estimation > 5)
            {
                throw new InvalidOperationException("Оценка должна быть в диапазоне от 1 до 5.");
            }

            return new Exam
            {
                date = date,
                tab_num = tabNum,
                kod = kod,
                reg_num = regNum,
                auditorium = auditorium,
                estimation = estimation
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Exam newExam = GetExam();
                DatabaseManager.AddExam(newExam);
                DatabaseManager.GetExam();
                _parentPage.LoadExamData();
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении экзамена: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EstimationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(EstimationTextBox.Text, out int estimation))
            {
                if (estimation < 1 || estimation > 5)
                {
                    EstimationTextBox.Text = "1";
                    EstimationTextBox.SelectAll();
                }
            }
            else
            {
                EstimationTextBox.Text = "1"; 
                EstimationTextBox.SelectAll();
            }
        }
    }
}