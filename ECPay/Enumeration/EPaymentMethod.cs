using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Enumeration
{
    /// <summary>
    /// 付款方式。
    /// </summary>
    public enum EPaymentMethod
    {
        Credit,
        Union,
        WebATM,
        ATM,
        CVS,
        BARCODE,
        ALL,
    }
}
