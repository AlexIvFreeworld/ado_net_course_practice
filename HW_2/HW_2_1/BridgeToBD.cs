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
        public static List<UserShop> ListUser = new List<UserShop>();
        public static List<UserRoleShop> ListUserRole = new List<UserRoleShop>();
        public static List<ProductShop> Products = new List<ProductShop>();
        public static List<ProductMoving> ProductsMoving = new List<ProductMoving>();
        public static TypeBD ChoiceBD { set; get; }

        static BridgeToBD()
        {
            PathFileProducts = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Products.xml");
            PathFileProductsMoving = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ProductsMoving.xml");
            PathFileUserRoles = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "UserRoles.xml");
            PathFileUsers = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Users.xml");
        }
        public static void LoadProductShopFromDB(TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    LoadProductShopFromXML();
                    break;
                case TypeBD.SQL:
                    LoadProductShopFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void LoadUserShopFromDB(TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    LoadUserShopFromXML();
                    break;
                case TypeBD.SQL:
                    LoadUserShopFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void LoadProductMovingFromDB(TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    LoadProductMovingFromXML();
                    break;
                case TypeBD.SQL:
                    LoadProductMovingFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void LoadUserRoleShopFromDB(TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    LoadUserRoleShopFromXML();
                    break;
                case TypeBD.SQL:
                    LoadUserRoleShopFromSql();
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveProductShopToBD(ProductShop prShop, TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    SaveProductShopToXML(prShop);
                    break;
                case TypeBD.SQL:
                    SaveProductShopToSQL(prShop);
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveProductShopToBD(int id, TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    SaveProductShopToXML(id);
                    break;
                case TypeBD.SQL:
                    SaveProductShopToSQL(id);
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveProductMovingToBD(ProductMoving prMove, TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    SaveProductMovingToXML(prMove);
                    break;
                case TypeBD.SQL:
                    SaveProductMovingToSQL(prMove);
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveProductMovingToBD(int id, TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    SaveProductMovingToXML(id);
                    break;
                case TypeBD.SQL:
                    SaveProductMovingToSQL(id);
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveUserShopToBD(UserShop us, TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    SaveUserShopToXML(us);
                    break;
                case TypeBD.SQL:
                    SaveUserShopToSQL(us);
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveUserShopToBD(int id, TypeBD typeBD)
        {
            switch (typeBD)
            {
                case TypeBD.XML:
                    SaveUserShopToXML(id);
                    break;
                case TypeBD.SQL:
                    SaveUserShopToSQL(id);
                    break;
                case TypeBD.Entity:
                    break;
                default:
                    MessageBox.Show("ERROR typeBD");
                    break;
            }
        }
        public static void SaveProductShopToXML(ProductShop product)
        {
            LoadProductShopFromXML();
            if(Products.Any(it => it.ID == product.ID))
            {
                Products.First(it => it.ID == product.ID).Name = product.Name;
                Products.First(it => it.ID == product.ID).PriceBuy = product.PriceBuy;
                Products.First(it => it.ID == product.ID).PriceSell = product.PriceSell;
                Products.First(it => it.ID == product.ID).UoM = product.UoM;
            }
            else
            {
                product.ID = Products.Last().ID + 1;
                Products.Add(product);
            }
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductShop>));
                using (var stream = File.Create(PathFileProducts))
                {
                    xmlReader.Serialize(stream, Products);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void SaveProductShopToXML(int id)
        {
            LoadProductShopFromXML();
            Products.Remove(Products.First(it => it.ID == id));
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductShop>));
                using (var stream = File.Create(PathFileProducts))
                {
                    xmlReader.Serialize(stream, Products);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void SaveProductMovingToXML(ProductMoving product)
        {
            LoadProductMovingFromXML();
            if (ProductsMoving.Any(it => it.ID == product.ID))
            {
                ProductsMoving.First(it => it.ID == product.ID).IDproduct = product.IDproduct ;
                ProductsMoving.First(it => it.ID == product.ID).CountStore = product.CountStore;
                ProductsMoving.First(it => it.ID == product.ID).CountShop = product.CountShop;
                ProductsMoving.First(it => it.ID == product.ID).Sold = product.Sold;
            }
            else
            {
                product.ID = ProductsMoving.Last().ID + 1;
                ProductsMoving.Add(product);
            }
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductMoving>));
                using (var stream = File.Create(PathFileProductsMoving))
                {
                    xmlReader.Serialize(stream, ProductsMoving);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void SaveProductMovingToXML(int id)
        {
            LoadProductMovingFromXML();
            ProductsMoving.Remove(ProductsMoving.First(it => it.ID == id));
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<ProductMoving>));
                using (var stream = File.Create(PathFileProductsMoving))
                {
                    xmlReader.Serialize(stream, ProductsMoving);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void SaveUserShopToXML(UserShop user)
        {
            LoadUserShopFromXML();
            if (ListUser.Any(it => it.ID == user.ID))
            {
                ListUser.First(it => it.ID == user.ID).Login = user.Login;
                ListUser.First(it => it.ID == user.ID).Passwword = user.Passwword;
                ListUser.First(it => it.ID == user.ID).IDrole = user.IDrole;
            }
            else
            {
                user.ID = ListUser.Last().ID + 1;
                ListUser.Add(user);
            }
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<UserShop>));
                using (var stream = File.Create(PathFileUsers))
                {
                    xmlReader.Serialize(stream, ListUser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void SaveUserShopToXML(int id)
        {
            LoadUserShopFromXML();
            ListUser.Remove(ListUser.First(it => it.ID == id));
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<UserShop>));
                using (var stream = File.Create(PathFileUsers))
                {
                    xmlReader.Serialize(stream, ListUser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void LoadProductShopFromXML()
        {
            try
            {
                Products.Clear();
                var xmlReader = new XmlSerializer(typeof(List<ProductShop>));
                using (var stream = File.OpenRead(PathFileProducts))
                {
                    Products = (List<ProductShop>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void LoadProductMovingFromXML()
        {
            try
            {
                ProductsMoving.Clear();
                var xmlReader = new XmlSerializer(typeof(List<ProductMoving>));
                using (var stream = File.OpenRead(PathFileProductsMoving))
                {
                    ProductsMoving = (List<ProductMoving>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        public static void LoadProductMovingFromSql()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from ProductsMoving";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    ProductsMoving.Clear();
                    while (rdr.Read())
                    {
                        ProductMoving pr = new ProductMoving();
                        pr.ID = Int32.Parse(rdr[0].ToString());
                        pr.IDproduct = Int32.Parse(rdr[1].ToString());
                        pr.CountStore = double.Parse(rdr[2].ToString());
                        pr.CountShop = double.Parse(rdr[3].ToString());
                        pr.Sold = double.Parse(rdr[4].ToString());
                        ProductsMoving.Add(pr);
                    }
                }
            }
        }
        public static void LoadProductShopFromSql()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from Products";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    Products.Clear();
                    while (rdr.Read())
                    {
                        ProductShop pr = new ProductShop();
                        pr.ID = Int32.Parse(rdr[0].ToString());
                        pr.Name = rdr[1].ToString();
                        pr.PriceBuy = float.Parse(rdr[2].ToString());
                        pr.PriceSell = float.Parse(rdr[3].ToString());
                        pr.UoM = (UoM)Enum.Parse(typeof(UoM), rdr[4].ToString());
                        Products.Add(pr);
                    }
                }
            }
        }
        public static void SaveProductShopToSQL(ProductShop prShop)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                LoadProductShopFromSql();
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@id", SqlDbType.Int).Value = prShop.ID;
                comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = prShop.Name;
                comm.Parameters.Add("@price_buy", SqlDbType.Money).Value = prShop.PriceBuy;
                comm.Parameters.Add("@price_sell", SqlDbType.Money).Value = prShop.PriceSell;
                comm.Parameters.Add("@uom", SqlDbType.NVarChar).Value = prShop.UoM;
                if (Products.Any(it => it.ID == prShop.ID))
                {
                    comm.CommandText = "ai_update_products";
                    comm.ExecuteNonQuery();
                }
                else
                {
                    comm.CommandText = "ai_insert_products";
                    comm.ExecuteNonQuery();
                }
            }
        }
        public static void SaveProductShopToSQL(int id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@id", SqlDbType.Int).Value = id;
                comm.CommandText = "ai_delete_products";
                comm.ExecuteNonQuery();
            }
        }
        public static void SaveProductMovingToSQL(ProductMoving prodmoving)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                LoadProductMovingFromSql();
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@id", SqlDbType.Int).Value = prodmoving.ID;
                comm.Parameters.Add("@id_product", SqlDbType.Int).Value = prodmoving.IDproduct;
                comm.Parameters.Add("@count_store", SqlDbType.Decimal).Value = prodmoving.CountStore;
                comm.Parameters.Add("@count_shop", SqlDbType.Decimal).Value = prodmoving.CountShop;
                comm.Parameters.Add("@sold", SqlDbType.Decimal).Value = prodmoving.Sold;
                if (ProductsMoving.Any(it => it.ID == prodmoving.ID))
                {
                    comm.CommandText = "ai_update_products_moving";
                    comm.ExecuteNonQuery();
                }
                else
                {
                    comm.CommandText = "ai_insert_products_moving";
                    comm.ExecuteNonQuery();
                }
            }
        }
        public static void SaveProductMovingToSQL(int id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_delete_products_moving", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@id", SqlDbType.Int).Value = id;
                comm.ExecuteNonQuery();
            }
        }
        public static void SaveUserShopToSQL(UserShop usShop)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                LoadUserShopFromSql();
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@id", SqlDbType.Int).Value = usShop.ID;
                comm.Parameters.Add("@login", SqlDbType.NVarChar).Value = usShop.Login;
                comm.Parameters.Add("@password", SqlDbType.NVarChar).Value = usShop.Passwword;
                comm.Parameters.Add("@id_role", SqlDbType.Int).Value = usShop.IDrole;
                if (ListUser.Any(it => it.ID == usShop.ID))
                {
                    comm.CommandText = "ai_update_users";
                    comm.ExecuteNonQuery();
                }
                else
                {
                    comm.CommandText = "ai_insert_users";
                    comm.ExecuteNonQuery();
                }
            }
        }
        public static void SaveUserShopToSQL(int id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_delete_users", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@id", SqlDbType.Int).Value = id;
                comm.ExecuteNonQuery();
            }
        }
        public static void LoadUserShopFromSql()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from Users";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    ListUser.Clear();
                    while (rdr.Read())
                    {
                        UserShop us = new UserShop();
                        us.ID = Int32.Parse(rdr[0].ToString());
                        us.Login = rdr[1].ToString();
                        us.Passwword = rdr[2].ToString();
                        us.IDrole = Int32.Parse(rdr[3].ToString());
                        ListUser.Add(us);
                    }
                }
            }
        }
        public static void LoadUserShopFromXML()
        {
            try
            {
                ListUser.Clear();
                var xmlReader = new XmlSerializer(typeof(List<UserShop>));
                using (var stream = File.OpenRead(PathFileUsers))
                {
                  ListUser = (List<UserShop>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void LoadUserRoleShopFromXML()
        {
            try
            {
                ListUserRole.Clear();
                var xmlReader = new XmlSerializer(typeof(List<UserRoleShop>));
                using (var stream = File.OpenRead(PathFileUserRoles))
                {
                    ListUserRole = (List<UserRoleShop>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        public static void LoadUserRoleShopFromSql()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from UserRoles";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    ListUserRole.Clear();
                    while (rdr.Read())
                    {
                        UserRoleShop us = new UserRoleShop();
                        us.ID = Int32.Parse(rdr[0].ToString());
                        us.Role = (UserRole)Enum.Parse(typeof(UserRole), rdr[1].ToString());
                        ListUserRole.Add(us);
                    }
                }
            }
        }
    }
}
