using Npgsql;
using StudInfo;
using StudInfo.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using static StudInfo.MsgBox;

namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewSpec.xaml
    /// </summary>
    public partial class ListViewSpec : Page
    {
        private string conn = String.Format("Server={0};Port={1};" +
"User Id={2};Password={3};Database={4}",
$"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
$"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        ListViewUpdateSpec updateWindow;
        private Spec selectedSpec;
        private ObservableCollection<Spec> _spec;
        private ICollectionView _collectionView;

        public ListViewSpec()
        {
            InitializeComponent();
            _spec = new SpecViewModel().Spec;
            InitializeCollectionView();
        }
        public void LoadTable()
        {
            _spec = new SpecViewModel().Spec;
            ListBoxSpec.ItemsSource = _spec;
        }
        private void InitializeCollectionView()
        {
            _collectionView = CollectionViewSource.GetDefaultView(_spec);
            ListBoxSpec.ItemsSource = _collectionView;
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = searchTextBox.Text.ToLower();

            if (searchText != "поиск..." && ListBoxSpec.ItemsSource != null)
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

        private void searchTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void SpecInfoBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //selectedSpec = ListBoxSpec.SelectedItem as Spec;
            //updateWindow = new ListViewUpdateSpec(selectedSpec);
            //updateWindow.SaveNewSpecBtn.Visibility = Visibility.Hidden;
            //updateWindow.SaveNewSpecBtn.IsEnabled = false;
            //updateWindow.BackBtn.HorizontalAlignment = HorizontalAlignment.Center;
            //updateWindow.BackBtn.VerticalAlignment = VerticalAlignment.Center;
            //updateWindow.BackBtn.Margin = new Thickness(0);
            //updateWindow.SpecCodeTextBox.IsEnabled = false;
            //updateWindow.SpecFullTitleTextBox.IsEnabled = false;
            //updateWindow.SpecTitleTextBox.IsEnabled = false;
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
            selectedSpec = ListBoxSpec.SelectedItem as Spec;
            updateWindow = new ListViewUpdateSpec(selectedSpec);
            updateWindow.SaveNewSpecBtn.Visibility = Visibility.Hidden;
            updateWindow.SaveNewSpecBtn.IsEnabled = false;
            updateWindow.BackBtn.HorizontalAlignment = HorizontalAlignment.Center;
            updateWindow.BackBtn.VerticalAlignment = VerticalAlignment.Center;
            updateWindow.BackBtn.Margin = new Thickness(0);
            updateWindow.SpecTitleTextBox.IsEnabled = false;
            updateWindow.SpecFullTitleTextBox.IsEnabled = false;
            updateWindow.SpecCodeTextBox.IsEnabled = false;
            updateWindow.ChangeSpecLabel.Content = "Просмотр";
            updateWindow.Closing += ListViewUpdateGroup_Closing;
            if (selectedSpec != null)
            {

                updateWindow.Show();

            }
            else
            {
                MsgBox.Show("Выберите группу!.");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
          "Вы уверены, что хотите удалить группу?",
          "ВНИМАНИЕ",
          MessageBoxButton.YesNo,
          MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (Spec spec in ListBoxSpec.SelectedItems.Cast<Spec>().ToList())
                {
                    try
                    {
                        if (spec != null && spec.Id > 0) // Проверяем ID
                        {
                            using (var sqlConn = new NpgsqlConnection(conn))
                            {
                                sqlConn.Open();
                                var cmd = new NpgsqlCommand($"call delete_spec({spec.Id})", sqlConn);
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

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            ListViewUpdateSpec listViewUpdateSpec = new ListViewUpdateSpec(ListBoxSpec.SelectedItem as Spec);
            listViewUpdateSpec.Closing += ListViewUpdateGroup_Closing;
            listViewUpdateSpec.Show();
        }
    }
}
