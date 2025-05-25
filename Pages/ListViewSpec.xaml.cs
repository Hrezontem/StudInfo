using System;
using System.Collections.Generic;
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
using System.Linq;
using StudInfo.Pages;

namespace StudentSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewSpec.xaml
    /// </summary>
    public partial class ListViewSpec : Page
    {
        private Spec selectedSpec;
        ListViewUpdateSpec updateWindow;

        public ListViewSpec()
        {
            InitializeComponent();
            DataContext = new SpecViewModel();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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
    }
}
