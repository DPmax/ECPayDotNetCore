using System.Security.Cryptography;
using System.Text;

namespace ECPay.Services
{
    /// <summary>
    /// MD5 雜湊加密演算法物件。
    /// </summary>
    internal static class MD5Encoder
    {
        private static readonly HashAlgorithm _crypto = null;

        static MD5Encoder()
        {
            _crypto = new MD5CryptoServiceProvider();
        }

        public static string Encrypt(string originalString)
        {
            var byValue = Encoding.UTF8.GetBytes(originalString);
            var byHash = _crypto.ComputeHash(byValue);

            var stringBuilder = new StringBuilder();

            for (var i = 0; i < byHash.Length; i++)
            {
                stringBuilder.Append(byHash[i].ToString("X").PadLeft(2, '0'));
            }

            return stringBuilder.ToString().ToUpper();
        }
    }
}
