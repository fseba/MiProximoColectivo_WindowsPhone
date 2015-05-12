using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.Groups;
using MiProximoColectivo.Classes.Local;
using MiProximoColectivo.Classes.ServerReceived;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace MiProximoColectivo.Model
{
    public static class CommonModel
    {
        public static ObservableCollection<Feature> BusFeatures
        {
            get;
            set;
        }
        public static Busses CurrentBusses
        {
            get;
            set;
        }
        public static ObservableCollection<RecorridoYParadas> ParadasYRecorridos
        {
            get;
            set; 
        }
        /// <summary>
        /// Nombres de los Recorridos
        /// </summary>
        public static ObservableCollection<string> TracksNames
        {
            get;
            set;
        }
        /// <summary>
        /// Nombres de los Recorridos paara el Pivot de NearFrom
        /// </summary>
        public static ObservableCollection<string> TracksNamesForNearFrom
        {
            get;
            set;
        }
        /// <summary>
        /// IDS de los Recorridos. Clave: Nombre del Recorrido
        /// </summary>
        public static Dictionary<string, int> TracksIds
        {
            get;
            set;
        }

        public static Geolocator DeviceLocator
        {
            get;
            set;
        }
        public static Geoposition DevicePosition
        {
            get;
            set;
        }
        public static bool DevicePositionReady
        {
            get { return (DeviceLocator != null && DeviceLocator.LocationStatus == PositionStatus.Ready && DevicePosition != null); }
        }
        public static UIObservableCollection<MapElement> NearFromPageMapElements
        {
            get;
            set;
        }
        public static Busses NearFromPageMapBusses
        {
            get;
            set;
        }
        public static UIObservableCollection<MapElement> ViewTrackMapElements
        {
            get;
            set;
        }


        public static void Initialize()
        {
            TracksNamesForNearFrom = new ObservableCollection<string>();
            TracksNamesForNearFrom.Add("Todos los Recorridos");
            TracksNamesForNearFrom.Add("San Luis - La Punta por R146 (vuelve: Bolivar)");
            TracksNamesForNearFrom.Add("San Luis - La Punta por Ruta 3");
            TracksNamesForNearFrom.Add("San Luis - La Punta ULP por Ruta 3");
            TracksNamesForNearFrom.Add("San Luis - La Punta por R146 (vuelve: J. Daract)");
            TracksNamesForNearFrom.Add("San Luis - La Punta");
            TracksNamesForNearFrom.Add("La Punta - San Luis");
            TracksNamesForNearFrom.Add("San Luis - Villa Mercedes");
            TracksNamesForNearFrom.Add("Merlo - San Luis");
            TracksNamesForNearFrom.Add("Merlo - Papagayos por Ruta 1");
            TracksNamesForNearFrom.Add("San Luis - Candelaria");
            TracksNamesForNearFrom.Add("San Luis - Juana Koslay (Servicio Rápido)");
            TracksNamesForNearFrom.Add("San Luis - Potrero de los Funes ");
            TracksNamesForNearFrom.Add("San Luis - San Roque - Las Chacras");
            TracksNamesForNearFrom.Add("San Luis - El Volcan");
            TracksNamesForNearFrom.Add("San Luis - Peaje");
            TracksNamesForNearFrom.Add("San Luis - Trapiche - La Florida");
            

            //Agrego los nombres de los recorridos disponibles en la página web
            TracksNames = new ObservableCollection<string>();
            TracksNames.Add("Balde - San Luis");
            TracksNames.Add("San Luis - Balde");
            TracksNames.Add("San Luis - Desaguadero");
            TracksNames.Add("Desaguadero - San Luis");
            TracksNames.Add("San Luis - La Punta");
            TracksNames.Add("San Luis - La Punta por Ruta 3");
            TracksNames.Add("San Luis - La Punta ULP por Ruta 3");
            TracksNames.Add("San Luis - La Punta por R146 (vuelve: J.Daract)");
            TracksNames.Add("San Luis - La Punta por R146 (vuelve: Bolívar)");
            TracksNames.Add("La Punta - San Luis");
            TracksNames.Add("San Luis - Unión");
            TracksNames.Add("Unión - San Luis");
            TracksNames.Add("Unión - Villa Mercedes");
            TracksNames.Add("Villa Mercedes - Unión");            
            TracksNames.Add("Merlo - Papagayos por Ruta 1");
            TracksNames.Add("San Luis - Candelaria");
            TracksNames.Add("San Luis - Villa Mercedes");
            TracksNames.Add("Merlo - Santa Rosa del Conlara");
            TracksNames.Add("Merlo - Villa Mercedes");
            TracksNames.Add("Merlo - San Luis");
            TracksNames.Add("San Luis - Juana Koslay (Serv. Rápido)");
            TracksNames.Add("San Luis - Potrero de los Funes");
            TracksNames.Add("San Luis - San Roque - Las Chacras");
            TracksNames.Add("San Luis - El Volcán");
            TracksNames.Add("San Luis - Peaje");
            TracksNames.Add("San Luis - El Trapiche - La Florida");
            TracksNames.Add("San Luis - La Carolina - Intihuasi");

            TracksIds = new Dictionary<string, int>()
            {
                {"Balde - San Luis", 1},
                {"San Luis - Balde", 2},
                {"San Luis - Desaguadero", 3},
                {"Desaguadero - San Luis", 4},
                {"San Luis - La Punta", 5},
                {"La Punta - San Luis", 6},
                {"San Luis - Unión", 7},
                {"Unión - San Luis", 8},
                {"Unión - Villa Mercedes", 9},
                {"Villa Mercedes - Unión", 10},
                {"San Luis - La Punta por Ruta 3", 11},
                {"San Luis - La Punta ULP por Ruta 3", 12},
                {"Merlo - Papagayos por Ruta 1", 13},
                {"San Luis - Candelaria", 14},
                {"San Luis - La Punta por R146 (vuelve: J.Daract)", 15},
                {"San Luis - Villa Mercedes", 16},
                {"Merlo - Santa Rosa del Conlara", 17},
                {"San Luis - La Punta por R146 (vuelve: Bolívar)", 18},
                {"Merlo - Villa Mercedes", 19},
                {"Merlo - San Luis", 20},
                {"San Luis - Juana Koslay (Serv. Rápido)", 21},
                {"San Luis - Potrero de los Funes", 22},
                {"San Luis - San Roque - Las Chacras", 23},
                {"San Luis - El Volcán", 24},
                {"San Luis - Peaje", 25},
                {"San Luis - El Trapiche - La Florida", 26},
                {"San Luis - La Carolina - Intihuasi", 27},
            };

            CurrentBusses = new Busses();
            CurrentBusses.Busseses = new UIObservableCollection<Bus>();
            NearFromPageMapBusses = new Busses();
            NearFromPageMapBusses.Busseses = new UIObservableCollection<Bus>();
            BusFeatures = new UIObservableCollection<Feature>();
            ParadasYRecorridos = new ObservableCollection<RecorridoYParadas>();
            NearFromPageMapElements = new UIObservableCollection<MapElement>();
            ViewTrackMapElements = new UIObservableCollection<MapElement>();
        }

        /// <summary>  
        /// Returns the distance in miles or kilometers of any two  
        /// latitude / longitude points.  
        /// </summary>  
        public static double DistanceBetweenGeopoints(BasicGeoposition pos1, BasicGeoposition pos2, DistanceType type = DistanceType.Kilometers)
        {            
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            double dLat = ToRadian(pos2.Latitude - pos1.Latitude);
            double dLon = ToRadian(pos2.Longitude - pos1.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(pos1.Latitude)) * Math.Cos(ToRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }
        /// <summary>  
        /// Convert to Radians.  
        /// </summary>  
        private static double ToRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
        public static bool PointsNear(BasicGeoposition pos1, BasicGeoposition pos2, DistanceType type = DistanceType.Kilometers)
        {
            return (DistanceBetweenGeopoints(pos1, pos2, type) <= 3);
        }

        public static double distance(BasicGeoposition pos1, Geoposition pos2, char unit)
        {
            double theta = pos1.Longitude - pos2.Coordinate.Longitude;
            double dist = Math.Sin(deg2rad(pos1.Latitude)) * Math.Sin(deg2rad(pos2.Coordinate.Latitude)) + Math.Cos(deg2rad(pos1.Latitude)) * Math.Cos(deg2rad(pos2.Coordinate.Latitude)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        public static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public enum DistanceType { Miles, Kilometers };  
    }
}
