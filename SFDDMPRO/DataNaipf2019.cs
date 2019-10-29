using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json;


namespace SFDDMPRO
{
    public class DataNaipf2019 : INotifyPropertyChanged
    {
        private static DataNaipf2019 instance = new DataNaipf2019();

        public DataNaipf2019() { }

        public static DataNaipf2019 GetInstance()
        {
            return instance;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class CRefItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        public class CRefType
        {
            public IList<CRefItem> refs { get; set; }
        }

        private string _TxtTbIdUnique;
        public string TxtTbIdUnique
        {
            get { return _TxtTbIdUnique; }
            set
            {
                _TxtTbIdUnique = value;
                NotifyPropertyChanged("TxtTbIdUnique");
            }
        }

        private string _TxtTbNoPeup;
        public string TxtTbNoPeup
        {
            get { return _TxtTbNoPeup; }
            set
            {
                _TxtTbNoPeup = value;
                NotifyPropertyChanged("TxtTbNoPeup");
            }
        }

        private string _TxtTbPeeNoAcq;
        public string TxtTbPeeNoAcq
        {
            get { return _TxtTbPeeNoAcq; }
            set
            {
                _TxtTbPeeNoAcq = value;
                NotifyPropertyChanged("TxtTbPeeNoAcq");
            }
        }

        private CRefType _RefType = new DataNaipf2019.CRefType();
        public CRefType RefType
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\type.json"))
                {
                    string json = r.ReadToEnd();
                    _RefType = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefType;
            }
            set { }
        }

        private string _TxtTbType1;
        public string TxtTbType1
        {
            get { return _TxtTbType1; }
            set
            {
                _TxtTbType1 = value.ToUpper();
                NotifyPropertyChanged("TxtTbType1");
            }
        }

        private string _TxtTbType2;
        public string TxtTbType2
        {
            get { return _TxtTbType2; }
            set
            {
                _TxtTbType2 = value.ToUpper();
                NotifyPropertyChanged("TxtTbType2");
            }
        }
    }
}
