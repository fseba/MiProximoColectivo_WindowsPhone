using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;

namespace MiProximoColectivo.Classes.ServerReceived
{
    public class MpcPuntoControl : ObservableObject
    {
        private int _id;
        private string _name;
        private object _description;        
        private int _typeId;
        private string _radio;
        private string _rawPointString;
        private object _pcopi;
        private BasicGeoposition _position;

        [JsonProperty("MpcPuntoControlID")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }
        [JsonProperty("Nombre")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        [JsonProperty("Descripcion")]
        public object Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }
        [JsonProperty("MpcTipoPuntoControlID")]
        public int TypeId
        {
            get { return _typeId; }
            set
            {
                _typeId = value;
                RaisePropertyChanged();
            }
        }
        [JsonProperty("Radio")]
        public string Radio
        {
            get { return _radio; }
            set
            {
                _radio = value;
                RaisePropertyChanged();
            }
        }
        [JsonProperty("WKTPuntoControl")]
        public string RawPointString
        {
            get { return _rawPointString; }
            set
            {
                _rawPointString = value;
                RaisePropertyChanged();

                try
                {
                    string tempPointStr = _rawPointString.Substring(_rawPointString.IndexOf('(') + 1);
                    tempPointStr = tempPointStr.Replace(")", "");
                    tempPointStr = tempPointStr.Replace(".", ",");
                    var tempPointCoordinates = tempPointStr.Split(' ');
                    var x = double.Parse(tempPointCoordinates[1], System.Globalization.NumberStyles.Number);
                    var y = double.Parse(tempPointCoordinates[0]);
                    Position = new BasicGeoposition() { Latitude = x, Longitude = y };
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("No se pudo crear el punto: RawString: " + _rawPointString + " exception: " + ex.Message);
                }
            }
        }
        /*[JsonProperty("PCoPI")]
        public object PCoPI
        {
            get { return _pcopi; }
            set
            {
                _pcopi = value;
                RaisePropertyChanged();
            }
        }*/
        public BasicGeoposition Position
        {
            get { return _position; }
            set
            {
                _position = value;
                //RaisePropertyChanged();
            }
        }
    } 
}

