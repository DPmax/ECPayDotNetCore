
namespace ECPay.Models
{
    /// <summary>
    /// Action 回傳Model
    /// </summary>
    public interface IActionReturn
    {
        /// <summary>
        /// 特店編號。
        /// </summary>
        string MerchantID { get; set; }
        /// <summary>
        /// 特店交易編號。訂單產生時傳送給綠界的特店交易編號。英數字大小寫混合。
        /// </summary>
        string MerchantTradeNo { get; set; }
        /// <summary>
        /// 綠界的交易編號。請保存綠界的交易編號與特店交易編號【MerchantTradeNo】的關連。
        /// </summary>
        string TradeNo { get; set; }
        /// <summary>
        /// 回應代碼
        /// </summary>
        string RtnCode { get; set; }
        /// <summary>
        /// 回應代碼說明
        /// </summary>
        string RtnMsg { get; set; }
    }
}
