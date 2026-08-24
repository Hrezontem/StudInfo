using Npgsql;
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
    public partial class ListViewNewStudent : Window 
    {
        private string conn = String.Format("Server={0};Port={1};" +
"User Id={2};Password={3};Database={4}",
$"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
$"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        
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

        private void SaveNewStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            bool StStudies = true;
            NpgsqlConnection sqlConn = new NpgsqlConnection(conn);
            dt.AsDataView();
            if (StudNameTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'ФИО'", "Внимание", type: MessageBoxType.Info);
            }
            else if (GroupComboBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'Группа'", "Внимание", type: MessageBoxType.Info);
            }
            else if (NumberStudBiletTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!!, 'Студенческий билет'", "Внимание", type: MessageBoxType.Info);
            }
            else if(DateOfBirthStud.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!!, 'Дата рождения'", "Внимание", type: MessageBoxType.Info);
            }
            else
            {
                sqlConn.Open();
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"group_name = '{GroupComboBox.Text.ToString()}'";


                foreach (var rowView in dv)
                {       
                    if (rowView is DataRowView dataRow)
                    {
                        try
                        {
                            // Проверяем наличие столбца "group_id" в DataRowView
                            if (!dataRow.Row.Table.Columns.Contains("group_id"))
                            {
                                MessageBox.Show("Столбец 'group_id' не найден в источнике данных.");
                                return;
                            }
                            
                            var groupId = dataRow["group_id"].ToString();

                            // Используем параметризованный запрос для безопасности
                            var sql = "call add_student(cast(@students_name as varchar), cast(@group_id as int), cast(@students_card as varchar),cast(@students_dateborn as date), cast(@students_desc as varchar), cast('2004-01-01' as date))";

                            using (var npgsqlConnection = new NpgsqlConnection(conn))
                            using (var cmd = new NpgsqlCommand(sql, sqlConn))
                            {
                                cmd.Parameters.AddWithValue("@students_name", StudNameTextBox.Text); // Исправлено: добавлен .Text
                                cmd.Parameters.AddWithValue("@group_id", groupId);
                                cmd.Parameters.AddWithValue("@students_card", NumberStudBiletTextBox.Text);
                                cmd.Parameters.AddWithValue("@students_isStudies", StStudies);
                                cmd.Parameters.AddWithValue("@students_dateborn", DateOfBirthStud.Text);
                                cmd.Parameters.AddWithValue("@students_desc", NoteStudTextBox.Text);
                                cmd.ExecuteNonQuery();
                                sqlConn.Close();
                            }

                            MsgBox.Show("Добавлено", "Успех!", type: MessageBoxType.Success);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Show($"ОШИБКА: {ex.Message}", "Ошибка", type: MessageBoxType.Error);
                            sqlConn.Close();
                        }
                    }
                }
            }
    }
        
    }
}




