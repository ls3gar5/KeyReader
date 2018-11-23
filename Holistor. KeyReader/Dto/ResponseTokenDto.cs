using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holistor.KeyReader.Dto
{
    class ResponseTokenDto
    {
        public TokenResult result { get; set; }
        public string targetUrl { get; set; }
        public bool success { get; set; }
        public ErrorDto error { get; set; }
    }
}
