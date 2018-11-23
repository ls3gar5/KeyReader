using System.Runtime.InteropServices;

namespace Holistor.KeyReader.Dto
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class KeyResult : IKeyResult
    {
        public string Holiwin { get; set; }
        public string NroSuscripcion { get; set; }
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public string Lote { get; set; }
        public bool LLaveDigital { get; set; }
        public bool SuscripcionOperativa { get; set; }
        public bool IsActive { get; set; }
        public string Mensaje { get; set; }
        public bool Success { get; set; }
        public string ApellidoRSocial { get; set; }
    }
}
