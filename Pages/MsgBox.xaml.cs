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
using System.Windows.Shapes;

namespace StudInfo
{
    /// <summary>
    /// Логика взаимодействия для MsgBox.xaml
    /// </summary>
    public partial class MsgBox : Window
    {
        public enum MessageBoxType
        {
            Info,
            Warning,
            Error,
            Success
        }
        public MsgBox(string message, string title, MessageBoxType type, MessageBoxButton buttons)
        {
            InitializeComponent();
            SetMessageType(type);
            TitleText.Text = string.IsNullOrEmpty(title) ? GetDefaultTitle(type) : title;
            MessageText.Text = message;
            ConfigureButtons(buttons);
        }


        private void SetMessageType(MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.Error:
                    MainBorder.Style = (Style)FindResource("ErrorStyle");
                    IconPath.Data = (Geometry)FindResource("ErrorIcon");
                    IconPath.Fill = Brushes.Red;
                    break;
                case MessageBoxType.Warning:
                    MainBorder.Style = (Style)FindResource("WarningStyle");
                    IconPath.Data = (Geometry)FindResource("WarningIcon");
                    IconPath.Fill = (Brush)(new BrushConverter().ConvertFrom("#0055CC"));
                    break;
                case MessageBoxType.Info:
                    MainBorder.Style = (Style)FindResource("InfoStyle");
                    IconPath.Data = (Geometry)FindResource("InfoIcon");
                    IconPath.Fill = (Brush)(new BrushConverter().ConvertFrom("#0055CC"));
                    break;
                case MessageBoxType.Success:
                    MainBorder.Style = (Style)FindResource("SuccessStyle");
                    IconPath.Data = (Geometry)FindResource("SuccessIcon");
                    IconPath.Fill = Brushes.Green;
                    break;
            }
        }

        private string GetDefaultTitle(MessageBoxType type) => type switch
        {
            MessageBoxType.Error => "Ошибка",
            MessageBoxType.Warning => "Предупреждение",
            MessageBoxType.Info => "Информация",
            MessageBoxType.Success => "Успешно",
            _ => "Сообщение"
        };

        private void ConfigureButtons(MessageBoxButton buttons)
        {
            switch (buttons)
            {
                case MessageBoxButton.OKCancel:
                    CancelButton.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNo:
                    // Кастомные кнопки при необходимости
                    break;
            }
        }

        // Статический метод для вызова
        public static bool? Show(
            string message,
            string title = "",
            MessageBoxType type = MessageBoxType.Info,
            MessageBoxButton buttons = MessageBoxButton.OK)
        {
            var dialog = new MsgBox(message, title, type, buttons);
            return dialog.ShowDialog();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e) => CloseWithResult(true);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => CloseWithResult(false);
        private void CloseWithResult(bool? result) => DialogResult = result;
    }
}
