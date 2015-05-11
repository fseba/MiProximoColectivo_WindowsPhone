using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Devices.Geolocation;

namespace MiProximoColectivo.Classes.Local
{
    public class Bus
    {
        private string _imageUri;
        private string _nombre;
        private BasicGeoposition _position;
        private string _rawPointString;

        public string ImageUri {
            get { return _imageUri; }
            set { _imageUri = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        
        public BasicGeoposition Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }

        public string RawPointString
        {
            get { return _rawPointString; }
            set
            {
                _rawPointString = value;
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
                catch (Exception ex)
                {
                    Debug.WriteLine("No se pudo crear el punto: RawString: " + _rawPointString + " exception: " + ex.Message);
                }
            }
        }

        public Bus()
        {
            
        }


        public Bus(string imageUri, BasicGeoposition position, string nombre)
        {
            ImageUri = imageUri;
            Position = position;
            Nombre = nombre;
        }

    }
}
