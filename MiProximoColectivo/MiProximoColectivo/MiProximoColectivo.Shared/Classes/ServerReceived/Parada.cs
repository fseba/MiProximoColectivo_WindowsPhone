using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiProximoColectivo.Classes.ServerReceived
{
    public class Parada : ObservableObject
    {
        private UIObservableCollection<MpcPuntoControl> _checkPoints;
        public UIObservableCollection<MpcPuntoControl> CheckPoints
        {
            get { return _checkPoints; }
            set
            {
                _checkPoints = value;
                RaisePropertyChanged();
            }
        }
    }

    public class MpcPuntoControl : ObservableObject
    {
        private string _description;
        private int _id;
        private int _typeId;
        private string _name;
        private string _pcopi;
        private float _radio;
        private object _point;

        public string Description
        {
            get{return _description;}
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }
        public int TypeId
        {
            get { return _typeId; }
            set
            {
                _typeId = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        public string PCopI
        {
            get { return _pcopi; }
            set
            {
                _pcopi = value;
                RaisePropertyChanged();
            }
        }
        public float Radio
        {
            get { return _radio; }
            set
            {
                _radio = value;
                RaisePropertyChanged();
            }
        }
        public object Point
        {
            get { return _point; }
            set
            {
                _point = value;
                RaisePropertyChanged();
            }
        }
    }
}

