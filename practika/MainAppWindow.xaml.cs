using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using practika.BD;

namespace practika
{
    public partial class MainAppWindow : Window
    {
        private string userRole;
        private Department userDepartment;

        public MainAppWindow(string role, Department department)
        {
            InitializeComponent();
            userRole = role;
            userDepartment = department;
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            Style navigationButtonStyle = new Style(typeof(TextBlock))
            {
                Setters =
                {
                    new Setter(TextBlock.ForegroundProperty, Brushes.White),
                    new Setter(TextBlock.FontSizeProperty, 18.0),
                    new Setter(TextBlock.MarginProperty, new Thickness(15)),
                    new Setter(TextBlock.CursorProperty, Cursors.Hand)
                }
            };

            AddActionButton("Выйти", () =>
            {
                var authWindow = new AuthWindow();
                authWindow.Show();
                this.Close();
            }, navigationButtonStyle);

            switch (userRole)
            {
                case Role.Инженер:
                    AddNavigationButton("Кафедра", new DepartmentPage(userRole, userDepartment), navigationButtonStyle);
                    AddNavigationButton("Предметы", new DisciplinePage(userRole, userDepartment), navigationButtonStyle);
                    AddNavigationButton("Экзамены", new ExamPage(userRole, userDepartment), navigationButtonStyle);
                    AddNavigationButton("Преподаватели", new TeacherPage(userRole, userDepartment), navigationButtonStyle);
                    break;
                case Role.ЗавКафедрой:
                    AddNavigationButton("Предметы", new DisciplinePage(userRole, userDepartment), navigationButtonStyle);
                    AddNavigationButton("Преподаватели", new TeacherPage(userRole, userDepartment), navigationButtonStyle);
                    break;
                case Role.Преподаватель:
                    AddNavigationButton("Предметы", new DisciplinePage(userRole, userDepartment), navigationButtonStyle);
                    AddNavigationButton("Экзамены", new ExamPage(userRole, userDepartment), navigationButtonStyle);
                    break;
            }
        }

        private void AddNavigationButton(string text, Page page, Style style)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                Style = style
            };
            textBlock.PreviewMouseLeftButtonUp += (sender, e) => Nav_PreviewMouseLeftButtonUp(sender, e, page);
            ((StackPanel)this.FindName("NavigationPanel")).Children.Add(textBlock);
        }

        private void AddActionButton(string text, Action action, Style style)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                Style = style
            };
            textBlock.PreviewMouseLeftButtonUp += (sender, e) => action();
            ((StackPanel)this.FindName("NavigationPanel")).Children.Add(textBlock);
        }

        private void Nav_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e, Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}