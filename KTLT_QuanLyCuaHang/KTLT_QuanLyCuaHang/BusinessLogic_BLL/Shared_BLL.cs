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
    }
}
