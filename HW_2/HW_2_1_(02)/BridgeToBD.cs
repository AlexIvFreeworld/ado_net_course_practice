using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Lesson_6_Cashier
{
   public static class BridgeToBD
    {
        private static string PathFileProducts;
        private static string PathFileProductsMoving;
        private static string PathFileUserRoles;
        private static string PathFileUsers;
        static string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

     static BridgeToBD()
        {
            PathFileProducts = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Products.xml");
            PathFileProductsMoving = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ProductsMoving.xml");
            PathFileUserRoles = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "UserRoles.xml");
            PathFileUsers = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Users.xml");
        }
        public static List<ProductShop> LoadProductShopFromDB(TypeBD typeBD)
        {
            List<ProductShop> TempProducts = new List<ProductShop>();
            switch (typeBD)
            {
                case TypeBD.XML:
                    TempProducts = LoadProductShopFromXML();
                    break;
                case TypeBD.SQL:
                    TempProducts = LoadProductShopFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
            return TempProducts;
        }
        public static List<UserShop> LoadUserShopFromDB(TypeBD typeBD)
        {
            List<UserShop> TempUsers = new List<UserShop>();
            switch (typeBD)
            {
                case TypeBD.XML:
                    TempUsers = LoadUserShopFromXML();
                    break;
                case TypeBD.SQL:
                    TempUsers = LoadUserShopFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
            return TempUsers;
        }
        public static List<ProductMoving> LoadProductMovingFromDB(TypeBD typeBD)
        {
            List<ProductMoving> TempProductMoving = new List<ProductMoving>();
            switch (typeBD)
            {
                case TypeBD.XML:
                    TempProductMoving = LoadProductMovingFromXML();
                    break;
                case TypeBD.SQL:
                    TempProductMoving = LoadProductMovingFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
            return TempProductMoving;
        }
        public static List<UserRoleShop> LoadUserRoleShopFromDB(TypeBD typeBD)
        {
            List<UserRoleShop> TempUsersRole = new List<UserRoleShop>();
            switch (typeBD)
            {
                case TypeBD.XML:
                    TempUsersRole = LoadUserRoleShopFromXML();
                    break;
                case TypeBD.SQL:
                    TempUsersRole = LoadUserRoleShopFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
            return TempUsersRole;
        }

        public static void SaveProductShopToBD(List<ProductShop> products, TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    SaveProductShopToXML(products);
                    break;
                case TypeBD.SQL:
                    SaveProductShopToSQL(products);
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveProductShopToXML(List<ProductShop> products)
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductShop>));
                using (var stream = File.Create(PathFileProducts))
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
        public static List<ProductShop> LoadProductShopFromXML()
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductShop>));
                using (var stream = File.OpenRead(PathFileProducts))
                {
                    return (List<ProductShop>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

        }
        public static List<ProductMoving> LoadProductMovingFromXML()
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductMoving>));
                using (var stream = File.OpenRead(PathFileProductsMoving))
                {
                    return (List<ProductMoving>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
        }
        public static List<ProductMoving> LoadProductMovingFromSql()
        {
            List<ProductMoving> listProductsMoving = new List<ProductMoving>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from ProductsMoving";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ProductMoving pr = new ProductMoving();
                        pr.ID = Int32.Parse(rdr[0].ToString());
                        pr.IDproduct = Int32.Parse(rdr[1].ToString());
                        pr.CountStore = double.Parse(rdr[2].ToString());
                        pr.CountShop = double.Parse(rdr[3].ToString());
                        pr.Sold = double.Parse(rdr[4].ToString());
                        listProductsMoving.Add(pr);
                    }
                }
            }
            return listProductsMoving;
        }


        public static List<ProductShop> LoadProductShopFromSql()
        {
            List<ProductShop> listProducts = new List<ProductShop>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from Products";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ProductShop pr = new ProductShop();
                        pr.ID = Int32.Parse(rdr[0].ToString());
                        pr.Name = rdr[1].ToString();
                        pr.PriceBuy = float.Parse(rdr[2].ToString());
                        pr.PriceSell = float.Parse(rdr[3].ToString());
                        pr.UoM = (UoM)Enum.Parse(typeof(UoM), rdr[4].ToString());
                        listProducts.Add(pr);
                    }
                }
            }
            return listProducts;
        }
        public static void SaveProductShopToSQL(List<ProductShop> products)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                // to take current bd from sql and compare with list in memory, if id from bd exist in memory, update all fields, 
                // else insert to bd
                List<ProductShop> TemmBD = LoadProductShopFromSql();
                conn.Open();
                int start = products[0].ID;
                int finish = products[products.Count - 1].ID;
                for(int i = start; i <= finish; i++)
                {
                    if(TemmBD.Any(it => it.ID == i) && products.Any(it => it.ID == i))
                    {
                        SqlCommand comm = new SqlCommand("ai_update_products", conn);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.Add("@id", SqlDbType.Int).Value = products.First(it => it.ID == i).ID;
                        comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = products.First(it => it.ID == i).Name;
                        comm.Parameters.Add("@price_buy", SqlDbType.Money).Value = products.First(it => it.ID == i).PriceBuy;
                        comm.Parameters.Add("@price_sell", SqlDbType.Money).Value = products.First(it => it.ID == i).PriceSell;
                        comm.Parameters.Add("@uom", SqlDbType.NVarChar).Value = products.First(it => it.ID == i).UoM;
                        comm.ExecuteNonQuery();
                    }
                    else if (TemmBD.Count(it => it.ID == i) == 0 && products.Any(it => it.ID == i))
                    {
                        SqlCommand comm = new SqlCommand("ai_insert_products", conn);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.Add("@id", SqlDbType.Int).Value = products.First(it => it.ID == i).ID;
                        comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = products.First(it => it.ID == i).Name;
                        comm.Parameters.Add("@price_buy", SqlDbType.Money).Value = products.First(it => it.ID == i).PriceBuy;
                        comm.Parameters.Add("@price_sell", SqlDbType.Money).Value = products.First(it => it.ID == i).PriceSell;
                        comm.Parameters.Add("@uom", SqlDbType.NVarChar).Value = products.First(it => it.ID == i).UoM;
                        comm.ExecuteNonQuery();
                    }
                    else if (TemmBD.Any(it => it.ID == i) && products.Count(it => it.ID == i) == 0)
                    {
                        SqlCommand comm = new SqlCommand("ai_delete_products", conn);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.Add("@id", SqlDbType.Int).Value = TemmBD.First(it => it.ID == i).ID;
                        comm.ExecuteNonQuery();
                    }
                }
            }
        }
        public static void SaveProductMovingToSQL(List<ProductMoving> prodmoving)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                List<ProductMoving> TemmBD = LoadProductMovingFromSql();
                conn.Open();
                int start = prodmoving[0].ID;
                int finish = prodmoving[prodmoving.Count - 1].ID;
                for (int i = start; i <= finish; i++)
                {
                    if (TemmBD.Any(it => it.ID == i) && prodmoving.Any(it => it.ID == i))
                    {
                        SqlCommand comm = new SqlCommand("ai_update_products_moving", conn);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.Add("@id", SqlDbType.Int).Value = prodmoving.First(it => it.ID == i).ID;
                        comm.Parameters.Add("@id_product", SqlDbType.Int).Value = prodmoving.First(it => it.ID == i).IDproduct;
                        comm.Parameters.Add("@count_store", SqlDbType.Decimal).Value = prodmoving.First(it => it.ID == i).CountStore;
                        comm.Parameters.Add("@count_shop", SqlDbType.Decimal).Value = prodmoving.First(it => it.ID == i).CountShop;
                        comm.Parameters.Add("@sold", SqlDbType.Decimal).Value = prodmoving.First(it => it.ID == i).Sold;
                        comm.ExecuteNonQuery();
                    }
                    else if (TemmBD.Count(it => it.ID == i) == 0 && prodmoving.Any(it => it.ID == i))
                    {
                        SqlCommand comm = new SqlCommand("ai_insert_products_moving", conn);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.Add("@id", SqlDbType.Int).Value = prodmoving.First(it => it.ID == i).ID;
                        comm.Parameters.Add("@id_product", SqlDbType.Int).Value = prodmoving.First(it => it.ID == i).IDproduct;
                        comm.Parameters.Add("@count_store", SqlDbType.Decimal).Value = prodmoving.First(it => it.ID == i).CountStore;
                        comm.Parameters.Add("@count_shop", SqlDbType.Decimal).Value = prodmoving.First(it => it.ID == i).CountShop;
                        comm.Parameters.Add("@sold", SqlDbType.Decimal).Value = prodmoving.First(it => it.ID == i).Sold;
                        comm.ExecuteNonQuery();
                    }
                    else if (TemmBD.Any(it => it.ID == i) && prodmoving.Count(it => it.ID == i) == 0)
                    {
                        SqlCommand comm = new SqlCommand("ai_delete_products_moving", conn);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.Add("@id", SqlDbType.Int).Value = TemmBD.First(it => it.ID == i).ID;
                        comm.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<UserShop> LoadUserShopFromSql()
        {
            List<UserShop> listUsers = new List<UserShop>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from Users";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        UserShop us = new UserShop();
                        us.ID = Int32.Parse(rdr[0].ToString());
                        us.Login = rdr[1].ToString();
                        us.Passwword = rdr[2].ToString();
                        us.IDrole = Int32.Parse(rdr[3].ToString());
                        listUsers.Add(us);
                    }
                }
            }
            return listUsers;
        }
        public static List<UserShop> LoadUserShopFromXML()
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<UserShop>));
                using (var stream = File.OpenRead(PathFileUsers))
                {
                    return (List<UserShop>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

        }
        public static List<UserRoleShop> LoadUserRoleShopFromXML()
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<UserRoleShop>));
                using (var stream = File.OpenRead(PathFileUserRoles))
                {
                    return (List<UserRoleShop>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

        }
        public static List<UserRoleShop> LoadUserRoleShopFromSql()
        {
            List<UserRoleShop> listUsers = new List<UserRoleShop>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from UserRoles";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        UserRoleShop us = new UserRoleShop();
                        us.ID = Int32.Parse(rdr[0].ToString());
                        us.Role = (UserRole)Enum.Parse(typeof(UserRole), rdr[1].ToString());
                        listUsers.Add(us);
                    }
                }
            }
            return listUsers;
        }
    }
}
