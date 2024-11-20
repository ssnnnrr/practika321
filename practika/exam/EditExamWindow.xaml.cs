using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public partial class EditExamWindow : Window
    {
        private Exam _exam;
        private ExamPage _parentPage;

        public EditExamWindow(ExamPage parentPage, Exam exam)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _exam = exam;
            LoadData();
        }

        private void LoadData()
        {
            List<Teacher> teachers = DatabaseManager.GetTeacher();
            foreach (var teacher in teachers)
            {
                Debug.WriteLine($"Teacher TabNumber: {teacher.tab_num}");
            }
            TabNumComboBox.ItemsSource = teachers;

            List<Employee> employees = DatabaseManager.GetEmployee("преподаватель");
            foreach (var employee in employees)
            {
                Debug.WriteLine($"Employee TabNumber: {employee.tab_number}");
            }
            KodComboBox.ItemsSource = employees;

            List<Student> registrations = DatabaseManager.GetStudent();
            foreach (var student in registrations)
            {
                Debug.WriteLine($"Student RegNum: {student.num}");
            }
            RegNumComboBox.ItemsSource = registrations;

            DatabaseManager.GetExam();

            LoadExamData();
        }

        private void LoadExamData()
        {
            DatePicker.SelectedDate = _exam.date;
            KodComboBox.SelectedValue = _exam.kod;
            RegNumComboBox.SelectedValue = _exam.reg_num;
            AuditoriumTextBox.Text = _exam.auditorium;
            EstimationTextBox.Text = _exam.estimation.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = DatePicker.SelectedDate ?? DateTime.Now;
            int kod = 0;
            if (KodComboBox.SelectedValue != null)
            {
                kod = (int)KodComboBox.SelectedValue;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите значение из KodComboBox.");
                return;
            }

            int regNum = 0;
            if (RegNumComboBox.SelectedValue != null)
            {
                regNum = (int)RegNumComboBox.SelectedValue;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите значение из RegNumComboBox.");
                return;
            }

            string auditorium = AuditoriumTextBox.Text;
            int estimation = int.TryParse(EstimationTextBox.Text, out int est) ? est : 0;

            if (estimation < 1 || estimation > 5)
            {
                MessageBox.Show("Оценка должна быть в диапазоне от 1 до 5.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _exam.date = date;
            _exam.kod = kod;
            _exam.reg_num = regNum;
            _exam.auditorium = auditorium;
            _exam.estimation = estimation;

            DatabaseManager.GetExam(); 

            _parentPage.LoadExamData();

            this.Close();
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