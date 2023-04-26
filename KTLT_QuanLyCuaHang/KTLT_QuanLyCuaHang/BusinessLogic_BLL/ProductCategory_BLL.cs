using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.DataAccess_DAL;
using KTLT_QuanLyCuaHang.Constants;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class ProductCategory_BLL
    {
        public static string[]? GetCategoryList()
        {
            Product[]? products = Product_BLL.GetProductList();

            if (products == null)
            {
                return null;
            }

            int counter = 0;
            string[] tempCategoryList = new string[products.Length];
            foreach (Product p in products)
            {
                if (!tempCategoryList.Contains(p.category))
                {
                    tempCategoryList[counter++] = p.category;
                }
            }

            string[] categoryList = new string[counter];

            for (int i = 0; i < counter; i++)
            {
                categoryList[i] = tempCategoryList[i];
            }

            return categoryList;
        }

        public static string[]? SearchCategory(string rawCateText)
        {
            if (Shared_BLL.isNoE(rawCateText))
            {
                return null;
            }

            string[]? categoryList = GetCategoryList();

            if (categoryList == null)
            {
                return null;
            }

            string searchContent = Shared_BLL.TransformToContinuousLowercaseString(rawCateText);

            int counter = 0;
            foreach (string category in categoryList)
            {
                if (Shared_BLL.TransformToContinuousLowercaseString(category).Contains(searchContent))
                {
                    ++counter;
                }
            }

            if (counter == 0)
            {
                return null;
            }

            string[] matchedCategories = new string[counter];

            int index = 0;
            foreach (string category in categoryList)
            {
                if (Shared_BLL.TransformToContinuousLowercaseString(category).Contains(searchContent))
                {
                    matchedCategories[index++] = category;
                }
            }

            return matchedCategories;
        }

        public static bool CheckExactCategoryExist(string category)
        {
            string[]? categories = GetCategoryList();

            if (categories == null)
            {
                return false;
            }

            string trimmedCategory = category.Trim();

            return categories.Contains(trimmedCategory);
             
        }

        public static string AddNewProductCategory(Product newProduct)
        {
            if (CheckExactCategoryExist(newProduct.category))
            {
                return ProcessStatusConstants.PRODUCT_CATEGORY_ADDING_FAIL_DUPLICATED_ID;
            }

            return Product_BLL.AddProduct(newProduct);
        }

        public static Product[]? GetProductsUnderCategory(string category)
        {
            Product[]? products = Product_BLL.GetProductList();

            if (products == null)
            {
                return null;
            }

            int counter = 0;
            foreach (Product p in products)
            {
                if(p.category == category)
                {
                    counter++;
                }
            }

            if(counter == 0)
            {
                return null;
            }

            Product[] matchedProducts= new Product[counter];

            int index = 0;
            foreach (Product p in products)
            {
                if (p.category == category)
                {
                    matchedProducts[index++] = p;
                }
            }

            return matchedProducts;
        }

        public static string UpdateProductsUnderCategory(string rawCategory, Product[] toBeUpdatedProducts)
        {
            string category = rawCategory.Trim();

            if (Shared_BLL.isNoE(category))
            {
                return ProcessStatusConstants.PRODUCT_CATEGORY_UPDATING_FAIL_CATEGORY_NULL_OR_EMPTY;
            }

            if(toBeUpdatedProducts.Length == 0)
            {
                return ProcessStatusConstants.PRODUCT_CATEGORY_UPDATING_FAIL_NO_PRODUCT;
            }

            Product[]? products = Product_BLL.GetProductList();

            if (products == null)
            {
                return ProcessStatusConstants.PRODUCT_EMPTY_PRODUCT_DATABASE;
            }
            int noOfToBeUpdatedProducts = toBeUpdatedProducts.Length;

            string[] toBeUpdatedProdIds = new string[noOfToBeUpdatedProducts];
            for(int i = 0; i < noOfToBeUpdatedProducts; i++)
            {
                toBeUpdatedProdIds[i] = toBeUpdatedProducts[i].id;
            }

            int counter = 0;
            for( int i = 0; i < products.Length; i++)
            {
                if (toBeUpdatedProdIds.Contains(products[i].id))
                {
                    products[i].category = category;
                    ++counter;
                }

                if(counter == noOfToBeUpdatedProducts)
                {
                    break;
                }
            }

            Product_DAL.SaveProductList(products);

            return ProcessStatusConstants.PRODUCT_CATEGORY_UPDATING_SUCCESS;
        }

        public static string DeleteProductsUnderCategory(Product[] toBeDeletedProducts)
        {
            if(toBeDeletedProducts == null || toBeDeletedProducts.Length == 0)
            {
                return ProcessStatusConstants.PRODUCT_CATEGORY_DELETING_FAIL_NO_PRODUCT;
            }

            Product[]? products = Product_BLL.GetProductList();

            if (products == null)
            {
                return ProcessStatusConstants.PRODUCT_EMPTY_PRODUCT_DATABASE;
            }

            int noOfToBeDeletedProducts = toBeDeletedProducts.Length;

            string[] toBeDeletedProdIds = new string[noOfToBeDeletedProducts];
            for(int i = 0; i < noOfToBeDeletedProducts; i++)
            {
                toBeDeletedProdIds[i] = toBeDeletedProducts[i].id;
            }

            Product[] updatedProductList = new Product[products.Length - noOfToBeDeletedProducts];
            int counter = 0;
            foreach( Product p in products)
            {
                if (!toBeDeletedProdIds.Contains(p.id))
                {
                    updatedProductList[counter] = p;
                    ++counter;
                }
            }

            Product_DAL.SaveProductList(updatedProductList);

            return ProcessStatusConstants.PRODUCT_CATEGORY_UPDATING_SUCCESS;
        }
    }
}
