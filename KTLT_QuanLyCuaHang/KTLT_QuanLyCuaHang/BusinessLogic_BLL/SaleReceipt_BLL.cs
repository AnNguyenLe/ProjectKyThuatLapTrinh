using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.DataAccess_DAL;
using KTLT_QuanLyCuaHang.Constants;
using System.Security.Cryptography.X509Certificates;

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
                Product? product = Product_BLL.SearchProductBasedOnIDInProvidedList(exportP.id, products);

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

            string checkingResultSaleReceiptId = CheckSaleReceiptID(newSaleReceipt.id, currentSaleReceipts);

            if (checkingResultSaleReceiptId != ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL_UNIQUE_ID)
            {
                return checkingResultSaleReceiptId;
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
            SaleReceipt_DAL.SaveSaleReceiptList(updatedSaleReceipts);
            return ProcessStatusConstants.SALE_RECEIPT_CREATING_SUCCESSFUL;
        }

        public static string CreateSaleReceipt(string newSaleReceiptId, Product[] exportingGoods)
        {
            // Refine exportingGoods to contains no product has quantity equals Zero
            Product[]? refinedExportingGoods = RefineExportingProductList(exportingGoods);
            if(refinedExportingGoods == null) {
                return ProcessStatusConstants.SALE_RECEIPT_CREATING_FAIL_ALL_QUANTITY_ARE_ZEROS;
            }

            // Map to information into SaleReceipt format
            SaleReceipt newSaleReceipt = new SaleReceipt();
            newSaleReceipt.id = newSaleReceiptId;
            newSaleReceipt.createdDateTimeUTC = DateTime.UtcNow;
            newSaleReceipt.exportedGoods = refinedExportingGoods;

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

        public static Product[]? RefineExportingProductList(Product[] toBeExportedProducts)
        {
            int exportingListLength = toBeExportedProducts.Length;
            if(toBeExportedProducts == null || exportingListLength == 0) {
                return null;
            }

            int noOfZeros = 0;
            int counter = 0;
            foreach(Product p in toBeExportedProducts)
            {
                if(p.quantity > 0)
                {
                    ++counter;
                }
                else
                {
                    ++noOfZeros;
                }
            }

            if(noOfZeros == exportingListLength)
            {
                return null;
            }

            Product[] refinedProducts = new Product[counter];

            int index = 0;
            for(int i = 0; i < exportingListLength; i++)
            {
                Product p = toBeExportedProducts[i];
                if(p.quantity > 0)
                {
                    refinedProducts[index++] = p;
                }
            }

            return refinedProducts;
        }

        public static SaleReceipt[]? SearchSaleReceipt(string rawSearchText)
        {
            SaleReceipt[]? saleReceipts = GetSaleReceiptList();

            if (saleReceipts == null)
            {
                return null;
            }

            string str = string.Join("", rawSearchText.Trim().ToLower().Split());

            int counter = 0;

            bool matchedCondition(SaleReceipt s)
            {
                return s.id.ToLower().Contains(str) || s.createdDateTimeUTC.ToString().Contains(str);
            }

            foreach (SaleReceipt s in saleReceipts)
            {
                if (matchedCondition(s))
                {
                    ++counter;
                }
            }

            if (counter == 0)
            {
                return null;
            }

            SaleReceipt[] matchedSaleReceipts = new SaleReceipt[counter];

            int index = 0;
            foreach (SaleReceipt s in saleReceipts)
            {
                if (matchedCondition(s))
                {
                    matchedSaleReceipts[index] = s;
                    ++index;
                }
            }

            return matchedSaleReceipts;
        }

        public static SaleReceipt? SearchSaleReceiptBasedOnID(string saleReceiptId)
        {
            SaleReceipt[]? saleReceipts = GetSaleReceiptList();

            if(saleReceipts == null || saleReceipts.Length == 0)
            {
                return null;
            }

            foreach(SaleReceipt s in saleReceipts)
            {
                if(s.id == saleReceiptId)
                {
                    return s;
                }
            }

            return null;
        }

        public static int? FindIndexOfSaleReceipt(string saleReceiptId) {
            SaleReceipt[]? saleReceipts = GetSaleReceiptList();
            for(int i = 0; i < saleReceipts?.Length; i++)
            {
                if (saleReceipts[i].id == saleReceiptId)
                {
                    return i;
                }
            }
            return null;
        }

        public static void UpdateSaleReceipt(string saleReceiptId, Product[] exportingGoods)
        {
            SaleReceipt[]? saleReceipts = GetSaleReceiptList();
            if(saleReceipts == null)
            {
                return;
            }
            int? index = FindIndexOfSaleReceipt(saleReceiptId);
            if(index == null)
            {
                return;
            }
            saleReceipts[(int)index].exportedGoods = exportingGoods;
            saleReceipts[(int)index].lastUpdateTimeUTC = DateTime.UtcNow;
            SaleReceipt_DAL.SaveSaleReceiptList(saleReceipts);
        }

        public static void DeleteSaleReceipt(string saleReceiptId)
        {
            SaleReceipt[]? saleReceipts = GetSaleReceiptList();
            if(saleReceipts == null)
            {
                return;
            }

            SaleReceipt[] newSaleReceipts = new SaleReceipt[saleReceipts.Length - 1];
            int counter = 0;
            foreach(SaleReceipt s in saleReceipts)
            {
                if(s.id != saleReceiptId)
                {
                    newSaleReceipts[counter++] = s;
                }
            }
            SaleReceipt_DAL.SaveSaleReceiptList(newSaleReceipts);

        }
    }
}
