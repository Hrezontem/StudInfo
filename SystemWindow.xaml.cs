
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
namespace StudentSystem
{
    /// <summary>
    /// Логика взаимодействия для SystemWindow.xaml
    /// </summary>
    public partial class SystemWindow : Window
    {
        private double windowHeight = 0;

        public SystemWindow()
        {
            InitializeComponent();
        // button.BeginAnimation(Button.WidthProperty, buttonAnimation);
            


    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                ((SystemWindow)System.Windows.Application.Current.MainWindow).DragMove();

        }

        private void btnMenu1_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewStudents.xaml", UriKind.RelativeOrAbsolute));
        }
        private void btnMenu2_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewGroups.xaml", UriKind.RelativeOrAbsolute));
        }
        private void btnMenu3_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/ListViewStudents.xaml", UriKind.RelativeOrAbsolute));
        }
        private void btnMenu4_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAnimation(sender as RadioButton);
            fContainer.Navigate(new System.Uri("Pages/DataGridStudents.xaml", UriKind.RelativeOrAbsolute));
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
    }
}
