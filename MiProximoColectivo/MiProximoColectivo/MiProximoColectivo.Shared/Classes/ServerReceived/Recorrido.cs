using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiProximoColectivo.Classes.ServerReceived
{
    public class Recorrido : ObservableObject
    {
        private int _id;
        private string _name;
        private object _description;
        private int _idEmpresa;
        private object _geom;        
        private string _nameEmpresa;
        private string _color; 
               
        [JsonProperty ("MpcRecorridoID")]
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
        [JsonProperty("MpcEmpresaID")]
        public int IdEmpresa
        {
            get { return _idEmpresa; }
            set
            {
                _idEmpresa = value;
                RaisePropertyChanged();
            }
        }
        /*[JsonProperty("Geom")]
        public object Geom
        {
            get { return _geom; }
            set
            {
                _geom = value;
                //RaisePropertyChanged();
            }
        }*/
        [JsonProperty("NombreEmpresa")]
        public string NameEmpresa
        {
            get { return _nameEmpresa; }
            set
            {
                _nameEmpresa = value;
                RaisePropertyChanged();
            }
        }
        [JsonProperty("Color")]
        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged();
            }
        }
    }
}


/*
 corrido>
<Color>FFFF00</Color>
<Descripcion i:nil="true"/>
<Geom>
 MpcEmpresaID>13</MpcEmpresaID>
<MpcRecorridoID>15</MpcRecorridoID>
<Nombre>San Luis - La Punta por R146 (vuelve: J. Daract)</Nombre>
<NombreEmpresa>Tpte Sol Bus</NombreEmpresa
 */