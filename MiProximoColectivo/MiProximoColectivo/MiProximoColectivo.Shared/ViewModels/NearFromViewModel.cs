using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls.Maps;
using MiProximoColectivo.Classes.Local;
using MiProximoColectivo.Functions;
using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.RequestTasks;
using System.Threading.Tasks;
using MiProximoColectivo.Model;
using MiProximoColectivo.Classes.ServerReceived;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using System.Diagnostics;
using GalaSoft.MvvmLight.Threading;

namespace MiProximoColectivo.ViewModels
{
    public class NearFromViewModel : ViewModelBase
    {
        private UIObservableCollection<Bus> _busses;
        private UIObservableCollection<Feature> _currentBussesFeatures;
        private UIObservableCollection<string> _busTracks;
        private string _busTrackSelected;
        private string _lastTrackSelected;
        private bool _showNearBusses;
        private bool _isFilteringByTrack;

        public UIObservableCollection<Bus> Busses
        {
            get { return _busses; }
            set
            {
                _busses = value;
                RaisePropertyChanged();
            }
        }
        public UIObservableCollection<string> BusTracks
        {
            get { return _busTracks; }
            set
            {
                _busTracks = value;
                RaisePropertyChanged();
            }
        }
        public string BusTrackSelected
        {
            get { return _busTrackSelected; }
            set
            {
                _busTrackSelected = value;
                RaisePropertyChanged();

                //Si no es un string vacion o nulo, hago el GetRecorrido
                if (!string.IsNullOrEmpty(_busTrackSelected) && !string.IsNullOrWhiteSpace(_busTrackSelected))
                {
                    FilterBusses(ShowNearBusses, _busTrackSelected);
                    //GetParadasYRecorrido(_busLineSelected);
                }
            }
        }
        public bool ShowNearBusses
        {
            get { return _showNearBusses; }
            set
            {
                _showNearBusses = value;
                RaisePropertyChanged();
            }
        }
        public MapControl MyMapControl
        {
            get;
            set;
        }

        public NearFromViewModel()
        {
            BusTracks = new UIObservableCollection<string>(CommonModel.TracksNamesForNearFrom);
            BusTrackSelected = "Todos los Recorridos";
            _lastTrackSelected = "Todos los Recorridos";
            Task busses = GetBussesTask();
        }

        public async System.Threading.Tasks.Task GetBussesTask()
        {
            IsBusy = true;
            try
            {
                var allBondis = await Requests.RequestGetLastestPositions2();
                Busses bs = new Busses();
                bs.Busseses = new UIObservableCollection<Bus>();

                CleanMapElements(); 
                CommonModel.CurrentBusses.Busseses.Clear();
                foreach (Feature feature in allBondis.Data.features)
                {
                    var newBus = new Bus() { ImageUri = feature.imageUrl, RawPointString = feature.wkt.ToString(), Nombre = feature.properties.MovilNombre };
                    bs.Busseses.Add(newBus);
                    CommonModel.BusFeatures.Add(feature);
                    CommonModel.CurrentBusses.Busseses.Add(newBus);
                }

                SetBusses(bs);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error in GetbussesTask: " + ex.Message);
                IsBusy = false;
            }
        }

        public async void SetBusses(Busses busses)
        {
            try
            {
                foreach (Bus bus in busses.Busseses)
                {
                    var pointIcon = new MapIcon();
                    var geoPoint = new Geopoint(bus.Position, AltitudeReferenceSystem.Terrain);

                    pointIcon.NormalizedAnchorPoint = new Point(0.5, 1);
                    pointIcon.Location = geoPoint;
                    pointIcon.Title = bus.Nombre;
                    pointIcon.Image = SetAssetBus(bus.ImageUri);

                    AddElementToMap(pointIcon);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in SetBusses: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public RandomAccessStreamReference SetAssetBus(string imageUri)
        {
            int i = imageUri.LastIndexOf('/');
            string colorBusIcon = imageUri.Substring(i + 1);
            return RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/Bus/{0}", colorBusIcon)));
        }

        private async void FilterBusses(bool colectivosCercanos, string recorrido = "Todos los Recorridos")
        {
            await DispatcherHelper.RunAsync(async () =>
            {
                try
                {
                //Si no desea filtrar por recorrido y estaba filtrando por recorrido
                if ((recorrido == "Todos los Recorridos") && _isFilteringByTrack)
                {
                    IsBusy = true;
                    CleanMapElements();
                    _lastTrackSelected = "Todos los Recorridos";
                    SetBusses(CommonModel.CurrentBusses);
                    
                }
                //Si desea filtrar por recorrido y no estaba filtrando por recorrido o seleccionó otro recorrido
                else if (recorrido != "Todos los Recorridos" && (!_isFilteringByTrack || recorrido != _lastTrackSelected))
                {
                    IsBusy = true;
                    CleanMapElements();
                    Busses busses = new Busses();
                    busses.Busseses = new UIObservableCollection<Bus>();
                    foreach (Feature feature in CommonModel.BusFeatures)
                    {
                        if (feature.properties.NombreRecorrido == recorrido)
                        {
                            busses.Busseses.Add(new Bus() { ImageUri = feature.imageUrl, RawPointString = feature.wkt.ToString(), Nombre = feature.properties.MovilNombre });
                        }
                    }
                    
                    _lastTrackSelected = recorrido;
                    SetBusses(busses);
                    await MyMapControl.TrySetViewAsync(new Geopoint(busses.Busseses[0].Position), 11);
                }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error on FIlterBusses: " + ex.Message);
                }
            });
        }

        public void AddElementToMap(MapElement elementToAdd)
        {
            if (MyMapControl != null && elementToAdd != null)
            {
                MyMapControl.MapElements.Add(elementToAdd);
                CommonModel.NearFromPageMapElements.Add(elementToAdd);
            }
        }
        public void CleanMapElements()
        {
            if (MyMapControl != null)
            {
                foreach (MapElement element in CommonModel.NearFromPageMapElements)
                {
                    if (MyMapControl.MapElements.Contains(element))
                        MyMapControl.MapElements.Remove(element);
                }
                CommonModel.NearFromPageMapElements.Clear();
            }
        }

        public override System.Threading.Tasks.Task OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            return null;
        }

        public override System.Threading.Tasks.Task OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs args)
        {
            return null;
        }

        public override System.Threading.Tasks.Task OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            return null;
        }

        public override System.Threading.Tasks.Task BackKeyPressed(BackPressedEventArgs args)
        {
            return null;
        }

        public override void CleanViewModel()
        {
        }
    }
}
