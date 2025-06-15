
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.Win32;
using StudentSystem.Pages;
using StudInfo;
using StudInfo.Pages;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Collections;
using static StudInfo.MsgBox;
using Npgsql;
namespace StudentSystem
{
    /// <summary>
    /// Логика взаимодействия для SystemWindow.xaml
    /// </summary>
    public partial class SystemWindow : Window
    {
        private string conn = String.Format("Server={0};Port={1};" +
"User Id={2};Password={3};Database={4}",
$"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
$"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private double windowHeight = 0;
        public DataTable dt;
        private ICollectionView _collectionView;
        private DataGridStudents StudentsPage;
        private ListViewGroup GroupsPage;
        private ListViewSpec SpecsPage;
        public SystemWindow()
        {

            InitializeComponent();
            // button.BeginAnimation(Button.WidthProperty, buttonAnimation);
            //if (SystemWindow.ShowActivatedProperty.Properties.Settings.Default.login_base == "client_students")
            //{
            //    UserIndicator.Text = "Клиент";
            //    BTNInsertST.Visible = false;
            //    jToolStripMenuItem.Visible = false;
            //    contextMenuStrip1.Enabled = false;
            //    contextMenuStrip2.Enabled = false;
            //    CMSChangeGroup.Enabled = false;
            //}
            //else
            //{
            //    UserIndicator.Text = "Админ";

            //}
            StudentsPage = new DataGridStudents();
            GroupsPage = new ListViewGroup();
            SpecsPage = new ListViewSpec();
            fContainer.Navigate(new System.Uri("Pages/MainStatisticPage.xaml", UriKind.RelativeOrAbsolute));
            SettingsLabel.Visibility = Visibility.Visible;
            CreateNewStudentBtn.Visibility = Visibility.Hidden;
            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden; 
            CreateNewStudentBtn.Visibility = Visibility.Hidden;

            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden;
            Next_year.Visibility = Visibility.Hidden;
            SettingsLabel.Visibility = Visibility.Hidden;
            
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) 
            {
                try 
                {
                    this.DragMove();
                }
                catch { }
            }

        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {

            /**DataView dv = DefaultView;
            dv.RowFilter = $" LIKE '" + searchTextBox.Text + "%'";
            dgvStudents.DataSource = dv;**/
        }



        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMenuAnimation(RadioButton sender)
        {

        }

        private void btnSize_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            
            if (btn.IsChecked == false)
            {
                // Exit fullscreen
                this.ResizeMode = ResizeMode.CanResize;
                this.WindowState = WindowState.Normal;
                this.MaxHeight = windowHeight;
            }
            else
            {
                // Enter fullscreen
                windowHeight = this.MaxHeight;
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Normal;
                this.WindowState = WindowState.Maximized;
                this.MaxHeight = this.Height - 40;

            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CreateNewStudentBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewNewStudent NS = new ListViewNewStudent();
            NS.Show();
        }

        private void NewSpecBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewNewSpec NSpec = new ListViewNewSpec();
            NSpec.Show();
        }

        private void NewGroupBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewNewGroup NGroup = new ListViewNewGroup();
            NGroup.Show();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewSettings.xaml", UriKind.RelativeOrAbsolute));
            SettingsLabel.Visibility = Visibility.Visible;
            CreateNewStudentBtn.Visibility = Visibility.Hidden;
            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden;
            Next_year.Visibility = Visibility.Hidden;
            LoadXlsxBtn.Visibility = Visibility.Visible;
        }

        private void ExcelLoadMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/LoadExcelPage.xaml", UriKind.RelativeOrAbsolute));
            CreateNewStudentBtn.Visibility = Visibility.Hidden;
            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden;
            SettingsLabel.Visibility = Visibility.Hidden;
            LoadXlsxBtn.Visibility = Visibility.Hidden;
            Next_year.Visibility = Visibility.Hidden;
        }

        private void SpecMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(SpecsPage);
            CreateNewStudentBtn.Visibility = Visibility.Visible;
            NewGroupBtn.Visibility = Visibility.Visible;
            NewSpecBtn.Visibility = Visibility.Visible;
            SettingsLabel.Visibility = Visibility.Hidden;
            Next_year.Visibility = Visibility.Visible;
            LoadXlsxBtn.Visibility = Visibility.Visible;
        }

        private void GroupMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(GroupsPage);
            CreateNewStudentBtn.Visibility = Visibility.Visible;
            NewGroupBtn.Visibility = Visibility.Visible;
            NewSpecBtn.Visibility = Visibility.Visible;
            SettingsLabel.Visibility = Visibility.Hidden;
            Next_year.Visibility = Visibility.Visible;
            LoadXlsxBtn.Visibility = Visibility.Visible;
        }

        private void StudentsMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(StudentsPage, this);
            CreateNewStudentBtn.Visibility = Visibility.Visible;
            NewGroupBtn.Visibility = Visibility.Visible;
            NewSpecBtn.Visibility = Visibility.Visible;
            SettingsLabel.Visibility = Visibility.Hidden;
            Next_year.Visibility = Visibility.Visible;
            LoadXlsxBtn.Visibility = Visibility.Visible;
        }

        private void fContainer_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(new System.Uri("Pages/MainStatisticPage.xaml", UriKind.RelativeOrAbsolute));
            CreateNewStudentBtn.Visibility = Visibility.Hidden;
            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden;
            CreateNewStudentBtn.Visibility = Visibility.Hidden;
            Next_year.Visibility = Visibility.Hidden;
            LoadXlsxBtn.Visibility = Visibility.Visible;
        }

        public static DataTable ItemsSourceToDataTable(IEnumerable items)
        {
            if (items == null) return new DataTable();

            var table = new DataTable();
            var enumerable = items as object[] ?? items.Cast<object>().ToArray();

            // Создаем колонки на основе свойств первого элемента
            if (enumerable.Any())
            {
                Type itemType = enumerable.First().GetType();
                foreach (PropertyInfo prop in itemType.GetProperties())
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }

            // Добавляем строки
            foreach (var item in enumerable)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo prop in item.GetType().GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item, null) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
        public static void ExportToXlsx(DataTable dataTable, string filePath)
        {
            using (var spreadsheet = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = spreadsheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet1"
                };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Заголовки
                Row headerRow = new Row();
                foreach (DataColumn column in dataTable.Columns)
                {
                    Cell cell = new Cell()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(column.ColumnName)
                    };
                    headerRow.Append(cell);
                }
                sheetData.Append(headerRow);

                // Данные
                foreach (DataRow dr in dataTable.Rows)
                {
                    Row row = new Row();
                    foreach (var item in dr.ItemArray)
                    {
                        Cell cell = new Cell()
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue(item?.ToString() ?? "")
                        };
                        row.Append(cell);
                    }
                    sheetData.Append(row);
                }

                workbookPart.Workbook.Save();
            }
        }
        private void LoadXlsxBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataTable dataTable = new DataTable();
            if (GroupsPage.IsVisible == true)
            {
                dataTable = ItemsSourceToDataTable(GroupsPage.ListBoxGroup.ItemsSource as IEnumerable);
            }
            else if (StudentsPage.IsVisible == true)
            {
                dataTable = ItemsSourceToDataTable(StudentsPage.dgvStudents.ItemsSource as IEnumerable);
            }
            else if (SpecsPage.IsVisible == true)
            {
                dataTable = ItemsSourceToDataTable(SpecsPage.ListBoxSpec.ItemsSource as IEnumerable);

            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            if (saveDialog.ShowDialog() == true)
            {
                ExportToXlsx(dataTable, saveDialog.FileName);
                MsgBox.Show("Экспорт завершен!", "Успех!", type: MessageBoxType.Success);
            }
        }

        private void Next_year_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show(
            "Эта функция переведёт всех студентов на следующий курс обучения",
            "Внимание",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) 
            {
                var next_result = MessageBox.Show(
                "Процесс не обратим, вы всё ещё уверены?",
                "ВНИМАНИЕ",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
                if (next_result == MessageBoxResult.Yes)
                {
                    using (NpgsqlConnection sqlConn = new NpgsqlConnection(conn))
                    {
                        sqlConn.Open();
                        NpgsqlCommand sqlCmd = new NpgsqlCommand("call promote_groups_buildings()", sqlConn);
                    }
                }
            }


        }
    }
}
