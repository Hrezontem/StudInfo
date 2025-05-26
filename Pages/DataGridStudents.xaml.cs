
using Microsoft.VisualBasic;
using Npgsql;
using StudInfo;
using StudInfo.Pages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using static StudInfo.MsgBox;

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
        private string conn = String.Format("Server={0};Port={1};" +
"User Id={2};Password={3};Database={4}",
$"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
$"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private DataTable dt;
        private Student selectedStudent;
        private NpgsqlConnection sqlConn;
        private string sql;
        NpgsqlCommand cmd = new NpgsqlCommand();
        SystemWindow window;
        ListViewUpdateStudent updateWindow;
        ListViewNewStudent NewStWindow;
        private int rowIndex = -1;
        private ObservableCollection<Student> _students;
        private ICollectionView _collectionView;
        public DataGridStudents()
        {
            InitializeComponent();
            _students = new StudentViewModel().Students;
            InitializeCollectionView();



        }
        private void InitializeCollectionView()
        {
            _collectionView = CollectionViewSource.GetDefaultView(_students);
            dgvStudents.ItemsSource = _collectionView;
        }
        public void LoadTable()
        {
            _students = new StudentViewModel().Students;
            dgvStudents.ItemsSource = _students;
            dgvStudents.Columns[0].Visibility = Visibility.Hidden;
            dgvStudents.Columns[5].Visibility = Visibility.Hidden;
          
            
        }
        private void dgvStudents_Loaded(object sender, RoutedEventArgs e)
        {
            dgvStudents.Columns[0].Visibility = Visibility.Hidden;
            dgvStudents.Columns[5].Visibility = Visibility.Hidden;
            window = Application.Current.MainWindow as SystemWindow;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var searchText = searchTextBox.Text.ToLower();

            if (searchText != "поиск..." && dgvStudents.ItemsSource != null) 
            {
                _collectionView.Filter = item =>
                {
                    if (string.IsNullOrWhiteSpace(searchText)) return true;

                    var type = item.GetType();
                    var properties = type.GetProperties();

                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(item)?.ToString();
                        if (value?.ToLower().Contains(searchText) == true)
                        {
                            return true;
                        }
                    }
                    return false;
                };
            }

        }

        private void searchTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void CMUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InformationLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            selectedStudent = dgvStudents.SelectedItem as Student;
            updateWindow = new ListViewUpdateStudent(selectedStudent);
            updateWindow.SaveNewStudentBtn.Visibility = Visibility.Hidden;
            updateWindow.SaveNewStudentBtn.IsEnabled = false;
            updateWindow.BackBtn.HorizontalAlignment = HorizontalAlignment.Center;
            updateWindow.BackBtn.VerticalAlignment = VerticalAlignment.Center;
            updateWindow.BackBtn.Margin = new Thickness(0);
            updateWindow.GroupComboBox.IsEnabled = false;
            updateWindow.NoteStudTextBox.IsEnabled = false;
            updateWindow.NumberStudBiletTextBox.IsEnabled = false;
            updateWindow.StudNameTextBox.IsEnabled = false;
            updateWindow.Closing += ListViewUpdateStudent_Closing;
            if (selectedStudent != null)
            {
                
                updateWindow.NS = this;
                updateWindow.Show();
                
            }
            else
            {
                MsgBox.Show("Выберите студента!.");
            }
            
        }

        public void ListViewUpdateStudent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadTable();
        }
        public void ListViewNewStudent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadTable();
        }

        private void UpdateLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            selectedStudent = dgvStudents.SelectedItem as Student;
            updateWindow = new ListViewUpdateStudent(selectedStudent);
            updateWindow.Closing += ListViewUpdateStudent_Closing;
            if (selectedStudent != null)
            {
                updateWindow.Show();
            }
            else
            {
                MessageBox.Show("Выберите студента для редактирования.");
            }
        }

        private void DismissStudentLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void DeleteStudentLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var result = MessageBox.Show(
            "Вы уверены, что хотите удалить студента?",
            "ВНИМАНИЕ",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (Student student in dgvStudents.SelectedItems.Cast<Student>().ToList())
                {
                    try
                    {
                        if (student != null && student.Id > 0) // Проверяем ID
                        {
                            using (var sqlConn = new NpgsqlConnection(conn))
                            {
                                sqlConn.Open();
                                var cmd = new NpgsqlCommand($"call delete_student({student.Id})", sqlConn);
                                cmd.ExecuteNonQuery();
                            }

                            MsgBox.Show("Удалено успешно", "Успех",
                                          type: MessageBoxType.Success);
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show($"Ошибка доступа. Ошибка: {ex.Message}", type: MessageBoxType.Error);
                    }
                }
                LoadTable();
            }
        }

        private void dgvStudents_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void dgvStudents_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            selectedStudent = dgvStudents.SelectedItem as Student;
            updateWindow = new ListViewUpdateStudent(selectedStudent);
            updateWindow.SaveNewStudentBtn.Visibility = Visibility.Hidden;
            updateWindow.SaveNewStudentBtn.IsEnabled = false;
            updateWindow.BackBtn.HorizontalAlignment = HorizontalAlignment.Center;
            updateWindow.BackBtn.VerticalAlignment = VerticalAlignment.Center;
            updateWindow.BackBtn.Margin = new Thickness(0);
            updateWindow.GroupComboBox.IsEnabled = false;
            updateWindow.NoteStudTextBox.IsEnabled = false;
            updateWindow.NumberStudBiletTextBox.IsEnabled = false;
            updateWindow.StudNameTextBox.IsEnabled = false;
            updateWindow.Closing += ListViewUpdateStudent_Closing;
            if (selectedStudent != null)
            {
                updateWindow.Show();
                
            }
            else
            {
                MsgBox.Show("Выберите студента!.");
            }
        }
    }
}
