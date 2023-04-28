namespace KTLT_QuanLyCuaHang.Entities
{
    public struct PurchaseInvoiceList
    {
        public int total;
        public PurchaseInvoice[] purchaseInvoices;
    }

    public struct PurchaseInvoice
    {
        public string id;
        public DateTime createdDateTimeUTC;
        public Product[] importedGoods;
        public DateTime? lastUpdateTimeUTC;
    }
}
