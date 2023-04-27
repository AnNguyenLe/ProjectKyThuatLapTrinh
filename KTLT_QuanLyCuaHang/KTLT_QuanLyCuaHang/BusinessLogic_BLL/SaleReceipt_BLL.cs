using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.DataAccess_DAL;
using KTLT_QuanLyCuaHang.Constants;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class SaleReceipt_BLL
    {
        public static SaleReceipt[]? GetSaleReceiptList()
        {
            SaleReceiptList saleReceiptList = SaleReceipt_DAL.GetSaleReceiptList();

            if(saleReceiptList.total == 0 || saleReceiptList.saleReceipts.Length == 0)
            {
                return null;
            }

            return saleReceiptList.saleReceipts;
        }

        public static string CheckSaleReceiptID(string newSaleReceiptId, SaleReceipt[] saleReceipts)
        {
            if (Shared_BLL.isNoE(newSaleReceiptId))
            {
                return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_ID_EMPTY_OR_NULL;
            }

            if(saleReceipts == null || saleReceipts.Length == 0)
            {
                return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL_UNIQUE_ID;
            }

            foreach(SaleReceipt s in saleReceipts)
            {
                if(s.id == newSaleReceiptId)
                {
                    return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_ID_NOT_UNIQUE;
                }
            }

            return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL_UNIQUE_ID;
        }

        public static string CheckSaleReceiptExportingGoods(Product[] toBeExportedProducts)
        {
            if (toBeExportedProducts == null || toBeExportedProducts.Length == 0)
            {
                return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_EXPORTING_PRODUCT_LIST_EMPTY;
            }

            Product[]? products = Product_BLL.GetUnexpiredProductList();

            if (products == null)
            {
                return ProcessStatusConstants.PRODUCT_EMPTY_PRODUCT_DATABASE;
            }

            foreach (Product exportP in toBeExportedProducts)
            {
                Product? product = Product_BLL.SearchProductBasedOnIDInProviedList(exportP.id, products);

                if (product == null)
                {
                    return $"{ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_CONTAINS_NON_EXIST_PRODUCT}. ProductID {exportP.id} - {exportP.name}";
                }

                if (exportP.quantity > product.Value.quantity)
                {
                    return $"{ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_INSUFFICIENT_INSTOCK}. Max quantity of ProductID {exportP.id} - {exportP.name}: {product.Value.quantity}";
                }
            }

            return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL_EXPORTING_PRODUCTS_QUALIFIED;
        }

        public static string CheckEligibilityOfSaleReceipt(SaleReceipt newSaleReceipt, SaleReceipt[] saleReceipts)
        {
            // Checking the ID of the newly created sale receipt
            string checkingResultSaleReceiptId = CheckSaleReceiptID(newSaleReceipt.id, saleReceipts);

            if (checkingResultSaleReceiptId != ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL_UNIQUE_ID)
            {
                return checkingResultSaleReceiptId;
            }

            // Checking the to-be-exported-products of the newly created sale receipt

            Product[] toBeExportedProducts = newSaleReceipt.exportedGoods;

            string checkingResultSaleReceiptExportingGoods = CheckSaleReceiptExportingGoods(toBeExportedProducts);

            if(checkingResultSaleReceiptExportingGoods != ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL_EXPORTING_PRODUCTS_QUALIFIED)
            {
                return checkingResultSaleReceiptExportingGoods;
            }

            return ProcessStatusConstants.SALE_RECEIPT_CREATING_QUALIFIED_SALE_RECEIPT;

        }
        public static string SaveNewlyCreatedSaleReceipt(SaleReceipt newSaleReceipt)
        {
            SaleReceipt[]? currentSaleReceipts = GetSaleReceiptList();

            if(currentSaleReceipts != null && currentSaleReceipts.Length != 0)
            {
                bool isUniqueId = CheckSaleReceiptID(newSaleReceipt.id, currentSaleReceipts) == ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL_UNIQUE_ID;
                if (!isUniqueId)
                {
                    return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_ID_NOT_UNIQUE;
                }
            }

            if (currentSaleReceipts == null || currentSaleReceipts.Length == 0)
            {
                SaleReceipt_DAL.SaveSaleReceiptList(new SaleReceipt[1] { newSaleReceipt });
                return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL;
            }

            string checkingResultSaleReceipt = CheckEligibilityOfSaleReceipt(newSaleReceipt, currentSaleReceipts);

            if(checkingResultSaleReceipt != ProcessStatusConstants.SALE_RECEIPT_CREATING_QUALIFIED_SALE_RECEIPT)
            {
                return checkingResultSaleReceipt;
            }

            SaleReceipt[] updatedSaleReceipts = new SaleReceipt[currentSaleReceipts.Length + 1];
            for(int i = 0; i < currentSaleReceipts.Length; i++)
            {
                updatedSaleReceipts[i] = currentSaleReceipts[i];
            }
            updatedSaleReceipts[currentSaleReceipts.Length] = newSaleReceipt;
            return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL;
        }

        public static string CreateSaleReceipt(SaleReceipt newSaleReceipt)
        {
            // Save the newly created sale receipt.
            string creatingProcessStatus = SaveNewlyCreatedSaleReceipt(newSaleReceipt);

            if(creatingProcessStatus != ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL)
            {
                return creatingProcessStatus;
            }

            // Recalculate the inventory.
            Product[]? updatedInstockProducts = CalculateInventoryAfterExporting(newSaleReceipt.exportedGoods);

            if(updatedInstockProducts == null)
            {
                return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_UPDATE_INVENTORY_FAIL;
            }

            Product_DAL.SaveProductList(updatedInstockProducts);

            return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL;
        }

        public static Product[]? CalculateInventoryAfterExporting(Product[] toBeExportedProducts)
        {

            Product[]? products = Product_BLL.GetProductList();

            if (products == null)
            {
                return null;
            }


            int counter = 0;
            foreach(Product p in toBeExportedProducts)
            {
                int? positionIndex = Product_BLL.FindIndexOfProductID(p.id, products);

                if(positionIndex == null || p.quantity > products[(int)positionIndex].quantity)
                {
                    return null;
                }

                products[(int)positionIndex].quantity -= p.quantity;
                ++counter;

                if(counter == toBeExportedProducts.Length)
                {
                    break;
                }
            }

            if(counter < toBeExportedProducts.Length)
            {
                return null;
            }

            return products;
        }

    }
}
