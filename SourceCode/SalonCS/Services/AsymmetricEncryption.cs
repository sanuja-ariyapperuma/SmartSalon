using SalonCS.IServices;
using System.Security.Cryptography;

namespace SalonCS.Services
{
    public class AsymmetricEncryption : IAsymmetricEncryption
    {
        public string GetPrivateKey()
        {
            return Environment.GetEnvironmentVariable("PrivateKey");
        }

        public string GetPublicKey()
        {
            return Environment.GetEnvironmentVariable("PublicKey");
        }
    }
}
