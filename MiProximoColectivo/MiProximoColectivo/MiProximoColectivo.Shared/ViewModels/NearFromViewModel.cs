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

        private bool pidoDatos = true;
        private bool firstRemove = false;
        private bool isDownloadingBusses;

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
                    _isFilteringByTrack = (_busTrackSelected != "Todos los Recorridos");
                    //FilterBusses(ShowNearBusses, _busTrackSelected);
                    FilterBusses2(false, ShowNearBusses, _busTrackSelected);
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
            DownloadBusses();
        }





        private Task bussesTask;
        private async void DownloadBusses()
        {
            while (true)
            {
                if (pidoDatos)
                {
                    try
                    {
                        bussesTask = GetBussesTask2();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                    }
                }

                await Task.Delay(12000);
                //await Task.Run(() => { new System.Threading.ManualResetEvent(false).WaitOne(10000); });
                
            }
        }
      
        //public async Task GetBussesTask()
        //{
        //    isDownloadingBusses = true;
        //    //IsBusy = true;

        //    try
        //    {
        //        var allBondis = await Requests.RequestGetLastestPositions2();
        //        Busses bs = new Busses();
        //        bs.Busseses = new UIObservableCollection<Bus>();

        //        //CleanMapElements(); 
        //        CommonModel.CurrentBusses.Busseses.Clear();
        //        foreach (Feature feature in allBondis.Data.features)
        //        {
        //            var newBus = new Bus() { ImageUri = feature.imageUrl, RawPointString = feature.wkt.ToString(), Nombre = feature.properties.MovilNombre, Track = feature.properties.NombreRecorrido };
        //            bs.Busseses.Add(newBus);
        //            CommonModel.BusFeatures.Add(feature);
        //            CommonModel.CurrentBusses.Busseses.Add(newBus);
        //        }

        //        isDownloadingBusses = false;

        //        if(!firstRemove || (_busTrackSelected == "Todos los Recorridos" && !_isFilteringByTrack))
        //            SetBusses(bs, firstRemove);
        //        else
        //            FilterBusses(ShowNearBusses, _busTrackSelected);
                
        //        //SetBusses(bs);
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine("Error in GetbussesTask: " + ex.Message);
        //        IsBusy = false;
        //        isDownloadingBusses = false;
        //    }
        //}
        public async Task GetBussesTask2()
        {
            isDownloadingBusses = true;
            //IsBusy = true;

            try
            {
                var downloadedBusses = await Requests.RequestGetLastestPositions2();
                Busses bs = new Busses();
                bs.Busseses = new UIObservableCollection<Bus>();

                //CleanMapElements(); 
                CommonModel.BusFeatures.Clear();
                CommonModel.CurrentBusses.Busseses.Clear();                
                foreach (Feature feature in downloadedBusses.Data.features)
                {
                    var newBus = new Bus() { ImageUri = feature.imageUrl, RawPointString = feature.wkt.ToString(), Nombre = feature.properties.MovilNombre, Track = feature.properties.NombreRecorrido };

                    bs.Busseses.Add(newBus);
                    CommonModel.BusFeatures.Add(feature);
                    CommonModel.CurrentBusses.Busseses.Add(newBus);
                }

                isDownloadingBusses = false;

                if (!firstRemove || (_busTrackSelected == "Todos los Recorridos" && !_isFilteringByTrack))
                    SetBusses2(bs);
                else
                    FilterBusses2(true, ShowNearBusses, _busTrackSelected);

                //SetBusses(bs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GetbussesTask: " + ex.Message);
                IsBusy = false;
                isDownloadingBusses = false;
            }
        }

        public void SetBusses2(Busses busses, string condicionPorRecorrido = "")
        {
            int c = 0;
            try
            {
                foreach (Bus bus in busses.Busseses)
                {
                    int index = c;

                    if(index == 5)
                    {

                    }
                    //Si se debe remover este Bus
                    if (CommonModel.NearFromPageMapBusses.Busseses.Count > 0 && CommonModel.NearFromPageMapBusses.Busseses.Contains(bus))
                    {
                        int indexOfBus = CommonModel.NearFromPageMapBusses.Busseses.IndexOf(bus);
                        MyMapControl.MapElements.RemoveAt(indexOfBus);
                        //CommonModel.NearFromPageMapElements.RemoveAt(indexOfBus);
                        CommonModel.NearFromPageMapBusses.Busseses.RemoveAt(indexOfBus);
                        index = indexOfBus;              
                    }
                    firstRemove = true;

                    if (condicionPorRecorrido == "" || bus.Track == condicionPorRecorrido)
                    {
                        var pointIcon = new MapIcon();
                        var geoPoint = new Geopoint(bus.Position, AltitudeReferenceSystem.Terrain);

                        pointIcon.NormalizedAnchorPoint = new Point(0.5, 1);
                        pointIcon.Location = geoPoint;
                        pointIcon.Title = bus.Nombre;
                        pointIcon.Image = SetAssetBus(bus.ImageUri);

                        AddElementToMapAt2(pointIcon, index, bus);
                    }
                    c++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in SetBusses: " + ex.Message + " - Index: " + c);
            }
            finally
            {

            }
        }

                
        //public void SetBusses(Busses busses, bool remove, string condicionPorRecorrido = "")
        //{
        //    try
        //    {
        //        int c = 0;
        //        foreach (Bus bus in busses.Busseses)
        //        {
        //            var pointIcon = new MapIcon();
        //            var geoPoint = new Geopoint(bus.Position, AltitudeReferenceSystem.Terrain);

        //            pointIcon.NormalizedAnchorPoint = new Point(0.5, 1);
        //            pointIcon.Location = geoPoint;
        //            pointIcon.Title = bus.Nombre;
        //            pointIcon.Image = SetAssetBus(bus.ImageUri);
                                        
        //            //Si se debe remover este Bus
        //            if (remove && CommonModel.NearFromPageMapBusses.Busseses.Contains(bus))
        //            {
        //                int indexOfBus = CommonModel.NearFromPageMapBusses.Busseses.IndexOf(bus);

        //                CommonModel.NearFromPageMapElements.RemoveAt(indexOfBus);
        //                CommonModel.NearFromPageMapBusses.Busseses.RemoveAt(indexOfBus);
        //                MyMapControl.MapElements.RemoveAt(indexOfBus);                        
        //            }
        //            firstRemove = true;

        //            if(condicionPorRecorrido == "" || bus.Track == condicionPorRecorrido)
        //                AddElementToMapAt(pointIcon, c,pointIcon.Title);
        //            c++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Error in SetBusses: " + ex.Message);
        //    }
        //    finally
        //    {
                   
        //    }
        //}

        public RandomAccessStreamReference SetAssetBus(string imageUri)
        {
            int i = imageUri.LastIndexOf('/');
            string colorBusIcon = imageUri.Substring(i + 1);
            return RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///Assets/Bus/{0}", colorBusIcon)));
        }

        //private async void FilterBusses(bool colectivosCercanos, string recorrido = "Todos los Recorridos")
        //{
        //    await DispatcherHelper.RunAsync(async () =>
        //    {
        //        try
        //        {
        //            //Si no esta descargando datos nuevos
        //            if (!isDownloadingBusses)
        //            {
        //                //Si no desea filtrar por recorrido y estaba filtrando por recorrido
        //                if ((recorrido == "Todos los Recorridos") && _isFilteringByTrack)
        //                {
        //                    IsBusy = true;
        //                    //CleanMapElements();
        //                    _lastTrackSelected = "Todos los Recorridos";
        //                    SetBusses(CommonModel.CurrentBusses, firstRemove);

        //                }
        //                //Si desea filtrar por recorrido y no estaba filtrando por recorrido o seleccionó otro recorrido
        //                else if (recorrido != "Todos los Recorridos" && (!_isFilteringByTrack || recorrido != _lastTrackSelected))
        //                {
        //                    IsBusy = true;
        //                    //CleanMapElements();
        //                    /*Busses busses = new Busses();
        //                    busses.Busseses = new UIObservableCollection<Bus>();
        //                    foreach (Feature feature in CommonModel.BusFeatures)
        //                    {
        //                        if (feature.properties.NombreRecorrido == recorrido)
        //                        {
        //                            busses.Busseses.Add(new Bus() { ImageUri = feature.imageUrl, RawPointString = feature.wkt.ToString(), Nombre = feature.properties.MovilNombre });
        //                        }
        //                    }
        //                    */
        //                    _lastTrackSelected = recorrido;
        //                    SetBusses(CommonModel.CurrentBusses, firstRemove, recorrido);
        //                    //SetBusses(busses, firstRemove);
        //                    await MyMapControl.TrySetViewAsync(new Geopoint(CommonModel.CurrentBusses.Busseses[0].Position), 11);
        //                }
        //                IsBusy = false;
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            Debug.WriteLine("Error on FIlterBusses: " + ex.Message);
        //            IsBusy = false;
        //        }
        //        finally
        //        {
                    
        //        }
        //    });
        //}
        private async void FilterBusses2(bool seteoForzado, bool colectivosCercanos, string recorrido = "Todos los Recorridos")
        {
            await DispatcherHelper.RunAsync(async () =>
            {
                try
                {
                    //Si no esta descargando datos nuevos
                    if (!isDownloadingBusses)
                    {
                        //Si no desea filtrar por recorrido y estaba filtrando por recorrido
                        if ((recorrido == "Todos los Recorridos") && (_isFilteringByTrack || seteoForzado))
                        {
                            IsBusy = true;
                            //CleanMapElements();
                            _lastTrackSelected = "Todos los Recorridos";
                            SetBusses2(CommonModel.CurrentBusses);
                        }
                        //Si desea filtrar por recorrido y no estaba filtrando por recorrido o seleccionó otro recorrido
                        else if (recorrido != "Todos los Recorridos" && (!_isFilteringByTrack || recorrido != _lastTrackSelected || seteoForzado))
                        {
                            IsBusy = true;
                            _lastTrackSelected = recorrido;
                            SetBusses2(CommonModel.CurrentBusses, recorrido);
                            
                            //Solo centro las nuevas paradas en el mapa  si son nuevas, y no si fueron llamadas al 
                            //actualizar los datos actuales.
                            if(recorrido != _lastTrackSelected)
                                await MyMapControl.TrySetViewAsync(new Geopoint(CommonModel.NearFromPageMapBusses.Busseses[0].Position), 11);
                        }
                        IsBusy = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error on FilterBusses: " + ex.Message);
                    IsBusy = false;
                }
                finally
                {

                }
            });
        }

        public void AddElementToMap(MapElement elementToAdd, string name)
        {
            if (MyMapControl != null && elementToAdd != null)
            {
                MyMapControl.MapElements.Add(elementToAdd);
                CommonModel.NearFromPageMapElements.Add(elementToAdd);
                CommonModel.NearFromPageMapBusses.Busseses.Add(new Bus(name));
            }
        }
        public void AddElementToMapAt(MapElement elementToAdd, int index, string name)
        {
            if (MyMapControl != null && elementToAdd != null)
            {
                MyMapControl.MapElements.Insert(index, elementToAdd);
                CommonModel.NearFromPageMapElements.Insert(index, elementToAdd);
                CommonModel.NearFromPageMapBusses.Busseses.Insert(index, new Bus(name));
            }
        }
        public void AddElementToMapAt2(MapElement elementToAdd, int index, Bus busOfElementToAdd)
        {
            if (MyMapControl != null && elementToAdd != null)
            {
                if (index < MyMapControl.MapElements.Count)
                {
                    MyMapControl.MapElements.Insert(index, elementToAdd);
                    //CommonModel.NearFromPageMapElements.Insert(index, elementToAdd);
                    CommonModel.NearFromPageMapBusses.Busseses.Insert(index, busOfElementToAdd);
                }
                else
                {
                    MyMapControl.MapElements.Add(elementToAdd);
                    //CommonModel.NearFromPageMapElements.Insert(index, elementToAdd);
                    CommonModel.NearFromPageMapBusses.Busseses.Add(busOfElementToAdd);
                }
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
                CommonModel.NearFromPageMapBusses.Busseses.Clear();
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
