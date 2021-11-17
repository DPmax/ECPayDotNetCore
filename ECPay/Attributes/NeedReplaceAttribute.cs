using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Attributes
{
    /// <summary>
    /// 表示需要Replace動作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class NeedReplaceAttribute : Attribute
    {
        public NeedReplaceAttribute()
        {
        }
    }
}
