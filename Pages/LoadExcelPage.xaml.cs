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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using Npgsql;


namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadExcelPage.xaml
    /// </summary>
    /// 

    class TablesCB
    {
        public string text { get; set; }
        public string value { get; set; }
    }

    public partial class LoadExcelPage : System.Windows.Controls.Page
    {
        private DataTable dt;
        public LoadExcelPage()
        {
            InitializeComponent();
            var list = new List<TablesCB> 
            {
                new TablesCB{text="Студенты", value="students"},
                new TablesCB{text="Специальности", value="spec"},
                new TablesCB{text="Группы", value="groups"},
            };
            comboboxGroups.ItemsSource = list;
            comboboxGroups.DisplayMemberPath = "text";
            comboboxGroups.SelectedValuePath = "value";
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
                    dt = LoadExcelToDataTable(OPF.FileName);
                    dgvLoad.ItemsSource = dt.DefaultView;
                    foreach (DataGridColumn col in dgvLoad.Columns)
                    {
                       if(col.Header.ToString().Contains("Column"))
                       {
                            col.Visibility = Visibility.Hidden;
                       }
                    }
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
            var dataTable = new DataTable();

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                SharedStringTablePart stringTable = workbookPart.SharedStringTablePart;

                // Создаем колонки на основе первой строки
                Row headerRow = sheetData.Elements<Row>().First();
                foreach (Cell cell in headerRow.Elements<Cell>())
                {
                    string columnName = GetCellValue(cell, stringTable);
                    dataTable.Columns.Add(columnName);
                }

                // Читаем остальные строки (данные)
                foreach (Row row in sheetData.Elements<Row>().Skip(1))
                {
                    DataRow dataRow = dataTable.NewRow();
                    int columnIndex = 0;

                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        if (cell.CellValue != null)
                        {
                            string cellValue = GetCellValue(cell, stringTable);
                            dataRow[columnIndex] = cellValue;
                            columnIndex++;
                        }

                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }


        private void btn_LoadtoDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dataTable = dt;
                InsertDataIntoPostgres(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private string GetCellValue(Cell cell, SharedStringTablePart stringTable)
        {
            if (cell.DataType == null || cell.DataType.Value != CellValues.SharedString)
                return cell.CellValue?.Text ?? "";

            // Для значений из Shared String Table
            int index = int.Parse(cell.CellValue.Text);
            return stringTable.SharedStringTable.Elements<SharedStringItem>().ElementAt(index).InnerText;
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
                                switch (comboboxGroups.SelectedValue) 
                                {
                                    case "students":
                                        cmd.CommandText = @"call add_student(cast(@s_name as varchar), @group_id, cast(@s_card as varchar), cast(@dateborn as date), @s_desc, cast(@p_enrollment_date as date))";
                                        cmd.Parameters.AddWithValue("@s_name", row["students_name"].ToString());
                                        cmd.Parameters.AddWithValue("@group_id", int.Parse(row["group_id"].ToString()));
                                        cmd.Parameters.AddWithValue("@s_card", row["students_card"].ToString());
                                        cmd.Parameters.AddWithValue("@dateborn", row["students_dateborn"].ToString());
                                        cmd.Parameters.AddWithValue("@s_desc", row["students_desc"].ToString());
                                        cmd.Parameters.AddWithValue("@p_enrollment_date", row["enrollment_date"].ToString());
                                        break;
                                    case "groups":
                                        //g_spec_id, g_num, g_years
                                        cmd.CommandText = @"call add_group(@g_spec_id, @g_num, @g_years)";
                                        cmd.Parameters.AddWithValue("@g_spec_id", int.Parse(row["specializations_id"].ToString()));
                                        cmd.Parameters.AddWithValue("@g_num", int.Parse(row["group_num"].ToString()));
                                        cmd.Parameters.AddWithValue("@g_years", row["group_years"].ToString());
                                        break;
                                    case "spec":
                                        //spec_title, spec_fulltitle, spec_code
                                        cmd.CommandText = @"call add_spec(@spec_title, @spec_fulltitle, @spec_code)";
                                        cmd.Parameters.AddWithValue("@spec_title", row["specializations_title"].ToString());
                                        cmd.Parameters.AddWithValue("@spec_fulltitle", int.Parse(row["specializations_fulltitle"].ToString()));
                                        cmd.Parameters.AddWithValue("@spec_code", row["specializations_code"].ToString());
                                        break;
                                    default:
                                        MessageBox.Show("Нужно выбрать таблицу для загрузки");
                                        return;
                                }
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Данные успешно сохранены в PostgreSQL!");

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
