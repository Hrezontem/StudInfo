
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataGridStudents.xaml
    /// </summary>
    public partial class DataGridStudents : Page
    {

        //private string connstring = String.Format("Server={0};Port={1};" +
        //"User Id={2};Password={3};Database={4}",
        //$"{Properties.Settings.Default.address_base}", $"{Properties.Settings.Default.port_base}", $"{Properties.Settings.Default.login_base}",
        //$"{Properties.Settings.Default.password_base}", $"{Properties.Settings.Default.name_base}");



        private ObservableCollection<Student> students;
        public DataGridStudents()
        {
            students = new StudentViewModel().Students;

            InitializeComponent();
            dgvStudents.ItemsSource = students;

        }

        private void dgvStudents_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void searchTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
    }
}
