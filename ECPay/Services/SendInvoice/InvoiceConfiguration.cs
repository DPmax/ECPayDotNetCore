using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ECPay.Models;
using ECPay.Enumeration;
using ECPay.Attributes;
using System.Reflection;
using System.Web;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace ECPay.Services.SendInvoice
{
    /// <summary>
    /// 操作發票各功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InvoiceConfiguration<T> : IDisposable
    {
        #region CTOR

        /// <summary>
        /// initial, 供內部測試注入 apimodel 使用
        /// </summary>
        /// <param name="obj"></param>
        internal InvoiceConfiguration(IApiUrlModel apimodel)
        {
            _iapi = apimodel;
        }

        /// <summary>
        /// initial
        /// </summary>
        /// <param name="obj"></param>
        public InvoiceConfiguration()
        {
            _iapi = new ApiUrlModel();
        }

        #endregion CTOR


        #region - Public 屬性欄位成員

        /// <summary>
        /// 介接的 HashKey。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        public string HashKey { get; set; }

        /// <summary>
        /// 介接的 HashIV。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        public string HashIV { get; set; }

        /// <summary>
        /// 執行環境
        /// Stage -- 測試
        /// Prod -- 正式
        /// </summary>
        public EEnvironment Environment
        {
            get => _environment;
            set => _environment = value;
        }

        #endregion - Public 屬性欄位成員

        #region - Private 屬性欄位成員

        /// <summary>
        /// 廠商驗證時間(自動產生)。
        /// </summary>
        private EEnvironment _environment = EEnvironment.Stage;

        /// <summary>
        /// 代入API的 Iinvoice 介面
        /// </summary>
        private IInvoiceConfiguration _iinv;

        /// <summary>
        /// 各API位置的資料參考介面
        /// </summary>
        private IApiUrlModel _iapi;

        /// <summary>
        /// 將輸入的物件各參數轉為 namevalue collection 字串
        /// </summary>
        private string _parameters = string.Empty;

        /// <summary>
        /// 不加入驗證的參數
        /// </summary>
        private string[] _ignoreMacValues = { "CHECKMACVALUE", "ITEMNAME", "ITEMWORD", "REASON", "INVOICEREMARK", "SPECSOURCE", "ITEMREMARK", "POSBARCODE", "QRCODE_LEFT", "QRCODE_RIGHT" };

        #endregion - Private 屬性欄位成員

        /// <summary>
        /// 開始執行發票功能, 並取得結果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Post(T obj)
        {
            _iinv = obj as IInvoiceConfiguration;

            //先作驗證
            var valid = Validate(obj);

            if (!string.IsNullOrEmpty(valid))
                return string.Format("{{\"RtnCode\":0, \"RtnMsg\":\"{0}\"}}", valid);

            //組出傳送字串
            ObjectToNameValueCollection(obj);

            //取出API位置
            var url = _iapi.GetList().Where(p => p._invM == _iinv._invM && p._env == Environment).FirstOrDefault();
            //var url = _iapi.GetList().Where(p => p._invM == _inv.InvMethod && p._env == Environment).FirstOrDefault();

            //作壓碼字串
            var checkmacvalue = BuildCheckMacValue(_parameters);

            //組出實際傳送的字串
            var urlstring = string.Format("{0}&{1}", _parameters, "CheckMacValue=" + checkmacvalue);

            //執行api功能, 並取得回傳結果
            var result = ServerPost(urlstring, url._apiUrl);

            return ValidReturnString(result);
        }

        #region - 私用方法

        /// <summary>
        /// 驗證欄位並傳回字串
        /// </summary>
        /// <param name="obj">傳入需驗證的物件</param>
        /// <returns>回傳驗證後訊息</returns>
        private string Validate(T obj)
        {
            var result = new StringBuilder();
            var errList = new List<string>();

            errList.AddRange(ServerValidator.Validate(obj));

            if (errList.Count > 0)
            {
                foreach (var item in errList)
                {
                    result.Append(item.ToString() + " ");
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 產生物件的參數字串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private void ObjectToNameValueCollection(T obj)
        {
            var elemType = obj.GetType();
            var value = string.Empty;
            object attr = null;

            //取出物件的原型
            foreach (var item in elemType.GetProperties(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance)
                //.Where(x => !x.GetCustomAttributes(typeof(NonProcessValueAttribute),true).Any())
                .OrderBy(i => i.Name))
            {
                //如果Attribute有設定不處理就直接跳過
                attr = item.GetCustomAttributes(typeof(NonProcessValueAttribute), true).FirstOrDefault();
                if (attr != null)
                    continue;

                try
                {
                    if (item.PropertyType.IsEnum) //Enum
                    {
                        int enumVlue = (int)Enum.Parse(item.PropertyType, item.GetValue(obj, null).ToString());
                        value = enumVlue.ToString();
                    }
                    else if (item.PropertyType.IsClass && item.PropertyType.IsSerializable) //String
                        value = (string)item.GetValue(obj, null) ?? "";
                    else if (item.PropertyType.IsValueType && item.PropertyType.IsSerializable && !item.PropertyType.IsEnum) //Int
                        value = item.GetValue(obj, null).ToString();
                    //else if (item.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(item.PropertyType.GetGenericTypeDefinition()) ||
                    //        item.PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
                    //    continue;
                    else //本來要排除 Itemcollection, 不過上面的排除方法太費工, 所以改成找不到的就當作是 Itemcollection
                        continue;
                }
                catch
                {
                    throw new Exception("Failed to set property value for our Foreign Key");
                }

                // 特定 Attribute 不作Encode
                attr = item.GetCustomAttributes(typeof(NeedEncodeAttribute), true).FirstOrDefault();
                if (attr != null)
                    value = HttpUtility.UrlEncode(value);

                // 特定 Attribute 需要Replace
                attr = item.GetCustomAttributes(typeof(NeedReplaceAttribute), true).FirstOrDefault();
                if (attr != null)
                    value = value.ToString().Replace('+', ' ');

                if (string.IsNullOrEmpty(_parameters))
                    _parameters = string.Format("{0}={1}", item.Name, value);
                else
                    _parameters += string.Format("&{0}={1}", item.Name, value);
            }
        }

        /// <summary>
        /// 產生檢查碼。
        /// 並排除不作驗證的字串
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string BuildCheckMacValue(string param)
        {
            //排除不作驗證的字串
            string urlparams = RemoveIgnoreMacValues(param);
            // 產生檢查碼。
            string szCheckMacValue = string.Empty;
            szCheckMacValue = string.Format("HashKey={0}&{1}&HashIV={2}", HashKey, urlparams, HashIV);
            //saveLog("傳送字串Encode前:" + szCheckMacValue);
            szCheckMacValue = HttpUtility.UrlEncode(szCheckMacValue).ToLower();
            //saveLog("傳送字串Encode後:" + szCheckMacValue);
            szCheckMacValue = MD5Encoder.Encrypt(szCheckMacValue);
            return szCheckMacValue;
        }

        /// <summary>
        /// 驗證回傳字串，並回覆結果字串
        /// </summary>
        /// <param name="returnString"></param>
        /// <returns>Json格式的字串</returns>
        private string ValidReturnString(string returnString)
        {
            //整理回傳結果
            var nvc = HttpUtility.ParseQueryString(returnString);
            //取出結果及驗證碼
            var rtnCode = nvc["RtnCode"].ToString();
            var checkMacValue = nvc["CheckMacValue"];
            //saveLog("結果:" + returnString);

            //回傳成功的資訊, 如果回覆成功就比對驗證碼確認資料正確
            if (rtnCode == "1")
            {
                var returnBuildCheckMacValue = BuildCheckMacValue(returnString);
                //saveLog("自己產生的驗證字串:" + returnBuildCheckMacValue);
                if (checkMacValue != returnBuildCheckMacValue)
                {
                    nvc["RtnCode"] = "1000001";
                    nvc["RtnMsg"] = "計算回傳檢核碼失敗";
                }
            }

            return JsonConvert.SerializeObject(nvc.AllKeys.ToDictionary(x => x, y => nvc[y]));
        }

        /// <summary>
        /// 將輸入的URL String 字串, 排除不加入驗證規則的參數
        /// </summary>
        /// <param name="urlstring"></param>
        /// <returns></returns>
        private string RemoveIgnoreMacValues(string urlstring)
        {
            //Regex regex = new Regex("(?<Key>[^= ]+)\\s*=\\s*\"(?<Value>[^\"]+)\"\\s+");
            var regexExam = @"([^=|^\&]+)\=([^\&]+)?";
            var regex = new Regex(regexExam);
            var matches = regex.Matches(urlstring);

            var nvc = new NameValueCollection();
            foreach (Match m in matches)
            {
                var kv = m.Value.ToString().Split('=');
                var key = kv[0];
                var value = kv[1];
                if (key.ToUpper() == "IIS_CARRUER_NUM")
                    value = value.Replace('+', ' ');
                if (!_ignoreMacValues.Contains(key.ToUpper()))
                    nvc.Add(key, value);
            }
            var param = string.Join("&", nvc.AllKeys.OrderBy(key => key).Select(key => key + "=" + nvc[key]).ToArray());
            return param;
        }

        /// <summary>
        /// 伺服器端傳送參數請求資料。
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="apiURL"></param>
        /// <returns></returns>
        private string ServerPost(string parameters, string apiURL)
        {
            var szResult = string.Empty;
            var byContent = Encoding.UTF8.GetBytes(parameters);

            //saveLog("實際字串:" + parameters);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls1.2;

            var webRequest = WebRequest.Create(apiURL);
            {
                webRequest.Credentials = CredentialCache.DefaultCredentials;

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

        /// <summary>
        /// 紀錄參數 DeBug 用
        /// </summary>
        /// <param name="requestForm"></param>
        /// <param name="logType"></param>
        private void SaveLog(string str)
        {
            var fileName = "c://" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            var fileContent = new StringBuilder();
            fileContent.Append("QueryString:").Append(str).AppendLine();
            fileContent.Append("Log建立時間:").Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).AppendLine();
            fileContent.AppendLine();

            File.AppendAllText(fileName, fileContent.ToString());
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        #endregion - 私用方法

        #region - 釋放使用資源

        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作。
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
            ;
        }

        #endregion - 釋放使用資源
    }
}
