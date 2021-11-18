
namespace ECPay.Enumeration
{
    /// <summary>
    /// 信用卡訂單處理動作。
    /// </summary>
    public enum EActionType
    {
        /// <summary>
        /// 關帳。Close Account
        /// </summary>
        C = 0,
        /// <summary>
        /// 退刷。Refund
        /// </summary>
        R = 1,
        /// <summary>
        /// 取消。Cancelling Close Account
        /// </summary>
        E = 2,
        /// <summary>
        /// 放棄。Abandoning Transaction
        /// </summary>
        N = 3
    }
}
