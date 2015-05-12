using MiProximoColectivo.ViewModels;
using MiProximoColectivo.Views.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MiProximoColectivo.Model;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkID=390556

namespace MiProximoColectivo.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MapPage : PageBase
    {
        public MapPage()
        {
            this.InitializeComponent();

            //Indico a los ViewModels cual será el mapa a modificar
            (this.DataContext as MapPageViewModel).MyMapControl = myMapControl;
            (NearFromPivotItem.DataContext as NearFromViewModel).MyMapControl = myMapControl;            
            (ViewTrackPivotItem.DataContext as ViewTrackViewModel).MyMapControl = myMapControl;            
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                var pivotItemActual = e.AddedItems[0] as PivotItem;
                
                switch(pivotItemActual.Name)
                {
                    case "NearFromPivotItem":
                        (pivotItemActual.DataContext as NearFromViewModel).OnNavigatedTo(null);
                        break;
                    case "HowToArrivePivotItem":
                        (pivotItemActual.DataContext as HowToArriveViewModel).OnNavigatedTo(null);
                        break;
                    case "ViewTrackPivotItem":
                        (pivotItemActual.DataContext as ViewTrackViewModel).OnNavigatedTo(null);
                        break;
                }
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tbox = e.ClickedItem as TextBlock;
            string name = tbox.Text;
            string saved = "";
            Color color = new Color();

            try
            {
                string[] paradaCoordinates;
                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido1"] as string[];

                if (paradaCoordinates[0].ToString() == name)
                {
                    saved = "TemporalCoordinatesRecorrido1";
                    color = Colors.Blue;
                }

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido2"] as string[];

                if (paradaCoordinates[0].ToString() == name)
                {
                    saved = "TemporalCoordinatesRecorrido2";
                    color = Colors.Green;
                }

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido3"] as string[];

                if (paradaCoordinates[0].ToString() == name)
                {
                    saved = "TemporalCoordinatesRecorrido3";
                    color = Colors.Gold;
                }

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido4"] as string[];

                if (paradaCoordinates[0].ToString() == name)
                {
                    saved = "TemporalCoordinatesRecorrido4";
                    color = Colors.Indigo;
                }

                paradaCoordinates = ApplicationData.Current.LocalSettings.Values["TemporalCoordinatesRecorrido5"] as string[];

                if (paradaCoordinates[0].ToString() == name)
                {
                    saved = "TemporalCoordinatesRecorrido5";
                    color = Colors.OrangeRed;
                }
            }
            catch
            {
            }

            try 
            { 
                string[] coord = ApplicationData.Current.LocalSettings.Values[saved] as string[];


                BasicGeoposition endLocation = new BasicGeoposition
                {
                    Latitude = Double.Parse(coord[1]),
                    Longitude = Double.Parse(coord[2])
                };

                (this.DataContext as MapPageViewModel).TraceRouteFromPoint(endLocation, color);
            }
            catch
            {
            }
        }
    }
}
