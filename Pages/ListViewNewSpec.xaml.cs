using Npgsql;
using StudentSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static StudInfo.MsgBox;

namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewNewSpec.xaml
    /// </summary>
    public partial class ListViewNewSpec : Window
    {
        private string conn = String.Format("Server={0};Port={1};" +
       "User Id={2};Password={3};Database={4}",
       $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
       $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from test.groups_display";
        private DataTable dt;


        public ListViewNewSpec()
        {
            InitializeComponent();
            DataContext = new NewStudentViewModel();
            fill_dt();
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



        private void fill_dt()
        {
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
            {
                sqlConn.Open();
                NpgsqlCommand sqlCmd = new NpgsqlCommand(sql, sqlConn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlCmd);
                dt = new DataTable();
                da.Fill(dt);
                sqlConn.Close();


            }
        }

        private void GroupComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SaveNewSpecBtn_Click(object sender, RoutedEventArgs e)
        {
            bool StStudies = true;
            NpgsqlConnection sqlConn = new NpgsqlConnection(conn);
            dt.AsDataView();
            if (SpecFullTitleTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'Полное наименование специальности'", "Внимание" , type: MessageBoxType.Info);
            }
            else if (SpecTitleTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'Наиманование группы'", "Внимание", type: MessageBoxType.Info);
            }
            else if (SpecCodeTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!!, 'Код специальности'", "Внимание", type: MessageBoxType.Info);
            }
            else
            {       
                        try
                        {
                            // Используем параметризованный запрос для безопасности
                            var sql = "call test.add_spec(cast(@specilizations_title as varchar), cast(@specilizations_fulltitle as varchar), cast(@specilizations_code as varchar))";

                            using (var npgsqlConnection = new NpgsqlConnection(conn))
                            using (var cmd = new NpgsqlCommand(sql, sqlConn))
                            {
                                sqlConn.Open();
                                cmd.Parameters.AddWithValue("@specilizations_title", SpecTitleTextBox.Text.ToCharArray()); // Исправлено: добавлен .Text
                                cmd.Parameters.AddWithValue("@specilizations_fulltitle", SpecFullTitleTextBox.Text.ToCharArray());
                                cmd.Parameters.AddWithValue("@specilizations_code", SpecCodeTextBox.Text.ToCharArray());
                                cmd.ExecuteNonQuery();
                                sqlConn.Close();

                            }
                            MsgBox.Show("Добавлено",  type: MessageBoxType.Success);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Show($"ОШИБКА: {ex.Message}", type: MessageBoxType.Error);
                            sqlConn.Close();
                        }
            }
        }
    }
}
