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
            int estimation = int.TryParse(EstimationTextBox.Text, out int est) ? est : 0;
            _exam.estimation = estimation;
            DatabaseManager.GetExam();
            _parentPage.LoadExamData();
            this.Close();
        }
    }
}