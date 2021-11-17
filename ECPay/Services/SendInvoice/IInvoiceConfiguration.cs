using ECPay.Enumeration;

namespace ECPay.Models
{
    /// <summary>
    /// 發票類別
    /// </summary>
    public interface IInvoiceConfiguration
    {
        EInvoiceMethod _invM { get; }
    }
}
