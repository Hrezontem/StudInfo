using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using StudInfo.Properties;
using System.Configuration;

namespace StudentSystem
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        private string conn = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4}",
            $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
            $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from students_select(1) order by students_name ";
        public ObservableCollection<Student> Students { get; set; }
        public DataTable DataTableStudents;
        Stopwatch stopwatch = new Stopwatch();
        public StudentViewModel() 
        {
            Students = new ObservableCollection<Student>();

            using (NpgsqlConnection connection = new NpgsqlConnection(conn))
            {
                stopwatch.Start();
                connection.Open();
                DataTableStudents = new DataTable();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) 
                    {
                        while (reader.Read()) 
                        {
                            string name = reader.GetValue(1).ToString();
                            string card = reader.GetValue(2).ToString();
                            string group = reader.GetValue(3).ToString();

                            Students.Add(new Student { Name = name, Card = card, Group = group });
                        }
                    }
                }
                stopwatch.Stop();
                Debug.WriteLine($"Отработала загрузка данных из базы за {((Double)stopwatch.ElapsedMilliseconds / 1000)} мс");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
