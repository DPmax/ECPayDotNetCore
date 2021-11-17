using System;
using System.Reflection;
using ECPay.Attributes;

namespace ECPay.Services
{
    public static class EnumExtensions
    {
        public static string ToText(this Enum enumeration)
        {
            var memberInfo = enumeration.GetType().GetMember(enumeration.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(TextAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return ((TextAttribute)attributes[0])._text;
                }
            }
            return enumeration.ToString();
        }
    }
}
