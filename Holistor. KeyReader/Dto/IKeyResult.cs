using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holistor.KeyReader.Dto
{
    public interface IKeyResult
    {
        string Holiwin { get; set; }
        string NroSuscripcion { get; set; }
        string Numero { get; set; }
        string Tipo { get; set; }
        string Lote { get; set; }
        bool LLaveDigital { get; set; }
        bool SuscripcionOperativa { get; set; }
        bool IsActive { get; set; }
        string Mensaje { get; set; }
        bool Success { get; set; }
        string ApellidoRSocial { get; set; }
    }
}
