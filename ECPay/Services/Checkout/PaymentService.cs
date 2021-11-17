
namespace ECPay.Services.Checkout
{
    /// <summary>
    /// 集中設定與綠界金流服務介接有關之參數, 付款完成後與回應有關之參數。
    /// </summary>
    public class PaymentService
    {
        #region Public Fields

        public string _url;
        public string _merchantId;
        public string _hashKey;
        public string _hashIV;
        public string _serverUrl;
        public string _clientUrl;
        public string _clientUrlWithExtraPaidInfo;

        #endregion

        #region CTOR

        public PaymentService(bool test = false)
        {
            // checkout post url
            // Test
            // Product
            if (test)
            {
                _url = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";
                _merchantId = "2000132";
                _hashKey = "5294y06jbISpM5x9";
                _hashIV = "v77hoKGq4kWxNNIS";
                // api callback url
                _serverUrl = "http://111c-91-196-220-35.ngrok.io/api/ecpay/callback";
                // customer redirect url
                _clientUrl = "http://localhost:15536/api/ECPay/PaymentView";
                // api callback url with extra info
                _clientUrlWithExtraPaidInfo = "http://111c-91-196-220-35.ngrok.io/api/ecpay/callback";
            }
            else
            {
                _url = "https://payment.ecpay.com.tw/Cashier/AioCheckOut/V5";
                _merchantId = "";
                _hashKey = "";
                _hashIV = "";
                // api callback url
                _serverUrl = "";
                // customer redirect url
                _clientUrl = "";
                // api callback url with extra info
                _clientUrlWithExtraPaidInfo = "";
            }
            
            
        }

        #endregion
    }
}
