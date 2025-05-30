using Npgsql;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentSystem
{
    internal class GroupsViewModel : INotifyPropertyChanged
    {
        private string conn = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4}",
            $"{StudInfo.Properties.Settings.Default.BaseIP}", $"{StudInfo.Properties.Settings.Default.BasePort}", $"{StudInfo.Properties.Settings.Default.BaseLogIn}",
            $"{StudInfo.Properties.Settings.Default.BasePassword}", $"{StudInfo.Properties.Settings.Default.BaseName}");
        private string sql = @"select * from group_display";
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
                            Groups.Add(new Groups
                            {
                                Id = reader.GetInt16(0),
                                Title = reader.GetValue(1).ToString(),
                                GroupNum = reader.GetValue(2).ToString(),
                                FullTitle = reader.GetValue(3).ToString(),
                                BuildingId = reader.GetInt16(4),
                                CurrentYear = reader.GetValue(5).ToString(),
                                StartYear = reader.GetValue(6).ToString(),
                                EndYear = reader.GetValue(7).ToString()
                            });

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
