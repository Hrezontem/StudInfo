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
    internal class GroupsViewModel : INotifyPropertyChanged
    {
        private string conn = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4}",
            $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
            $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from groups";
        public ObservableCollection<Groups> Groups { get; set; }

        public GroupsViewModel()
        {
            Groups = new ObservableCollection<Groups>();

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
                            string title = reader.GetValue(0).ToString();
                            string specTitle = reader.GetValue(2).ToString();
                            string code = reader.GetValue(3).ToString();

                            Groups.Add(new Groups { Title = title, SpecTitle = specTitle, Code = code }); 
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
