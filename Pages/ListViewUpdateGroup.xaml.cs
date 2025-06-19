using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Npgsql;
using StudentSystem;
using StudentSystem.Pages;
using StudInfo.MVVM;
using static StudInfo.MsgBox;

namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewUpdateGroup.xaml
    /// </summary>
    public partial class ListViewUpdateGroup : Window
    {
        private string conn = String.Format("Server={0};Port={1};" +
        "User Id={2};Password={3};Database={4}",
        $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
        $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private DataTable dt;
        public DataGridStudents NS;
        private Groups _selectedGroup;
        private string sql = @"select * from specializations s";

        public ListViewUpdateGroup(Groups group)
        {
            InitializeComponent();
            _selectedGroup = group; // Сохраняем выбранного студента
            DataContext = new NewGroupViewModel();
            fill_combo();
            LoadGroupData(); // Загружаем данные в форму
        }
        private void LoadGroupData()
        {
            if (_selectedGroup != null)
            {
                var num  = _selectedGroup.Title.Split("-");
                NumGroupTextBox.Text = _selectedGroup.GroupNum;
                SpecComboBox.Text = _selectedGroup.FullTitle; // Прямое присвоение
                dateTextBox.Text = _selectedGroup.StartYear + _selectedGroup.EndYear;
                currentYearTextBox.Text = _selectedGroup.CurrentYear;

                // Установка выбранной группы в ComboBox
                foreach (DataRowView item in SpecComboBox.Items)
                {
                    if (item["specializations_id"].ToString() == _selectedGroup.FullTitle.ToString())
                    {
                        SpecComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {

            }

        }

        private void dateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и дефисы
            var regex = new Regex("[^0-9-]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void dateTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text = dateTextBox.Text;

            // Убираем все, что не является цифрами или дефисами
            text = new string(text.Where(c => char.IsDigit(c) || c == '-').ToArray());

            // Форматируем в "yyyy-yyyy" (если возможно)
            if (text.Length > 4 && text[4] != '-')
            {
                text = text.Insert(4, "-");
            }

            // Ограничиваем длину до двух частей по 4 символа
            if (text.Length > 9)
            {
                text = text.Substring(0, 9);
            }

            // Обновляем текст в TextBox
            dateTextBox.Text = text;

            // Перемещаем курсор в конец
            dateTextBox.SelectionStart = text.Length;
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
                SpecComboBox.ItemsSource = dt.DefaultView;
                SpecComboBox.DisplayMemberPath = "specializations_fulltitle";
            }
        }


        private void SaveGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection sqlConn = new NpgsqlConnection(conn);
            dt.AsDataView();
            if (SpecComboBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'Специальность'", "Внимание", type: MessageBoxType.Info);
            }
            else if (dateTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!! 'Даты обучения'", "Внимание", type: MessageBoxType.Info);
            }
            else if (NumGroupTextBox.Text == "")
            {
                MsgBox.Show("Не заполненное поле!!!, 'Номер группы'", type: MessageBoxType.Info);
            }
            else
            {

                {
                    try
                    {


                        sqlConn.Open();
                        DataRowView selectedSpec = (DataRowView)SpecComboBox.SelectedItem;

                        using (NpgsqlCommand cmd = new NpgsqlCommand("call update_group(cast(@group_id as int), cast(@buildings_id as int), cast(@specializations_id as int), cast(@group_num as int), cast(@start_year as int) , cast(@end_year as int), cast(@current_year as int))", sqlConn))
                        {
                            var years = dateTextBox.Text.Split("-");
                            cmd.Parameters.AddWithValue("@group_id", _selectedGroup.Id);
                            cmd.Parameters.AddWithValue("@buildings_id", _selectedGroup.Id);
                            cmd.Parameters.AddWithValue("@specializations_id", selectedSpec["specializations_id"]);
                            cmd.Parameters.AddWithValue("@group_num", NumGroupTextBox.Text); // Исправлено: добавлен .Text
                            cmd.Parameters.AddWithValue("@start_year", years[0]);
                            cmd.Parameters.AddWithValue("@end_year", years[1]);
                            cmd.Parameters.AddWithValue("@current_year", currentYearTextBox.Text);
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
    }
}
