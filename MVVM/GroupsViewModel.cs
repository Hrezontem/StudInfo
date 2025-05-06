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
        private string conn = "Server=localhost;Port=5432;User Id=postgres;Password=123; Database=postgres";
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
                            string title = reader.GetValue(1).ToString();
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
