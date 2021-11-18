using System;

namespace ECPay.EventHandlers
{
    /// <summary>
    /// 產品項目集合變更事件的 Handler。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ItemCollectionEventHandler(object sender, InvoiceItemCollectionEventArgs e);

    /// <summary>
    /// 產品項目集合變更事件的事件參數類別。
    /// </summary>
    public class InvoiceItemCollectionEventArgs : EventArgs
    {
        /// <summary>
        /// 變更事件的方法名稱。
        /// </summary>
        public string _methodName { get; set; }

        /// <summary>
        /// 產品項目集合變更事件的事件參數類別建構式。
        /// </summary>
        /// <param name="sender">觸發事件的物件來源。</param>
        /// <param name="methodName">變更事件的方法名稱。</param>
        public InvoiceItemCollectionEventArgs(object sender, string methodName)
        {
            _methodName = methodName;
        }
    }
}
