using Holistor.Proteccion;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Holistor.KeyReader.Dto;
using Holistor.KeyReader.Service;

namespace Holistor.KeyReader
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class HolistorKeyReader
    {
        #region CONSTRUCTOR
        public HolistorKeyReader()
        {

        }
        #endregion


        #region PROPERTIES
        private string GetCurrentDirectory
        {
            get
            {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(assemblyFolder, "config.xml");
            }
        }
        /// <summary>
        /// Retorna true si el usuarios corre el programa como Administrador
        /// </summary>
        public bool EsAdministrador
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// Retorna si la llave USB esta conectada a la PC en forma local
        /// </summary>
        public bool ExisteLLaveConectada
        {
            get
            {
                try
                {
                    var proteccion = new ProteccionReader();
                    return proteccion.LLaveConectada;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retorna el número de la llave
        /// </summary>
        public string GetNroLLaveConectada
        {
            get
            {
                try
                {
                    var proteccion = new ProteccionReader();
                    return proteccion.GetLoteNumeroLlave();
                }
                catch (Exception ex)
                {
                    return "Error." + ex.Message;
                }
            }
        }

        public bool ExisteInternetConnection
        {
            get
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        using (client.OpenRead("http://clients3.google.com/generate_204"))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public string Version
        {
            get { return "1.1.0"; }
        }

        public string VersionTst
        {
            get { return "KEY_DEV-20181023 EncryptData"; }
        }
        #endregion


        /// <summary>
        /// Obtiene los datos del Holiwin con el Nro de llave
        /// </summary>
        /// <returns></returns>
        public KeyResult ReadHoliwinByLlave()
        {
            //Para no pasarle parametos leemos la llave nuevamente

            var nroLoteLLave = string.Empty;
            var nroLote = string.Empty;
            var nroLlave = string.Empty;

            try
            {
                var llaveProteccion = new Proteccion.ProteccionReader();

                if (llaveProteccion.LLaveConectada)
                {
                    nroLoteLLave = llaveProteccion.GetLoteNumeroLlave();
                    nroLote = nroLoteLLave.Substring(0, nroLoteLLave.IndexOf('-'));
                    nroLlave = nroLoteLLave.Substring(nroLoteLLave.IndexOf('-') + 1);
                }
                else
                {
                    return new KeyResult()
                    {
                        Mensaje = "Error llave no conectada.",
                        Success = false
                    };
                }

                var objParam = new Parametros()
                {
                    lote = nroLote,
                    numero = int.Parse(nroLlave)
                };

                return HelperService.ConnetToAzureService<KeyResult, Parametros, ResponseLlaveEncrypDto>(objParam);

            }
            catch (Exception ex)
            {
                return new KeyResult()
                {
                    Mensaje = "Error del Sistema. " + ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// /Obtiene los datos del Holiwin con el Nro de Suscriptor
        /// </summary>
        /// <param name="nroSuscripcion"></param>
        /// <returns></returns>
        public KeyResult ReadHoliwinByNroSuscripcion(string nroSuscripcion)
        {
            //Controlamos que sea un nro entero
            try
            {
                var nro = Int32.Parse(nroSuscripcion);
            }
            catch
            {
                return new KeyResult()
                {
                    Mensaje = "Error no ingreso un número de suscriptor válido.",
                    Success = false
                };
            }

            try
            {
                var objParam = new Parametros()
                {
                    nroSuscripcion = nroSuscripcion
                };

                return HelperService.ConnetToAzureService<KeyResult, Parametros, ResponseLlaveEncrypDto>(objParam);

                //var original = Newtonsoft.Json.JsonConvert.SerializeObject(objParam);

                //var hybrid = new HybridEncryption();

                //var encryptedBlock = hybrid.EncryptData(Encoding.UTF8.GetBytes(original));

                //var serilizacion = Newtonsoft.Json.JsonConvert.SerializeObject(encryptedBlock);

                //string accessToken;
                //string urlConexion;

                //try
                //{
                //    accessToken = GetToken();
                //}
                //catch
                //{
                //    return new KeyResult()
                //    {
                //        MensajeError = "No se pudo obtener el token de autorización para conectar al servicio web de lectura de llave. Verifique conexion a Internet.",
                //        Success = false
                //    };
                //}

                //try
                //{
                //    var url = ReadXML();
                //    urlConexion = url.url;
                //}
                //catch
                //{
                //    return new KeyResult()
                //    {
                //        MensajeError = "No se pudo abrir el archivo de configuración Config.xml.",
                //        Success = false
                //    };
                //}

                //var client = new RestClient(urlConexion);
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("Cache-Control", "no-cache");
                //request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Authorization", "Bearer " + accessToken);
                //request.AddParameter("undefined", serilizacion, ParameterType.RequestBody);

                //IRestResponse<ResponseLlaveEncrypDto> response2 = client.Execute<ResponseLlaveEncrypDto>(request);

                //if (response2.StatusCode == HttpStatusCode.OK)
                //{
                //    ResponseLlaveEncrypDto respons = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseLlaveEncrypDto>(response2.Content);

                //    //SERVIDOR
                //    var decrypted = hybrid.DescryptData(respons.result);
                //    var jsonParams = Encoding.UTF8.GetString(decrypted);
                //    //Parametros paramsQuery = Newtonsoft.Json.JsonConvert.DeserializeObject<Parametros>(jsonParams);

                //    LLaveResult paramsResult = Newtonsoft.Json.JsonConvert.DeserializeObject<LLaveResult>(jsonParams);

                //    return new KeyResult()
                //    {
                //        Holiwin = paramsResult.Holiwin,
                //        NroSuscripcion = paramsResult.NroSuscripcion,
                //        NroLlave = paramsResult.Numero,
                //        MensajeError = paramsResult.Mensaje,
                //        Success = paramsResult.Success
                //    };
                //}
                //else
                //{
                //    return new KeyResult()
                //    {
                //        MensajeError = "No se pudo conectar al servicio web de lectura de llave. Verifique conexion a Internet.",
                //        Success = false
                //    };
                //}

            }
            catch (Exception ex)
            {
                return new KeyResult()
                {
                    Mensaje = "Error del Sistema. " + ex.Message,
                    Success = false
                };
            }
        }


        public string GetHashCode()
        {
            return HelperService.GetMyHashCode();
        }

        public string GetCurrentPath()
        {
            return HelperService.GetCurrentPath();
        }


        //private KeyResult GetHoliwin(Parametros objParam)
        //{
        //    string accessToken = string.Empty;
        //    string urlConexion = string.Empty;

        //    try
        //    {
        //        accessToken = GetToken();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new KeyResult()
        //        {
        //            MensajeError = ex.Message,
        //            Success = false
        //        };
        //    }

        //    var serilizacion = Newtonsoft.Json.JsonConvert.SerializeObject(objParam);

        //    try
        //    {
        //        var url = ReadXML();
        //        urlConexion = url.url;
        //    }
        //    catch
        //    {
        //        return new KeyResult()
        //        {
        //            MensajeError = "No se pudo abrir el archivo de configuración Config.xml.",
        //            Success = false
        //        };
        //    }

        //    var client = new RestClient(urlConexion);
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization", "Bearer " + accessToken);
        //    request.AddParameter("undefined", serilizacion, ParameterType.RequestBody);
        //    IRestResponse<ResponseLlaveDto> response2 = client.Execute<ResponseLlaveDto>(request);

        //    if (response2.StatusCode == HttpStatusCode.OK)
        //    {
        //        return new KeyResult()
        //        {
        //            Holiwin = response2.Data.result.Holiwin,
        //            NroSuscripcion = response2.Data.result.NroSuscripcion,
        //            NroLlave = response2.Data.result.Numero,
        //            MensajeError = response2.Data.result.Mensaje,
        //            Success = response2.Data.result.Success
        //        };
        //    }
        //    else
        //    {
        //        return new KeyResult()
        //        {
        //            MensajeError = "No se pudo conectar al servicio web de lectura de llave. Verifique conexion a Internet.",
        //            Success = false
        //        };
        //    }
        //}


        /// <summary>
        /// /Obtiene los datos del Holiwin con el Nro de Suscriptor
        /// </summary>
        /// <param name="nroSuscripcion"></param>
        /// <returns></returns>
        //public KeyResult ReadHoliwinByNroSuscripcion(string nroSuscripcion)
        //{
        //    //Controlamos que sea un nro entero
        //    try
        //    {
        //        var nro = Int32.Parse(nroSuscripcion);
        //    }
        //    catch (Exception)
        //    {
        //        return new KeyResult()
        //        {
        //            MensajeError = "Error no ingreso un número de suscriptor válido",
        //            Success = false
        //        };
        //    }

        //    try
        //    {
        //        var objParam = new Parametros()
        //        {
        //            nroSuscripcion = nroSuscripcion
        //        };

        //        return GetHoliwin(objParam);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new KeyResult()
        //        {
        //            MensajeError = "Error del Sistema. " + ex.Message,
        //            Success = false
        //        };
        //    }

        //}
    }
}