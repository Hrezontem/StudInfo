using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewStudents.xaml
    /// </summary>
    public partial class ListViewStudents : Page
    {


        public ListViewStudents()
        {
            InitializeComponent();
            DataContext = new StudentViewModel();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
