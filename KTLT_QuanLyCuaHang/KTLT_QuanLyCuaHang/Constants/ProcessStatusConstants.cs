namespace KTLT_QuanLyCuaHang.Constants
{
    public class ProcessStatusConstants
    {
        public static readonly string APPROVED = "approved";
        public static readonly string REJECTED = "rejected";
        public static readonly string PRODUCT_ADDING_SUCCESS = "Successfully added new product.";
        public static readonly string PRODUCT_ADDING_FAIL_DUPLICATED_ID = "This ProductID had been registered already. Please choose another ProductID";
        
        public static readonly string PRODUCT_FAIL_INPUTS_ALL_FIELDS_REQUIRED = "Please make sure: All fields are NOT left empty or null";
        public static readonly string PRODUCT_FAIL_INPUTS_QUANTITY_NOT_ALLOW_BELOW_ZERO = "Please make sure: Quantity cannot be less than 0.";
        public static readonly string PRODUCT_FAIL_INPUTS_PRICE_NOT_ALLOW_BELOW_ZERO = "Please make sure: Price cannot be less than 0.";
        public static readonly string PRODUCT_FAIL_INPUTS_MFG_DATE_NOT_IN_FUTURE = "Please make sure: Manufacturing Date cannot be in the future.";
        public static readonly string PRODUCT_FAIL_INPUTS_EXP_DATE_NOT_IN_PAST = "Please make sure: Exp Date cannot be in the past.";

        public static readonly string PRODUCT_EMPTY_PRODUCT_DATABASE = "Product Database is empty.";
        public static readonly string PRODUCT_UPDATING_SUCCESS = "Successfully updated product.";
        public static readonly string PRODUCT_UPDATING_FAIL = "Failed updating product.";
        public static readonly string PRODUCT_DELETING_SUCCESS = "Successfully deleted product.";
        public static readonly string PRODUCT_CANNOT_FIND_PRODUCT_ID = "Cannot find product that matched your input Product ID.";

    }
}
