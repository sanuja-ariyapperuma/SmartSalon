namespace SalonCS.IServices
{
    public interface IPasswordService
    {
        public string GeneratePassword(string? password);
        public bool MatchPassword(string requestPassword,string actualPassword);
        public bool VerifyPassword(string requestPassword, string actualPassword);
    }
}
