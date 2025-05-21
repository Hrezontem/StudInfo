using Npgsql;
using StudentSystem;
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

namespace StudInfo.Pages
{
    /// <summary>
    /// Interaction logic for ListViewNewStudent.xaml
    /// </summary>
    public partial class ListViewNewStudent : Window 
    {
        private string conn = String.Format("Server={0};Port={1};" +
"User Id={2};Password={3};Database={4}",
$"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
$"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from groups_select()";
        private DataTable dt;

        public ListViewNewStudent()
        {
            InitializeComponent();
            DataContext = new NewStudentViewModel();
            fill_combo();
        }

        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            
        }



        private void fill_combo()
        {
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
            {
                sqlConn.Open();
                NpgsqlCommand sqlCmd = new NpgsqlCommand(sql, sqlConn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlCmd);
                dt = new DataTable();
                da.Fill(dt);
                GroupComboBox.ItemsSource = dt.DefaultView;
                GroupComboBox.DisplayMemberPath = "title";
            }
        }

        private void GroupComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void SaveNewStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            bool StStudies = true;
            NpgsqlConnection sqlConn = new NpgsqlConnection(conn);
            dt.AsDataView();
            if (StudNameTextBox.Text == "")
            {
                MessageBox.Show("Не заполненное поле!!! 'ФИО'");
            }
            else if (GroupComboBox.Text == "")
            {
                MessageBox.Show("Не заполненное поле!!! 'Группа'");
            }
            else if (NumberStudBiletTextBox.Text == "")
            {
                MessageBox.Show("Не заполненное поле!!!, 'Студенческий билет'");
            }
            else if(DateOfBirthStud.Text == "")
            {
                MessageBox.Show("Не заполненное поле!!!, 'Дата рождения'");
            }
            else
            {
                sqlConn.Open();
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"title = '{GroupComboBox.Text.ToString()}'";


                foreach (var rowView in dv)
                {       
                    if (rowView is DataRowView dataRow)
                    {
                        try
                        {
                            // Проверяем наличие столбца "group_id" в DataRowView
                            if (!dataRow.Row.Table.Columns.Contains("id"))
                            {
                                MessageBox.Show("Столбец 'group_id' не найден в источнике данных.");
                                return;
                            }
                            
                            var groupId = dataRow["id"].ToString();

                            // Используем параметризованный запрос для безопасности
                            var sql = "call add_student(cast(@students_name as varchar), cast(@group_id as int), cast(@students_card as varchar), @students_isStudies, cast(@students_dateborn as date), cast(@students_desc as varchar))";

                            using (var npgsqlConnection = new NpgsqlConnection(conn))
                            using (var cmd = new NpgsqlCommand(sql, sqlConn))
                            {
                                cmd.Parameters.AddWithValue("@students_name", StudNameTextBox.Text.ToCharArray()); // Исправлено: добавлен .Text
                                cmd.Parameters.AddWithValue("@group_id", groupId);
                                cmd.Parameters.AddWithValue("@students_card", NumberStudBiletTextBox.Text.ToCharArray());
                                cmd.Parameters.AddWithValue("@students_isStudies", StStudies);
                                cmd.Parameters.AddWithValue("@students_dateborn", DateOfBirthStud.Text);
                                cmd.Parameters.AddWithValue("@students_desc", NoteStudTextBox.Text.ToCharArray());

                                cmd.ExecuteNonQuery();
                                sqlConn.Close();
                            }

                            MessageBox.Show("Добавлено");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"ОШИБКА: {ex.Message}");
                            sqlConn.Close();
                        }
                    }
                }
            }
    }
        
    }
}




