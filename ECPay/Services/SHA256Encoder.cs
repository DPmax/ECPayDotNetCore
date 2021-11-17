using System.Security.Cryptography;
using System.Text;

namespace ECPay
{
    /// <summary>
    /// SHA256 雜湊加密演算法物件。
    /// </summary>
    internal static class SHA256Encoder
    {
        private static readonly HashAlgorithm _crypto = null;

        static SHA256Encoder()
        {
            _crypto = new SHA256CryptoServiceProvider();
        }

        public static string Encrypt(string originalString)
        {
            var source = Encoding.Default.GetBytes(originalString);//將字串轉為Byte[]
            var crypto = _crypto.ComputeHash(source);//進行SHA256加密
            var result = string.Empty;

            for (var i = 0; i < crypto.Length; i++)
            {
                result += crypto[i].ToString("X2");
            }

            return result.ToUpper();
        }
    }
}
