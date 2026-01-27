using System.Security.Cryptography;
using System.Text;

namespace DAL.Helpers
{
    public static class PasswordHelper
    {
        public static string Md5(string text)
        {
            using MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
