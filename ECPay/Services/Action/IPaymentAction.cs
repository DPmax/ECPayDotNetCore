using ECPay.Enumeration;

namespace ECPay.Services.Action
{
    public interface IPaymentAction
    {
        EActionType _actionType { get; }
    }
}
