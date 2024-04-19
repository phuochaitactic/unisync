using System.Security.Cryptography;
using System.Text;

namespace BuildCongRenLuyen.Services
{
    public class PasswordGenerator
    {
        public static string HashPassword(string pwd)
        {
            if (string.IsNullOrEmpty(pwd)) return "";
            SHA256 sha = new SHA256Managed();
            var pwdBuff = Encoding.ASCII.GetBytes(pwd);
            var hashedPwd = sha.TransformFinalBlock(pwdBuff, 0, pwdBuff.Length);
            var hash = new StringBuilder();
            foreach (var b in sha.Hash)
            {
                hash.Append(string.Format("{0:x2}", b));
            }
            sha.Clear();
            return hash.ToString();
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Hash input password
            string hashedInput = HashPassword(password);

            // Compare hashed input to stored hash
            if (hashedInput == hashedPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}