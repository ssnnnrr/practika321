using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using practika.BD;

namespace practika
{
    public partial class DepartmentPage : Page
    {
        private string _userRole;
        private Department _userDepartment;

        private CollectionViewSource _viewSource;

        public DepartmentPage(string userRole, Department userDepartment)
        {
            InitializeComponent();
            _userRole = userRole;
            _userDepartment = userDepartment;
            _viewSource = (CollectionViewSource)FindResource("DepartmentViewSource");
            LoadDepartmentData();
            SetButtonVisibility(userRole);
        }

        public void LoadDepartmentData()
        {
            List<Department> departmentList;
            if (_userRole == Role.ЗавКафедрой)
            {
                departmentList = DatabaseManager.GetDepartment(_userDepartment);
            }
            else
            {
                departmentList = DatabaseManager.GetDepartment();
            }
            _viewSource.Source = departmentList;


        }

        private void SetButtonVisibility(string userRole)
        {
            if (userRole == Role.Инженер)
            {
                AddButton.Visibility = Visibility.Visible;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddDepartmentWindow addDepartmentWindow = new AddDepartmentWindow(this, _userRole);
            bool? result = addDepartmentWindow.ShowDialog();
            if (result == true)
            {
                LoadDepartmentData();
            }
        }

        private void DepartmentDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Department selectedDepartment = DepartmentDataGrid.SelectedItem as Department;
            if (selectedDepartment == null)
            {
                e.Handled = true;
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Department selectedDepartment = DepartmentDataGrid.SelectedItem as Department;
            if (selectedDepartment != null)
            {
                EditDepartmentWindow editDepartmentWindow = new EditDepartmentWindow(this, selectedDepartment);
                bool? result = editDepartmentWindow.ShowDialog();
                if (result == true)
                {
                    LoadDepartmentData();
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Department selectedDepartment = DepartmentDataGrid.SelectedItem as Department;
            if (selectedDepartment != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту кафедру?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DatabaseManager.DeleteDepartment(selectedDepartment.code);
                        LoadDepartmentData();
                        MessageBox.Show("Кафедра успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении кафедры: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewSource.View != null)
            {
                _viewSource.View.Filter = department =>
                {
                    var dep = department as Department;
                    return dep.name.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
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