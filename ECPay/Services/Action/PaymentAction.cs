using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using ECPay.Attributes;
using ECPay.Enumeration;

namespace ECPay.Services.Action
{
    /// <summary>
    /// Actions for payment Close Account, Refund, Cancel Close Account and Abandon Transaction
    /// </summary>
    /// <value>String(10)</value>
    public class PaymentAction : IPaymentAction
    {
        public string _url;
        public string _hashKey;
        public string _hashIV;

        /// <summary>
        /// 特店編號。
        /// </summary>
        /// <value>String(10)</value>
        private string _merchantID = string.Empty;
        /// <summary>
        /// 特店交易編號。訂單產生時傳送給綠界的特店交易編號。英數字大小寫混合。
        /// </summary>
        /// <value>String(20)</value>
        private string _merchantTradeNo = string.Empty;
        /// <summary>
        /// 綠界的交易編號。請保存綠界的交易編號與特店交易編號【MerchantTradeNo】的關連。
        /// </summary>
        /// <value>String(20)</value>
        private string _tradeNo = string.Empty;
        /// <summary>
        /// 訂單總金額。
        /// </summary>
        /// <value>Decimal</value>
        private decimal _totalAmount = 0;
        /// <summary>
        /// 特約合作平台商代號。
        /// </summary>
        /// <value>String(10)</value>
        private string _platformID = string.Empty;

        public PaymentAction()
        {
            _url = "https://payment.ecpay.com.tw/CreditDetail/DoAction";
            _hashKey = "5294y06jbISpM5x9"; // cannot test
            _hashIV = "v77hoKGq4kWxNNIS"; // cannot test
            _merchantID = "2000132"; //廠商編號
            _merchantTradeNo = "test0000135090201376"; // from payment
            _tradeNo = "2111110929139582"; // from payment
            _totalAmount = decimal.Parse("300"); // from payment
        }

        /// <summary>
        /// Action類別(自動產生 Refund)
        /// </summary>
        [NonProcessValueAttribute]
        EActionType IPaymentAction._actionType
        {
            get { return EActionType.R; }
        }

        /// <summary>
        /// 廠商編號(必填)
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
        public string MerchantID { get => _merchantID; set => _merchantID = value; }

        /// <summary>
        /// 特店交易編號。訂單產生時傳送給綠界的特店交易編號。英數字大小寫混合。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
        public string MerchantTradeNo { get => _merchantTradeNo; set => _merchantTradeNo = value; }

        /// <summary>
        /// 綠界的交易編號。請保存綠界的交易編號與特店交易編號【MerchantTradeNo】的關連。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
        public string TradeNo { get => _tradeNo; set => _tradeNo = value; }

        /// <summary>
        /// 訂單總金額。
        /// </summary>
        /// <value>Decimal</value>
        [Required(ErrorMessage = "{0} is required.")]
        public decimal TotalAmount { get => _totalAmount; set => _totalAmount = value; }

        /// <summary>
        /// 產生參數字串。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string BuildParameter(string id, object value)
        {
            var szValue = string.Empty;
            var szParameter = string.Empty;

            if (null != value)
            {
                if (value.GetType().Equals(typeof(DateTime)))
                    szValue = ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss");
                else
                    szValue = value.ToString();
            }

            szParameter = string.Format("&{0}={1}", id, szValue);

            return szParameter;
        }

        /// <summary>
        /// 產生檢查碼。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string BuildCheckMacValue(string parameters, int encryptType = 0)
        {
            var szCheckMacValue = string.Empty;
            // 產生檢查碼。
            szCheckMacValue = string.Format("HashKey={0}{1}&HashIV={2}", _hashKey, parameters, _hashIV);
            szCheckMacValue = HttpUtility.UrlEncode(szCheckMacValue).ToLower();
            if (encryptType == 1)
            {
                szCheckMacValue = SHA256Encoder.Encrypt(szCheckMacValue);
            }
            else
            {
                szCheckMacValue = MD5Encoder.Encrypt(szCheckMacValue);
            }

            return szCheckMacValue;
        }

        /// <summary>
        /// 伺服器端傳送參數請求資料。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string ServerPost(string parameters)
        {
            var szResult = string.Empty;
            var byContent = Encoding.UTF8.GetBytes(parameters);
            var webRequest = WebRequest.Create(_url);
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                webRequest.ContentLength = byContent.Length;

                using (var oStream = webRequest.GetRequestStream())
                {
                    oStream.Write(byContent, 0, byContent.Length); //Push it out there
                    oStream.Close();
                }

                var webResponse = webRequest.GetResponse();
                {
                    if (null != webResponse)
                    {
                        using (var oReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            szResult = oReader.ReadToEnd().Trim();
                        }
                    }

                    webResponse.Close();
                    webResponse = null;
                }

                webRequest = null;
            }

            return szResult;
        }

        public IEnumerable<string> DoAction(ref Hashtable feedback)
        {
            var parameters = string.Empty;
            var macValue = string.Empty;
            var serverReturn = string.Empty;

            var errList = new List<string>();

            if (feedback == null)
            {
                feedback = new Hashtable();
            }

            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(EActionType.R));

            if(errList.Count == 0)
            {
                // 產生畫面控制項與傳遞參數。
                parameters += BuildParameter("Action", EActionType.R.ToString());
                parameters += BuildParameter("MerchantID", _merchantID);
                parameters += BuildParameter("MerchantTradeNo", _merchantTradeNo);
                parameters += BuildParameter("TotalAmount", _totalAmount);
                parameters += BuildParameter("TradeNo", _tradeNo);
                // 產生檢查碼。using MD5
                macValue = BuildCheckMacValue(parameters);
                // 繪製 MD5 檢查碼控制項。
                parameters += BuildParameter("CheckMacValue", macValue);
                // 紀錄記錄檔
                //Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInOne.DoAction: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));
                // 自遠端伺服器取得資料。
                serverReturn = ServerPost(parameters);
                // 紀錄記錄檔
                //Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInOne.DoAction: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
                //重新整理取回的參數。
                if (!string.IsNullOrEmpty(serverReturn))
                {
                    parameters = string.Empty;
                    macValue = string.Empty;

                    foreach (var szData in serverReturn.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(szData))
                        {
                            var saData = szData.Split(new char[] { '=' });
                            var szKey = saData[0];
                            var szValue = saData[1];

                            if (szKey != "CheckMacValue")
                            {
                                parameters += string.Format("&{0}={1}", szKey, szValue);
                                feedback.Add(szKey, szValue);
                            }
                            else
                            {
                                macValue = szValue;
                            }
                        }
                    }

                    if (feedback.ContainsKey("RtnCode") && !"1".Equals(feedback["RtnCode"]))
                    {
                        errList.Add(string.Format("{0}: {1}", feedback["RtnCode"], feedback["RtnMsg"]));
                    }
                }
            }

            return errList;
        }
    }
}
