using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem
{
    public class Student : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string card;
        private string group;
        private string dateBirth;
        private string description;
        private string enrollment_date;
        private string status;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("id");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("ФИО");
            }
        }
        public string Card
        {
            get { return card; }
            set
            {
                card = value;
                OnPropertyChanged("Студенческий билет");
            }
        }
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Группа");
            }
        }
        public string DateBirth
        {
            get { return dateBirth; }
            set
            {
                dateBirth = value;
                OnPropertyChanged("Дата рождения");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Описание");
            }
        }


        public string EnrollmentDate
        {
            get { return enrollment_date; }
            set
            {
                enrollment_date = value;
                OnPropertyChanged("enrollment_date");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("status");
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
