using System.Windows;
using practika.BD;

namespace practika
{
    public partial class EditTeacherWindow : Window
    {
        private Teacher _teacher;
        private TeacherPage _parentPage;

        public EditTeacherWindow(TeacherPage parentPage, Teacher teacher)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _teacher = teacher;
            LoadTeacherData();
        }

        private void LoadTeacherData()
        {
            TabNumTextBox.Text = _teacher.tab_num.ToString();
            RankTextBox.Text = _teacher.rank;
            DegreeTextBox.Text = _teacher.degree;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int tabNum = int.Parse(TabNumTextBox.Text);
            string rank = RankTextBox.Text;
            string degree = DegreeTextBox.Text;

            _teacher.rank = rank;
            _teacher.degree = degree;

            DatabaseManager.UpdateTeacher(_teacher);
            _parentPage.LoadTeacherData();

            this.Close();
        }
    }
}