using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using practika.BD;

namespace practika
{
    public partial class DisciplinePage : Page
    {
        private string _userRole;
        private Department _userDepartment;
        private CollectionViewSource _viewSource;

        public DisciplinePage(string userRole, Department userDepartment)
        {
            InitializeComponent();
            _userRole = userRole;
            _userDepartment = userDepartment;
            _viewSource = (CollectionViewSource)FindResource("DisciplineViewSource");
            LoadDisciplineData();
            SetButtonVisibility(userRole);
        }

        public void LoadDisciplineData()
        {
            List<Discipline> disciplineList;
            if (_userRole == Role.ЗавКафедрой)
            {
                disciplineList = DatabaseManager.GetDiscipline(_userDepartment);
            }
            else
            {
                disciplineList = DatabaseManager.GetDiscipline();
            }
            _viewSource.Source = disciplineList;
        }

        private void SetButtonVisibility(string userRole)
        {
            if (userRole == Role.Инженер)
            {
                AddButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
            else if (userRole == Role.ЗавКафедрой)
            {
                foreach (var item in DisciplineDataGrid.ContextMenu.Items)
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
            AddDisciplineWindow addDisciplineWindow = new AddDisciplineWindow(this);
            bool? result = addDisciplineWindow.ShowDialog();
            if (result == true)
            {
                LoadDisciplineData();
            }
        }

        private void DisciplineDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Discipline selectedDiscipline = DisciplineDataGrid.SelectedItem as Discipline;
            if (selectedDiscipline == null)
            {
                e.Handled = true;
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Discipline selectedDiscipline = DisciplineDataGrid.SelectedItem as Discipline;
            if (selectedDiscipline != null)
            {
                EditDisciplineWindow editDisciplineWindow = new EditDisciplineWindow(this, selectedDiscipline);
                bool? result = editDisciplineWindow.ShowDialog();
                if (result == true)
                {
                    LoadDisciplineData();
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Discipline selectedDiscipline = DisciplineDataGrid.SelectedItem as Discipline;
            if (selectedDiscipline != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту дисциплину?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DatabaseManager.DeleteDiscipline(selectedDiscipline.kod);
                        LoadDisciplineData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении дисциплины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewSource.View != null)
            {
                _viewSource.View.Filter = discipline =>
                {
                    var disc = discipline as Discipline;
                    return disc.name.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewSource.View != null)
            {
                _viewSource.View.SortDescriptions.Clear();
                if (SortComboBox.SelectedIndex == 0)
                {
                    _viewSource.View.SortDescriptions.Add(new SortDescription("name", ListSortDirection.Ascending));
                }
                else if (SortComboBox.SelectedIndex == 1)
                {
                    _viewSource.View.SortDescriptions.Add(new SortDescription("name", ListSortDirection.Descending));
                }
                else if (SortComboBox.SelectedIndex == 2)
                {
                    _viewSource.View.SortDescriptions.Clear();
                }
            }
        }
    }
}