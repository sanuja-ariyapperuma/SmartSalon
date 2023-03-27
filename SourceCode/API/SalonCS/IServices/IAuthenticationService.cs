using SalonCS.DTO;

namespace SalonCS.IServices
{
    public interface IAuthenticationService
    {
        public Task<string> Authenticate(UserCredentials usercred);
        public bool Register(UserCredentials usercred);

        //public bool ResetPassword(int id, string username, string password);
    }
}
