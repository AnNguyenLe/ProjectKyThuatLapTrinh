using KTLT_QuanLyCuaHang.Entities;
using Newtonsoft.Json;

namespace KTLT_QuanLyCuaHang.DataAccess_DAL
{
    public class Product_DAL
    {
        public static ProductList GetProductList()
        {
            StreamReader file = new StreamReader(Constants_DAL.productListFilePath);
            string jsonContent = file.ReadToEnd();
            file.Close();
            return JsonConvert.DeserializeObject<ProductList>(jsonContent);
        }

        public static void SaveProductList(Product[] updatedProducts)
        {
            ProductList newProductList = new ProductList();
            newProductList.noOfProducts = updatedProducts.Length;
            newProductList.products = updatedProducts;
            string jsonContent = JsonConvert.SerializeObject(newProductList);

            StreamWriter file = new StreamWriter(Constants_DAL.productListFilePath);
            file.Write(jsonContent);

            file.Close();
        }
    }
}
