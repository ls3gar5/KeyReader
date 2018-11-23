using System;

namespace Holistor.Proteccion
{
    /// <summary>
    /// 
    /// </summary>
    public class ProteccionReader
    {
        public enum Estado
        {
            OK,
            ERROR
        }

        private Result oResult;
        private HKEY oHKey;

        #region CONSTRCTOR
        public ProteccionReader()
        {
        }

        public ProteccionReader(string pathHoliwin)
        {
            oHKey = new HKEY();
            oResult = oHKey.iniciarConexion();
            
            _pathHoliwin = pathHoliwin;
            HGPARTXT();
        }

        #endregion

        #region PROPIEDADES Y MIEMBROS
        private string _pathHoliwin;
        private string[] uLeer;
        private Estado _estadoProteccion;
        public Estado EstadoProteccion
        {
            get { return _estadoProteccion; }
            //set { _estado = value; }
        }

        private string _mensaje;

        public string Mensaje
        {
            get { return _mensaje; }
            //set { _mensaje = value; }
        }

        private string _caption;

        public string Caption
        {
            get { return _caption; }
            //set { _caption = value; }
        }

        private bool _llveConcetada;

        public bool LLaveConectada
        {
            get
            {
                var oHKeyLocal = new HKEY()
                {
                    BuscaLocalmente = true,
                    ContarConexiones = true
                };

                var oResultLocal = oHKeyLocal.testHkey();

                return oResultLocal.lConectado;
            }
            
        }


        #endregion

        #region METODOS

        public string GetLoteNumeroLlave()
        {
            Result oResult;

            var oHKeyLocal = new HKEY()
            {
                BuscaLocalmente = true,
                ContarConexiones = true
            };

            try
            {
                for (int i = 0; i <= 1; i++)
                {
                    oHKeyLocal.Clave1 = i;
                    oHKeyLocal.Clave2 = i;

                    oResult = oHKeyLocal.testHkey();

                    if (oResult.lConectado)
                    {
                        return oResult.LoteSerie.ToUpper();

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return "";
        }



        public bool iniciarValidacion() 
        {
            return true;
        }

        private string BUSPARAM()
        {
            return "";
        }
        private string FUNREV()
        {
            return "";
        }
        private int XORBYTE()
        {
            return 0;
        }
        private string FIBON()
        {
            return "";
        }
        private string FUNDER() 
        {
            return "";
        }
        private int MICRC() 
        {
            return 0;
        }
        private string HEXBYTE() 
        {
            return "";
        }
        private string FXH8() 
        {
            return "";
        }
        private string HEXACAR() 
        {
            return "";
        }
        private void HGBPKEY() 
        {

        }
        private void HGBPHARDKEY() 
        {

        }
        private void VALIDARCRC() 
        {

        }
        private void HGPARTXT()
        {

        }
        #endregion
    }

}
