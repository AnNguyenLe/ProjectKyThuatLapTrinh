namespace KTLT_QuanLyCuaHang.Constants
{
    public class ProcessStatusConstants
    {
        public static readonly string APPROVED = "approved";
        public static readonly string REJECTED = "rejected";
        public static readonly string PRODUCT_ADDING_SUCCESS = "Successfully added new product.";
        public static readonly string PRODUCT_ADDING_FAIL_DUPLICATED_ID = "ProductID must have value or this ProductID had been registered already.";
        public static readonly string PRODUCT_ADDING_FAIL_INPUTS = "Please make sure: \n - All fields are NOT left empty or null.\n- Quantity must be greater than 0.\n- Price cannot be less than 0.\n-Manufacturing Date cannot be in the future\n - And Exp Date cannot be in the past.";
        public static readonly string PRODUCT_ADDING_EMPTY_PRODUCT_DATABASE = "Product Database is empty.";
        public static readonly string PRODUCT_UPDATING_SUCCESS = "Successfully updated product.";
        public static readonly string PRODUCT_UPDATING_FAIL = "Failed updating product.";

    }
}
