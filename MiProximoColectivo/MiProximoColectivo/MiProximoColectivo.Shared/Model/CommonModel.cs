﻿using MiProximoColectivo.Classes;
using MiProximoColectivo.Classes.Groups;
using MiProximoColectivo.Classes.ServerReceived;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MiProximoColectivo.Model
{
    public static class CommonModel
    {
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


        public static void Initialize()
        {
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

            ParadasYRecorridos = new ObservableCollection<RecorridoYParadas>();            
        }        
    }
}
