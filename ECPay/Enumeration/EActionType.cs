using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Enumeration
{
    /// <summary>
    /// 信用卡訂單處理動作。
    /// </summary>
    public enum EActionType
    {
        /// <summary>
        /// 關帳。Capture
        /// </summary>
        C = 0,
        /// <summary>
        /// 退刷。Refund
        /// </summary>
        R = 1,
        /// <summary>
        /// 取消。Cancelling Capture
        /// </summary>
        E = 2,
        /// <summary>
        /// 放棄。Abandoning Transaction
        /// </summary>
        N = 3
    }
}
