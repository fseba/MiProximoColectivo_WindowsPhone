using GalaSoft.MvvmLight;
using MiProximoColectivo.Classes.ServerReceived;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiProximoColectivo.Classes.Groups
{
    public class RecorridoYParadas : ObservableObject
    {
        private Recorrido _track;
        private UIObservableCollection<MpcPuntoControl> _stops;

        /// <summary>
        /// Paradas
        /// </summary>        
        public UIObservableCollection<MpcPuntoControl> Stops
        {
            get { return _stops; }
            set
            {
                _stops = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Recorrido
        /// </summary>
        public Recorrido Track
        {
            get { return _track; }
            set
            {
                _track = value;
                RaisePropertyChanged();
            }
        }
    }
}
