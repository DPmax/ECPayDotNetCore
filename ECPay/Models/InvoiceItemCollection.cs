using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using ECPay.EventHandlers;


namespace ECPay.Models
{
    /// <summary>
    /// 商品項目。
    /// </summary>
    public class InvoiceItem
    {
        /// <summary>
        /// 商品名稱。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        public string ItemName { get; set; }

        /// <summary>
        /// 商品訂購數量。
        /// </summary>
        //[Range(1, int.MaxValue, ErrorMessage = "{0} is out of range. ")]
        public string ItemCount { get; set; }

        /// <summary>
        /// 商品單位(當 InvoiceMark=Yes 時，則必填)
        /// </summary>
        //[RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(6, ErrorMessage = "{0} max langth as {1}.")]
        public string ItemWord { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "{0} is incorrect format.")]
        public string ItemPrice { get; set; }

        /// <summary>
        /// 商品課稅別(當 InvoiceMark=Yes 時，則必填)。
        /// </summary>
        //[RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
        public string ItemTaxType { get; set; }

        /// <summary>
        /// 商品合計
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "{0} is incorrect format.")]
        public string ItemAmount { get; set; }

        /// <summary>
        /// 商品備註
        /// </summary>
        public string ItemRemark { get; set; }

        /// <summary>
        /// 商品項目的建構式。
        /// </summary>
        public InvoiceItem()
        {
            //this.ItemTaxType = TaxTypeEnum.Taxable;
        }
    }

    /// <summary>
    /// 商品項目的集合類別。
    /// </summary>
    public class InvoiceItemCollection : List<InvoiceItem>
    {
        internal string ItemName { get; private set; }
        internal string ItemCount { get; private set; }
        internal string ItemWord { get; private set; }
        internal string ItemPrice { get; private set; }
        internal string ItemTaxType { get; private set; }
        internal string ItemAmount { get; private set; }
        internal string ItemRemark { get; private set; }

        /// <summary>
        /// 將物件加入至商品集合的結尾。
        /// </summary>
        /// <param name="item">要加入至商品集合結尾的物件。</param>
        public new void Add(InvoiceItem item)
        {
            base.Add(item);
            RaiseCollectionEvents("Add");
        }

        /// <summary>
        /// 將特定商品集合的元素加入至商品集合的結尾。
        /// </summary>
        /// <param name="collection">商品集合，其元素應加入至商品集合的結尾。集合本身不能是 null，但它可以包含 null 的元素。</param>
        public new void AddRange(IEnumerable<InvoiceItem> collection)
        {
            base.AddRange(collection);
            RaiseCollectionEvents("AddRange");
        }

        /// <summary>
        /// 將所有元素從商品集合移除。
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            RaiseCollectionEvents("Clear");
        }

        /// <summary>
        /// 將項目插入商品集合中指定的索引處。
        /// </summary>
        /// <param name="index">應在該處插入 item 之以零起始的索引。</param>
        /// <param name="item">要插入的物件。</param>
        public new void Insert(int index, InvoiceItem item)
        {
            base.Insert(index, item);
            RaiseCollectionEvents("Insert");
        }

        /// <summary>
        /// 將商品集合的元素插入至位於指定索引的商品集合中。
        /// </summary>
        /// <param name="index">應插入新元素處的以零起始的索引。</param>
        /// <param name="collection">商品集合，其項目應插入至商品集合。集合本身不能是 null，但它可以包含 null 的項目。</param>
        public new void InsertRange(int index, IEnumerable<InvoiceItem> collection)
        {
            base.InsertRange(index, collection);
            RaiseCollectionEvents("InsertRange");
        }

        /// <summary>
        /// 從商品集合移除特定物件的第一個相符項目。
        /// </summary>
        /// <param name="item">要從商品集合中移除的物件。參考型別的值可以是 null。</param>
        /// <returns>如果成功移除 item 則為 true，否則為 false。如果在商品集合中找不到 item，則這個方法也會傳回 false。</returns>
        public new bool Remove(InvoiceItem item)
        {
            bool blResult = base.Remove(item);
            RaiseCollectionEvents("Remove");
            return blResult;
        }

        /// <summary>
        /// 移除符合指定之述詞所定義的條件之所有項目。
        /// </summary>
        /// <param name="match">定義要移除項目之條件的 System.Predicate&lt;T&gt; 委派。</param>
        /// <returns>商品集合中已移除的項目數。</returns>
        public new int RemoveAll(Predicate<InvoiceItem> match)
        {
            int nResult = base.RemoveAll(match);
            RaiseCollectionEvents("RemoveAll");
            return nResult;
        }

        /// <summary>
        /// 移除商品集合中指定之索引處的項目。
        /// </summary>
        /// <param name="index">要移除元素之以零起始的索引。</param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            RaiseCollectionEvents("RemoveAt");
        }

        /// <summary>
        /// 從商品集合移除的元素範圍。
        /// </summary>
        /// <param name="index">要移除之元素範圍內之以零起始的起始索引。</param>
        /// <param name="count">要移除的元素數目。</param>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            RaiseCollectionEvents("RemoveRange");
        }

        /// <summary>
        /// 資料集合被異動時的事件。
        /// </summary>
        private event ItemCollectionEventHandler CollectionChanged;

        /// <summary>
        /// 當產品集合被異動時，觸發連動的事件。
        /// </summary>
        /// <param name="sender">來源物件。</param>
        /// <param name="e">提供 RaisePropertyEvents 事件的資料。</param>
        private void Items_CollectionChanged(object sender, InvoiceItemCollectionEventArgs e)
        {
            if (!string.IsNullOrEmpty(e._methodName))
                RaisePropertyEvents(p => p.Items);
        }

        /// <summary>
        /// 實作事件觸發時要處理的方法。
        /// </summary>
        /// <param name="methodName">屬性參數。</param>
        private void RaiseCollectionEvents(string methodName)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new InvoiceItemCollectionEventArgs(this, methodName));
        }

        /// <summary>
        /// 屬性值變更時，所要觸發的事件。
        /// </summary>
        private event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 實作事件觸發時要處理的方法。
        /// </summary>
        /// <typeparam name="T">觸發事件的屬性型別。</typeparam>
        /// <param name="property">屬性參數。</param>
        private void RaisePropertyEvents<T>(Expression<Func<InvoiceCreate, T>> property)
        {
            var meExpression = property.Body as MemberExpression;

            if (meExpression == null || meExpression.Expression != property.Parameters[0] || meExpression.Member.MemberType != MemberTypes.Property)
                throw new InvalidOperationException("Now tell me about the property");

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(meExpression.Member.Name));
            }
        }

        /// <summary>
        /// 特定屬性參數修改時觸發同步異動的事件。
        /// </summary>
        /// <param name="sender">來源物件。</param>
        /// <param name="e">提供 System.ComponentModel.INotifyPropertyChanged.PropertyChanged 事件的資料。</param>
        private void InvoiceCreate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            {
                if (Count > 0)
                {
                    var invItemName = string.Empty;
                    var invItemCount = string.Empty;
                    var invItemWord = string.Empty;
                    var invItemPrice = string.Empty;
                    var invItemTaxType = string.Empty;
                    var invItemAmount = string.Empty;
                    var invItemRemark = string.Empty;

                    foreach (var oItem in this)
                    {
                        invItemName += string.Format("{0}|", oItem.ItemName);
                        invItemCount += string.Format("{0}|", oItem.ItemCount);
                        invItemWord += string.Format("{0}|", oItem.ItemWord);
                        invItemPrice += string.Format("{0}|", oItem.ItemPrice);
                        invItemTaxType += string.Format("{0}|", oItem.ItemTaxType);
                        invItemAmount += string.Format("{0}|", oItem.ItemAmount);
                        invItemRemark += string.Format("{0}|", oItem.ItemRemark);
                    }
                    // 電子發票
                    invItemName = invItemName.Substring(0, invItemName.Length - 1);
                    invItemCount = invItemCount.Substring(0, invItemCount.Length - 1);
                    invItemWord = (invItemWord.Length == Count ? string.Empty : invItemWord.Substring(0, invItemWord.Length - 1));
                    invItemPrice = invItemPrice.Substring(0, invItemPrice.Length - 1);
                    invItemTaxType = (invItemTaxType.Length == Count ? string.Empty : invItemTaxType.Substring(0, invItemTaxType.Length - 1));
                    invItemAmount = invItemAmount.Substring(0, invItemAmount.Length - 1);
                    invItemRemark = invItemRemark.Substring(0, invItemRemark.Length - 1);

                    ItemName = invItemName;
                    ItemCount = invItemCount;
                    ItemWord = invItemWord;
                    ItemPrice = invItemPrice;
                    ItemTaxType = invItemTaxType;
                    ItemAmount = invItemAmount;
                    ItemRemark = invItemRemark;
                }
            }
        }

        /// <summary>
        /// Initail ItemCollection
        /// </summary>
        public InvoiceItemCollection()
        {
            CollectionChanged += new ItemCollectionEventHandler(Items_CollectionChanged);
            PropertyChanged += new PropertyChangedEventHandler(InvoiceCreate_PropertyChanged);
        }
    }
}
