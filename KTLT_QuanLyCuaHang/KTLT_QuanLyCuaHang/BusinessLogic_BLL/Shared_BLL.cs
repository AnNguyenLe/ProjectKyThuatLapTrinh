using KTLT_QuanLyCuaHang.Constants;
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
            return string.IsNullOrEmpty(str) || str.Trim().ToLower() == "null" || str.Trim().ToLower() == "undefined";
        }

        public static bool CheckProductExpired(DateTime expDate) {
            return DateTime.Today.CompareTo(expDate) >= 0;
        }

        public static bool CheckProductExpiresNext7Days(DateTime expDate)
        {
            return DateTime.Today.CompareTo(expDate) <= 0 && DateTime.Today.AddDays(7).CompareTo(expDate) >= 0;
        }

        public static bool CheckProductExpiresNext14Days(DateTime expDate)
        {
            return DateTime.Today.CompareTo(expDate) <= 0 && DateTime.Today.AddDays(14).CompareTo(expDate) >= 0;
        }

        public static string[] GetUnsortedKeysFromQueryString(string rawQueryString)
        {
            string queryString = rawQueryString;

            if(rawQueryString.Contains("?"))
            {
                queryString = rawQueryString.Substring(1);
            }

            string[] keyValuePairs = queryString.Split("&");

            string[] queryKeys = new string[keyValuePairs.Length];

            for(int i = 0; i < keyValuePairs.Length; i++)
            {
                string rawKeyValuePair = keyValuePairs[i];
                queryKeys[i] = rawKeyValuePair.Split("=")[0].Trim();
            }
            return queryKeys;
        }

        public static string[] GetUnsortedValuesFromQueryString(string rawQueryString)
        {
            string queryString = rawQueryString;

            if (rawQueryString.Contains("?"))
            {
                queryString = rawQueryString.Substring(1);
            }

            string[] keyValuePairs = queryString.Split("&");

            string[] queryValues = new string[keyValuePairs.Length];

            for (int i = 0; i < keyValuePairs.Length; i++)
            {
                string rawKeyValuePair = keyValuePairs[i];
                string valueContent = rawKeyValuePair.Split("=")[1].Trim();
                if (isNoE(valueContent))
                {
                    valueContent= "0";
                }

                queryValues[i] = valueContent;
            }
            return queryValues;
        }
    }
}
