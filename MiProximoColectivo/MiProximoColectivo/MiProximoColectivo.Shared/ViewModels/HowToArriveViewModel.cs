using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using MiProximoColectivo.Model;

namespace MiProximoColectivo.ViewModels
{
    public class HowToArriveViewModel : ViewModelBase
    {
        private string _tbParada1;
        private string _tbParada2;
        private string _tbParada3;
        private string _tbParada4;
        private string _tbParada5;

        public string tbParada1
        {
            get { return _tbParada1; }
            set
            {
                _tbParada1 = value;
                RaisePropertyChanged();
            }
        }

        public string tbParada2
        {
            get { return _tbParada2; }
            set
            {
                _tbParada2 = value;
                RaisePropertyChanged();
            }
        }

        public string tbParada3
        {
            get { return _tbParada3; }
            set
            {
                _tbParada3 = value;
                RaisePropertyChanged();
            }
        }

        public string tbParada4
        {
            get { return _tbParada4; }
            set
            {
                _tbParada4 = value;
                RaisePropertyChanged();
            }
        }

        public string tbParada5
        {
            get { return _tbParada5; }
            set
            {
                _tbParada5 = value;
                RaisePropertyChanged();
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

        public async override System.Threading.Tasks.Task OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            try
            {
                string[] paradaCoordinates;
                paradaCoordinates =  ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido1"] as string[];
                tbParada1 = paradaCoordinates[0].ToString();

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido2"] as string[];
                tbParada2 = paradaCoordinates[0].ToString();

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido3"] as string[];
                tbParada3 = paradaCoordinates[0].ToString();

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido4"] as string[];
                tbParada4 = paradaCoordinates[0].ToString();

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido5"] as string[];
                tbParada5 = paradaCoordinates[0].ToString();
            }
            catch (Exception e)
            {
            }
            return;
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
