using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;

namespace StudentSystem
{
    internal class SpecViewModel : INotifyPropertyChanged
    {
        private string conn = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4}",
            $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
            $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from specializations s";
        public ObservableCollection<Spec> Spec { get; set; }

        public SpecViewModel()
        {
            Spec = new ObservableCollection<Spec>();

            using (NpgsqlConnection connection = new NpgsqlConnection(conn))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt16(0);
                            string title = reader.GetValue(1).ToString();
                            string fullTitle = reader.GetValue(2).ToString();
                            string specCode = reader.GetValue(3).ToString();

                            Spec.Add(new Spec { Id = id, Title = title, FullTitle = fullTitle, SpecCode = specCode });
                        }
                    }
                }
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
