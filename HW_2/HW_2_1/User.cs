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
    [Serializable]
    public class User
    {
        public string Login { get; set; }

        public string Passwword { get; set; }

        public UserRole UserRole { get; set; }
    }

/*    public enum UserRole
    {
        Cashier,
        Manager
    }*/

    public static class Helper
    {
        private static string PathFile;
        static string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        static Helper()
        {
            PathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Users.xml");
        }

        public static void SaveUsersFromFile(List<User> users)
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<User>));
                using (var stream = File.Create(PathFile))
                {
                    xmlReader.Serialize(stream, users);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи списка пользователей", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public static List<User> LoadUsersFromFile()
        {
            try
            {
                var xmlReader = new XmlSerializer(typeof(List<User>));
                using (var stream = File.OpenRead(PathFile))
                {
                    return (List<User>)xmlReader.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки списка пользователей", "ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static List<User> LoadFromSql()
        {
            List<User> listUsers = new List<User>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"select * from Users";
                SqlCommand com = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        User us = new User();
                        us.Login = rdr[1].ToString();
                        us.Passwword = rdr[2].ToString();
                        us.UserRole = (UserRole)Enum.Parse(typeof(UserRole), rdr[3].ToString());
                        listUsers.Add(us);
                    }
                }
            }
            return listUsers;
        }
        public static void InsertToSql(User newUser)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_insert_to_users", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@login", SqlDbType.NVarChar).Value = newUser.Login;
                comm.Parameters.Add("@password", SqlDbType.NVarChar).Value = newUser.Passwword;
                comm.Parameters.Add("@userrole", SqlDbType.NVarChar).Value = newUser.UserRole;
                comm.ExecuteNonQuery();
            }
        }
        public static void DeleteFromSql(string loginForDel)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_delete_user", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@login", SqlDbType.NVarChar).Value = loginForDel;
                comm.ExecuteNonQuery();
            }
        }
        public static void UpdateToSql(String curLog, User user)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ai_update_user_2", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@currentlogin", SqlDbType.NVarChar).Value = curLog;
                comm.Parameters.Add("@login", SqlDbType.NVarChar).Value = user.Login;
                comm.Parameters.Add("@password", SqlDbType.NVarChar).Value = user.Passwword;
                comm.Parameters.Add("@userrole", SqlDbType.NVarChar).Value = user.UserRole;
                comm.ExecuteNonQuery();
            }
        }
    }

}
