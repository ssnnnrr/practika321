using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using practika.BD;

namespace practika
{
    public partial class TeacherPage : Page
    {
        private string _userRole;
        private Department _userDepartment;
        private CollectionViewSource _viewSource;

        public TeacherPage(string userRole, Department userDepartment)
        {
            InitializeComponent();
            _userRole = userRole;
            _userDepartment = userDepartment;
            _viewSource = (CollectionViewSource)FindResource("TeacherViewSource");
            LoadTeacherData();
            SetButtonVisibility(userRole);
        }

        public void LoadTeacherData()
        {
            List<Teacher> teacherList;
            if (_userRole == Role.ЗавКафедрой)
            {
                teacherList = DatabaseManager.GetTeacher(_userDepartment);
            }
            else
            {
                teacherList = DatabaseManager.GetTeacher();
            }
            _viewSource.Source = teacherList;
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
                AddButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTeacherWindow addTeacherWindow = new AddTeacherWindow(this);
            bool? result = addTeacherWindow.ShowDialog();
            if (result == true)
            {
                LoadTeacherData();
            }
        }

        private void TeacherDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Teacher selectedTeacher = TeacherDataGrid.SelectedItem as Teacher;
            if (selectedTeacher == null)
            {
                e.Handled = true;
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Teacher selectedTeacher = TeacherDataGrid.SelectedItem as Teacher;
            if (selectedTeacher != null)
            {
                EditTeacherWindow editTeacherWindow = new EditTeacherWindow(this, selectedTeacher);
                bool? result = editTeacherWindow.ShowDialog();
                if (result == true)
                {
                    LoadTeacherData();
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Teacher selectedTeacher = TeacherDataGrid.SelectedItem as Teacher;
            if (selectedTeacher != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого преподавателя?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DatabaseManager.DeleteTeacher(selectedTeacher.tab_num);
                        LoadTeacherData();
                        MessageBox.Show("Преподаватель успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении преподавателя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewSource.View != null)
            {
                _viewSource.View.Filter = teacher =>
                {
                    var teach = teacher as Teacher;
                    if (teach == null)
                    {
                        return false;
                    }

                    bool matchesSearch = teach.rank.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;

                    if (FilterRankComboBox.SelectedIndex > 0)
                    {
                        string selectedRank = ((ComboBoxItem)FilterRankComboBox.SelectedItem).Content.ToString();
                        return matchesSearch && teach.rank == selectedRank;
                    }

                    return matchesSearch;
                };
            }
            _viewSource.View.Refresh();
        }
        private void FilterRankComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewSource.View != null)
            {
                _viewSource.View.Filter = teacher =>
                {
                    var teach = teacher as Teacher;
                    if (teach == null)
                    {
                        return false;
                    }

                    bool matchesSearch = teach.rank.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;

                    if (FilterRankComboBox.SelectedIndex > 0)
                    {
                        string selectedRank = ((ComboBoxItem)FilterRankComboBox.SelectedItem).Content.ToString();
                        return matchesSearch && teach.rank == selectedRank;
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
                    _viewSource.View.SortDescriptions.Add(new SortDescription("rank", ListSortDirection.Ascending));
                }
                else if (SortComboBox.SelectedIndex == 1)
                {
                    _viewSource.View.SortDescriptions.Add(new SortDescription("rank", ListSortDirection.Descending));
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