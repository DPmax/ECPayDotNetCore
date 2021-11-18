
namespace ECPay.Models
{
    /// <summary>
    /// Test Stage only, later will replace with shopping cart item
    /// </summary>
    public class TestCheckoutItem : ITestCheckoutItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
