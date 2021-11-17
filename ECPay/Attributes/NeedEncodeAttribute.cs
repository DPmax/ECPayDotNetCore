using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Attributes
{
    /// <summary>
    /// 表示不作UrlEncode動作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class NeedEncodeAttribute : Attribute
    {
        public NeedEncodeAttribute()
        {
        }
    }
}
