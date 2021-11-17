
namespace ECPay.Models
{
    /// <summary>
    /// 開立發票回傳Model
    /// </summary>
    public class InvoiceReturn : IInvoiceReturn
    {
        /// <summary>
        /// 發票號碼    ‧若回應代碼 = '1'時，則VAL = '新的發票號碼'
        ///             ‧若回應代碼 != '1'時，則VAL = ''
        /// </summary>
        public string InvoiceNumber { get ; set; }
        /// <summary>
        /// 發票開立時間  ‧回傳格式為「yyyy-MM-dd HH:mm:ss」
        ///               ‧若回應代碼 = '1'時，則VAL = '開立時間'
        ///               ‧若回應代碼 != '1'時，則VAL = ''
        /// </summary>
        public string InvoiceDate { get ; set ; }
        /// <summary>
        /// 隨機碼
        /// </summary>
        public string RandomNumber { get ; set ; }
        /// <summary>
        /// 回應代碼
        /// </summary>
        public string RtnCode { get ; set ; }
        /// <summary>
        /// 回應代碼說明
        /// </summary>
        public string RtnMsg { get ; set ; }
    }
}
