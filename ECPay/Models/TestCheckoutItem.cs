﻿
namespace ECPay.Models
{
    public class TestCheckoutItem : ITestCheckoutItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
