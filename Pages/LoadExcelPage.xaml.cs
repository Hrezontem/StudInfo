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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IronXL;
using Microsoft.Win32;
using Npgsql;

namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadExcelPage.xaml
    /// </summary>
    public partial class LoadExcelPage : Page
    {
        private DataTable dt;
        public LoadExcelPage()
        {
            InitializeComponent();
        }

        private string conn = String.Format("Server={0};Port={1};" +
    "User Id={2};Password={3};Database={4}",
    $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
    $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");

        private void ExcelLoadElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                Title = "Выберите файл Excel"
            };
            if (OPF.ShowDialog() == true)
            {

                try
                {
                    DataTable dataTable = LoadExcelToDataTable(OPF.FileName);
                    dgvLoad.ItemsSource = dataTable.DefaultView;
                    LoadData.Visibility = Visibility.Visible;
                    ExcelLoadElement.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }

            }
        }

        private DataTable LoadExcelToDataTable(string filePath)
        {
            var workBook = WorkBook.Load(filePath);
            var workSheet = workBook.DefaultWorkSheet;

            // Создаем DataTable вручную
            DataTable dataTable = new DataTable();

            // Читаем заголовки из первой строки
            var headers = workSheet.Rows[0].Columns
                           .Select(c => c.StringValue)
                           .ToArray();

            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            // Читаем данные, начиная со второй строки
            for (int i = 1; i < workSheet.Rows.Count(); i++)
            {
                var row = workSheet.Rows[i];
                DataRow dataRow = dataTable.NewRow();

                for (int j = 0; j < headers.Length; j++)
                {
                    dataRow[j] = row.Columns[j].StringValue;
                }

                dataTable.Rows.Add(dataRow);
            }
            dt = dataTable;
            return dataTable;
        }

        private void btn_LoadtoDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dataTable = dt;
                InsertDataIntoPostgres(dataTable);
                MessageBox.Show("Данные успешно сохранены в PostgreSQL!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void InsertDataIntoPostgres(DataTable dataTable)
        {
            using (var connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                // Используем транзакцию для ускорения массовой вставки
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            using (var cmd = new NpgsqlCommand())
                            {
                                cmd.Connection = connection;
                                cmd.Transaction = transaction;
                                cmd.CommandText = @"
                                INSERT INTO test.students (students_name, group_id, students_card, ""students_isStudies"", students_dateborn, students_desc)
                                VALUES (@s_name, @group_id, @s_card, @isStudies, cast(@dateborn as date), @s_desc)";
                                //s_name, group_id, s_card, isStudies, dateborn, s_desc
                                // Параметры (типы данных должны совпадать с PostgreSQL)
                                cmd.Parameters.AddWithValue("@s_name", row["students_name"].ToString());
                                cmd.Parameters.AddWithValue("@group_id", int.Parse(row["group_id"].ToString()));
                                cmd.Parameters.AddWithValue("@s_card", row["students_card"].ToString());
                                cmd.Parameters.AddWithValue("@isStudies", Boolean.Parse(row["students_isStudies"].ToString()));
                                cmd.Parameters.AddWithValue("@dateborn",  row["students_dateborn"].ToString());
                                cmd.Parameters.AddWithValue("@s_desc", row["students_desc"].ToString());

                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
