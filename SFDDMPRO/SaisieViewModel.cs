using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Events;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Events;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;


namespace SFDDMPRO
{
    internal class SaisieViewModel : DockPane
    {
        private const string _dockPaneID = "SFDDMPRO_Saisie";

        private SubscriptionToken _eventToken = null;

        public ReadOnlyObservableCollection<string> AllFeatureLayers { get; private set; }
        private ObservableCollection<string> _allFeatureLayers;
        public string SelectedFeatureLayer { get; set; }

        private DataNaipf2019 _dataNaipf2019 = new DataNaipf2019();
        public DataNaipf2019 CurrentDataNaipf
        {
            get { return _dataNaipf2019; }
            set { }
        }

        public ICommand RefreshLayers => _refreshLayers;
        private RelayCommand _refreshLayers;

        public ICommand GroupEss => _groupEss;
        private RelayCommand _groupEss;

        public ICommand NotePrest => _notePrest;
        private RelayCommand _notePrest;

        public ICommand SaveTableEdits => _saveTableEdits;
        private RelayCommand _saveTableEdits;

        protected SaisieViewModel()
        {
            _refreshLayers = new RelayCommand(() => refreshLayers(), () => true);
            _saveTableEdits = new RelayCommand(() => saveTableEdits(), () => true);
            _groupEss = new RelayCommand(() => groupEss(), () => true);
            _notePrest = new RelayCommand(() => notePrest(), () => true);
        }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            pane.Activate();
        }

        protected override void OnShow(bool isVisible)
        {
            if (isVisible && _eventToken == null) //Subscribe to event when dockpane is visible
            {
                _eventToken = MapSelectionChangedEvent.Subscribe(OnMapSelectionChangedEvent);
            }

            if (!isVisible && _eventToken != null) //Unsubscribe as the dockpane closes.
            {
                MapSelectionChangedEvent.Unsubscribe(_eventToken);
                _eventToken = null;
            }
        }

        private void groupEss()
        {

        }

        private void notePrest()
        {

        }

        private void saveTableEdits()
        {
            if (MapView.Active == null)
            {
                return;
            }

            QueuedTask.Run(() =>
            {
                // Check to see if there is a selected feature layer
                var featLayer = MapView.Active.Map.GetLayersAsFlattenedList().FirstOrDefault(l => l.Name == SelectedFeatureLayer) as FeatureLayer;
                if (featLayer == null)
                {
                    return;
                }
                // Get the selected records, and check/exit if there are none:
                var featSelectionOIDs = featLayer.GetSelection().GetObjectIDs();
                if (featSelectionOIDs.Count == 0 || featSelectionOIDs.Count > 1)
                {
                    return;
                }

                try
                {
                    var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector(true);
                    inspector.Load(featLayer, featSelectionOIDs);

                    if (inspector.HasAttributes)
                    {
                        if (inspector.Count(a => a.FieldName.ToUpper() == "type_couv1") > 0) inspector["type_couv1"] = _dataNaipf2019.TxtTbType1;

                        var editOp = new EditOperation();
                        editOp.Name = "Edit " + featLayer.Name + ", " + Convert.ToString(featSelectionOIDs.Count) + " records.";
                        editOp.Modify(inspector);
                        editOp.ExecuteAsync();

                        //MessageBox.Show("Update operation completed.", "Editing with Inspector");
                    }
                    else
                    {
                        //MessageBox.Show("The Attribute provided is not valid. " +
                        //    "\r\n Ensure your attribute name is correct.", "Invalid attribute");
                        // return;
                    }
                }
                catch (Exception exc)
                {
                    // Catch any exception found and display a message box.
                    MessageBox.Show("Exception caught: " + exc.Message);
                    return;
                }
                finally
                {
                    if (Project.Current.HasEdits)
                        Project.Current.SaveEditsAsync();
                }
            });
        }

        //Event handler when the MapSelection event is triggered.
        private void OnMapSelectionChangedEvent(MapSelectionChangedEventArgs obj)
        {
            if (MapView.Active == null)
            {
                return;
            }

            QueuedTask.Run(() =>
            {
                // Check to see if there is a selected feature layer
                var featLayer = MapView.Active.Map.GetLayersAsFlattenedList().FirstOrDefault(l => l.Name == SelectedFeatureLayer) as FeatureLayer;
                if (featLayer == null)
                {
                    return;
                }
                // Get the selected records, and check/exit if there are none:
                var featSelectionOIDs = featLayer.GetSelection().GetObjectIDs();
                if (featSelectionOIDs.Count == 0 || featSelectionOIDs.Count > 1)
                {
                    return;
                }

                using (var rowCursor = featLayer.GetSelection().Search(null))
                {
                    rowCursor.MoveNext();
                    var anyRow = rowCursor.Current;
                    _dataNaipf2019.TxtTbType1 = anyRow["type_couv1"].ToString();
                    _dataNaipf2019.TxtTbType2 = anyRow["type_couv2"].ToString();
                    _dataNaipf2019.TxtTbPart = anyRow["part_str"].ToString();
                    _dataNaipf2019.TxtTbDens1 = anyRow["et1_dens"].ToString();
                    _dataNaipf2019.TxtTbDens2 = anyRow["et2_dens"].ToString();
                    _dataNaipf2019.TxtTbHaut1 = anyRow["et1_haut"].ToString();
                    _dataNaipf2019.TxtTbHaut2 = anyRow["et2_haut"].ToString();
                    _dataNaipf2019.TxtTbCouvGaule = anyRow["couv_gaule"].ToString();
                    _dataNaipf2019.TxtTbEtDomi = anyRow["et_domi"].ToString();
                    _dataNaipf2019.TxtTbOrigine = anyRow["origine"].ToString();
                    _dataNaipf2019.TxtTbAnnOrg = anyRow["an_origine"].ToString();
                    _dataNaipf2019.TxtTbRebEss1 = anyRow["reb_ess1"].ToString();
                    _dataNaipf2019.TxtTbRebEss2 = anyRow["reb_ess2"].ToString();
                    _dataNaipf2019.TxtTbRebEss3 = anyRow["reb_ess3"].ToString();
                    _dataNaipf2019.TxtTbEtage = anyRow["etagement"].ToString();
                    _dataNaipf2019.TxtTbAge1 = anyRow["et1_age"].ToString();
                    _dataNaipf2019.TxtTbAge2 = anyRow["et2_age"].ToString();
                    _dataNaipf2019.TxtTbPertMoy = anyRow["perturb"].ToString();
                    _dataNaipf2019.TxtTbAnnPertMoy = anyRow["an_perturb"].ToString();
                    _dataNaipf2019.TxtTbCdeTerr = anyRow["co_ter"].ToString();
                }
            });
        }

        /// <summary>
        /// Text shown near the top of the DockPane.
        /// </summary>
        private string _heading = "SFDDMPRO v1.00.0";
        public string Heading
        {
            get { return _heading; }
            set
            {
                SetProperty(ref _heading, value, () => Heading);
            }
        }

        public void refreshLayers()
        {
            if (MapView.Active == null)
            {
                return;
            }

            QueuedTask.Run(() =>
            {
                //Combobox
                _allFeatureLayers = new ObservableCollection<string>();

                foreach (var f in MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>())
                {
                    var okAdd = true;
                    var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector(true);

                    var featSelectionOIDs = f.GetSelection().GetObjectIDs();

                    inspector.Load(f, featSelectionOIDs);

                    if (inspector.HasAttributes)
                    {
                        if (inspector.Count(a => a.FieldName.ToLower() == "type_couv1") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "type_couv2") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "part_str") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "et1_dens") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "et2_dens") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "et1_haut") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "et2_haut") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "couv_gaule") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "et_domi") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "origine") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "an_origine") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "reb_ess1") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "reb_ess2") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "reb_ess3") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "etagement") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "et1_age") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "et2_age") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "perturb") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "an_perturb") == 0) okAdd = false;
                        if (inspector.Count(a => a.FieldName.ToLower() == "co_ter") == 0) okAdd = false;

                        if (okAdd)
                            _allFeatureLayers.Add(f.Name);
                    }
                }

                this.AllFeatureLayers = new ReadOnlyObservableCollection<string>(_allFeatureLayers);
                NotifyPropertyChanged("AllFeatureLayers");
            });
        }
    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class Saisie_ShowButton : Button
    {
        protected override void OnClick()
        {
            SaisieViewModel.Show();
        }
    }
}
