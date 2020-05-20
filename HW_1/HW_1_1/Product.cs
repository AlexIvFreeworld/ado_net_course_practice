using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Lesson_6_Cashier
{
    [Serializable]
    public class Product
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public float PricePurchasing { get; set; }

        public double CountWarehouse { get; set; }

        public double CountMarket { get; set; }

        public UoM UoM { get; set; }

        public string[] setGridView1()
        {
            return new string[]{
                Name,
                CountWarehouse.ToString() + " " + UoM,
                CountMarket.ToString() + " " + UoM,
                PricePurchasing.ToString() + " " + "руб.",
                Price.ToString() + " " + "руб.",
                ((CountWarehouse + CountMarket) * Price).ToString() + " " + "руб.",
                ((CountWarehouse + CountMarket) * Price - (CountWarehouse + CountMarket) * PricePurchasing).ToString() + " " + "руб."
        };
        }

    }

    public class ProductCashier
    {
        [DisplayName("Название продукта")]
        public string Name { get; set; }

        [DisplayName("Стоимость за 1 единицу")]
        public float Price { get; set; }

        [DisplayName("Количество товаров")]
        public double CountByPrices { get; set; }

        [DisplayName("Единицы измерения")]
        public UoM UoM { get; set; }

    }
    public class ProductSell
    {
        [DisplayName("Название продукта")]
        public string Name { get; set; }

        [DisplayName("Количество продажи")]
        public double CountSell { get; set; }

        [DisplayName("Цена продажи")]
        public float PriceSell { get; set; }

        [DisplayName("Цена закупки")]
        public float PriceBuy { get; set; }

        [DisplayName("Общая сумма продажи")]
        public double SumSell => PriceSell * CountSell;

        [DisplayName("Маржа продажи")]
        public double ProfitSell => (PriceSell - PriceBuy) * CountSell;

        [DisplayName("Единицы измерения")]
        public UoM UoM { get; set; }

        public string[] setGridView2()
        {
            return new string[]{
                Name,
                CountSell.ToString() + " " + UoM,
                PriceSell.ToString() + " " + "руб.",
                PriceBuy.ToString() + " " + "руб.",
                SumSell.ToString() + " " + "руб.",
                ProfitSell.ToString() + " " + "руб."
        };
        }
    }

    public enum UoM
    {
        Piece,
        Kg,
        Liter,
        Cans
    }

    public static class HelperProduct
    {
        private static string PathFile;
        static string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        static HelperProduct()
        {
            PathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Products.xml");
        }

        public static void SaveUsersFromFile(List<Product> products)
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<Product>));
                using (var stream = File.Create(PathFile))
                {
                    xmlReader.Serialize(stream, products);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }

        public static List<Product> LoadUsersFromFile()
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<Product>));
                using (var stream = File.OpenRead(PathFile))
                {
                    return (List<Product>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

        }
        public static List<Product> LoadFromSql()
        {
            List<Product> listProducts = new List<Product>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from Products"; 
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Product pr = new Product();
                        pr.Name = rdr[1].ToString();
                        pr.Price = float.Parse(rdr[2].ToString());
                        pr.PricePurchasing = float.Parse(rdr[3].ToString());
                        pr.CountWarehouse = double.Parse(rdr[4].ToString());
                        pr.CountMarket = double.Parse(rdr[5].ToString());
                        pr.UoM = (UoM)Enum.Parse(typeof(UoM), rdr[6].ToString());
                        listProducts.Add(pr);
                    }
                }
            }
            return listProducts;
        }
        public static void InsertToSql(Product product)
        {
            using(var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_insert_to_products", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.Name;
                comm.Parameters.Add("@price", SqlDbType.Money).Value = product.Price;
                comm.Parameters.Add("@pricepurchasing", SqlDbType.Money).Value = product.PricePurchasing;
                comm.Parameters.Add("@countwarehous", SqlDbType.Decimal).Value = product.CountWarehouse;
                comm.Parameters.Add("@countmarket", SqlDbType.Decimal).Value = product.CountMarket;
                comm.Parameters.Add("@uom", SqlDbType.NVarChar).Value = product.UoM;
                comm.ExecuteNonQuery();
            }
        }
        public static void DeleteFromSql(string nameForDel)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_delete_product", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = nameForDel;
                comm.ExecuteNonQuery();
            }
        }
        public static void UpdateCountToSql(String name, double newcount)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_update_count_products", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                comm.Parameters.Add("@newcount", SqlDbType.Decimal).Value = newcount;
                comm.ExecuteNonQuery();
            }
        }
    }
    public static class HelperProductSell
    {
        private static string PathFile;
        static string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;


        static HelperProductSell()
        {
            PathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ProductsSell.xml");
        }

        public static void SaveFromFile(List<ProductSell> products)
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductSell>));
                using (var stream = File.Create(PathFile))
                {
                    xmlReader.Serialize(stream, products);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка проданных товаров", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }

        public static List<ProductSell> LoadFromFile()
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductSell>));
                using (var stream = File.OpenRead(PathFile))
                {
                    return (List<ProductSell>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка проданных товаров", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

        }
        public static List<ProductSell> LoadFromSql()
        {
            List<ProductSell> listProductSell = new List<ProductSell>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from Selling";
                SqlCommand com = new SqlCommand(sql, conn);
                using(SqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ProductSell prSell = new ProductSell();
                        prSell.Name = rdr[1].ToString();
                        prSell.CountSell = Int32.Parse(rdr[2].ToString());
                        prSell.PriceSell = float.Parse(rdr[3].ToString());
                        prSell.PriceBuy = float.Parse(rdr[4].ToString());
                        prSell.UoM = (UoM)Enum.Parse(typeof(UoM), rdr[5].ToString());
                        listProductSell.Add(prSell);
                    }
                }
            }
            return listProductSell;
        }
        public static void UpdateCountToSql(String name, double newcount)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_update_count_selling", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                comm.Parameters.Add("@newcount", SqlDbType.Int).Value = newcount;
                comm.ExecuteNonQuery();
            }
        }
        public static void InsertToSql(Product product)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_insert_to_selling", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.Name;
                comm.Parameters.Add("@CountSell", SqlDbType.Decimal).Value = 0;
                comm.Parameters.Add("@PriceSell", SqlDbType.Money).Value = product.Price;
                comm.Parameters.Add("@PriceBuy", SqlDbType.Money).Value = product.PricePurchasing;
                comm.Parameters.Add("@uom", SqlDbType.NVarChar).Value = product.UoM;
                comm.ExecuteNonQuery();
            }
        }
    }
}
