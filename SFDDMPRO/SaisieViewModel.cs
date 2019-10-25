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

        private string _txtText1;
        public string TxtText1
        {
            get { return _txtText1; }
            set
            {
                _txtText1 = value;
                NotifyPropertyChanged("TxtText1");
            }
        }

        public ICommand RefreshLayers => _refreshLayers;
        private RelayCommand _refreshLayers;

        public ICommand SaveTableEdits => _saveTableEdits;
        private RelayCommand _saveTableEdits;

        protected SaisieViewModel()
        {
            _refreshLayers = new RelayCommand(() => refreshLayers(), () => true);
            _saveTableEdits = new RelayCommand(() => saveTableEdits(), () => true);
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
                        if (inspector.Count(a => a.FieldName.ToUpper() == "SAMPLINGST") > 0) inspector["SamplingSt"] = TxtText1;

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
                    TxtText1 = anyRow["SamplingSt"].ToString();
                }
            });
        }

        /// <summary>
        /// Text shown near the top of the DockPane.
        /// </summary>
        private string _heading = "SFDDMPRO v1.00.1";
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
                    _allFeatureLayers.Add(f.Name);
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
