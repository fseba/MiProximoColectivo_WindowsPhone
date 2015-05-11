using GalaSoft.MvvmLight;
using MiProximoColectivo.Classes.ServerReceived;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MiProximoColectivo.Classes.Groups
{
    public class RecorridoYParadas : ObservableObject
    {
        private Recorrido _track;
        private UIObservableCollection<MpcPuntoControl> _stops;
                
        [JsonProperty("Recorrido")]
        public Recorrido Track
        {
            get { return _track; }
            set
            {
                _track = value;
                RaisePropertyChanged();
            }
        }
        [JsonProperty("Paradas")]
        public UIObservableCollection<MpcPuntoControl> Stops
        {
            get { return _stops; }
            set
            {
                _stops = value;
                RaisePropertyChanged();
            }
        }
    }
}
