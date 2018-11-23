using System;
using System.Runtime.InteropServices;
using datos;

namespace Holistor.Proteccion
{
    public class Import
    {
        [DllImport(@"hkey-w32.dll")]
        public static extern int HARDkey(byte[] data);
    }

    public class HKEY
    {
        #region PROPIEDADES
        
        #region Públicas
        public int Clave1
        {
            get { return _clave1; }
            set { _clave1 = value; }
        }
        public int Clave2
        {
            get { return _clave2; }
            set { _clave2 = value; }
        }
        public bool BuscaLocalmente
        {
            get { return _buscaLocalmente; }
            set { _buscaLocalmente = value; }
        }
        public bool ControlarCantidad
        {
            get { return _controlarCantidad; }
            set { _controlarCantidad = value; }
        }
        public bool ControlPorModulo
        {
            get { return _controlPorModulo; }
            set { _controlPorModulo = value; }
        }
        public bool ContarConexiones
        {
            get { return _contarConexiones; }
            set { _contarConexiones = value; }
        }
        public string NroModulo
        {
            get { return _nroModulo; }
            set { _nroModulo = value; }
        }
        #endregion
        
        #region Privadas
        private Result oResult;
        private sBox _s1;
        private Import _I1;
        private int _clave1;
        private int _clave2;
        private bool _buscaLocalmente;
        private bool _controlarCantidad;
        private bool _controlPorModulo;
        private bool _contarConexiones;
        private string _ctrlLic;
        private string _nroModulo;
        private char[] _conexion = new Char[8];
        private char[] _loteSerie = new Char[10];
        private char[] _estado = new Char[5];
        private char[] _versionDll = new Char[5];
        private char[] _versionDrv = new Char[5];
        private char[] _versionDrvServer = new Char[5];
        private char[] _fechaVencim = new Char[10];
        private char[] _modLeidos = new Char[32];
        private char[] _licGlobalLeido = new Char[5];
        private char[] _licGlobalEnUso = new Char[5];
        private char[] _celdaLeida = new Char[5];
        private char[] _textoLeido = new Char[108];
        private char[] _cpasswd = new char[16];
        private byte[] _passwd = new byte[16];
        #endregion

        #endregion

        #region CONSTRUCTORES
        public HKEY()
        {            
            _clave1 = 0;
            _clave2 = 0;
            _buscaLocalmente = false;
            _controlarCantidad = true;
            _controlPorModulo = false;
            _contarConexiones = false;
            _ctrlLic = "0";
            _nroModulo = "00";
        }
        #endregion

        #region MÉTODOS

        #region Públicos
        public void abrirLog()
        {
            throw new System.NotImplementedException();
        }
        public void cambiarClaves()
        {
            throw new System.NotImplementedException();
        }
        public void cerrarLog()
        {
            throw new System.NotImplementedException();
        }        
        public void genLog()
        {
            throw new System.NotImplementedException();
        }
        public void setearConexionesMaximo(int pConMax)
        {
            //grabarCelda(3, pConMax);
        }


        public Result testHkey()
        {
            Result oResult = new Result();

            if (this.inicio())
            {
                //this.BuscaLocalmente = true;
                //this.ContarConexiones = true;

                oResult =  this.iniciarConexion();
            }

            return oResult;
        }

        public bool inicio()
        {
            try
            {
                // En el proceso anterior se controla que exista el archivo hkey-w32.dll y luego se instancia.
                // Ver qué hacemos si ocurre un error. Cómo devolvemos false?
                _I1 = new Import();
                _s1 = new sBox();

                _s1.spasswd.CopyTo(0, this._cpasswd, 0, _s1.spasswd.Length);

                //Convierte a byte la variable cpasswd
                for (int i = 0; i < 16; i++)
                    this._passwd[i] = Convert.ToByte(this._cpasswd[i]);

                //Si termino todo bien completo las propiedades de la clase estatica.
                return true;
            }
            catch (Exception)
            {
                //Si termino todo bien completo las propiedades de la clase estatica.
                return false;
            }
        }

        public Result iniciarConexion()
        {
            try
            {
                this.configurarModoChequeo();

                oResult = new Result();

                // Esta funcion chequea la presencia de la llave mediante el comando Iniciar Conexion

                // Armado de la cadena para chequear la llave con el comando Iniciar Conexion
                /* 00000000 - Conexion
                 * 00000 - Clave1
                 * 00000 - Clave2
                 * 0000 - Comando
                 * 00 - Modulo a validar
                 * 0 - Tipo de inicio - 0 No limitar usuarios en red
                                      - 1 Incrementar usuario sin controlar
                                      - 2 Control de licencia global
                                      - 3 Control de licencias por modulo */

                string scadena;
                char[] ccadena = new char[200];
                byte[] cadena = new byte[200];

                Random R = new Random();
                int MaxLimit = 10;
                int j;

                if (_controlarCantidad)
                {
                    // Se controla para limitar la cantidad de conexiones en red.
                    if (_controlPorModulo)
                    {
                        // Control de Cantidad de Licencias por Módulo
                        _ctrlLic = "3";
                        _nroModulo = this._nroModulo.PadLeft(2, '0');
                    }
                    else
                    {
                        // Control de Cantidad de Licencias Globales
                        _ctrlLic = "2";
                    }
                }
                else
                {
                    // Sin control de cantidad de conexiones
                    if (_contarConexiones)
                    {
                        // No se controla pero se incrementa la cantidad de usuarios para poder consultar cantidad de conexiones abiertas
                        _ctrlLic = "1";
                    }
                }

                scadena = " 00000000 " + Convert.ToString(_clave1).PadLeft(5, '0') + " " + Convert.ToString(_clave2).PadLeft(5, '0') + " 0000 " + _nroModulo + " " + _ctrlLic;

                scadena.CopyTo(0, ccadena, 0, scadena.Length);

                //Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
                for (j = 0; j < 10; j++)
                    cadena[j] = (byte)R.Next(MaxLimit);

                //Convierte a byte ccadena
                for (int i = 10; i < 200; i++)
                    cadena[i] = Convert.ToByte(ccadena[i - 10]);

                encripta(cadena, _passwd);
                Import.HARDkey(cadena);
                desencripta(cadena, _passwd);

                if (validaCadena(cadena))
                {
                    this.parseaEstado(cadena,20,25);
                    this.chequeaEstado();
                    if (oResult.EstadoCorrecto)
                    {
                        oResult.lConectado = true;
                        this.parseaConexion(cadena,11,19);
                        this.parseaLoteSerie(cadena,26,36);                        
                        this.parseaVersionLibreria(cadena,37,42);
                        this.parseaVersionDriver(cadena, 43, 48);
                        this.parseaVersionDriverServer(cadena, 49, 54);
                        this.parseaFechaVencimiento(cadena,55,65);                        
                        this.parseaModulosLeidos(cadena,66,98);
                        this.parseaLicGlobalLeido(cadena,105,110);
                        this.parseaLicGlobalEnUso(cadena,111,116);
                    }
                }
            }
            catch (Exception)
            {
                oResult.ErrorValidacion = true;
                oResult.MensajeErrorValidacion = "Error de validación de cadena al intentar conectar.";
            }
            return oResult;
        }
        public Result verificarConexion(string pNroConexion)
        {
            try
            {
                oResult = new Result();

                string scadena;
                char[] ccadena = new char[200];
                byte[] cadena = new byte[200];

                Random R = new Random();
                int MaxLimit = 10;
                int j;

                scadena = " " + pNroConexion + " " + Convert.ToString(_clave1).PadLeft(5, '0') + " " + Convert.ToString(_clave2).PadLeft(5, '0') + " 0001";

                scadena.CopyTo(0, ccadena, 0, scadena.Length);

                // Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
                for (j = 0; j < 10; j++)
                    cadena[j] = (byte)R.Next(MaxLimit);

                // Convierte a byte ccadena
                for (int i = 10; i < 200; i++)
                    cadena[i] = Convert.ToByte(ccadena[i - 10]);

                encripta(cadena, _passwd);
                Import.HARDkey(cadena);
                desencripta(cadena, _passwd);

                if (validaCadena(cadena))
                {
                    this.parseaEstado(cadena,20,25);
                    this.chequeaEstado();
                    if (oResult.EstadoCorrecto)
                    {
                        oResult.lConectado = true;
                        this.parseaConexion(cadena, 11, 19);
                        this.parseaLoteSerie(cadena, 26, 36);
                        this.parseaFechaVencimiento(cadena, 37, 47);
                        this.parseaLicGlobalLeido(cadena, 48, 53);
                        this.parseaLicGlobalEnUso(cadena, 54, 59);
                    }
                    else
                    {
                        oResult.lConectado = false;
                    }
                }
            }
            catch (Exception)
            {
                oResult.ErrorValidacion = true;
                oResult.MensajeErrorValidacion = "Error de validación de cadena al intentar validar conexión.";
            }
            return oResult;
        }
        //public Result finalizarConexion()
        //{
        //    /* Esta funcion finaliza la conexion con la llave */

        //    string scadena;
        //    char[] ccadena = new char[200];
        //    char[] aux = new char[200];
        //    byte[] cadena = new byte[200];

        //    Random R = new Random();
        //    int MaxLimit = 10;
        //    int j;

        //    /* Armado de la cadena para Finalizar Conexion */
        //    scadena = " 00001 00001 0002 00";

        //    /* 
        //     * 00000 - clave1
        //     * 00000 - clave2
        //     * 0002 - Comando
        //     * 00 - Modulo a finalizar */

        //    scadena.CopyTo(0, aux, 0, scadena.Length);

        //    // Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
        //    for (j = 0; j < 10; j++)
        //        cadena[j] = (byte)R.Next(MaxLimit);

        //    ccadena[0] = ' ';
        //    for (int i = 1; i < 10; i++)
        //        ccadena[i] = _conexion[i - 1];

        //    for (int i = 0; i < 190; i++)
        //        ccadena[i + 9] = aux[i];

        //    for (int i = 10; i < 200; i++)
        //        cadena[i] = Convert.ToByte(ccadena[i - 10]);

        //    encripta(cadena, _passwd);
        //    Import.HARDkey(cadena);
        //    desencripta(cadena, _passwd);
        //    validaCadena(cadena);

        //    //Convierte la cadena a char 
        //    for (int i = 0; i < 200; i++)
        //        ccadena[i] = Convert.ToChar(cadena[i]);

        //    //Almacena en variables los datos de cadena de salida
        //    for (int i = 11; i < 19; i++)
        //        _conexion[i - 11] = ccadena[i];
        //    for (int i = 20; i < 26; i++)
        //        _estado[i - 20] = ccadena[i];
        //    for (int i = 26; i < 37; i++)
        //        _loteSerie[i - 26] = ccadena[i];

        //    return false;
        //}
        public Result finalizarConexion(string pNroConexion)
        {
            try
            {
                oResult = new Result();
                string scadena;
                char[] ccadena = new char[200];
                byte[] cadena = new byte[200];
                Random R = new Random();
                int MaxLimit = 10;
                int j;

                /* Armado de la cadena para Finalizar Conexion */
                scadena = " " + Convert.ToString(pNroConexion).PadLeft(8, '0') +
                          " " + Convert.ToString(_clave1).PadLeft(5, '0') +
                          " " + Convert.ToString(_clave2).PadLeft(5, '0') +
                          " 0002 " + 
                          "00";
                /* 
                 * 00000 - clave1
                 * 00000 - clave2
                 * 0002 - Comando
                 * 00 - Modulo a finalizar */

                scadena.CopyTo(0, ccadena, 0, scadena.Length);

                //Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
                for (j = 0; j < 10; j++)
                    cadena[j] = (byte)R.Next(MaxLimit);

                //Convierte a byte ccadena
                for (int i = 10; i < 200; i++)
                    cadena[i] = Convert.ToByte(ccadena[i - 10]);

                encripta(cadena, _passwd);
                Import.HARDkey(cadena);
                desencripta(cadena, _passwd);

                if (validaCadena(cadena))
                {
                    this.parseaEstado(cadena, 20, 25);
                    this.chequeaEstado();
                    if (oResult.EstadoCorrecto)
                    {
                        this.parseaConexion(cadena, 11, 19);
                        this.parseaLoteSerie(cadena, 26, 36);
                    }
                }
            }
            catch (Exception)
            {
                oResult.ErrorValidacion = true;
                oResult.MensajeErrorValidacion = "Error de validación de cadena al intentar finalizar conexión.";
            }
            return oResult;
        }
        public Result leerCelda(string pNroConexion, int pCelda)
        {
            try
            {
                oResult = new Result();

                string scadena;
                char[] ccadena = new char[200];
                byte[] cadena = new byte[200];

                Random R = new Random();
                int MaxLimit = 10;
                int j;

                scadena = " " + Convert.ToString(pNroConexion).PadLeft(8, '0') +
                          " " + Convert.ToString(_clave1).PadLeft(5, '0') +
                          " " + Convert.ToString(_clave2).PadLeft(5, '0') +
                          " 0003 " + Convert.ToString(pCelda).PadLeft(5, '0');

                scadena.CopyTo(0, ccadena, 0, scadena.Length);

                // Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
                for (j = 0; j < 10; j++)
                    cadena[j] = (byte)R.Next(MaxLimit);

                // Convierte a byte ccadena
                for (int i = 10; i < 200; i++)
                    cadena[i] = Convert.ToByte(ccadena[i - 10]);

                encripta(cadena, _passwd);
                Import.HARDkey(cadena);
                desencripta(cadena, _passwd);

                if (validaCadena(cadena))
                {
                    this.parseaEstado(cadena, 20, 25);
                    this.chequeaEstado();
                    if (oResult.EstadoCorrecto)
                    {
                        oResult.lConectado = true;
                        this.parseaConexion(cadena, 11, 19);
                        this.parseaLoteSerie(cadena, 26, 36);
                        this.parseaCeldaLeida(cadena,37,42);
                    }
                }
                else
                {
                }
            }
            catch (Exception)
            {

            }

            return oResult;
        }
        public Result grabarCelda(string pNroConexion, int pCelda, int pDato)
        {
            try
            {
                oResult = new Result();

                string scadena;
                char[] ccadena = new char[200];
                byte[] cadena = new byte[200];

                Random R = new Random();
                int MaxLimit = 10;
                int j;

                scadena = " " + Convert.ToString(pNroConexion).PadLeft(8, '0') +
                          " " + Convert.ToString(_clave1).PadLeft(5, '0') +
                          " " + Convert.ToString(_clave2).PadLeft(5, '0') +
                          " 0004 " + Convert.ToString(pCelda).PadLeft(5, '0') +
                          " " + Convert.ToString(pDato).PadLeft(5, '0');

                scadena.CopyTo(0, ccadena, 0, scadena.Length);

                // Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
                for (j = 0; j < 10; j++)
                    cadena[j] = (byte)R.Next(MaxLimit);

                // Convierte a byte ccadena
                for (int i = 10; i < 200; i++)
                    cadena[i] = Convert.ToByte(ccadena[i - 10]);

                encripta(cadena, _passwd);
                Import.HARDkey(cadena);
                desencripta(cadena, _passwd);

                if (validaCadena(cadena))
                {
                    
                }
                else
                {

                }
            }
            catch (Exception)
            {

            }

            return oResult;
        }
        public Result leerTexto(string pNroConexion)
        {
            try
            {
                oResult = new Result();

                string scadena;
                char[] ccadena = new char[200];
                byte[] cadena = new byte[200];

                Random R = new Random();
                int MaxLimit = 10;
                int j;

                scadena = " " + Convert.ToString(pNroConexion).PadLeft(8, '0') +
                          " " + Convert.ToString(_clave1).PadLeft(5, '0') +
                          " " + Convert.ToString(_clave2).PadLeft(5, '0') +
                          " 0005 00000 00108";

                scadena.CopyTo(0, ccadena, 0, scadena.Length);

                // Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
                for (j = 0; j < 10; j++)
                    cadena[j] = (byte)R.Next(MaxLimit);

                // Convierte a byte ccadena
                for (int i = 10; i < 200; i++)
                    cadena[i] = Convert.ToByte(ccadena[i - 10]);


                encripta(cadena, _passwd);
                Import.HARDkey(cadena);
                desencripta(cadena, _passwd);

                if (validaCadena(cadena))
                {
                    this.parseaEstado(cadena, 20, 25);
                    this.chequeaEstado();
                    if (oResult.EstadoCorrecto)
                    {
                        oResult.lConectado = true;
                        this.parseaConexion(cadena, 11, 19);
                        this.parseaLoteSerie(cadena, 26, 36);
                        this.parseaTextoLeido(cadena,37,145);
                    }
                }
                else
                {
                }
            }
            catch (Exception)
            {                
                
            }

            return oResult;
        }
        public Result grabarTexto(string pNroConexion, string pTexto, int pInicio)
        {
            try
            {
                oResult = new Result();

                string scadena;
                char[] ccadena = new char[200];
                byte[] cadena = new byte[200];

                Random R = new Random();
                int MaxLimit = 10;
                int j;

                // Para llaves HARD KEY todo se lee desde el 44
                pInicio += 44;

                scadena = " " + Convert.ToString(pNroConexion).PadLeft(8, '0') +
                          " " + Convert.ToString(_clave1).PadLeft(5, '0') +
                          " " + Convert.ToString(_clave2).PadLeft(5, '0') +
                          " 0006 " + Convert.ToString(pInicio).PadLeft(5, '0') +
                          " " + Convert.ToString(pTexto.Length).PadLeft(5, '0') +
                          " " + pTexto;

                scadena.CopyTo(0, ccadena, 0, scadena.Length);

                // Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
                for (j = 0; j < 10; j++)
                    cadena[j] = (byte)R.Next(MaxLimit);

                // Convierte a byte ccadena
                for (int i = 10; i < 200; i++)
                    cadena[i] = Convert.ToByte(ccadena[i - 10]);

                encripta(cadena, _passwd);
                Import.HARDkey(cadena);
                desencripta(cadena, _passwd);

                if (validaCadena(cadena))
                {
                }
                else
                {
                }
            }
            catch (Exception)
            {
                
            }

            return oResult;
        }
        //public Result leerString()
        //{
        //    try
        //    {
        //        string scadena;
        //        char[] stringLlave = new char[200];
        //        char[] ccadena = new char[200];
        //        char[] aux = new char[200];
        //        byte[] cadena = new byte[200];
        //        Random R = new Random();
        //        int MaxLimit = 10;
        //        int j;

        //        // Esta funcion permite leer los 64 bytes de memoria no volatil de la llave reservados para el desarrollador

        //        //Armado de la cadena para el Comando Leer Srting
        //        scadena = " 00001 00001 0005 00044 00064";

        //        /* 00000 - clave1
        //           00000 - clave2
        //           0005 - Comando
        //           00044 - Byte de inicio de lectura
        //           00064 - Cantidad de Bytes a leer */

        //        scadena.CopyTo(0, aux, 0, scadena.Length);

        //        // Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
        //        for (j = 0; j < 10; j++)
        //            cadena[j] = (byte)R.Next(MaxLimit);

        //        // Copio a ccadena el nro de Conexion
        //        ccadena[0] = ' ';
        //        for (int i = 1; i < 10; i++)
        //            ccadena[i] = _conexion[i - 1];

        //        // Copio a ccadeena el resto del comando Leer String
        //        for (int i = 0; i < 190; i++)
        //            ccadena[i + 9] = aux[i];

        //        for (int i = 10; i < 200; i++)
        //            cadena[i] = Convert.ToByte(ccadena[i - 10]);

        //        encripta(cadena, _passwd);
        //        Import.HARDkey(cadena);
        //        desencripta(cadena, _passwd);
        //        _valida = validaCadena(cadena);

        //        // Convierte la cadena a char 
        //        for (int i = 0; i < 200; i++)
        //            ccadena[i] = Convert.ToChar(cadena[i]);

        //        // Almacena en variables los datos de cadena de salida
        //        for (int i = 11; i < 19; i++)
        //            _conexion[i - 11] = ccadena[i];
        //        for (int i = 20; i < 26; i++)
        //            _estado[i - 20] = ccadena[i];
        //        for (int i = 26; i < 37; i++)
        //            _loteSerie[i - 26] = ccadena[i];
        //        for (int i = 37; i < 200; i++)
        //            stringLlave[i - 26] = ccadena[i];
        //    }
        //    catch (Exception)
        //    {
                
        //    }

        //    return oResult;
        //}
        #endregion

        #region Privados
        void configurarModoChequeo()
        {
            //En esta sección se configura la forma de busqueda de la llave mediante el comando Configurar Modo de Chequeo.

            /* 00000000 - Conexión
             * 00000 - Clave1
             * 00000 - Calve2
             * 0009 - Comando
             * 00001 - 1° lugar de búsqueda - 00001 (HARDkey en puertos locales LPT y USB) 
             * 00000 - 2° lugar de búsqueda - 00000 (Parametro ignorado)
             * 00000 - 3° lugar de búsqueda - 00000 (Parametro ignorado)
             * 00000 - 4° lugar de búsqueda - 00000 (Parametro ignorado)
             * 00000 - 5° lugar de búsqueda - 00000 (Parametro ignorado)
             * 00000 - 6° lugar de búsqueda - 00000 (Parametro ignorado)
             * 00000 - 7° lugar de búsqueda - 00000 (Parametro ignorado)
             * 00000 - 8° lugar de búsqueda - 00000 (Parametro ignorado)
             Ver en manual los parámetros posibles */

            string scadena;
            char[] ccadena = new char[200];
            byte[] cadena = new byte[200];

            Random R = new Random();
            int MaxLimit = 10;
            int j;

            if (_buscaLocalmente)
            {
                //Búsqueda en puertos locales únicamente
                scadena = " 00000000 " + Convert.ToString(_clave1).PadLeft(5, '0') + " " + Convert.ToString(_clave2).PadLeft(5, '0') + " 0009 00000 00001 00006 00000 00004 00000 00000 00000";
            }
            else
            {
                scadena = " 00000000 " + Convert.ToString(_clave1).PadLeft(5, '0') + " " + Convert.ToString(_clave2).PadLeft(5, '0') + " 0009 00107 00001 00006 00003 00004 00000 00000 00000";
            }

            scadena.CopyTo(0, ccadena, 0, scadena.Length);

            //Inicializa con 10 bytes al azar la string usada para pasar los parametros a la funcion HARDkey
            for (j = 0; j < 10; j++)
                cadena[j] = (byte)R.Next(MaxLimit);

            //cadena[0] = (byte)1;
            //cadena[1] = (byte)1;
            //cadena[2] = (byte)1;
            //cadena[3] = (byte)1;
            //cadena[4] = (byte)1;
            //cadena[5] = (byte)1;
            //cadena[6] = (byte)1;
            //cadena[7] = (byte)1;
            //cadena[8] = (byte)1;
            //cadena[9] = (byte)1;

            //Convierte a byte ccadena
            for (int i = 10; i < 200; i++)
                cadena[i] = Convert.ToByte(ccadena[i - 10]);

            encripta(cadena, _passwd);
            Import.HARDkey(cadena);
            desencripta(cadena, _passwd);
            validaCadena(cadena);
        }
        short hkXor(short a, short b)
        {
            return ((short)(a ^ b));
        }
        short hkMod(short a, short b)
        {
            return ((short)(a % b));
        }
        short hkSuma(short a, short b)
        {
            return ((short)(a + b));
        }
        bool validaCadena(byte[] sTemp)
        {
            // Esta función analiza que la string devuelta por la función HARDkey sea consistente
            short i;

            for (i = 0; i < 10; i++)
                sTemp[i] = _s1.sBox2[sTemp[i]];

            if (sTemp[10] != ' ')
                return false;
            if (sTemp[19] != ' ')
                return false;
            if (sTemp[25] != ' ')
                return false;
            if (sTemp[30] != '-')
                return false;            

            return true;
        }
        int encripta(byte[] buffer, byte[] pw)
        {
            /* Esta rutina encripta la cadena que se pasa como parámetro a la función HARDkey */

            byte cAnterior = 0;
            byte cTemp;
            byte i;
            byte k;
            short[] password = new short[16];

            for (i = 0; i < 16; i++)
            {
                password[i] = pw[i];
                if (password[i] < 0)
                    password[i] = hkSuma(password[i], 256);
            }
            for (i = 0; i < 200; i++)
            {
                cTemp = buffer[i];
                if (cTemp < 0)
                    cTemp = (byte)(cTemp + 256);

                cTemp = (byte)(cTemp ^ _s1.sBox1[cAnterior]);
                for (k = 0; k < 16; k++)
                {
                    if ((k % 2) == 1)
                    {
                        cTemp = (byte)(cTemp ^ _s1.sBox1[_s1.sBox2[password[k]]]);
                        cTemp = _s1.sBox2[cTemp];
                    }
                    else
                    {
                        cTemp = (byte)(cTemp ^ _s1.sBox2[_s1.sBox1[password[k]]]);
                        cTemp = _s1.sBox1[cTemp];
                    }
                }//for(k=0; k<16; k++)	 
                cTemp = (byte)(cTemp ^ _s1.sBox1[i]);
                cAnterior = cTemp;
                buffer[i] = (byte)cTemp;
            }//for (i=0; i<200; i++)  
            return 0;
        }
        int desencripta(byte[] buffer, byte[] pw)
        {
            /* Esta función desencripta la cadena que devuelve la función HARDkey */

            short cAnterior = 0;
            short cTemp;
            short i;
            short k;
            short[] password = new short[16];

            for (i = 0; i < 16; i++)
            {
                password[i] = pw[i];
                if (password[i] < 0)
                    password[i] = hkSuma(password[i], 256);
            } /* for (i=0;i<16;i++) */

            for (i = 0; i < 200; i++)
            {
                cTemp = buffer[i];
                if (cTemp < 0)
                    cTemp = hkSuma(cTemp, 256);
                cTemp = hkXor(cTemp, _s1.sBox1[cAnterior]);
                for (k = 0; k < 16; k++)
                {
                    if (hkMod(k, 2) == 1)
                    {
                        cTemp = hkXor(cTemp, _s1.sBox1[_s1.sBox2[password[k]]]);
                        cTemp = _s1.sBox2[cTemp];
                    }
                    else /* if (hkMod(k,2) == 1) */
                    {
                        cTemp = hkXor(cTemp, _s1.sBox2[_s1.sBox1[password[k]]]);
                        cTemp = _s1.sBox1[cTemp];
                    } /* if (hkMod(k,2) == 1) */
                } /* for(k=0; k<16; k++) */
                cTemp = hkXor(cTemp, _s1.sBox1[i]);
                cAnterior = buffer[i];
                if (cAnterior < 0)
                    cAnterior = hkSuma(cAnterior, 256);
                buffer[i] = (byte)cTemp;
            } /* for (i=0; i<200; i++) */

            return 0;
        } /* int desencripta(byte[] buffer,byte[] pw) */
        void parseaConexion(byte[] cadena , int desde , int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _conexion[i - desde] = ccadena[i];

            for (int i = 0; i < _conexion.Length; i++)
            {
                oResult.NroConexion += _conexion[i].ToString().Trim();
            }
        }
        void parseaEstado(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _estado[i - desde] = ccadena[i];

            for (int i = 0; i < _estado.Length; i++)
            {
                oResult.codigoEstado += _estado[i].ToString().Trim();
            }
        }
        void parseaLoteSerie(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _loteSerie[i - desde] = ccadena[i];

            for (int i = 0; i < _loteSerie.Length; i++)
            {
                oResult.LoteSerie += _loteSerie[i].ToString().Trim();
            }
        }
        void parseaVersionLibreria(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _versionDll[i - desde] = ccadena[i];

            for (int i = 0; i < _versionDll.Length; i++)
            {
                oResult.VersionLibreria += _versionDll[i].ToString().Trim();
            }            
        }
        void parseaVersionDriver(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _versionDrv[i - desde] = ccadena[i];

            for (int i = 0; i < _versionDrv.Length; i++)
            {
                oResult.VersionDriver += _versionDrv[i].ToString().Trim();
            }            
        }
        void parseaVersionDriverServer(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _versionDrvServer[i - desde] = ccadena[i];

            for (int i = 0; i < _versionDrvServer.Length; i++)
            {
                oResult.VersionDriverServer += _versionDrvServer[i].ToString().Trim();
            }            
        }
        void parseaFechaVencimiento(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _fechaVencim[i - desde] = ccadena[i];

            for (int i = 0; i < _fechaVencim.Length; i++)
            {
                oResult.FechaVencimiento += _fechaVencim[i].ToString().Trim();
            }             
        }
        void parseaModulosLeidos(byte[] cadena, int desde, int hasta) 
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _modLeidos[i - desde] = ccadena[i];

            for (int i = 0; i < _modLeidos.Length; i++)
            {
                oResult.ModulosLeidos += _modLeidos[i].ToString().Trim();
            }            
        }
        void parseaLicGlobalLeido(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _licGlobalLeido[i - desde] = ccadena[i];

            for (int i = 0; i < _licGlobalLeido.Length; i++)
            {
                oResult.Lic_Global_Leido += _licGlobalLeido[i].ToString().Trim();
            }
        }
        void parseaLicGlobalEnUso(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _licGlobalEnUso[i - desde] = ccadena[i];

            for (int i = 0; i < _licGlobalEnUso.Length; i++)
            {
                oResult.Lic_Global_En_Uso += _licGlobalEnUso[i].ToString().Trim();                
            }
        }
        void parseaCeldaLeida(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _celdaLeida[i - desde] = ccadena[i];

            for (int i = 0; i < _celdaLeida.Length; i++)
            {
                oResult.CeldaLeida += _celdaLeida[i].ToString().Trim();
            }
        }
        void parseaTextoLeido(byte[] cadena, int desde, int hasta)
        {
            // Convierte la cadena a char
            char[] ccadena = new char[200];

            for (int i = 0; i < 200; i++)
                ccadena[i] = Convert.ToChar(cadena[i]);

            for (int i = desde; i < hasta; i++)
                _textoLeido[i - desde] = ccadena[i];

            for (int i = 0; i < _textoLeido.Length; i++)
            {
                oResult.TextoLeido += _textoLeido[i].ToString().Trim();
            }
        }
        void chequeaEstado()
        {
            switch (oResult.codigoEstado)
            {
                case "00000":
                    oResult.EstadoCorrecto = true;
                    oResult.MensajeEstado = "El comando se completó con éxito";
                    break;

                case "00002":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "No se encontró protector";
                    break;

                case "00004":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "Formato de cadena o parámetro incorrecto";
                    break;

                case "00010":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "Número de conexión no válida";
                    break;

                case "00011":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "Se superó límite de usuarios permitidos";
                    break;

                case "00012":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "Módulo ya en uso por la aplicación";
                    break;

                case "00013":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "Módulo no levantado por la aplicación";
                    break;

                case "00020":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "No hay drivers HARDkey instalados";
                    break;

                case "00021":
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "Versión de drivers obsoleta";
                    break;

                default:
                    oResult.EstadoCorrecto = false;
                    oResult.MensajeEstado = "";
                    break;
            }
        }
        #endregion

        #endregion

    }
}
