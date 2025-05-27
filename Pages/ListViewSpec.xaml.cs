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

namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewSpec.xaml
    /// </summary>
    public partial class ListViewSpec : Page
    {
        private Spec selectedSpec;
        private ObservableCollection<Spec> _spec;
        private ICollectionView _collectionView;

        public ListViewSpec()
        {
            InitializeComponent();
            _spec = new SpecViewModel().Spec;
            InitializeCollectionView();
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

        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
