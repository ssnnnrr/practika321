using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public partial class EditDisciplineWindow : Window
    {
        private Discipline _discipline;
        private DisciplinePage _parentPage;
        private string _userRole;
        private string _userDepartmentCode;
        private readonly string _originalExecutor;

        public EditDisciplineWindow(DisciplinePage parentPage, Discipline discipline, string userRole, string userDepartmentCode)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _discipline = discipline;
            _userRole = userRole;
            _userDepartmentCode = userDepartmentCode;
            _originalExecutor = discipline.executor;
            LoadDisciplineData();
            LoadData();
            InitializeControls();
        }

        private void InitializeControls()
        {
            if (_userRole == _originalExecutor)
            {
                ExecutorTextBox.IsEnabled = false;
                KodComboBox.Visibility = Visibility.Collapsed;
                ExecutorTextBox.Visibility = Visibility.Collapsed; 
            }
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
            int kod = _userRole == "Зав. кафедрой" ? int.Parse(_userDepartmentCode) : ((Discipline)KodComboBox.SelectedItem)?.kod ?? 0;
            string name = NameTextBox.Text;
            int volume = int.TryParse(VolumeTextBox.Text, out int vol) ? vol : 0;
            string executor = _userRole == "Зав. кафедрой" ? _originalExecutor : ExecutorTextBox.Text;

            if (kod == 0 || string.IsNullOrWhiteSpace(name) || volume == 0 || string.IsNullOrWhiteSpace(executor))
            {
                MessageBox.Show("Все поля должны быть заполнены корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (volume < 1 || volume > 400)
            {
                MessageBox.Show("Объем дисциплины должен быть в диапазоне от 1 до 400.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            _discipline.kod = kod;
            _discipline.name = name;
            _discipline.volume = volume;
            _discipline.executor = executor;
            var dist = _userDepartmentCode.ToString();
            DatabaseManager.UpdateDiscipline(_discipline); 
            
            _parentPage.LoadDisciplineData();

            this.Close();
        }

        private void VolumeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(VolumeTextBox.Text, out int volume))
            {
                if (volume < 1 || volume > 100)
                {
                    VolumeTextBox.Text = "1";
                    VolumeTextBox.SelectAll();
                }
            }
            else
            {
                VolumeTextBox.Text = "1";
                VolumeTextBox.SelectAll();
            }
        }
    }
}