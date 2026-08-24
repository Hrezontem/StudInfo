    using Npgsql;
using StudentSystem;
using StudentSystem.Pages;
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
    /// Логика взаимодействия для ListViewNewGroup.xaml
    /// </summary>
    public partial class ListViewNewGroup : Window
    {
        private string conn = String.Format("Server={0};Port={1};" +
"User Id={2};Password={3};Database={4}",
$"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
$"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from specializations s";
        private DataTable dt;
        public ListViewNewGroup()
        {
            InitializeComponent();
            DataContext = new ListViewSpec();
            fill_combo();
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
        private void dateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и дефисы
            var regex = new Regex("[^0-9-]");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Обрабатываем изменение текста для форматирования
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
            var date = dateTextBox.Text.Split("-");
            // Перемещаем курсор в конец
            dateTextBox.SelectionStart = text.Length;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SpecComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveNewStudentBtn_Click(object sender, RoutedEventArgs e)
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
                sqlConn.Open();
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"specializations_fulltitle = '{SpecComboBox.Text.ToString()}'";


                foreach (var rowView in dv)
                {
                    if (rowView is DataRowView dataRow)
                    {
                        try
                        {
                            // Проверяем наличие столбца "group_id" в DataRowView
                            if (!dataRow.Row.Table.Columns.Contains("specializations_id"))
                            {
                                MsgBox.Show("Столбец 'specializations_id' не найден в источнике данных.");
                                return;
                            }

                            var specId = dataRow.DataView[0]["specializations_id"].ToString();

                            // Используем параметризованный запрос для безопасности
                            var sql = "call add_group(cast(@p_buildings_id as int), cast(@specializations_id as int), cast(@group_num as int), cast(@start_year as int), cast(@end_year as int), cast(@current_year as int))";

                            using (var npgsqlConnection = new NpgsqlConnection(conn))
                            using (var cmd = new NpgsqlCommand(sql, sqlConn))
                            {
                                var date = dateTextBox.Text.Split("-");
                                cmd.Parameters.AddWithValue("@p_buildings_id", CorpusTextBox.Text);
                                cmd.Parameters.AddWithValue("@specializations_id", specId);
                                cmd.Parameters.AddWithValue("@group_num", NumGroupTextBox.Text); // Исправлено: добавлен .Text
                                cmd.Parameters.AddWithValue("@start_year", date[0]);
                                cmd.Parameters.AddWithValue("@end_year", date[1]);
                                cmd.Parameters.AddWithValue("@current_year", 1);

                                cmd.ExecuteNonQuery();
                                sqlConn.Close();
                            }

                            MsgBox.Show("Добавлено", "Успешно", type: MessageBoxType.Success);
                            this.Close();

                        }
                        catch (Exception ex)
                        {
                            MsgBox.Show($"ОШИБКА: {ex.Message}", "Упс...", type: MessageBoxType.Error);
                            sqlConn.Close();
                        }
                    }
                }
            }
        }

        private void SpecComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SpecComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
