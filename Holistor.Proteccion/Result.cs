using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holistor.Proteccion
{
    public class Result
    {
        private bool _lconectado;
        public bool lConectado
        {
            get { return _lconectado; }
            set { _lconectado = value; }
        }
        
        private string _nroConexion;
        public string NroConexion
        {
            get { return _nroConexion; }
            set { _nroConexion = value; }
        }

        private bool _ErrorValidacion;
        public bool ErrorValidacion
        {
            get { return _ErrorValidacion; }
            set { _ErrorValidacion = value; }
        }

        private string _mensajeErrorValidacion;
        public string MensajeErrorValidacion
        {
            get { return _mensajeErrorValidacion; }
            set { _mensajeErrorValidacion = value; }
        }

        private bool _estadoCorrecto;
        public bool EstadoCorrecto
        {
            get { return _estadoCorrecto; }
            set { _estadoCorrecto = value; }
        }

        private string _codigoEstado;
        public string codigoEstado
        {
            get { return _codigoEstado; }
            set { _codigoEstado = value; }
        }        

        private string _mensajeEstado;
        public string MensajeEstado
        {
            get { return _mensajeEstado; }
            set { _mensajeEstado = value; }
        }

        private string _lic_global_leido;
        public string Lic_Global_Leido
        {
            get { return _lic_global_leido; }
            set { _lic_global_leido = value; }
        }

        private string _lic_global_en_uso;
        public string Lic_Global_En_Uso
        {
            get { return _lic_global_en_uso; }
            set { _lic_global_en_uso = value; }
        }

        private string _lote_serie;
        public string LoteSerie
        {
            get { return _lote_serie; }
            set { _lote_serie = value; }
        }

        private string _versionLibreria;
        public string VersionLibreria
        {
            get { return _versionLibreria; }
            set { _versionLibreria = value; }
        }

        private string _versionDriver;
        public string VersionDriver
        {
            get { return _versionDriver; }
            set { _versionDriver = value; }
        }

        private string _fechaVencimiento;
        public string FechaVencimiento
        {
            get { return _fechaVencimiento; }
            set { _fechaVencimiento = value; }
        }

        private string _modulosLeidos;
        public string ModulosLeidos
        {
            get { return _modulosLeidos; }
            set { _modulosLeidos = value; }
        }

        private string _versionDriverServer;
        public string VersionDriverServer
        {
            get { return _versionDriverServer; }
            set { _versionDriverServer = value; }
        }

        private string _celdaLeida;
        public string CeldaLeida
        {
            get { return _celdaLeida; }
            set { _celdaLeida = value; }
        }
        private string _textoLeido;
        public string TextoLeido
        {
            get { return _textoLeido; }
            set { _textoLeido = value; }
        }
        
    }
}
