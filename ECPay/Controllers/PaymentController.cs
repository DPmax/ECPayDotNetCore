using Microsoft.AspNetCore.Mvc;
using ECPay.Services.Checkout;
using Nop.Web.Framework.Controllers;

namespace ECPay.Controllers
{
    public class PaymentController : BasePaymentController
    {
        [HttpPost]
        public virtual IActionResult Checkout()
        {
            // setup 
            var service = new PaymentService(true);
            var transaction = new PaymentTransaction(true);
            var payment = new PaymentConfiguration()
                .Send.ToApi(
                    url: service._url)
                .Send.ToMerchant(
                    service._merchantId)
                .Send.UsingHash(
                    key: service._hashKey,
                    iv: service._hashIV)
                .Return.ToServer(
                    url: service._serverUrl)
                .Return.ToClient(
                    url: service._clientUrl)
                .Return.ToClient(
                    url: service._clientUrlWithExtraPaidInfo, needExtraPaidInfo: true)
                .Transaction.New(
                    no: transaction._transactionNo,
                    description: transaction._transactionDescription,
                    date: transaction._transactionDate)
                .Transaction.UseMethod(
                    method: transaction._transactionMethod)
                .Transaction.WithItems(
                    items: transaction._transactionItems)
                .Generate();

            return View(payment);
        }
    }
}
