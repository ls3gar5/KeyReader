using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holistor.KeyReader.Dto
{
    public class ErrorDto
    {
        public long Code { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public object ValidationErrors { get; set; }
    }
}
