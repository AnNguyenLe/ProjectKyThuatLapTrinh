using KTLT_QuanLyCuaHang.Entities;
using Newtonsoft.Json;

namespace KTLT_QuanLyCuaHang.DataAccess_DAL
{
    public class SaleReceipt_DAL
    {
        public static SaleReceiptList GetSaleReceiptList()
        {
            StreamReader file = new StreamReader(Constants_DAL.saleReceiptListFilePath);

            string jsonContent = file.ReadToEnd();

            file.Close();

            return JsonConvert.DeserializeObject<SaleReceiptList>(jsonContent);
        }

        public static void SaveSaleReceiptList(SaleReceipt[] saleReceipts)
        {
            SaleReceiptList saleReceiptList = new SaleReceiptList();

            saleReceiptList.total = saleReceipts.Length;
            saleReceiptList.saleReceipts= saleReceipts;

            StreamWriter file = new StreamWriter(Constants_DAL.saleReceiptListFilePath);

            string jsonContent = JsonConvert.SerializeObject(saleReceiptList);

            file.Write(jsonContent);

            file.Close();
        }
    }
}
