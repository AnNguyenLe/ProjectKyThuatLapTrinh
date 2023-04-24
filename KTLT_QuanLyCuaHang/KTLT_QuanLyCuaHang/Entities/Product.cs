using System;
namespace KTLT_QuanLyCuaHang.Entities
{
    public struct Product
    {
        public string id;
        public string name;
        public string manufacturer;
        public DateTime mfgDate;
        public DateTime expDate;
        public string category;
        public float price;
        public int quantity;
    }

    public struct ProductList
    {
        public int noOfProducts;
        public Product[] products;
    }
}

