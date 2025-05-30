
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
            fContainer.Navigate(new System.Uri("Pages/MainStatisticPage.xaml", UriKind.RelativeOrAbsolute));
            SettingsLabel.Visibility = Visibility.Visible;
            CreateNewStudentBtn.Visibility = Visibility.Hidden;
            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden; 
            CreateNewStudentBtn.Visibility = Visibility.Hidden;

            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden;
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
        }

        private void ExcelLoadMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/LoadExcelPage.xaml", UriKind.RelativeOrAbsolute));
            CreateNewStudentBtn.Visibility = Visibility.Hidden;
            NewGroupBtn.Visibility = Visibility.Hidden;
            NewSpecBtn.Visibility = Visibility.Hidden;
            SettingsLabel.Visibility = Visibility.Hidden;
        }

        private void SpecMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewSpec.xaml", UriKind.RelativeOrAbsolute));
            CreateNewStudentBtn.Visibility = Visibility.Visible;
            NewGroupBtn.Visibility = Visibility.Visible;
            NewSpecBtn.Visibility = Visibility.Visible;
            SettingsLabel.Visibility = Visibility.Hidden;
        }

        private void GroupMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewGroups.xaml", UriKind.RelativeOrAbsolute));
            CreateNewStudentBtn.Visibility = Visibility.Visible;
            NewGroupBtn.Visibility = Visibility.Visible;
            NewSpecBtn.Visibility = Visibility.Visible;
            SettingsLabel.Visibility = Visibility.Hidden;
        }

        private void StudentsMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/DataGridStudents.xaml", UriKind.RelativeOrAbsolute), this);
            CreateNewStudentBtn.Visibility = Visibility.Visible;
            NewGroupBtn.Visibility = Visibility.Visible;
            NewSpecBtn.Visibility = Visibility.Visible;
            SettingsLabel.Visibility = Visibility.Hidden;
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
        }
    }
}
