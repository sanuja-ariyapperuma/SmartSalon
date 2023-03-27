using SalonCS.Model;

namespace SalonCS.IServices
{
    public interface IUserService
    {
        public void RegisterUser(User newUser);
        public bool EditUser(int id, User user);
        public void InactiveUser(User user);
        public Task<User> GetUser(int id);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<bool> IsUsernameAlreadyExists(string username);
        public Task<bool> ResetPassword(int id,string password);

    }
}
