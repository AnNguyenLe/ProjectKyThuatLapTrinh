using KTLT_QuanLyCuaHang.Constants;
using KTLT_QuanLyCuaHang.DataAccess_DAL;
using KTLT_QuanLyCuaHang.Entities;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class PurchaseInvoice_BLL
    {
        public static PurchaseInvoice[]? GetPurchaseInvoiceList()
        {
            PurchaseInvoiceList purchaseInvoiceList = PurchaseInvoice_DAL.GetPurchaseInvoiceList();

            if (purchaseInvoiceList.total == 0 || purchaseInvoiceList.purchaseInvoices.Length == 0)
            {
                return null;
            }

            return purchaseInvoiceList.purchaseInvoices;
        }

        public static string CheckPurchaseInvoiceID(string newPurchaseInvoiceId, PurchaseInvoice[] purchaseInvoices)
        {
            if (Shared_BLL.isNoE(newPurchaseInvoiceId))
            {
                return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_FAIL_ID_EMPTY_OR_NULL;
            }

            if (purchaseInvoices == null || purchaseInvoices.Length == 0)
            {
                return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL_UNIQUE_ID;
            }

            foreach (PurchaseInvoice s in purchaseInvoices)
            {
                if (s.id == newPurchaseInvoiceId)
                {
                    return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_FAIL_ID_NOT_UNIQUE;
                }
            }

            return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL_UNIQUE_ID;
        }

        public static string CheckPurchaseInvoiceImportingGoods(Product[] toBeImportedProducts)
        {
            foreach (Product importP in toBeImportedProducts)
            {

                if (importP.quantity <= 0)
                {
                    return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_FAIL_PRODUCTS_QUANTITY_MUST_POSITIVE;
                }
            }

            return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL_IMPORTING_PRODUCTS_QUALIFIED;
        }

        public static string CheckEligibilityOfPurchaseInvoice(PurchaseInvoice newPurchaseInvoice, PurchaseInvoice[] purchaseInvoices)
        {
            // Checking the ID of the newly created purchase invoice
            string checkingResultPurchaseInvoiceId = CheckPurchaseInvoiceID(newPurchaseInvoice.id, purchaseInvoices);

            if (checkingResultPurchaseInvoiceId != ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL_UNIQUE_ID)
            {
                return checkingResultPurchaseInvoiceId;
            }

            // Checking the to-be-imported-products of the newly created purchase invoice

            Product[] toBeImportedProducts = newPurchaseInvoice.importedGoods;

            string checkingResultPurchaseInvoiceImportingGoods = CheckPurchaseInvoiceImportingGoods(toBeImportedProducts);

            if (checkingResultPurchaseInvoiceImportingGoods != ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL_IMPORTING_PRODUCTS_QUALIFIED)
            {
                return checkingResultPurchaseInvoiceImportingGoods;
            }

            return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_QUALIFIED_PURCHASE_INVOICE;

        }
        public static string SaveNewlyCreatedPurchaseInvoice(PurchaseInvoice newPurchaseInvoice)
        {
            PurchaseInvoice[]? currentPurchaseInvoices = GetPurchaseInvoiceList();

            string checkingResultPurchaseInvoiceId = CheckPurchaseInvoiceID(newPurchaseInvoice.id, currentPurchaseInvoices);

            if (checkingResultPurchaseInvoiceId != ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL_UNIQUE_ID)
            {
                return checkingResultPurchaseInvoiceId;
            }

            if (currentPurchaseInvoices == null || currentPurchaseInvoices.Length == 0)
            {
                PurchaseInvoice_DAL.SavePurchaseInvoiceList(new PurchaseInvoice[1] { newPurchaseInvoice });
                return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL;
            }

            string checkingResultPurchaseInvoice = CheckEligibilityOfPurchaseInvoice(newPurchaseInvoice, currentPurchaseInvoices);

            if (checkingResultPurchaseInvoice != ProcessStatusConstants.PURCHASE_INVOICE_CREATING_QUALIFIED_PURCHASE_INVOICE)
            {
                return checkingResultPurchaseInvoice;
            }

            PurchaseInvoice[] updatedPurchaseInvoices = new PurchaseInvoice[currentPurchaseInvoices.Length + 1];
            for (int i = 0; i < currentPurchaseInvoices.Length; i++)
            {
                updatedPurchaseInvoices[i] = currentPurchaseInvoices[i];
            }
            updatedPurchaseInvoices[currentPurchaseInvoices.Length] = newPurchaseInvoice;
            PurchaseInvoice_DAL.SavePurchaseInvoiceList(updatedPurchaseInvoices);
            return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL;
        }

        public static string CreatePurchaseInvoice(string newPurchaseInvoiceId, Product[] importingGoods)
        {
            // Refine importingGoods to contains no product has quantity equals Zero
            Product[]? refinedImportingGoods = RefineImportingProductList(importingGoods);
            if (refinedImportingGoods == null)
            {
                return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_FAIL_ALL_QUANTITY_ARE_ZEROS;
            }

            // Map to information into PurchaseInvoice format
            PurchaseInvoice newPurchaseInvoice = new PurchaseInvoice();
            newPurchaseInvoice.id = newPurchaseInvoiceId;
            newPurchaseInvoice.createdDateTimeUTC = DateTime.UtcNow;
            newPurchaseInvoice.importedGoods = refinedImportingGoods;

            // Save the newly created purchase invoice.
            string creatingProcessStatus = SaveNewlyCreatedPurchaseInvoice(newPurchaseInvoice);

            if (creatingProcessStatus != ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL)
            {
                return creatingProcessStatus;
            }

            // Recalculate the inventory.
            Product[]? updatedInstockProducts = CalculateInventoryAfterImporting(newPurchaseInvoice.importedGoods);

            if (updatedInstockProducts == null)
            {
                return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_FAIL_UPDATE_INVENTORY_FAIL;
            }

            Product_DAL.SaveProductList(updatedInstockProducts);

            return ProcessStatusConstants.PURCHASE_INVOICE_CREATING_SUCCESSFUL;
        }

        public static Product[]? CalculateInventoryAfterImporting(Product[] toBeImportedProducts)
        {

            Product[]? products = Product_BLL.GetProductList();

            if (products == null)
            {
                return null;
            }


            int counter = 0;
            foreach (Product p in toBeImportedProducts)
            {
                int? positionIndex = Product_BLL.FindIndexOfProductID(p.id, products);

                if (positionIndex == null || p.quantity < 0)
                {
                    return null;
                }

                products[(int)positionIndex].quantity += p.quantity;
                ++counter;

                if (counter == toBeImportedProducts.Length)
                {
                    break;
                }
            }

            if (counter < toBeImportedProducts.Length)
            {
                return null;
            }

            return products;
        }

        public static Product[]? RefineImportingProductList(Product[] toBeImportedProducts)
        {
            int exportingListLength = toBeImportedProducts.Length;
            if (toBeImportedProducts == null || exportingListLength == 0)
            {
                return null;
            }

            int noOfZeros = 0;
            int counter = 0;
            foreach (Product p in toBeImportedProducts)
            {
                if (p.quantity > 0)
                {
                    ++counter;
                }
                else
                {
                    ++noOfZeros;
                }
            }

            if (noOfZeros == exportingListLength)
            {
                return null;
            }

            Product[] refinedProducts = new Product[counter];

            int index = 0;
            for (int i = 0; i < exportingListLength; i++)
            {
                Product p = toBeImportedProducts[i];
                if (p.quantity > 0)
                {
                    refinedProducts[index++] = p;
                }
            }

            return refinedProducts;
        }

        public static PurchaseInvoice[]? SearchPurchaseInvoice(string rawSearchText)
        {
            PurchaseInvoice[]? purchaseInvoices = GetPurchaseInvoiceList();

            if (purchaseInvoices == null)
            {
                return null;
            }

            string str = string.Join("", rawSearchText.Trim().ToLower().Split());

            int counter = 0;

            bool matchedCondition(PurchaseInvoice s)
            {
                return s.id.ToLower().Contains(str) || s.createdDateTimeUTC.ToString().Contains(str);
            }

            foreach (PurchaseInvoice s in purchaseInvoices)
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

            PurchaseInvoice[] matchedPurchaseInvoices = new PurchaseInvoice[counter];

            int index = 0;
            foreach (PurchaseInvoice s in purchaseInvoices)
            {
                if (matchedCondition(s))
                {
                    matchedPurchaseInvoices[index] = s;
                    ++index;
                }
            }

            return matchedPurchaseInvoices;
        }

        public static PurchaseInvoice? SearchPurchaseInvoiceBasedOnID(string purchaseInvoiceId)
        {
            PurchaseInvoice[]? purchaseInvoices = GetPurchaseInvoiceList();

            if (purchaseInvoices == null || purchaseInvoices.Length == 0)
            {
                return null;
            }

            foreach (PurchaseInvoice s in purchaseInvoices)
            {
                if (s.id == purchaseInvoiceId)
                {
                    return s;
                }
            }

            return null;
        }

        public static int? FindIndexOfPurchaseInvoice(string purchaseInvoiceId)
        {
            PurchaseInvoice[]? purchaseInvoices = GetPurchaseInvoiceList();
            for (int i = 0; i < purchaseInvoices?.Length; i++)
            {
                if (purchaseInvoices[i].id == purchaseInvoiceId)
                {
                    return i;
                }
            }
            return null;
        }

        public static void UpdatePurchaseInvoice(string purchaseInvoiceId, Product[] importingGoods)
        {
            PurchaseInvoice[]? purchaseInvoices = GetPurchaseInvoiceList();
            if (purchaseInvoices == null)
            {
                return;
            }
            int? index = FindIndexOfPurchaseInvoice(purchaseInvoiceId);
            if (index == null)
            {
                return;
            }
            purchaseInvoices[(int)index].importedGoods = importingGoods;
            purchaseInvoices[(int)index].lastUpdateTimeUTC = DateTime.UtcNow;
            PurchaseInvoice_DAL.SavePurchaseInvoiceList(purchaseInvoices);
        }

        public static void DeletePurchaseInvoice(string purchaseInvoiceId)
        {
            PurchaseInvoice[]? purchaseInvoices = GetPurchaseInvoiceList();
            if (purchaseInvoices == null)
            {
                return;
            }

            PurchaseInvoice[] newPurchaseInvoices = new PurchaseInvoice[purchaseInvoices.Length - 1];
            int counter = 0;
            foreach (PurchaseInvoice s in purchaseInvoices)
            {
                if (s.id != purchaseInvoiceId)
                {
                    newPurchaseInvoices[counter++] = s;
                }
            }
            PurchaseInvoice_DAL.SavePurchaseInvoiceList(newPurchaseInvoices);

        }
    }
}

