using System;
using System.ComponentModel;
using System.Threading;
using System.Xml.Serialization;

namespace BanaBot.Data
{
    [XmlRoot(Namespace = "", IsNullable = false), XmlType(AnonymousType = true)]
    public class Config : INotifyPropertyChanged
    {
        private string _clientVersion;
        private string _champion;
        private string _maxlevel;
        private string _password;
        private string _queuetype;
        private string _region;
        private string _spell1;
        private string _spell2;
        private string _username;
        [XmlIgnore] public static Config Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }

        public string ClientVersion
        {
            get { return _clientVersion; }
            set
            {
                _clientVersion = value;
                OnPropertyChanged("ClientVersion");
            }
        }

        public string Champion
        {
            get { return _champion; }
            set
            {
                _champion = value;
                OnPropertyChanged("Champion");
            }
        }

        public string MaxLevel
        {
            get { return _maxlevel; }
            set
            {
                _maxlevel = value;
                OnPropertyChanged("MaxLevel");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string QueueType
        {
            get { return _queuetype; }
            set
            {
                _queuetype = value;
                OnPropertyChanged("QueueType");
            }
        }

        public string Region
        {
            get { return _region; }
            set
            {
                _region = value; 
                OnPropertyChanged("Region");
            }
        }

        public string Spell1
        {
            get { return _spell1; }
            set
            {
                _spell1 = value;
                OnPropertyChanged("Spell1");
            }
        }

        public string Spell2
        {
            get { return _spell2; }
            set
            {
                _spell2 = value;
                OnPropertyChanged("Spell2");
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
    }
}
