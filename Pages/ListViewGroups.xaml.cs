using System.Linq;
using System.Windows.Controls;

using Npgsql;
using StudInfo.Pages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

using System.Windows;
using System.Windows.Data;
using System.Text.RegularExpressions;
using static StudInfo.MsgBox;
using StudInfo;

namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewGroup.xaml
    /// </summary>

    public partial class ListViewGroup : Page
    {
        private string conn = String.Format("Server={0};Port={1};" +
"User Id={2};Password={3};Database={4}",
$"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
$"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private ICollectionView _collectionView;
        private ObservableCollection<Groups> Groups;
        private Groups selectedGroup;
        ListViewUpdateGroup updateWindow;
        public ListViewGroup()
        {
            InitializeComponent();
            Groups = new GroupsViewModel().Groups;
            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            _collectionView = CollectionViewSource.GetDefaultView(Groups);
            ListBoxGroup.ItemsSource = _collectionView;
        }
        public void LoadTable()
        {
            Groups = new GroupsViewModel().Groups;
            ListBoxGroup.ItemsSource = Groups;
        }





        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = searchTextBox.Text.ToLower();

            if (searchText != "поиск..." && ListBoxGroup.ItemsSource != null)
            {
                _collectionView.Filter = item =>
                {
                    if (string.IsNullOrWhiteSpace(searchText)) return true;

                    var type = item.GetType();
                    var properties = type.GetProperties();

                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(item)?.ToString();
                        if (value?.ToLower().Contains(searchText) == true)
                        {
                            return true;
                        }
                    }
                    return false;
                };
            }
        }



        public void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
           "Вы уверены, что хотите удалить группу?",
           "ВНИМАНИЕ",
           MessageBoxButton.YesNo,
           MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (Groups groups in ListBoxGroup.SelectedItems.Cast<Groups>().ToList())
                {
                    try
                    {
                        if (groups != null && groups.Id > 0) // Проверяем ID
                        {
                            using (var sqlConn = new NpgsqlConnection(conn))
                            {
                                sqlConn.Open();
                                var cmd = new NpgsqlCommand($"call delete_group({groups.Id})", sqlConn);
                                cmd.ExecuteNonQuery();
                            }

                            MsgBox.Show("Удалено успешно", "Успех",
                                          type: MessageBoxType.Success);
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show($"Ошибка доступа. Ошибка: {ex.Message}", "Упс...", type: MessageBoxType.Error);
                    }
                }
                LoadTable();
            }
        }

        public void ListViewUpdateGroup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadTable();
        }
        public void ListViewNewGroup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadTable();
        }
        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedGroup = ListBoxGroup.SelectedItem as Groups;
            updateWindow = new ListViewUpdateGroup(selectedGroup);
            updateWindow.SaveNewStudentBtn.Visibility = Visibility.Hidden;
            updateWindow.SaveNewStudentBtn.IsEnabled = false;
            updateWindow.BackBtn.HorizontalAlignment = HorizontalAlignment.Center;
            updateWindow.BackBtn.VerticalAlignment = VerticalAlignment.Center;
            updateWindow.BackBtn.Margin = new Thickness(0);
            updateWindow.SpecComboBox.IsEnabled = false;
            updateWindow.NumGroupTextBox.IsEnabled = false;
            updateWindow.dateTextBox.IsEnabled = false;
            updateWindow.currentYearTextBox.IsEnabled = false;
            updateWindow.ChangeGroupLabel.Content = "Просмотр";
            updateWindow.Closing += ListViewUpdateGroup_Closing;
            if (selectedGroup != null)
            {

                updateWindow.Show();

            }
            else
            {
                MsgBox.Show("Выберите группу!.");
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            ListViewUpdateGroup listViewUpdateGroup = new ListViewUpdateGroup(ListBoxGroup.SelectedItem as Groups);
            listViewUpdateGroup.Closing += ListViewUpdateGroup_Closing;
            listViewUpdateGroup.Show();
        }


        private void LoadStudentsHistory()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(conn))
                {
                    sqlConn.Open();
                    var cmd = new NpgsqlCommand($"select * from test.student_history_view", sqlConn);
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    //dgvHistoryGroups.ItemsSource = dt.DefaultView;
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show($"Ошибка доступа. Ошибка: {ex.Message}", type: MessageBoxType.Error);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb.SelectedItem == cb.Items[0])
            {
                ListBoxGroup.Visibility = Visibility.Visible;
                ListBoxHistoryGroup.Visibility = Visibility.Hidden;
            }
            else if (cb.SelectedItem == cb.Items[1])
            {
                ListBoxGroup.Visibility = Visibility.Hidden;
                ListBoxHistoryGroup.Visibility = Visibility.Visible;
                LoadStudentsHistory();
            }
        }
    }
}
