﻿namespace KTLT_QuanLyCuaHang.Constants
{
    public class ProcessStatusConstants
    {
        public static readonly string APPROVED = "approved";
        public static readonly string REJECTED = "rejected";
        public static readonly string PRODUCT_ADDING_SUCCESS = "Successfully added new product.";
        public static readonly string PRODUCT_ADDING_FAIL_DUPLICATED_ID = "This ProductID had been registered already. Please choose another ProductID.";
        public static readonly string PRODUCT_ADDING_FAIL_NULL_OR_EMPTY_ID = "ProductID cannot be Null or Empty. Please enter a value for ProductID.";

        public static readonly string PRODUCT_FAIL_INPUTS_ALL_FIELDS_REQUIRED = "Please make sure: All fields are NOT left empty or null";
        public static readonly string PRODUCT_FAIL_INPUTS_QUANTITY_NOT_ALLOW_BELOW_ZERO = "Please make sure: Quantity cannot be less than 0.";
        public static readonly string PRODUCT_FAIL_INPUTS_PRICE_NOT_ALLOW_BELOW_ZERO = "Please make sure: Price cannot be less than 0.";
        public static readonly string PRODUCT_FAIL_INPUTS_MFG_DATE_NOT_IN_FUTURE = $"Please make sure: Manufacturing Date cannot be in the future. Today: {DateTime.Today.ToShortDateString()}";
        public static readonly string PRODUCT_FAIL_INPUTS_EXP_DATE_NOT_IN_PAST = $"Please make sure: Exp Date cannot be in the past. Today: {DateTime.Today.ToShortDateString()}";

        public static readonly string PRODUCT_EMPTY_PRODUCT_DATABASE = "Product Database is empty.";
        public static readonly string PRODUCT_UPDATING_SUCCESS = "Successfully updated product.";
        public static readonly string PRODUCT_UPDATING_FAIL = "Failed updating product.";
        public static readonly string PRODUCT_DELETING_SUCCESS = "Successfully deleted product.";
        public static readonly string PRODUCT_CANNOT_FIND_PRODUCT_ID = "Cannot find product that matched your input Product ID.";

        public static readonly string PRODUCT_CATEGORY_ADDING_FAIL_DUPLICATED_ID = "This Product Category had been registered already. Please enter new Product Category.";
        public static readonly string PRODUCT_CATEGORY_UPDATING_FAIL_NO_PRODUCT = "Failed updating product(s) due to list of to be updated product(s) is empty.";
        public static readonly string PRODUCT_CATEGORY_UPDATING_SUCCESS = "Successfully updated products.";
        public static readonly string PRODUCT_CATEGORY_UPDATING_FAIL_CATEGORY_NULL_OR_EMPTY = "Product Category cannot be null or empty";

        public static readonly string PRODUCT_CATEGORY_DELETING_FAIL_NO_PRODUCT = "Failed deleting product(s) due to list of to be deleted product(s) is empty.";
        public static readonly string PRODUCT_CATEGORY_DELETING_SUCCESS = "Successfully updated products.";

        public static readonly string SALE_RECEIPT_CREATING_FAIL_EXPORTING_PRODUCT_LIST_EMPTY = "Cannot create sale receipt with empty product list.";
        public static readonly string SALE_RECEIPT_CREATING_FAIL_CONTAINS_NON_EXIST_PRODUCT = "Cannot create sale receipt due to a non-exist product.";
        public static readonly string SALE_RECEIPT_CREATING_FAIL_INSUFFICIENT_INSTOCK = "Cannot export more than instock quantity. Please check the max quantity and the exporting quantity of each product again.";
        public static readonly string SALE_RECEIPT_CREATING_FAIL_ID_EMPTY_OR_NULL = "Sale Receipt ID cannot be empty or null.";
        public static readonly string SALE_RECEIPT_CREATING_FAIL_ID_NOT_UNIQUE = "Please choose another Sale Receipt ID. This Sale Receipt ID has been registered.";
        public static readonly string SALE_RECEIPT_CREATING_SUCCESSFUL_UNIQUE_ID = "This Sale Receipt ID is unique.";
        public static readonly string SALE_RECEIPT_CREATING_SUCCESSFUL_EXPORTING_PRODUCTS_QUALIFIED = "Exporting Products are qualified.";
        public static readonly string SALE_RECEIPT_CREATING_QUALIFIED_SALE_RECEIPT = "Sale Receipt is qualified.";
        public static readonly string SALE_RECEIPT_CREATING_SUCCESSFUL = "Successfull created a sale receipt";
        public static readonly string SALE_RECEIPT_CREATING_FAIL_ALL_QUANTITY_ARE_ZEROS = "Cannot issue a Sale Receipt that quantity is 0 for all listed products.";
        public static readonly string SALE_RECEIPT_CREATING_FAIL_UPDATE_INVENTORY_FAIL = "Error occurs when trying update the inventory. Please check the max quantity and the exporting quantity of each product again.";

        public static readonly string SALE_RECEIPT_UPDATING_FAIL_CONTAINS_NON_EXIST_PRODUCT = "Cannot update sale receipt due to a non-exist product";
        public static readonly string SALE_RECEIPT_UPDATING_EMPTY_LIST = "Cannot update sale receipt due to sale receipt list is empty.";
        public static readonly string SALE_RECEIPT_UPDATING_SUCCESSFUL_UPDATING_INVENTORY = "Successfully update inventory before updating sale receipt";
        public static readonly string SALE_RECEIPT_UPDATING_SUCCESSFUL = "Successfully update sale receipt";


        public static readonly string PURCHASE_INVOICE_CREATING_FAIL_IMPORTING_PRODUCT_LIST_EMPTY = "Cannot create purchase invoice with empty product list.";
        public static readonly string PURCHASE_INVOICE_CREATING_FAIL_CONTAINS_NON_EXIST_PRODUCT = "Cannot create purchase invoice due to a non-exist product.";
        public static readonly string PURCHASE_INVOICE_CREATING_FAIL_PRODUCTS_QUANTITY_MUST_POSITIVE = "Cannot import NEGATIVE quantity. Please check the importing quantity of each product again.";
        public static readonly string PURCHASE_INVOICE_CREATING_FAIL_ID_EMPTY_OR_NULL = "Purchase Invoice ID cannot be empty or null.";
        public static readonly string PURCHASE_INVOICE_CREATING_FAIL_ID_NOT_UNIQUE = "Please choose another Purchase Invoice ID. This Purchase Invoice ID has been registered.";
        public static readonly string PURCHASE_INVOICE_CREATING_SUCCESSFUL_UNIQUE_ID = "This Purchase Invoice ID is unique.";
        public static readonly string PURCHASE_INVOICE_CREATING_SUCCESSFUL_IMPORTING_PRODUCTS_QUALIFIED = "Importing Products are qualified.";
        public static readonly string PURCHASE_INVOICE_CREATING_QUALIFIED_PURCHASE_INVOICE = "Purchase Invoice is qualified.";
        public static readonly string PURCHASE_INVOICE_CREATING_SUCCESSFUL = "Successfull created a purchase invoice";
        public static readonly string PURCHASE_INVOICE_CREATING_FAIL_ALL_QUANTITY_ARE_ZEROS = "Cannot issue a Purchase Invoice that quantity is 0 for all listed products.";
        public static readonly string PURCHASE_INVOICE_CREATING_FAIL_UPDATE_INVENTORY_FAIL = "Error occurs when trying update the inventory. Please check the importing quantity of each product again.";

        public static readonly string PURCHASE_INVOICE_UPDATING_FAIL_CONTAINS_NON_EXIST_PRODUCT = "Cannot update purchase invoice due to a non-exist product";
        public static readonly string PURCHASE_INVOICE_UPDATING_EMPTY_LIST = "Cannot update purchase invoice due to purchase invoice list is empty.";
        public static readonly string PURCHASE_INVOICE_UPDATING_SUCCESSFUL_UPDATING_INVENTORY = "Successfully update inventory before updating purchase invoice";
        public static readonly string PURCHASE_INVOICE_UPDATING_SUCCESSFUL = "Successfully update purchase invoice";
    }
}
