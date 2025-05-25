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

namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListViewSettings.xaml
    /// </summary>
    public partial class ListViewSettings : Page
    {
        public ListViewSettings()
        {
            InitializeComponent();
            SaveAPIBtn.Visibility = Visibility.Hidden;
            VKAPITextBox.IsEnabled = false;
            var APIurl = StudInfo.Properties.Settings.Default["APIurl"].ToString();
            VKAPITextBox.Text = APIurl;
        }

        private void SaveAPIBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeAPIBtn.Visibility = Visibility.Visible;
            SaveAPIBtn.Visibility = Visibility.Hidden;
            VKAPITextBox.IsEnabled = false;
            StudInfo.Properties.Settings.Default["APIurl"] = VKAPITextBox.Text;
        }

        private void ChangeAPIBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveAPIBtn.Visibility = Visibility.Visible;
            ChangeAPIBtn.Visibility = Visibility.Hidden;
            VKAPITextBox.IsEnabled = true;
            
        }
    }
}
