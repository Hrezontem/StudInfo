using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem
{
    internal class Spec : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private string fullTitle;
        private string specCode;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
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
        public string SpecCode
        {
            get { return specCode; }
            set
            {
                specCode = value;
                OnPropertyChanged("SpecCode");
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

