using System;

namespace ECPay.Attributes
{
    /// <summary>
    /// 完全不處理
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class NonProcessValueAttribute : Attribute
    {
        public NonProcessValueAttribute()
        : base()
        {
        }
    }
}
