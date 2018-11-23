using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holistor.KeyReader.Dto
{
    public interface IResponseEncrypDto
    {
        Encryptedpacket result { get; set; }
        string targetUrl { get; set; }
        bool success { get; set; }
        ErrorDto error { get; set; }
    }
}
