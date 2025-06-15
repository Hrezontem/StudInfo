using Npgsql;
using NpgsqlTypes;
using StudentSystem;
using StudentSystem.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static StudInfo.MsgBox;

namespace StudInfo.Pages
{
    /// <summary>
    /// Interaction logic for ListViewNewStudent.xaml
    /// </summary>
    public partial class ListViewUpdateStudent : Window
    {
        private string conn = String.Format("Server={0};Port={1};" +
        "User Id={2};Password={3};Database={4}",
        $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
        $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private DataTable dt;
        public DataGridStudents NS;


        public ListViewUpdateStudent(Student student)
        {
            /*
                                         <ComboBoxItem Content="Учится" Name="Учится"/>
                            <ComboBoxItem Content="Отчислен"/>
                            <ComboBoxItem Content="Выпущен" Name="Выпущен"/>
             */
            InitializeComponent();
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
            {
                sqlConn.Open();
                NpgsqlCommand sqlCmd = new NpgsqlCommand("select * from status", sqlConn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlCmd);
                dt = new DataTable();
                da.Fill(dt);
                cboxStatus.ItemsSource = dt.DefaultView;
                cboxStatus.DisplayMemberPath = "status_name";
                cboxStatus.SelectedValuePath = "status_title";
            }
            _selectedStudent = student; // Сохраняем выбранного студента
            DataContext = new NewStudentViewModel();
            fill_combo();
            LoadStudentData(); // Загружаем данные в форму

        }

        public ListViewUpdateStudent(StudentHistory studentHistory)
        {
            /*
                                         <ComboBoxItem Content="Учится" Name="Учится"/>
                            <ComboBoxItem Content="Отчислен"/>
                            <ComboBoxItem Content="Выпущен" Name="Выпущен"/>
             */
            InitializeComponent();
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
            {
                sqlConn.Open();
                NpgsqlCommand sqlCmd = new NpgsqlCommand("select * from status", sqlConn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlCmd);
                dt = new DataTable();
                da.Fill(dt);
                cboxStatus.ItemsSource = dt.DefaultView;
                cboxStatus.DisplayMemberPath = "status_name";
                cboxStatus.SelectedValuePath = "status_title";
            }
            _selectedStudentHistory = studentHistory; // Сохраняем выбранного студента
            DataContext = new NewStudentViewModel();
            fill_combo();
            LoadStudentData(); // Загружаем данные в форму

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        public void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private StudentHistory _selectedStudentHistory;
        private Student _selectedStudent;
        private string _updateSql = @"call update_student(cast(@id as int),  cast(@students_name as varchar), cast(@group_id as int), cast(@students_card as varchar), cast(@students_dateborn as date),cast(@status as varchar) ,cast(@students_desc as varchar))";
        /*
                                                                                 IN p_student_id integer, 
                                                                        IN p_name character varying, 
                                                                        IN p_group_id integer, 
                                                                        IN p_card character varying, 
                                                                        IN p_dateborn date, 
                                                                        IN p_desc text, 
                                                                        IN p_status character varying, 
                                                                        IN p_enrollment_date date)
         */
        private void fill_combo()
        {
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
            {
                sqlConn.Open();
                NpgsqlCommand sqlCmd = new NpgsqlCommand("select group_id, group_name from group_display", sqlConn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlCmd);
                dt = new DataTable();
                da.Fill(dt);
                GroupComboBox.ItemsSource = dt.DefaultView;
                GroupComboBox.DisplayMemberPath = "group_name";
                GroupComboBox.SelectedValuePath = "group_id";
            }
        }

        private void GroupComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void LoadStudentData()
        {
            if (_selectedStudent != null)
            {
                StudNameTextBox.Text = _selectedStudent.Name;
                DateOfBirthStud.Text = _selectedStudent.DateBirth.ToString(); // Прямое присвоение
                GroupComboBox.Text = _selectedStudent.Group;
                NumberStudBiletTextBox.Text = _selectedStudent.Card;
                NoteStudTextBox.Text = _selectedStudent.Description;
                cboxStatus.Text = _selectedStudent.Status;

                // Установка выбранной группы в ComboBox
                foreach (DataRowView item in GroupComboBox.Items)
                {
                    if (item["group_id"].ToString() == _selectedStudent.Id.ToString())
                    {
                        GroupComboBox.SelectedItem = item;
                        break;
                    }
                }

                foreach (DataRowView item in cboxStatus.Items)
                {
                    if (item["status_title"].ToString() == _selectedStudent.Status.ToString())
                    {
                        cboxStatus.SelectedItem = item;
                        break;
                    }
                }
            }
            else 
            {
                
            }
           
        }

        private void SaveNewStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
            {
                bool StStudies = true;
                dt.AsDataView();
                if (StudNameTextBox.Text == "")
                {
                    MsgBox.Show("Не заполненное поле!!! 'ФИО'", type: MessageBoxType.Warning);
                }
                else if (GroupComboBox.Text == "")
                {
                    MsgBox.Show("Не заполненное поле!!! 'Группа'", type: MessageBoxType.Warning);
                }
                else if (NumberStudBiletTextBox.Text == "")
                {
                    MsgBox.Show("Не заполненное поле!!!, 'Студенческий билет'", type: MessageBoxType.Warning);
                }
                else if (DateOfBirthStud.Text == "")
                {
                    MsgBox.Show("Не заполненное поле!!!, 'Дата рождения'", type: MessageBoxType.Warning);
                }
                else
                {
                    try
                    {
                        sqlConn.Open();
                        //DataRowView selectedGroup = (DataRowView);

                        using (NpgsqlCommand cmd = new NpgsqlCommand(_updateSql, sqlConn))
                        {
                            cmd.Parameters.AddWithValue("@id", _selectedStudent.Id);
                            cmd.Parameters.AddWithValue("@students_name", StudNameTextBox.Text);
                            cmd.Parameters.AddWithValue("@group_id", GroupComboBox.SelectedValue);
                            cmd.Parameters.AddWithValue("@students_card", NumberStudBiletTextBox.Text);
                            cmd.Parameters.AddWithValue("@students_dateborn", NpgsqlDbType.Date, DateOfBirthStud.SelectedDate);
                            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, cboxStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@students_desc", NoteStudTextBox.Text);

                            cmd.ExecuteNonQuery();
                            MsgBox.Show("Данные обновлены!", type: MessageBoxType.Success);
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show($"Ошибка: {ex.Message}", type: MessageBoxType.Error);
                    }
                }
            }
        }
    }
}




