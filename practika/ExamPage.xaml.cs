using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using practika.BD;

namespace practika
{
    public partial class ExamPage : Page
    {
        private string _userRole;
        private Department _userDepartment;
        private CollectionViewSource _viewSource;

        public ExamPage(string userRole, Department userDepartment)
        {
            InitializeComponent();
            _userRole = userRole;
            _userDepartment = userDepartment;
            _viewSource = (CollectionViewSource)FindResource("ExamViewSource");
            LoadExamData();
            SetButtonVisibility(userRole);
        }

        public void LoadExamData()
        {
            List<Exam> examList;
            if (_userRole == Role.ЗавКафедрой)
            {
                examList = DatabaseManager.GetExam(_userDepartment);
            }
            else
            {
                examList = DatabaseManager.GetExam();
            }
            _viewSource.Source = examList;
            ExamDataGrid.ItemsSource = _viewSource.View;
        }

        private void SetButtonVisibility(string userRole)
        {
            if (userRole == Role.Инженер)
            {
                AddButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
            else if (userRole == Role.Преподаватель)
            {
                foreach (var item in ExamDataGrid.ContextMenu.Items)
                {
                    if (item is MenuItem menuItem)
                    {
                        if (menuItem.Header.ToString() == "Удалить")
                        {
                            menuItem.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddExamWindow addExamWindow = new AddExamWindow(this);
            bool? result = addExamWindow.ShowDialog();
            if (result == true)
            {
                LoadExamData();
            }
        }

        private void ExamDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Exam selectedExam = ExamDataGrid.SelectedItem as Exam;
            if (selectedExam == null)
            {
                e.Handled = true;
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Exam selectedExam = ExamDataGrid.SelectedItem as Exam;
            if (selectedExam != null)
            {
                if (_userRole == Role.Преподаватель)
                {
                    EditEstimationWindow editEstimationWindow = new EditEstimationWindow(this, selectedExam);
                    bool? result = editEstimationWindow.ShowDialog();
                    if (result == true)
                    {
                        LoadExamData();
                    }
                }
                else
                {
                    EditExamWindow editExamWindow = new EditExamWindow(this, selectedExam);
                    bool? result = editExamWindow.ShowDialog();
                    if (result == true)
                    {
                        LoadExamData();
                    }
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Exam selectedExam = ExamDataGrid.SelectedItem as Exam;
            if (selectedExam != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот экзамен?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DatabaseManager.DeleteExam(selectedExam.id_exam);
                        LoadExamData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении экзамена: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterEstimationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_viewSource.View != null)
            {
                _viewSource.View.Filter = exam =>
                {
                    var examItem = exam as Exam;
                    if (examItem == null)
                    {
                        return false;
                    }

                    string searchText = SearchTextBox.Text.ToLower();

                    bool matchesSearch = (examItem.date.HasValue ? examItem.date.Value.ToString("yyyy-MM-dd") : "").ToLower().Contains(searchText) ||
                                         examItem.kod.ToString().ToLower().Contains(searchText) ||
                                         examItem.reg_num.ToString().ToLower().Contains(searchText) ||
                                         (examItem.Teacher != null && examItem.Teacher.tab_num.ToString().ToLower().Contains(searchText)) ||
                                         (examItem.auditorium != null && examItem.auditorium.ToLower().Contains(searchText)) ||
                                         examItem.estimation.ToString().ToLower().Contains(searchText);

                    if (FilterEstimationComboBox.SelectedIndex > 0)
                    {
                        string selectedEstimation = ((ComboBoxItem)FilterEstimationComboBox.SelectedItem).Content.ToString().ToLower();
                        return matchesSearch && examItem.estimation.ToString().ToLower() == selectedEstimation;
                    }

                    return matchesSearch;
                };
            }
            _viewSource.View.Refresh();
        }
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewSource.View != null)
            {
                _viewSource.View.SortDescriptions.Clear();
                if (SortComboBox.SelectedIndex == 0)
                {
                    _viewSource.View.SortDescriptions.Add(new SortDescription("auditorium", ListSortDirection.Ascending));
                }
                else if (SortComboBox.SelectedIndex == 1)
                {
                    _viewSource.View.SortDescriptions.Add(new SortDescription("auditorium", ListSortDirection.Descending));
                }
                else if (SortComboBox.SelectedIndex == 2)
                {
                    _viewSource.View.SortDescriptions.Clear();
                }
            }
            _viewSource.View.Refresh();
        }
    }
}