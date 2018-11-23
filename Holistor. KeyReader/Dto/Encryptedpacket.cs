using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holistor.KeyReader.Dto
{
    public class Encryptedpacket
    {
        public byte[] EncryptedSessionKey;

        public byte[] EncryptedData;

        public byte[] Iv; //Initialization vector

        public byte[] Hmac; //hash message authentication code.
    }
}
