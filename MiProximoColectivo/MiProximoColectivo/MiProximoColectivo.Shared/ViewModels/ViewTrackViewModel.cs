using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.Groups;
using MiProximoColectivo.Classes.ServerReceived;
using MiProximoColectivo.Model;
using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Foundation;
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

        public ViewTrackViewModel()
        {
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

            RecorridoYParadas rp = new RecorridoYParadas();
            rp.Stops = new UIObservableCollection<MpcPuntoControl>();
            rp.Stops.Add(new MpcPuntoControl() { RawPointString = "POINT (-66.338824 -33.271422)" });
            rp.Stops.Add(new MpcPuntoControl() { RawPointString = "POINT (-66.338127 -33.278207)" });
            rp.Stops.Add(new MpcPuntoControl() { RawPointString = "POINT (-66.339588 -33.260385)" });

            SetParadasYRecorrido(rp);
            //-66.342074 -33.29091

            return null;
        }

        /// <summary>
        /// Debe ser llamada luego de completar el GET de Recorrido y Paradas para el recorrido seleccionado
        /// </summary>
        /// <param name="recorridoYParadas"></param>
        public async void SetParadasYRecorrido(RecorridoYParadas recorridoYParadas)
        {
            //Ahora agrego las paradas al mapa
            if(recorridoYParadas != null && recorridoYParadas.Stops != null && recorridoYParadas.Stops.Count > 0)
            {
                CleanMapElements();
                int contador = 0;
                foreach (MpcPuntoControl stop in recorridoYParadas.Stops)
                {
                    var pointIcon = new MapIcon();
                    var geoPoint = new Geopoint(stop.Position, AltitudeReferenceSystem.Terrain);
                    
                    pointIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                    pointIcon.Location = geoPoint;
                    pointIcon.Title = "Parada " + contador.ToString() ;

                    AddElementToMap(pointIcon);
                    contador++;
                }
                await MyMapControl.TrySetViewAsync(new Geopoint(recorridoYParadas.Stops[0].Position),12);

                Debug.WriteLine(CommonModel.DistanceBetweenGeopoints(recorridoYParadas.Stops[0].Position, recorridoYParadas.Stops[1].Position));
                Debug.WriteLine(CommonModel.DistanceBetweenGeopoints(CommonModel.DevicePosition.Coordinate.Point.Position, recorridoYParadas.Stops[1].Position));
            }
        }
        public void AddElementToMap(MapElement elementToAdd) 
        {
            if (MyMapControl != null && elementToAdd != null)
            {
                MyMapControl.MapElements.Add(elementToAdd);
                CommonModel.ViewTrackMapElements.Add(elementToAdd);
            }
        }
        public void CleanMapElements()
        {
            if (MyMapControl != null)
            {
                foreach (MapElement element in CommonModel.ViewTrackMapElements)
                {
                    if (MyMapControl.MapElements.Contains(element))
                        MyMapControl.MapElements.Remove(element);                        
                }
                CommonModel.ViewTrackMapElements.Clear();
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
