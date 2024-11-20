using practika.BD;
using System.Windows;

namespace practika
{
    public partial class EditEstimationWindow : Window
    {
        private Exam _exam;
        private ExamPage _parentPage;

        public EditEstimationWindow(ExamPage parentPage, Exam exam)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _exam = exam;
            LoadData();
        }

        private void LoadData()
        {
            EstimationTextBox.Text = _exam.estimation.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(EstimationTextBox.Text, out int estimation))
            {
                if (estimation >= 1 && estimation <= 5)
                {
                    _exam.estimation = estimation;
                    DatabaseManager.GetExam(); 
                    _parentPage.LoadExamData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Оценка должна быть от 1 до 5.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное числовое значение для оценки.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}