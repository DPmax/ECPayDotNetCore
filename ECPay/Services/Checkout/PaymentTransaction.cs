using System;
using System.Collections.Generic;
using ECPay.Enumeration;
using ECPay.Models;

namespace ECPay.Services.Checkout
{
    /// <summary>
    /// 集中設定與綠界訂單交易有關之參數。
    /// </summary>
    public class PaymentTransaction
    {
        #region Public Fields

        public string _transactionNo;
        public string _transactionDescription;
        public DateTime _transactionDate;
        public EPaymentMethod _transactionMethod;
        // product
        // private List<ShoppingCartItem>
        // test only
        public List<TestCheckoutItem> _transactionItems;

        #endregion

        #region CTOR

        public PaymentTransaction(bool test = false)
        {
            // checkout post url
            // Test
            // Product
            if (test)
            {
                _transactionNo = "test1205";
                _transactionDescription = "TEST payment";
                _transactionDate = DateTime.Now;
                _transactionMethod = EPaymentMethod.Credit;
                _transactionItems = new List<TestCheckoutItem>
                {
                    new TestCheckoutItem
                    {
                        Name = "筆記本",
                        Price = 500,
                        Quantity = 2
                    },
                    new TestCheckoutItem
                    {
                        Name = "電子表",
                        Price = 200,
                        Quantity = 5
                    }
                };

            }
            else
            {
                _transactionNo = "";
                _transactionDescription = "";
                _transactionDate = DateTime.Now;
                _transactionMethod = EPaymentMethod.ALL;
                //_transactionItems = new List<ShoppingCartItem> { }
            }


        }

        #endregion
    }
}
