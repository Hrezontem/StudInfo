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
    public partial class ListViewUpdateSpec : Window
    {
        private string conn = String.Format("Server={0};Port={1};" +
        "User Id={2};Password={3};Database={4}",
        $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
        $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from groups_select()";
        private DataTable dt;
        public ListViewSpec S;

        public ListViewUpdateSpec()
        {
            InitializeComponent();
            //DataContext = new ListViewNewSpec();

            //LoadSpecData();

        }

        //public ListViewUpdateSpec(ListViewSpec spec)
        //{
        //    InitializeComponent();
        //    _selectedSpec = spec; // Сохраняем выбранного студента
        //    DataContext = new ViewModelNewSpec();

        //    LoadSpecData(); // Загружаем данные в форму
        //}

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }

        //public void ComboBox_Loaded(object sender, RoutedEventArgs e)
        //{

        //}

        //private Student _selectedSpec;
        //private string _updateSql = @"call update_spec(cast(@id as int),  cast(@students_name as varchar), cast(@group_id as int), cast(@students_card as varchar), @students_isStudies, cast(@students_dateborn as date), cast(@students_desc as varchar))";



        private void GroupComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void LoadSpecData()
        {
            //if (_selectedSpec != null)
            //{
            //    SpecCodeTextBox.Text = _selectedSpec.;
            //    SpecFullTitleTextBox.Text = _selectedSpec.SpecFullTitle; // Прямое присвоение
            //    SpecTitleTextBox.Text = _selectedSpec.SpecTitle;

            //    // Установка выбранной группы в ComboBox
            //}
            //else
            //{

            //}

        }

        private void SaveUpdateSpecBtn_Click(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
            {
                dt.AsDataView();
                if (SpecCodeTextBox.Text == "")
                {
                    MsgBox.Show("Не заполненное поле!!! 'Код специальности'", type: MessageBoxType.Warning);
                }
                else if (SpecFullTitleTextBox.Text == "")
                {
                    MsgBox.Show("Не заполненное поле!!! 'Полное наименование специальности'", type: MessageBoxType.Warning);
                }
                else if (SpecTitleTextBox.Text == "")
                {
                    MsgBox.Show("Не заполненное поле!!!, 'Группа специальности'", type: MessageBoxType.Warning);
                }
                else
                {
                    try
                    {
                        // Используем параметризованный запрос для безопасности
                        var sql = "call update_spec(cast(@specilizations_title as varchar), cast(@specilizations_fulltitle as varchar), cast(@specilizations_code as varchar))";

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
                        MsgBox.Show("Добавлено", type: MessageBoxType.Success);
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
}




