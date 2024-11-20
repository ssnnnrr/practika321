using System.Collections.Generic;
using System.Windows;
using practika.BD;

namespace practika
{
    public partial class EditDisciplineWindow : Window
    {
        private Discipline _discipline;
        private DisciplinePage _parentPage;

        public EditDisciplineWindow(DisciplinePage parentPage, Discipline discipline)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _discipline = discipline;
            LoadDisciplineData();
            LoadData();
        }

        private void LoadDisciplineData()
        {
            NameTextBox.Text = _discipline.name;
            VolumeTextBox.Text = _discipline.volume.ToString();
            ExecutorTextBox.Text = _discipline.executor;
        }

        private void LoadData()
        {
            List<Discipline> disciplines = DatabaseManager.GetDiscipline();
            KodComboBox.ItemsSource = disciplines;
            KodComboBox.SelectedValue = _discipline.kod;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int kod = ((Discipline)KodComboBox.SelectedItem)?.kod ?? 0;
            string name = NameTextBox.Text;
            int volume = int.Parse(VolumeTextBox.Text);
            string executor = ExecutorTextBox.Text;

            if (kod == 0 || string.IsNullOrWhiteSpace(name) || volume == 0 || string.IsNullOrWhiteSpace(executor))
            {
                MessageBox.Show("Все поля должны быть заполнены корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _discipline.kod = kod;
            _discipline.name = name;
            _discipline.volume = volume;
            _discipline.executor = executor;

            DatabaseManager.UpdateDiscipline(_discipline);
            _parentPage.LoadDisciplineData();

            this.Close();
        }
    }
}