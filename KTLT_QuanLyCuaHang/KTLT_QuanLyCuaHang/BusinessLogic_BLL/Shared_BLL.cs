using KTLT_QuanLyCuaHang.Entities;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class Shared_BLL
    {
        public static bool CheckUniqueProductID(string productID, Product[] products)
        {
            if(string.IsNullOrEmpty(productID))
            {
                return false;
            }

            foreach (Product existingProduct in products)
            {
                if(existingProduct.id == productID)
                {
                    return false;
                }
            }

            return true;
        }

        public static string TransformToContinuousLowercaseString(string str)
        {
            return string.Join("", str.Trim().ToLower().Split());
        }

        public static bool isNoE(string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().ToLower() == "null";
        }
    }
}
