using SalonCS.IServices;
using System.Security.Cryptography;
using System.Text;

namespace SalonCS.Services
{
    public class RSAService : IRSAService
    {
        public string Decrypt(string encryptedValue)
        {
            var privateKey = Environment.GetEnvironmentVariable("PrivateKey");

            if (String.IsNullOrEmpty(privateKey)) throw new Exception("Encryption keys not found");

            using (var rsa = new RSACryptoServiceProvider(512))
            {
                rsa.FromXmlString(privateKey);
                var resultBytes = Convert.FromBase64String(encryptedValue);
                var decryptedBytes = rsa.Decrypt(resultBytes, true);
                var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedData;
            }
        }
    }
}
