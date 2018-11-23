using Holistor.KeyReader.Dto;
using Holistor.KeyReader.Encrypt;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Holistor.KeyReader.Service
{
    class HelperService
    {
        private const string stringConection = "http://plataformawebhostmh-plataformatst.azurewebsites.net/api/TokenAuth/Authenticate";

        private static UrlClass ReadXML()
        {
            try
            {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string pathxml = Path.Combine(assemblyFolder, "Config.xml");

                // Create a new file stream for reading the XML file
                FileStream ReadFileStream = new FileStream(pathxml, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Create a new XmlSerializer instance with the type of the test class
                XmlSerializer SerializerObj = new XmlSerializer(typeof(UrlClass));
                // Load the object saved above by using the Deserialize function
                var LoadedObj = (UrlClass)SerializerObj.Deserialize(ReadFileStream);

                // Cleanup
                ReadFileStream.Close();

                return LoadedObj;
            }
            catch
            {
                throw new Exception("No se pudo abrir el archivo de configuración Config.xml.");
            }
        }

        private static string GetToken()
        {
            try
            {
                var token = string.Empty;
                var client = new RestClient(stringConection);
                var request = new RestRequest(Method.POST);
                var tokenAuth = new
                {
                    userNameOrEmailAddress = "holistorCD",
                    password = "123qwe"
                };

                var serilizacion = Newtonsoft.Json.JsonConvert.SerializeObject(tokenAuth);

                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("undefined", serilizacion, ParameterType.RequestBody);

                IRestResponse<ResponseTokenDto> response2 = client.Execute<ResponseTokenDto>(request);

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    token = response2.Data.result.AccessToken;
                }
                else
                {
                    var mensajeValidacion = "No se pudo obtener el token de autorización para conectar al servicio web.";

                    if (response2.Data != null && response2.Data.error != null)
                    {
                        mensajeValidacion += Environment.NewLine + "Mensaje del Sistema: "
                                + response2.Data.error.Details + " "
                                + response2.Data.error.Message;
                    }

                    throw new Exception(mensajeValidacion);
                }

                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static Key ConnetToAzureService<Key, Param, Result>(Param objParam)
            where Key : IKeyResult, new()
            where Result : IResponseEncrypDto
        {
            try
            {
                var original = Newtonsoft.Json.JsonConvert.SerializeObject(objParam);

                var hybrid = new HybridEncryption();

                var encryptedBlock = hybrid.EncryptData(Encoding.UTF8.GetBytes(original));

                var serilizacion = Newtonsoft.Json.JsonConvert.SerializeObject(encryptedBlock);

                string accessToken = string.Empty;
                string urlConexion = string.Empty;

                try
                {
                    accessToken = GetToken();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    var url = ReadXML();
                    urlConexion = url.url;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                var client = new RestClient(urlConexion);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddParameter("undefined", serilizacion, ParameterType.RequestBody);

                var response2 = client.Execute(request);

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    Result respons = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(response2.Content);

                    var decrypted = hybrid.DescryptData(respons.result);
                    var jsonParams = Encoding.UTF8.GetString(decrypted);

                    Key paramsResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Key>(jsonParams);

                    return new Key()
                    {
                        Holiwin = paramsResult.Holiwin,
                        NroSuscripcion = paramsResult.NroSuscripcion,
                        Numero = paramsResult.Numero,
                        Mensaje = paramsResult.Mensaje,
                        Success = paramsResult.Success,
                        Tipo = paramsResult.Tipo,
                        Lote = paramsResult.Lote,
                        LLaveDigital = paramsResult.LLaveDigital,
                        SuscripcionOperativa = paramsResult.SuscripcionOperativa,
                        IsActive = paramsResult.IsActive,
                        ApellidoRSocial = paramsResult.ApellidoRSocial
                    };
                }
                else
                {
                    throw new Exception("No se pudo conectar al servicio web de autorización. Verifique conexion a Internet.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error del Sistema. " + ex.Message);
            }
        }


        internal static string GetCurrentPath()
        {
            FileInfo fi = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            return fi.FullName;
        }

        internal static string GetMyHashCode()
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            FileInfo fi = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            FileStream stream = File.Create(Process.GetCurrentProcess().MainModule.FileName, (int)fi.Length, FileOptions.Asynchronous);

            md5.ComputeHash(stream);

            stream.Close();

            string rtrn = "";
            for (int i = 0; i < md5.Hash.Length; i++)
            {
                rtrn += (md5.Hash[i].ToString("x2"));
            }

            return rtrn.ToUpper();
        }
    }
}
