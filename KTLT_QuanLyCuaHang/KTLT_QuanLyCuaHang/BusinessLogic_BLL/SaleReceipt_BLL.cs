using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.DataAccess_DAL;
using KTLT_QuanLyCuaHang.Constants;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class SaleReceipt_BLL
    {
        public static string CreateSaleReceipt(Product[] toBeExportedProducts)
        {
            if(toBeExportedProducts.Length== 0)
            {
                return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_EXPORTING_PRODUCT_LIST_EMPTY;
            }

            Product[]? products = Product_BLL.GetUnexpiredProductList();

            if(products == null)
            {
                return ProcessStatusConstants.PRODUCT_EMPTY_PRODUCT_DATABASE;
            }

            foreach( Product exportP in toBeExportedProducts )
            {
                Product? product = Product_BLL.SearchProductBasedOnID(p.id);

                if( product == null )
                {
                    return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_CONTAINS_NON_EXIST_PRODUCT;
                }

                if(exportP.quantity > product.Value.quantity)
                {
                    return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_INSUFFICIENT_INSTOCK;
                }
            }

            // Todo: Write a function to Subtract the quantity of exported goods - Another function to store the newly created sale receipt.

            return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL;
        }
    }
}
