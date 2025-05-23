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
using Microsoft.Win32;

namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadExcelPage.xaml
    /// </summary>
    public partial class LoadExcelPage : Page
    {
        public LoadExcelPage()
        {
            InitializeComponent();
        }



        private void ExcelLoadElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            if (OPF.ShowDialog() == true)
            {
                dgvLoad.Visibility = Visibility.Visible;
                ExcelLoadElement.Visibility = Visibility.Hidden;
            }
        }
    }
}
