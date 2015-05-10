using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiProximoColectivo.Classes.ServerReceived
{
    public class Recorrido : ObservableObject
    {
        private string _color;
        private string _description;
        private Geometry _geom;
        private int _idEmpresa;
        private int _id;
        private string _name;
        private string _nameEmpresa;

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }
        public Geometry Geom
        {
            get { return _geom; }
            set
            {
                _geom = value;
                RaisePropertyChanged();
            }
        }    
        public int IdEmpresa
        {
            get { return _idEmpresa; }
            set
            {
                _idEmpresa = value;
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
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        public string NameEmpresa
        {
            get { return _nameEmpresa; }
            set
            {
                _nameEmpresa = value;
                RaisePropertyChanged();
            }
        }
        public Recorrido()
        { }
        public Recorrido(string name)
        {
            Name = name;
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