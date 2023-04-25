using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.DataAccess_DAL;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class ProductCategory_BLL
    {
        public static string[]? GetCategoryList()
        {
            Product[]? products = Product_BLL.GetProductList();

            if(products == null)
            {
                return null;
            }

            int counter = 0;
            string[] tempCategoryList = new string[products.Length];
            foreach( Product p in products )
            {
                if (!tempCategoryList.Contains(p.category))
                {
                    tempCategoryList[counter++] = p.category;
                }
            }

            string[] categoryList = new string[counter];

            for(int i = 0; i < counter; i++)
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
            foreach(string category in categoryList )
            {
                if (Shared_BLL.TransformToContinuousLowercaseString(category).Contains(searchContent))
                {
                    ++counter;
                }
            }

            if(counter == 0) {
                return null;
            }

            string[] matchedCategories = new string[counter];

            int index = 0;
            foreach(string category in categoryList )
            {
                if (Shared_BLL.TransformToContinuousLowercaseString(category).Contains(searchContent))
                {
                    matchedCategories[index++] = category;
                }
            }

            return matchedCategories;
        }
    }
}
