using Holistor.KeyReader.Dto;
using System.Security.Cryptography;

namespace Holistor.KeyReader.Encrypt
{
    class HybridEncryption
    {
        private readonly AesEncryption _aes = new AesEncryption();

        public Encryptedpacket EncryptData(byte[] original)
        {
            RSAWithRASParameterKey rsaParams = new RSAWithRASParameterKey();

            //Generate our session key
            var sessionKey = _aes.GeneratedRandomNumbre(32);

            //Create the encrypted packet and generate the IV
            var encryptedPaket = new Encryptedpacket { Iv = _aes.GeneratedRandomNumbre(16) };

            //Encrypt our data with AES
            encryptedPaket.EncryptedData = _aes.Encrypt(original, sessionKey, encryptedPaket.Iv);
            //Encrypt the session key with RSA
            encryptedPaket.EncryptedSessionKey = rsaParams.EncryptData(sessionKey);

            using (var hmac = new HMACSHA256(sessionKey))
            {
                encryptedPaket.Hmac = hmac.ComputeHash(encryptedPaket.EncryptedData);
            }

            return encryptedPaket;
        }

        public byte[] DescryptData(Encryptedpacket encryptedPacket)
        {
            RSAWithRASParameterKey rsaParams = new RSAWithRASParameterKey();

            //Decryp AES Key with RSA
            var decryptedSessionKey = rsaParams.DecryptData(encryptedPacket.EncryptedSessionKey);


            using (var hmac = new HMACSHA256(decryptedSessionKey))
            {
                var hmacToCheck = hmac.ComputeHash(encryptedPacket.EncryptedData);

                if (!Compare(encryptedPacket.Hmac, hmacToCheck))
                {
                    throw new CryptographicException("HMAC for decryption does not match");
                }
            }

            //Decrypt our data with AES using the decrypted session key
            var decryptData = _aes.Decrypt(encryptedPacket.EncryptedData,
                                            decryptedSessionKey, encryptedPacket.Iv);

            return decryptData;
        }

        private bool Compare(byte[] array1, byte[] array2)
        {
            var result = array1.Length == array2.Length;

            for (int i = 0; i < array1.Length && i < array2.Length; i++)
            {
                result &= array1[i] == array2[i];
            }

            return result;
        }

    }
}
