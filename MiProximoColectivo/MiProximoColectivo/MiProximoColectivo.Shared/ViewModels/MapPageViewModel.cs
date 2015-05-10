using GalaSoft.MvvmLight.Command;
using MiProximoColectivo.Model;
using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls.Maps;

namespace MiProximoColectivo.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private RelayCommand _centerOnDeviceLocationCommand;
        private MapIcon _devicePositionIcon;


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
                if (MyMapControl.MapElements.Contains(DevicePositionIcon))
                    MyMapControl.MapElements.Remove(DevicePositionIcon);

                DevicePositionIcon = new MapIcon();
                DevicePositionIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                DevicePositionIcon.Location = args.Position.Coordinate.Point;
                
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
            /*RequestTask<BankCard> requestAddCardTask = new RequestTask<BankCard>(() => Requests.RequestAddCard(Card), true);
            requestAddCardTask.TryStart();*/
            HideStatusBarProgressIndicator();
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
                DevicePositionIcon.NormalizedAnchorPoint = new Point(1.5, 2);
                DevicePositionIcon.Location = CommonModel.DevicePosition.Coordinate.Point;
                
                // Get the text to display above the map icon from the resource files.
                DevicePositionIcon.Title = "Estás aquí";
                MyMapControl.MapElements.Add(DevicePositionIcon);

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

        public override System.Threading.Tasks.Task BackKeyPressed(BackPressedEventArgs args)
        {
            return null;         
        }

        public override void CleanViewModel()
        {
        }
    }
}
