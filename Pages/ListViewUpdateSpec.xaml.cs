using Npgsql;
using NpgsqlTypes;
using StudentSystem;
using StudentSystem.Pages;
using StudInfo.MVVM;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using static StudInfo.MsgBox;

namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewUpdateGroup.xaml
    /// </summary>
    public partial class ListViewUpdateSpec : Window
    {
        private string conn = String.Format("Server={0};Port={1};" +
        "User Id={2};Password={3};Database={4}",
        $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
        $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private DataTable dt;
        public DataGridStudents NS;
        private Spec _selectedSpec;
        private string _updateSql = @"call update_spec( cast(@specializations_id as int), cast(@specializations_title as varchar), cast(@specializations_fulltitle as varchar), cast(@specializations_code as varchar))";

        public ListViewUpdateSpec(Spec spec)
        {
            InitializeComponent();
            _selectedSpec = spec; // Сохраняем выбранного студента
            DataContext = new NewSpecViewModel();
            LoadSpecData(); // Загружаем данные в форму
        }
        private void LoadSpecData()
        {
            if (_selectedSpec != null)
            {
                SpecCodeTextBox.Text = _selectedSpec.SpecCode;
                SpecFullTitleTextBox.Text = _selectedSpec.FullTitle; // Прямое присвоение
                SpecTitleTextBox.Text = _selectedSpec.Title;

                // Установка выбранной группы в ComboBox
            }
            else
            {

            }

        }


        private void SaveNewSpecBtn_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection sqlConn = new NpgsqlConnection(conn);
            if (SpecCodeTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'Код специальности'", "Внимание", type: MessageBoxType.Info);
            }
            else if (SpecFullTitleTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'Полное наименование специальности'", "Внимание", type: MessageBoxType.Info);
            }
            else if (SpecTitleTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!!, 'Номер группы'", type: MessageBoxType.Info);
            }
            else
            {

                {
                    try
                    {

                        sqlConn.Open();

                        using (NpgsqlCommand cmd = new NpgsqlCommand(_updateSql, sqlConn))
                        {
                            cmd.Parameters.AddWithValue("@specializations_id", _selectedSpec.Id);
                            cmd.Parameters.AddWithValue("@specializations_title", SpecTitleTextBox.Text);
                            cmd.Parameters.AddWithValue("@specializations_fulltitle", SpecFullTitleTextBox.Text); // Исправлено: добавлен .Text
                            cmd.Parameters.AddWithValue("@specializations_code", SpecCodeTextBox.Text);

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SpecComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SpecComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SpecComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
