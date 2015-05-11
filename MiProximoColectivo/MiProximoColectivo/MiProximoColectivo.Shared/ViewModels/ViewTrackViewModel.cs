using GalaSoft.MvvmLight.Threading;
using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.Groups;
using MiProximoColectivo.Classes.RequestTasks;
using MiProximoColectivo.Classes.ServerReceived;
using MiProximoColectivo.Model;
using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
        int reintentosGetRecorridoYParadas;
        private RequestTask<RecorridoYParadas> _recorridoYParadasTask;
        
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

                //Si no es un string vacion o nulo, hago el GetRecorrido
                if(!string.IsNullOrEmpty(_selectedTrack) && !string.IsNullOrWhiteSpace(_selectedTrack))
                {
                    GetParadasYRecorrido(_selectedTrack);
                }
            }
        }
        public MapControl MyMapControl
        {
            get; 
            set; 
        }

        public ViewTrackViewModel()
        {
            reintentosGetRecorridoYParadas = 0;
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
            TracksNames = new UIObservableCollection<string>(Model.CommonModel.TracksNames);

            RecorridoYParadas rp = new RecorridoYParadas();
            /*rp.Stops = new UIObservableCollection<MpcPuntoControl>();
            rp.Stops.Add(new MpcPuntoControl() { RawPointString = "POINT (-66.338824 -33.271422)" });
            rp.Stops.Add(new MpcPuntoControl() { RawPointString = "POINT (-66.338127 -33.278207)" });
            rp.Stops.Add(new MpcPuntoControl() { RawPointString = "POINT (-66.339588 -33.260385)" });

            await SetParadasYRecorrido(rp);                        */
        }

        public void GetParadasYRecorrido(string recorrido)
        {
            if (CommonModel.TracksIds.ContainsKey(recorrido))
            {
                IsBusy = true;
                int idRecorrido = CommonModel.TracksIds[recorrido];
                _recorridoYParadasTask = new RequestTask<RecorridoYParadas>(() => Functions.Requests.GetParadasYRecorridoById(idRecorrido), true);
                _recorridoYParadasTask.Completed += RecorridoYParadasTask_Completed;
                _recorridoYParadasTask.Failed += RecorridoYParadasTask_Failed;
                _recorridoYParadasTask.TryStart();
            }
        }

        private void RecorridoYParadasTask_Failed(object sender, RequestTaskFailedEventArgs<RecorridoYParadas> e)
        {
            if(e.Reason == "Not Found" && reintentosGetRecorridoYParadas < 2)
            {
                GetParadasYRecorrido(SelectedTrack);
                reintentosGetRecorridoYParadas++;
            }
            else
            {
                IsBusy = true;
                Debug.WriteLine("No se pudo solicitar el recorrido: " + e.ResponseAsString);
                reintentosGetRecorridoYParadas = 0;
                ShowDialog("No se pudo solicitar el recorrido: " + e.Reason);
            }
        }

        private async void RecorridoYParadasTask_Completed(object sender, RequestTaskCompletedEventArgs<RecorridoYParadas> e)
        {
            reintentosGetRecorridoYParadas = 0;
            await SetParadasYRecorrido(e.Result);
        }

        /// <summary>
        /// Debe ser llamada luego de completar el GET de Recorrido y Paradas para el recorrido seleccionado
        /// </summary>
        /// <param name="recorridoYParadas"></param>
        public async Task SetParadasYRecorrido(RecorridoYParadas recorridoYParadas)
        {
            try
            {
                //Ahora agrego las paradas al mapa
                if (recorridoYParadas != null && recorridoYParadas.Stops != null && recorridoYParadas.Stops.Count > 0)
                {
                    bool addingElementsError = false;
                    await DispatcherHelper.RunAsync(() =>
                    {
                        int contador = 0;
                        try
                        {
                            CleanMapElements();

                            foreach (MpcPuntoControl stop in recorridoYParadas.Stops)
                            {
                                var pointIcon = new MapIcon();
                                var geoPoint = new Geopoint(stop.Position, AltitudeReferenceSystem.Terrain);

                                pointIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                                pointIcon.Location = geoPoint;
                                pointIcon.Title = "Parada " + contador.ToString();
                                AddElementToMap(pointIcon);
                                contador++;
                            }
                        }
                        catch (Exception ex)
                        {
                            addingElementsError = true;
                            Debug.WriteLine("Exception adding Recorrido at position: {0} - Exception: {1}", contador, ex.Message);
                            ShowDialog(string.Format("Exception adding Recorrido at position: {0} - Exception: {1}", contador, ex.Message));
                        }
                    });
                    if (!addingElementsError)
                    {
                        await MyMapControl.TrySetViewAsync(new Geopoint(recorridoYParadas.Stops[0].Position), 12);

                        Debug.WriteLine(CommonModel.DistanceBetweenGeopoints(recorridoYParadas.Stops[0].Position, recorridoYParadas.Stops[1].Position));
                        if (CommonModel.DevicePosition != null)
                            Debug.WriteLine(CommonModel.DistanceBetweenGeopoints(CommonModel.DevicePosition.Coordinate.Point.Position, recorridoYParadas.Stops[1].Position));
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception en SetParadasYRecorridos: Exception: {0}", ex.Message);
                ShowDialog(string.Format("Exception en SetParadasYRecorridos: Exception: {0}", ex.Message));
            }
            finally
            {
                IsBusy = false;
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
