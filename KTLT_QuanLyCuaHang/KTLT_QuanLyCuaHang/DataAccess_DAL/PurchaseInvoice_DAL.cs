using KTLT_QuanLyCuaHang.Entities;
using Newtonsoft.Json;

namespace KTLT_QuanLyCuaHang.DataAccess_DAL
{
    public class PurchaseInvoice_DAL
    {
        public static PurchaseInvoiceList GetPurchaseInvoiceList()
        {
            StreamReader file = new StreamReader(Constants_DAL.purchaseInvoiceListFilePath);

            string jsonContent = file.ReadToEnd();

            file.Close();

            return JsonConvert.DeserializeObject<PurchaseInvoiceList>(jsonContent);
        }

        public static void SavePurchaseInvoiceList(PurchaseInvoice[] purchaseInvoices)
        {
            PurchaseInvoiceList purchaseInvoiceList = new PurchaseInvoiceList();

            purchaseInvoiceList.total = purchaseInvoices.Length;
            purchaseInvoiceList.purchaseInvoices = purchaseInvoices;

            StreamWriter file = new StreamWriter(Constants_DAL.purchaseInvoiceListFilePath);

            string jsonContent = JsonConvert.SerializeObject(purchaseInvoiceList);

            file.Write(jsonContent);

            file.Close();
        }
    }
}
