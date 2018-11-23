using System.IO;
using System.Security.Cryptography;

namespace Holistor.KeyReader.Encrypt
{
    class AesEncryption
    {
        //This is we use to generate our symmetric session key
        public byte[] GeneratedRandomNumbre(int? saltLength)
        {
            if (saltLength == null)
            {
                saltLength = 32;
            }

            using (var randomNumbreGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength.Value];
                randomNumbreGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new AesCryptoServiceProvider())
            {
                //Default values, we write then for more clear.
                des.Mode = CipherMode.CBC; //Represent Cipher Block Chaing
                des.Padding = PaddingMode.PKCS7;

                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var crytoStream = new CryptoStream(memoryStream
                                                    , des.CreateEncryptor()
                                                    , CryptoStreamMode.Write);

                    crytoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    crytoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new AesCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var crytoStream = new CryptoStream(memoryStream, des.CreateDecryptor()
                        , CryptoStreamMode.Write);

                    crytoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    crytoStream.FlushFinalBlock(); //update

                    return memoryStream.ToArray();
                }

            }
        }
    }
}
