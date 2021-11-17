using System;

namespace ECPay.Attributes
{
    public class TextAttribute : Attribute
    {
        public string _text;

        public TextAttribute(string text)
        {
            _text = text;
        }
    }
}
