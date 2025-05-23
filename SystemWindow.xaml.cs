
using StudentSystem.Pages;
using StudInfo.Pages;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
namespace StudentSystem
{
    /// <summary>
    /// Логика взаимодействия для SystemWindow.xaml
    /// </summary>
    public partial class SystemWindow : Window
    {
        private double windowHeight = 0;
        public DataTable dt;
        private ICollectionView _collectionView;
        public SystemWindow()
        {

            InitializeComponent();
            // button.BeginAnimation(Button.WidthProperty, buttonAnimation);
            SearchLabel.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            FilterComboBox.Visibility = Visibility.Hidden;
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

        private void btnMenu1_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/DataGridStudents.xaml", UriKind.RelativeOrAbsolute), this);
            SearchLabel.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            FilterComboBox.Visibility = Visibility.Hidden;
        }
        private void btnMenu2_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewGroups.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Visible;
            searchTextBox.Visibility = Visibility.Visible;
            FilterComboBox.Visibility = Visibility.Visible;
        }
        private void btnMenu3_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/DataGridStudents.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Visible;
            searchTextBox.Visibility = Visibility.Visible;
            FilterComboBox.Visibility = Visibility.Visible;
        }
        private void btnMenu4_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewSpec.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Visible;
            searchTextBox.Visibility = Visibility.Visible;
            FilterComboBox.Visibility = Visibility.Visible;
        }
        private void btnMenu5_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewNewStudent.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            FilterComboBox.Visibility = Visibility.Hidden;
        }
        private void btnMenu6_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewNewStudent.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            FilterComboBox.Visibility = Visibility.Hidden;
        }
        private void btnMenu7_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewNewStudent.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            FilterComboBox.Visibility = Visibility.Hidden;
        }
        private void btnMenu8_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/LoadExcelPage.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            FilterComboBox.Visibility = Visibility.Hidden;
        }
        private void btnMenu9_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewNewStudent.xaml", UriKind.RelativeOrAbsolute));
            SearchLabel.Visibility = Visibility.Hidden;
            searchTextBox.Visibility = Visibility.Hidden;
            FilterComboBox.Visibility = Visibility.Hidden;
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

        }
    }
}
