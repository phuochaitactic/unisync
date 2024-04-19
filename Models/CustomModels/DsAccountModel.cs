namespace BuildCongRenLuyen.Models.CustomModels
{
    public class DsAccountModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; } = null;
        public string? name { get; set; } = null;
    }
}