using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem
{
    public class Groups : INotifyPropertyChanged
    {
        private int id;
        private int buildingId;
        private string title;
        private string fullTitle;
        private string startYear;
        private string endYear;
        private string currentYear;


        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("id");
            }
        }

        public int BuildingId
        {
            get { return buildingId; }
            set
            {
                buildingId = value;
                OnPropertyChanged("buildingId");
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string FullTitle
        {
            get { return fullTitle; }
            set
            {
                fullTitle = value;
                OnPropertyChanged("fullTitle");
            }
        }
        public string StartYear
        {
            get { return startYear; }
            set
            {
                startYear = value;
                OnPropertyChanged("startYear");
            }
        }

        public string EndYear
        {
            get { return endYear; }
            set
            {
                endYear = value;
                OnPropertyChanged("endYear");
            }
        }

        public string CurrentYear
        {
            get { return currentYear; }
            set
            {
                currentYear = value;
                OnPropertyChanged("currentYear");
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

