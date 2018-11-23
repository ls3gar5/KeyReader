using System.Security.Cryptography;

namespace Holistor.KeyReader.Encrypt
{
    class RSAWithRASParameterKey
    {
        private const string KEY_PUBLIC_2 = "<RSAKeyValue><Modulus>+Qpyrz4suYdtQXC7fP7jYjW15OJhnGQeOR/ox8vkfMky2LdFYZsdRUSj1FX/io7jZqk47g5Cc35xJmEwTuPd2G0Em2+gnf+ZvfO+xNCVwDOyAOYJwClV+2RmaDdX9B5rmB6Q97fphC8b00GkJvaenzU0poU6BEIldC79lD/xfn3TahK3DdGZIK6A2MqiDTg+E+UsUNG1DhJ/L79FfYJA5z6jXZFALrRlxkY/7gpro+ObWeNCYqtgS07UpR5oTZInYTL+2R9izXbKugSHSBjvqRPG/2aU1kzsHVB5t6p0YxZVcbXehlk4wTFnr/8oAAcqYirqUpXHf18W4Zcy6aPwCQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string KEY_PRIVATE_1 = "<RSAKeyValue><Modulus>sRJR4XZDRykFd6O4BBRNjczlP2X3iNdg+tB4SSrGHzMaqpjQOUdtX0ZwEfHC+//o15Et5Ux92eYPOYN3bhpk/p4cbp/i8GMFRjpWSR3aR1pLG2ls1QzCAizmGVJ+SHuYTSCp+bW9RPGRej2hLy7PkE9FJ5IdZR7pNokv8HwnKo52whKmJ1pK1uNQYgTxFDGmBpwTz1RaA7i+AR53CtasQACA7BRnNZPCQkb3TIVS3Jj2WO2MUTiVi18w4dDXUlS/cELihQ4GlOE8tZCjlD8DTpHB/05JSVQ+e7FTQCZ46pNgfzqZM+DB/GADkJQj3WzykAYq2/mIXWyX8FQQhZlh1Q==</Modulus><Exponent>AQAB</Exponent><P>4MQaz1W9glySGv/IaWScZtey9mZVqcxGunWbH5bMvws0705iPXJ3INS+X1kTeSkYM9s7ns/qNZ/6dxQMzDq3VnA/r5EMtLh3g9mVHdmLs57CEXRBnw6PdqNndLbsEhoDcZZTYGiNBV1MvjskHigSnck7wsgzJY41ScSlPx9Svx8=</P><Q>ya2EeBbfez+3ZxF6rjDb5VC+RsHwD2CbLhteF6hoNS7omTrXHYDpThHaxUBdObJIIzFSVFmt7/dVlRi82A7q4heayticFoKWBF9bosAFTuQSJH3taW6u2mJ2qFgyDPWID92xLnBxWRrZPShLSApShhmTt7Yha3G54zFI7KQM5Is=</Q><DP>yHa+/pqW+fS8Lq5gvXcJc0QADnj5AAb8MgMARm1F0UIahTDgt7Mmgrab0AhvMMmxF1b51svTNN6pEZllTKU1rTdR5bE1pt07YvIlZ8kGKz52EgEhIc5nr8VPWS5oYECpP552YG7/D4DGGIhAz1CoQegfj7rkuyMZbeCgDStPrrs=</DP><DQ>qBeOhIUT6WbW1vnzlYG1lbCxlsoH6tkrHcfDqdY8XK9nQGeRac7LJb2t6J/X08HrbCGsrA8+8PXmB+nhY/czABSKeK0Hk59tp+FOwBxpkDJ5iJ7IpQqShARzP6aauMBgklDFX55qasj87YNLE5U+6PQicYlE24ejVl/6lvm4oTc=</DQ><InverseQ>MpUntq5AMMEExOw9TVH70hoVwyVNEmGJOdC1iafp2eVPJ4MPk7N1HydMkfGF09ecApzlgnvldkuwpfu2zv83lvK6SywKsvLtRVKnDdqgu4cYV08xsxuVoTxChW5JqQw4ePzYy+XekWY5lYwMTZHq9LGXh7P6gmYuFwEJ2xDSu4g=</InverseQ><D>qR4HKCAsl41dVGwAd5zwiUs1dLytk3upe0Oabr80pF3JzT/QN7S5iDKdHPZbtRUwzJozTHonBPMOfW7LpVP+O5TFMP7x34ejZPSYrpKdhGJ/s3mYYU8jCLLbPToltJKe3qMg1Ic4ct2JhUfRZ1/9DCY5tVZ/TrIcKWMTb0sUmTYKeZgG1LGyThLSEx1h3f+nDmK5yqpVAXwENGzPJmyZdr3VX404gw3EQp6cyz8apq/urSHtBATeiCnidCehHWBtKxF0xR2FdwCUtCZ8wdFony600V+VIpaaylrkuiXwtBorc+i0MSNhyssmPf+GSxBU2+iMv5OsUhVUFD03JC3ELQ==</D></RSAKeyValue>";

        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cipherbytes;

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                //rsa.ImportParameters(_publicKey);
                rsa.FromXmlString(KEY_PUBLIC_2);

                cipherbytes = rsa.Encrypt(dataToEncrypt, true);
            }

            return cipherbytes;
        }

        public byte[] DecryptData(byte[] dataToEncrypt)
        {
            byte[] cipherbytes;

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                //rsa.ImportParameters(_privateKey);
                rsa.FromXmlString(KEY_PRIVATE_1);

                cipherbytes = rsa.Decrypt(dataToEncrypt, true);
            }

            return cipherbytes;
        }
    }
}