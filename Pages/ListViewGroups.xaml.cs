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

namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewGroup.xaml
    /// </summary>

    public partial class ListViewGroup : Page
    {
        private ICollectionView _collectionView;
        private ObservableCollection<Groups> Groups;
        public ListViewGroup()
        {
            InitializeComponent();
            DataContext = new GroupsViewModel();
            //InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            //_collectionView = CollectionViewSource.GetDefaultView(Groups);
            //ListBoxStudent.ItemsSource = _collectionView;
        }
        public void LoadTable()
        {
            Groups = new GroupsViewModel().Groups;
            ListBoxStudent.ItemsSource = Groups;
        }





        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var searchText = searchTextBox.Text.ToLower();

            //_collectionView.Filter = item =>
            //{
            //    if (string.IsNullOrWhiteSpace(searchText)) return true;

            //    var type = item.GetType();
            //    var properties = type.GetProperties();

            //    foreach (var prop in properties)
            //    {
            //        var value = prop.GetValue(item)?.ToString();
            //        if (value?.ToLower().Contains(searchText) == true)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //};
        }

    }
}
