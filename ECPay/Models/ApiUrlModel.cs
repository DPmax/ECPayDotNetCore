using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Resources;
using ECPay.Resources;
using ECPay.Enumeration;
using System.Collections;
using System.Globalization;

namespace ECPay.Models
{
    public class ApiUrlModel : IApiUrlModel
    {
        private string _cacheName = "apiList";
        private ObjectCache _cache = MemoryCache.Default;

        public ApiUrlModel()
        {
        }

        public List<ApiUrl> GetList()
        {
            var _apiUrls = (List<ApiUrl>)_cache.Get(_cacheName);
            if (_apiUrls == null)
                _apiUrls = GetApiUrls();
            return _apiUrls;
        }

        private List<ApiUrl> GetApiUrls()
        {
            ResourceSet resourceSet;
            var list = new List<ApiUrl>();

            resourceSet = ApiUrl_Dev_Resource.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                list.Add(
                   new ApiUrl()
                   {
                       _apiUrl = entry.Value.ToString(),
                       _env = EEnvironment.Dev,
                       _invM = (EInvoiceMethod)Enum.Parse(typeof(EInvoiceMethod), entry.Key.ToString())
                   }
                );
            }

            resourceSet = ApiUrl_Stage_Resource.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                list.Add(
                   new ApiUrl()
                   {
                       _apiUrl = entry.Value.ToString(),
                       _env = EEnvironment.Stage,
                       _invM = (EInvoiceMethod)Enum.Parse(typeof(EInvoiceMethod), entry.Key.ToString())
                   }
                );
            }

            resourceSet = ApiUrl_Prod_Resource.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                list.Add(
                   new ApiUrl()
                   {
                       _apiUrl = entry.Value.ToString(),
                       _env = EEnvironment.Prod,
                       _invM = (EInvoiceMethod)Enum.Parse(typeof(EInvoiceMethod), entry.Key.ToString())
                   }
                );
            }
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddHours(12);
            _cache.Set(_cacheName, list, policy);
            return list;
        }
    }
}
