using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace MiProximoColectivo.Classes.Local
{
    public class Busses : ObservableObject
    {
        private UIObservableCollection<Bus> _busses;
        public UIObservableCollection<Bus> Busseses
        {
            get { return _busses; }
            set
            {
                _busses = value;
                RaisePropertyChanged();
            }
        }
    }
}
