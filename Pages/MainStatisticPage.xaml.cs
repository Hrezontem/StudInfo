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
using LiveCharts.Wpf;
using LiveCharts;
using Npgsql;
using StudInfo.MVVM;

namespace StudInfo.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainStatisticPage.xaml
    /// </summary>
    public partial class MainStatisticPage : Page
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Years { get; set; }
        public MainStatisticPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var statistics = LoadStatistics();

            if (statistics.Count == 0)
            {
                MessageBox.Show("Нет данных для отображения.");
                return;
            }


            // Собираем года для оси X
            Years = statistics.Select(s => s.Year.ToString()).ToList();

            // Создаем серии данных
            SeriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Активные",
                Values = new ChartValues<int>(statistics.Select(s => s.Active))
            },
            new LineSeries
            {
                Title = "Отчисленные",
                Values = new ChartValues<int>(statistics.Select(s => s.Expelled))
            },
            new LineSeries
            {
                Title = "Выпускники",
                Values = new ChartValues<int>(statistics.Select(s => s.Graduated))
            }

        };
            DataContext = this;

        }
        public List<YearlyStatistics> LoadStatistics()
        {
            List<YearlyStatistics> statistics = new List<YearlyStatistics>();
            string connectionString = String.Format("Server={0};Port={1};" +
    "User Id={2};Password={3};Database={4}",
    $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
    $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(
                        "SELECT year, active, expelled, graduated FROM student_statistics ORDER BY year",
                        conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statistics.Add(new YearlyStatistics
                            {
                                Year = reader.GetInt32(0),
                                Active = reader.GetInt32(1),
                                Expelled = reader.GetInt32(2),
                                Graduated = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            return statistics;
        }
    }
}
