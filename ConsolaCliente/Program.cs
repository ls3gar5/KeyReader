using Holistor.KeyReader;
using Holistor.KeyReader.Dto;
using Microsoft.Win32;
using System;

namespace ConsolaCliente
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetHoliwinTxt();
            //Get45PlusFromRegistry();
            GetEncrypted();
        }

        private static void GetEncrypted()
        {
            var holi = new HolistorKeyReader();
            var path = holi.GetCurrentPath();
            var md5 = holi.GetHashCode();
        }

        private static void GetHoliwinTxt()
        {
            KeyResult result;

            var holi = new HolistorKeyReader();

            Console.Write("Usuario Administrador: ");
            Console.WriteLine(holi.EsAdministrador);
            Console.Write("Tiene Internet: ");
            Console.WriteLine(holi.ExisteInternetConnection);

            if (holi.ExisteLLaveConectada)
            {
                Console.WriteLine("Llave Conectada: True");

                result = holi.ReadHoliwinByLlave();
            }
            else
            {
                var nroSuscripcion = "00290040";

                result = holi.ReadHoliwinByNroSuscripcion(nroSuscripcion);
            }

            
            if (result.Success)
            {
                Console.Write("Trabajo Exitoso: ");
                Console.WriteLine(result.Success);
                Console.WriteLine("Holiswin: " + result.Holiwin);
            }
            else
            {
                Console.Write("Trabajo con errores: ");
                Console.WriteLine(result.Success);
                Console.WriteLine("Mensaje Error: " + result.Mensaje);
            }

            Console.Read();
        }

        private static void Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    Console.WriteLine(".NET Framework Version: " + CheckFor45PlusVersion((int)ndpKey.GetValue("Release")));
                }
                else
                {
                    Console.WriteLine(".NET Framework Version 4.5 or later is not detected.");
                }
            }
        }

        // Checking the version using >= will enable forward compatibility.
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 461808)
                return "4.7.2 or later";
            if (releaseKey >= 461308)
                return "4.7.1";
            if (releaseKey >= 460798)
                return "4.7";
            if (releaseKey >= 394802)
                return "4.6.2";
            if (releaseKey >= 394254)
                return "4.6.1";
            if (releaseKey >= 393295)
                return "4.6";
            if (releaseKey >= 379893)
                return "4.5.2";
            if (releaseKey >= 378675)
                return "4.5.1";
            if (releaseKey >= 378389)
                return "4.5";
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }

    }

}
