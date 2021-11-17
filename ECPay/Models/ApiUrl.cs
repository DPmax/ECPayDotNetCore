using ECPay.Enumeration;

namespace ECPay.Models
{
    public class ApiUrl
    {
        /// <summary>
        /// API 的模式
        /// </summary>
        public EInvoiceMethod _invM { get; set; }

        /// <summary>
        /// API位置
        /// </summary>
        public string _apiUrl { get; set; }

        /// <summary>
        /// API環境
        /// </summary>
        public EEnvironment _env { get; set; }
    }
}
