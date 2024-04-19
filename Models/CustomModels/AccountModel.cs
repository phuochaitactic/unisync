namespace BuildCongRenLuyen.Models.CustomModels
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
        public string Role { get; set; } = null;
    }
}