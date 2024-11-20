using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
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
            int kod = (int)KodComboBox.SelectedValue;
            int regNum = (int)RegNumComboBox.SelectedValue;
            string auditorium = AuditoriumTextBox.Text;
            int estimation = int.TryParse(EstimationTextBox.Text, out int est) ? est : 0;

            _exam.date = date;
            _exam.kod = kod;
            _exam.reg_num = regNum;
            _exam.auditorium = auditorium;
            _exam.estimation = estimation;

            DatabaseManager.GetExam();

            _parentPage.LoadExamData();

            this.Close();
        }

    }
}