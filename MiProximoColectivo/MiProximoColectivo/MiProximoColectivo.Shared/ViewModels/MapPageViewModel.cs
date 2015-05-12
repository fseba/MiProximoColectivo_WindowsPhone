using GalaSoft.MvvmLight.Command;
using MiProximoColectivo.Model;
using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.Groups;
using MiProximoColectivo.Classes.Local;
using MiProximoColectivo.Classes.RequestTasks;
using MiProximoColectivo.Classes.ServerReceived;
using MiProximoColectivo.Functions;

namespace MiProximoColectivo.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private RelayCommand _hideMenuCommand;
        private RelayCommand _centerOnDeviceLocationCommand;
        private RelayCommand _cleanMapCommand;
        private MapIcon _devicePositionIcon;
        private Visibility _menuVisible;
        private string _label;
        private SymbolIcon _icon;
        private bool pidoDatos = true;
        private bool firstRemove = false;

        public SymbolIcon Icon
        {
            get { return _icon; }
            set{ _icon = value; RaisePropertyChanged(); }
        }
        public string Label
        {
            get { return _label; }
            set { _label = value; RaisePropertyChanged(); }
        }
        

        public Visibility MenuVisible
        {
            get { return _menuVisible; }
            set { _menuVisible = value; RaisePropertyChanged(); }
        }

        public MapControl MyMapControl
        {
            get;
            set;
        }
        public MapIcon DevicePositionIcon
        {
            get { return _devicePositionIcon; }
            set
            {
                _devicePositionIcon = value;
                RaisePropertyChanged();
            }
        }
        public bool DevicePositionReady
        {
            get { return CommonModel.DevicePositionReady; }
        }
        public MapPageViewModel()
        {
            
        }

        private void DeviceLocator_StatusChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.StatusChangedEventArgs args)
        {            
            RaisePropertyChanged("DevicePositionReady");

            if (args.Status == Windows.Devices.Geolocation.PositionStatus.Initializing)
                ShowStatusBarProgressIndicator(args.Status.ToString());
            //else
            //{

                //  HideStatusBarProgressIndicator(args.Status.ToString());
            //}
        }

        private void DeviceLocator_PositionChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            CommonModel.DevicePosition = args.Position;

            RaisePropertyChanged("DevicePositionReady");
            if(MyMapControl != null)
            {
                try
                {
                    if (MyMapControl.MapElements.Contains(DevicePositionIcon))
                        MyMapControl.MapElements.Remove(DevicePositionIcon);
                }
                catch (Exception ex)
                {
                    
                }

                DevicePositionIcon = new MapIcon();
                DevicePositionIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                DevicePositionIcon.Location = args.Position.Coordinate.Point;
                DevicePositionIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/Bus/{0}", "MyLocation_PushPin.png")));
                // Get the text to display above the map icon from the resource files.
                DevicePositionIcon.Title = "Estás aquí";
                MyMapControl.MapElements.Add(DevicePositionIcon);
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

        public override async System.Threading.Tasks.Task OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            HideStatusBarProgressIndicator();
            MenuVisible = Visibility.Visible;
            Label = "Ocultar Menú";
            Icon = new SymbolIcon(Symbol.Download); 
            try
            {
                CommonModel.DeviceLocator = new Windows.Devices.Geolocation.Geolocator();
                CommonModel.DeviceLocator.MovementThreshold = 15;
                //CommonModel.DeviceLocator.DesiredAccuracyInMeters = 50;
                CommonModel.DeviceLocator.StatusChanged += DeviceLocator_StatusChanged;

                ShowStatusBarProgressIndicator("Buscando posición en el mapa...");

                CommonModel.DevicePosition = await CommonModel.DeviceLocator.GetGeopositionAsync();

                RaisePropertyChanged("DevicePositionReady");

                // Various display options
                MyMapControl.LandmarksVisible = false;
                MyMapControl.TrafficFlowVisible = false;
                MyMapControl.PedestrianFeaturesVisible = false;

                if (DevicePositionIcon != null && MyMapControl.MapElements.Contains(DevicePositionIcon))
                    MyMapControl.MapElements.Remove(DevicePositionIcon);

                DevicePositionIcon = new MapIcon();
                DevicePositionIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                DevicePositionIcon.Location = CommonModel.DevicePosition.Coordinate.Point;
                DevicePositionIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/Bus/{0}", "MyLocation_PushPin.png")));
                // Get the text to display above the map icon from the resource files.
                DevicePositionIcon.Title = "Estás aquí";
                MyMapControl.MapElements.Add(DevicePositionIcon);

                DownloadBusses();


#if WINDOWS_PHONE_APP
                CenterOnDeviceLocationCommandDelegate();
#endif
                HideStatusBarProgressIndicator();
                CommonModel.DeviceLocator.PositionChanged += DeviceLocator_PositionChanged;
            }
            catch (Exception ex)
            {
                HideStatusBarProgressIndicator("No se ha podido cargar tu ubicación");
            }
            finally
            {
                
            }
        }

        

        public async System.Threading.Tasks.Task GetBussesTask()
        {
            var allBondis = await Requests.RequestGetLastestPositions2();
            Busses bs = new Busses();
            bs.Busseses = new UIObservableCollection<Bus>();
            foreach (Feature feature in allBondis.Data.features)
            {
                bs.Busseses.Add(new Bus() { ImageUri = feature.imageUrl, RawPointString = feature.wkt.ToString(), Nombre = feature.properties.MovilNombre });
            }
            SetBusses(bs,firstRemove);
        }

        private Task busses;
        private async void DownloadBusses()
        {
            while (true)
            {
                if (pidoDatos)
                {
                    try
                    {
                        busses = GetBussesTask();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                await Task.Run(() => { new System.Threading.ManualResetEvent(false).WaitOne(10000); });
            }
        }

        public async void SetBusses(Busses busses, bool remove)
        {
            int c = 0;
            foreach (Bus bus in busses.Busseses)
            {
                var pointIcon = new MapIcon();
                var geoPoint = new Geopoint(bus.Position, AltitudeReferenceSystem.Terrain);

                pointIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                pointIcon.Location = geoPoint;
                pointIcon.Title = bus.Nombre;
                pointIcon.Image = SetAssetBus(bus.ImageUri);

                if (remove) { 
                    MyMapControl.MapElements.RemoveAt(c);
                    CommonModel.MapPageMapElements.RemoveAt(c);
                }
                firstRemove = true;
                AddElementToMapAt(pointIcon,c);
                c++;
            }
        }

        public RandomAccessStreamReference SetAssetBus(string imageUri)
        {
            int i = imageUri.LastIndexOf('/');
            string colorBusIcon = imageUri.Substring(i + 1);
            return RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/Bus/{0}", colorBusIcon)));
        }


        public void AddElementToMap(MapElement elementToAdd)
        {
            if (MyMapControl != null && elementToAdd != null)
            {
                MyMapControl.MapElements.Add(elementToAdd);
                CommonModel.MapPageMapElements.Add(elementToAdd);
            }
        }

        public void AddElementToMapAt(MapElement elementToAdd, int index)
        {
            if (MyMapControl != null && elementToAdd != null)
            {
                MyMapControl.MapElements.Insert(index,elementToAdd);
                CommonModel.MapPageMapElements.Insert(index, elementToAdd);
            }
        }

        public RelayCommand CenterOnDeviceLocationCommand
        {
            get { return _centerOnDeviceLocationCommand ?? (_centerOnDeviceLocationCommand = new RelayCommand(CenterOnDeviceLocationCommandDelegate)); }
        }

        public async void CenterOnDeviceLocationCommandDelegate()
        {
            bool b = false;
#if WINDOWS_PHONE_APP
            if (MyMapControl != null && CommonModel.DevicePosition != null)
                b = await MyMapControl.TrySetViewAsync(CommonModel.DevicePosition.Coordinate.Point, 15);
#endif
        }

        public RelayCommand HideMenuCommand
        {
            get { return _hideMenuCommand ?? (_hideMenuCommand = new RelayCommand(HideMenuCommandDelegate)); }
        }
        public RelayCommand CleanCommand
        {
            get { return _cleanMapCommand ?? (_cleanMapCommand = new RelayCommand(CleanMapCommandDelegate)); }
        }

        public void HideMenuCommandDelegate()
        {
            if (MenuVisible == Visibility.Visible)
            {
                Label = "Mostrar Menú";
                Icon = new SymbolIcon(Symbol.Upload); 
                MenuVisible = Visibility.Collapsed;
            }
            else
            {
                Label = "Ocultar Menú";
                Icon = new SymbolIcon(Symbol.Download); 
                MenuVisible = Visibility.Visible;
            }
        }
        public void CleanMapCommandDelegate()
        {
            if(MyMapControl != null)
            {
                try
                {
                    MyMapControl.MapElements.Clear();
                    if(DevicePositionIcon != null)
                        MyMapControl.MapElements.Add(DevicePositionIcon);
                    CommonModel.MapPageMapElements.Clear();
                    CommonModel.ViewTrackMapElements.Clear();                    
                }
                catch(Exception ex)
                { }
            }
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
