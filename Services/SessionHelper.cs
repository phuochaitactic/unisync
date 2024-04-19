using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Services
{
    public static class SessionHelper
    {
        public static (string UserId, string Username, string Role) GetSessionUserInfo(this ControllerBase controller)
        {
            var userId = controller.HttpContext.Session.GetString("UserId");
            var username = controller.HttpContext.Session.GetString("Username");
            var role = controller.HttpContext.Session.GetString("Role");
            return (userId, username, role);
        }

        public static (string UserId, string Username, string Role, string tenKhoa) GetKhoaSessionInfo(this ControllerBase controller)
        {
            var userId = controller.HttpContext.Session.GetString("UserId");
            var username = controller.HttpContext.Session.GetString("Username");
            var role = controller.HttpContext.Session.GetString("Role");
            var tenKhoa = controller.HttpContext.Session.GetString("name");
            return (userId, username, role, tenKhoa);
        }
    }
}