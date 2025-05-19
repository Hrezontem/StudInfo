using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using StudInfo.Properties;
using System.ComponentModel;

namespace StudentSystem
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private double windowHeight = 0;

        public AuthWindow()
        {
            InitializeComponent();
            var BaseName = StudInfo.Properties.Settings.Default["BaseName"].ToString();
            var BaseIP = StudInfo.Properties.Settings.Default["BaseIP"].ToString();
            var BasePort = StudInfo.Properties.Settings.Default["BasePort"].ToString();
            var BaseLogIn = StudInfo.Properties.Settings.Default["BaseLogIn"].ToString();
            var BasePassword = StudInfo.Properties.Settings.Default["BasePassword"].ToString();
            BaseNameTB.Text = BaseName;
            IPTB.Text = BaseIP;
            PortTB.Text = BasePort;
            LoginTB.Text = BaseLogIn;
            PasswordTB.Text = BasePassword;
        }

        

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                ((AuthWindow)System.Windows.Application.Current.MainWindow).DragMove();

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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            StudInfo.Properties.Settings.Default["BaseName"] = BaseNameTB.Text;
            StudInfo.Properties.Settings.Default["BaseIP"] = IPTB.Text;
            StudInfo.Properties.Settings.Default["BasePort"] = PortTB.Text;
            StudInfo.Properties.Settings.Default["BaseLogIn"] = LoginTB.Text;
            StudInfo.Properties.Settings.Default["BasePassword"] = PasswordTB.Text;
            StudInfo.Properties.Settings.Default.Save();
            SystemWindow systemWindow = new SystemWindow();
            this.Hide();
            systemWindow.Show();
        }
    }

}
