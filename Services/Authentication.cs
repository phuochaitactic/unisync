using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Services
{
    //public static class Authentication
    //{
    //    public static bool ValidateAdmin(this ControllerBase controller)
    //    {
    //        var (userId, username, role) = controller.GetSessionUserInfo();
    //        return userId != null && username != null && role == "admin";
    //    }
    //}
    public interface IAuthService
    {
        bool ValidateAdmin(ControllerBase controller);

        bool ValidateKhoa(ControllerBase controller);

        bool ValidateSinhVien(ControllerBase controller);

        bool ValidateGiangVien(ControllerBase controller);

        bool ValidateThuKyKhoa(ControllerBase controller);
    }

    public class Authentication : IAuthService
    {
        public bool ValidateAdmin(ControllerBase controller)
        {
            // Authentication logic
            var (userId, username, role) = controller.GetSessionUserInfo();
            return userId != null && username != null && role == "admin";
        }

        public bool ValidateKhoa(ControllerBase controller)
        {
            // Authentication logic
            var (userId, username, role, tenKhoa) = controller.GetKhoaSessionInfo();
            return userId != null && username != null && role == "Khoa";
        }

        public bool ValidateSinhVien(ControllerBase controller)
        {
            // Authentication logic
            var (userId, username, role) = controller.GetSessionUserInfo();
            return userId != null && username != null && role == "Sinh Vien";
        }

        public bool ValidateGiangVien(ControllerBase controller)
        {
            // Authentication logic
            var (userId, username, role) = controller.GetSessionUserInfo();
            return userId != null && username != null && role == "Giang Vien";
        }

        public bool ValidateThuKyKhoa(ControllerBase controller)
        {
            // Authentication logic
            var (userId, username, role) = controller.GetSessionUserInfo();
            return userId != null && username != null && role == "Thu Ky Khoa";
        }
    }
}