namespace KTLT_QuanLyCuaHang.Entities
{
    public struct SaleReceiptList
    {
        public int total;
        public SaleReceipt[] saleReceipts;
    }

    public struct SaleReceipt
    {
        public string id;
        public DateTime createdDateTimeUTC;
        public Product[] exportedGoods;
    }
}
