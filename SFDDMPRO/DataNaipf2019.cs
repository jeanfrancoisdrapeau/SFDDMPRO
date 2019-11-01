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

        //References
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

        private CRefType _RefPart = new DataNaipf2019.CRefType();
        public CRefType RefPart
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\part.json"))
                {
                    string json = r.ReadToEnd();
                    _RefPart = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefPart;
            }
            set { }
        }

        private CRefType _RefDens = new DataNaipf2019.CRefType();
        public CRefType RefDens
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\dens.json"))
                {
                    string json = r.ReadToEnd();
                    _RefDens = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefDens;
            }
            set { }
        }

        private CRefType _RefHaut = new DataNaipf2019.CRefType();
        public CRefType RefHaut
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\haut.json"))
                {
                    string json = r.ReadToEnd();
                    _RefHaut = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefHaut;
            }
            set { }
        }

        private CRefType _RefEtDomi = new DataNaipf2019.CRefType();
        public CRefType RefEtDomi
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\etdomi.json"))
                {
                    string json = r.ReadToEnd();
                    _RefEtDomi = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefEtDomi;
            }
            set { }
        }

        private CRefType _RefOrigine = new DataNaipf2019.CRefType();
        public CRefType RefOrigine
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\origine.json"))
                {
                    string json = r.ReadToEnd();
                    _RefOrigine = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefOrigine;
            }
            set { }
        }

        private CRefType _RefRebEss = new DataNaipf2019.CRefType();
        public CRefType RefRebEss
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\rebess.json"))
                {
                    string json = r.ReadToEnd();
                    _RefRebEss = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefRebEss;
            }
            set { }
        }

        private CRefType _RefEtage = new DataNaipf2019.CRefType();
        public CRefType RefEtage
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\etage.json"))
                {
                    string json = r.ReadToEnd();
                    _RefEtage = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefEtage;
            }
            set { }
        }

        private CRefType _RefAge = new DataNaipf2019.CRefType();
        public CRefType RefAge
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\age.json"))
                {
                    string json = r.ReadToEnd();
                    _RefAge = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefAge;
            }
            set { }
        }

        private CRefType _RefPertMoy = new DataNaipf2019.CRefType();
        public CRefType RefPertMoy
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\pert.json"))
                {
                    string json = r.ReadToEnd();
                    _RefPertMoy = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefPertMoy;
            }
            set { }
        }

        private CRefType _RefCdeTer = new DataNaipf2019.CRefType();
        public CRefType RefCdeTer
        {
            get
            {
                using (StreamReader r = new StreamReader(@"C:\SFDDMPRO\terrain.json"))
                {
                    string json = r.ReadToEnd();
                    _RefCdeTer = JsonConvert.DeserializeObject<CRefType>(json);
                }
                return _RefCdeTer;
            }
            set { }
        }
        
        //Fields
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

        private string _TxtTbPart;
        public string TxtTbPart
        {
            get { return _TxtTbPart; }
            set
            {
                _TxtTbPart = value.ToUpper();
                NotifyPropertyChanged("TxtTbPart");
            }
        }

        private string _TxtTbDens1;
        public string TxtTbDens1
        {
            get { return _TxtTbDens1; }
            set
            {
                _TxtTbDens1 = value.ToUpper();
                NotifyPropertyChanged("TxtTbDens1");
            }
        }

        private string _TxtTbDens2;
        public string TxtTbDens2
        {
            get { return _TxtTbDens2; }
            set
            {
                _TxtTbDens2 = value.ToUpper();
                NotifyPropertyChanged("TxtTbDens2");
            }
        }

        private string _TxtTbHaut1;
        public string TxtTbHaut1
        {
            get { return _TxtTbHaut1; }
            set
            {
                _TxtTbHaut1 = value.ToUpper();
                NotifyPropertyChanged("TxtTbHaut1");
            }
        }

        private string _TxtTbHaut2;
        public string TxtTbHaut2
        {
            get { return _TxtTbHaut2; }
            set
            {
                _TxtTbHaut2 = value.ToUpper();
                NotifyPropertyChanged("TxtTbHaut2");
            }
        }

        private string _TxtTbCouvGaule;
        public string TxtTbCouvGaule
        {
            get { return _TxtTbCouvGaule; }
            set
            {
                _TxtTbCouvGaule = value.ToUpper();
                NotifyPropertyChanged("TxtTbCouvGaule");
            }
        }

        private string _TxtTbEtDomi;
        public string TxtTbEtDomi
        {
            get { return _TxtTbEtDomi; }
            set
            {
                _TxtTbEtDomi = value.ToUpper();
                NotifyPropertyChanged("TxtTbEtDomi");
            }
        }

        private string _TxtTbOrigine;
        public string TxtTbOrigine
        {
            get { return _TxtTbOrigine; }
            set
            {
                _TxtTbOrigine = value.ToUpper();
                NotifyPropertyChanged("TxtTbOrigine");
            }
        }

        private string _TxtTbAnnOrg;
        public string TxtTbAnnOrg
        {
            get { return _TxtTbAnnOrg; }
            set
            {
                _TxtTbAnnOrg = value.ToUpper();
                NotifyPropertyChanged("TxtTbAnnOrg");
            }
        }

        private string _TxtTbRebEss1;
        public string TxtTbRebEss1
        {
            get { return _TxtTbRebEss1; }
            set
            {
                _TxtTbRebEss1 = value.ToUpper();
                NotifyPropertyChanged("TxtTbRebEss1");
            }
        }

        private string _TxtTbRebEss2;
        public string TxtTbRebEss2
        {
            get { return _TxtTbRebEss2; }
            set
            {
                _TxtTbRebEss2 = value.ToUpper();
                NotifyPropertyChanged("TxtTbRebEss2");
            }
        }

        private string _TxtTbRebEss3;
        public string TxtTbRebEss3
        {
            get { return _TxtTbRebEss3; }
            set
            {
                _TxtTbRebEss3 = value.ToUpper();
                NotifyPropertyChanged("TxtTbRebEss3");
            }
        }

        private string _TxtTbEtage;
        public string TxtTbEtage
        {
            get { return _TxtTbEtage; }
            set
            {
                _TxtTbEtage = value.ToUpper();
                NotifyPropertyChanged("TxtTbEtage");
            }
        }

        private string _TxtTbAge1;
        public string TxtTbAge1
        {
            get { return _TxtTbAge1; }
            set
            {
                _TxtTbAge1 = value.ToUpper();
                NotifyPropertyChanged("TxtTbAge1");
            }
        }

        private string _TxtTbAge2;
        public string TxtTbAge2
        {
            get { return _TxtTbAge2; }
            set
            {
                _TxtTbAge2 = value.ToUpper();
                NotifyPropertyChanged("TxtTbAge2");
            }
        }

        private string _TxtTbPertMoy;
        public string TxtTbPertMoy
        {
            get { return _TxtTbPertMoy; }
            set
            {
                _TxtTbPertMoy = value.ToUpper();
                NotifyPropertyChanged("TxtTbPertMoy");
            }
        }

        private string _TxtTbAnnPertMoy;
        public string TxtTbAnnPertMoy
        {
            get { return _TxtTbAnnPertMoy; }
            set
            {
                _TxtTbAnnPertMoy = value.ToUpper();
                NotifyPropertyChanged("TxtTbAnnPertMoy");
            }
        }

        private string _TxtTbCdeTerr;
        public string TxtTbCdeTerr
        {
            get { return _TxtTbCdeTerr; }
            set
            {
                _TxtTbCdeTerr = value.ToUpper();
                NotifyPropertyChanged("TxtTbCdeTerr");
            }
        }
    }
}
