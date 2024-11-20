using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using practika.BD;

namespace practika
{
    public partial class AddDisciplineWindow : Window
    {
        private DisciplinePage _parentPage;

        public AddDisciplineWindow(DisciplinePage parentPage)
        {
            InitializeComponent();
            _parentPage = parentPage;
        }

        public Discipline GetDiscipline()
        {
            if (!int.TryParse(KodTextBox.Text, out int kod) || kod == 0)
            {
                throw new InvalidOperationException("Код должен быть корректным числом.");
            }

            string name = NameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Наименование не может быть пустым.");
            }

            if (!int.TryParse(VolumeTextBox.Text, out int volume) || volume == 0)
            {
                throw new InvalidOperationException("Объем часов должен быть корректным числом.");
            }

            string executor = ExecutorTextBox.Text;
            if (string.IsNullOrWhiteSpace(executor))
            {
                throw new InvalidOperationException("Исполнитель не может быть пустым.");
            }

            return new Discipline
            {
                kod = kod,
                name = name,
                volume = volume,
                executor = executor
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Discipline newDiscipline = GetDiscipline();
                DatabaseManager.AddDiscipline(newDiscipline);
                MessageBox.Show("Дисциплина успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _parentPage.LoadDisciplineData();
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении дисциплины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}