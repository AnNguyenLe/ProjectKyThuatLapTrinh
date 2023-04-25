using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.DataAccess_DAL;
using KTLT_QuanLyCuaHang.Constants;
using System.Linq;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class Product_BLL
    {
        public static Product[]? GetProductList()
        {
            ProductList productList = Product_DAL.GetProductList();

            if(productList.noOfProducts == 0)
            {
                return null;
            }

            return productList.products;
        }

        public static bool isNoE(string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().ToLower() == "null";
        }

        public static string AddProduct(Product p)
        {
            Product[]? products = GetProductList();

            if(products == null)
            {
                Product_DAL.SaveProductList(new Product[1] {p});
                return ProcessStatusConstants.PRODUCT_ADDING_SUCCESS;
            }

            bool hasUniqueProductID = Shared_BLL.CheckUniqueProductID(p.id, products);

            if(!hasUniqueProductID)
            {
                return ProcessStatusConstants.PRODUCT_ADDING_FAIL_DUPLICATED_ID;
            }

            if (isNoE(p.id) || isNoE(p.name) || isNoE(p.category) || isNoE(p.manufacturer) || isNoE(p.quantity.ToString()) || isNoE(p.price.ToString()) || isNoE(p.mfgDate.ToString()) || isNoE(p.expDate.ToString()) )
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_ALL_FIELDS_REQUIRED;
            }
            if (p.quantity < 0)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_QUANTITY_NOT_ALLOW_BELOW_ZERO;
            }
            if (p.price < 0)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_PRICE_NOT_ALLOW_BELOW_ZERO;
            }
            if (p.mfgDate > DateTime.Today)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_MFG_DATE_NOT_IN_FUTURE;
            }
            if (p.expDate < DateTime.Today)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_EXP_DATE_NOT_IN_PAST;
            }

            Product[] updatedProducts = new Product[products.Length + 1];
            for(int i = 0; i < products.Length; i++)
            {
                updatedProducts[i] = products[i];
            }

            updatedProducts[products.Length] = p;

            Product_DAL.SaveProductList(updatedProducts);

            return ProcessStatusConstants.PRODUCT_ADDING_SUCCESS;
        }

        public static Product[]? SearchProduct(string rawSearchText)
        {

            Product[]? products = GetProductList();

            if(products == null)
            {
                return null;
            }
            
            string s = string.Join("", rawSearchText.Trim().ToLower().Split());

            int counter = 0;
            foreach(Product p in products)
            {
                bool matchedCondition = p.id.ToLower().Contains(s) || p.name.ToLower().Contains(s) || p.category.ToLower().Contains(s) || p.manufacturer.ToLower().Contains(s) || p.mfgDate.ToString().Contains(s) || p.category.ToLower().Contains(s) || p.manufacturer.ToLower().Contains(s) || p.expDate.ToString().Contains(s) || p.quantity.ToString().Contains(s) || p.price.ToString().Contains(s);

                if (matchedCondition)
                {
                    ++counter;
                }
            }

            if(counter == 0)
            {
                return null;
            }

            Product[] matchedProducts = new Product[counter];

            int index = 0;
            foreach (Product p in products)
            {
                bool matchedCondition = p.id.ToLower().Contains(s) || p.name.ToLower().Contains(s) || p.category.ToLower().Contains(s) || p.manufacturer.ToLower().Contains(s) || p.mfgDate.ToString().Contains(s) || p.category.ToLower().Contains(s) || p.manufacturer.ToLower().Contains(s) || p.expDate.ToString().Contains(s) || p.quantity.ToString().Contains(s) || p.price.ToString().Contains(s);

                if (matchedCondition)
                {
                    matchedProducts[index] = p;
                    ++index;
                }
            }

            return matchedProducts;

        }

        public static Product? SearchProductBasedOnID(string productId)
        {
            if(string.IsNullOrEmpty(productId)) return null;

            Product[]? products = GetProductList();

            if(products == null) return null;

            foreach (Product p in products)
            {
                if(p.id == productId)
                {
                    return p;
                }
            }

            return null;
        }

        public static string UpdateProductBasedOnID(Product p)
        {
            Product[]? products = GetProductList();

            if(products == null)
            {
                return ProcessStatusConstants.PRODUCT_EMPTY_PRODUCT_DATABASE;
            }

            if (isNoE(p.id) || isNoE(p.name) || isNoE(p.category) || isNoE(p.manufacturer) || isNoE(p.quantity.ToString()) || isNoE(p.price.ToString()) || isNoE(p.mfgDate.ToString()) || isNoE(p.expDate.ToString()))
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_ALL_FIELDS_REQUIRED;
            }
            if (p.quantity < 0)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_QUANTITY_NOT_ALLOW_BELOW_ZERO;
            }
            if (p.price < 0)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_PRICE_NOT_ALLOW_BELOW_ZERO;
            }
            if (p.mfgDate > DateTime.Today)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_MFG_DATE_NOT_IN_FUTURE;
            }
            if (p.expDate < DateTime.Today)
            {
                return ProcessStatusConstants.PRODUCT_FAIL_INPUTS_EXP_DATE_NOT_IN_PAST;
            }

            for (int i = 0; i < products.Length; i++)
            {
                if(p.id == products[i].id)
                {
                    products[i] = p;
                    Product_DAL.SaveProductList(products);
                    return ProcessStatusConstants.PRODUCT_UPDATING_SUCCESS;
                }
            }

            return ProcessStatusConstants.PRODUCT_UPDATING_FAIL;
        }

        public static string DeleteProductBasedOnID(string productId)
        {
            Product[]? products = GetProductList();

            if (products == null)
            {
                return ProcessStatusConstants.PRODUCT_EMPTY_PRODUCT_DATABASE;
            }

            Product? matchedProduct = SearchProductBasedOnID(productId);

            if(matchedProduct == null)
            {
                return ProcessStatusConstants.PRODUCT_CANNOT_FIND_PRODUCT_ID;
            }

            Product[] updatedProducts = new Product[products.Length - 1];

            int counter = 0;
            foreach(Product p in products)
            {
                if(p.id == productId)
                {
                    continue;
                }
                updatedProducts[counter] = p;
                ++counter;
            }

            Product_DAL.SaveProductList(updatedProducts);

            return ProcessStatusConstants.PRODUCT_DELETING_SUCCESS;
        }
    }
}
