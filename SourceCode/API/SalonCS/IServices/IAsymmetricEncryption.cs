namespace SalonCS.IServices
{
    public interface IAsymmetricEncryption
    {
        public string GetPublicKey();
        public string GetPrivateKey();

    }
}
