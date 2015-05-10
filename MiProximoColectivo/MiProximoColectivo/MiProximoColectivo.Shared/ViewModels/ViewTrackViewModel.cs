using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.Groups;
using MiProximoColectivo.Classes.ServerReceived;
using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls.Maps;

namespace MiProximoColectivo.ViewModels
{
    public class ViewTrackViewModel : ViewModelBase
    {
        private UIObservableCollection<RecorridoYParadas> _stopsAndTracks;
        private UIObservableCollection<string> _tracksNames;
        private string _selectedTrack;
        
        public UIObservableCollection<RecorridoYParadas> StopsAndTracks
        {
            get { return _stopsAndTracks; }
            set
            {
                _stopsAndTracks = value;
                RaisePropertyChanged();
            }
        }
        public UIObservableCollection<string>TracksNames
        {
            get { return _tracksNames; }
            set
            {
                _tracksNames = value;
                RaisePropertyChanged();
            }
        }
        public string SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value;
                RaisePropertyChanged();
            }
        }
        public MapControl MyMapControl
        {
            get; 
            set; 
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
            TracksNames = new UIObservableCollection<string>(Model.CommonModel.TracksNames);
            
            return null;
        }

        /// <summary>
        /// Debe ser llamada luego de completar el GET de Recorrido y Paradas para el recorrido seleccionado
        /// </summary>
        /// <param name="recorridoYParadas"></param>
        public void SetParadasYRecorridos(RecorridoYParadas recorridoYParadas)
        {

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
