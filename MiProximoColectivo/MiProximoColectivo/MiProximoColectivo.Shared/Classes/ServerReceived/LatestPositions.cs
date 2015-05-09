using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;

namespace MiProximoColectivo.Classes.ServerReceived
{
    public class Feature : ObservableObject
    {
        public string type { get; set; }
        public string id { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
        public object punto { get; set; }
        public string wkt { get; set; }
        public object title { get; set; }
        public string color { get; set; }

        public string imageUrl { 
            get { return "http://www.miproximocolectivo.sanluis.gov.ar/Content/Images/" + color + ".png"; }
        }
        private Geopoint _ubicacion { get; set; }

        public Geopoint Ubicacion
        {
            get
            {
                return new Geopoint(new BasicGeoposition() { Latitude = properties.Latitud, Longitude = properties.Longitud });
            }
            set
            {
                _ubicacion = value;
                RaisePropertyChanged();
            }
        }
    }

    public class Properties
    {
        public object direccion { get; set; }
        public int EstadoUnidadID { get; set; }
        public int EstadoID { get; set; }
        public int EquipoID { get; set; }
        public int MovilID { get; set; }
        public string FechaMsg { get; set; }
        public string FechaPC { get; set; }
        public int TipoEquipoPaqueteID { get; set; }
        public string Interfaz { get; set; }
        public int CantReportes { get; set; }
        public string FechaGPS { get; set; }
        public double Velocidad { get; set; }
        public string EstadoGPS { get; set; }
        public int Mostrar { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Curso { get; set; }
        public string EstadoGPSDescripcion { get; set; }
        public string EventoDescripcion { get; set; }
        public string EventoValor { get; set; }
        public double KMTotales { get; set; }
        public object LlaveID { get; set; }
        public object TipoReporte { get; set; }
        public string MovilNombre { get; set; }
        public string Patente { get; set; }
        public string NumeroInterno { get; set; }
        public bool MotorEncendido { get; set; }
        public bool TiempoParadaExcedido { get; set; }
        public int VelocidadExcedida { get; set; }
        public int ChoferID { get; set; }
        public object ApellidoChofer { get; set; }
        public bool EsAlarma { get; set; }
        public int Asentida { get; set; }
        public bool Alarma { get; set; }
        public bool TieneIcono { get; set; }
        public bool TieneTipoImagen { get; set; }
        public object IMEI { get; set; }
        public string Direccion { get; set; }
        public string LongLatWKT { get; set; }
        public string Color { get; set; }
        public string NombreRecorrido { get; set; }
        public object DescripcionRecorrido { get; set; }
    }  
    public class LatestPositions : ObservableObject
    {
        public object ContentEncoding { get; set; }
        public object ContentType { get; set; }
        public Data Data { get; set; }
        public int JsonRequestBehavior { get; set; }
        public object MaxJsonLength { get; set; }
        public object RecursionLimit { get; set; }
    }

    public class Data : ObservableObject
    {
        public string type { get; set; }

        private List<Feature> _features = null;

        public List<Feature> features
        {
            get { return _features; }

            set
            {


                _features = value;
                RaisePropertyChanged();
            }
        }
    }
}
