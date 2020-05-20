using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HW_1
{
    public partial class Form1 : Form
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxResult.Text = "";
            // to fill author, check on uniqueallyty, if yes  - add new author take id, add new book with id author 
            // if no, search existing author, take his id, add new book with id author
            string sql = @"select * from Authors where FirstName = @p1 and LastName = @p2";
           // string sql2 = @"select * from Authors";
            SqlDataReader rdr = null;
            using (var conn = new SqlConnection(ConnectionString))
            {
               int author_id = -1;
               SqlCommand cmd = new SqlCommand(sql, conn);
               cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBoxFirstName.Text;
               cmd.Parameters.Add("@p2", SqlDbType.NVarChar).Value = textBoxLastName.Text;
                conn.Open();
                int line = 0;
                using(rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (line == 0)
                        {
                            //цикл по числу прочитанных полей
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                //вывести в окно имена полей
                                textBoxResult.Text += rdr.GetName(i).ToString() + "\t";
                            }

                            textBoxResult.Text += Environment.NewLine;
                        }
                        textBoxResult.Text += $"{rdr[0]}\t{rdr[1]}\t{rdr[2]}" + Environment.NewLine;
                        line++;
                        author_id = Int32.Parse(rdr[0].ToString());
                    }
                }
                MessageBox.Show(author_id.ToString());
                if(author_id == -1)
                {
                    string sql3 = @"insert into Authors (FirstName, LastName) values (@p1, @p2)";
                    SqlCommand cmd3 = new SqlCommand(sql3, conn);
                    cmd3.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBoxFirstName.Text;
                    cmd3.Parameters.Add("@p2", SqlDbType.NVarChar).Value = textBoxLastName.Text;
                    cmd3.ExecuteNonQuery();

                    string sql5 = @"select id from Authors where FirstName = @p7 and LastName = @p8";
                    SqlCommand cmd5 = new SqlCommand(sql5, conn);
                    cmd5.Parameters.Add("@p7", SqlDbType.NVarChar).Value = textBoxFirstName.Text;
                    cmd5.Parameters.Add("@p8", SqlDbType.NVarChar).Value = textBoxLastName.Text;
                    int new_author_id = -1;
                    int line2 = 0; 
                    using (SqlDataReader rdr2 = cmd5.ExecuteReader())
                    {
                      while (rdr2.Read())
                        {
                            new_author_id = Int32.Parse(rdr2[0].ToString());
                            line2++;
                        }
                    }
                    MessageBox.Show("new_author_id = " + new_author_id + "/ " + line2);
                    string sql4 = @"insert into Books (Authorid, Title, PRICE, PAGES) values (@p3, @p4, @p5, @p6)";
                    SqlCommand cmd4 = new SqlCommand(sql4, conn);
                    cmd4.Parameters.Add("@p3", SqlDbType.Int).Value = new_author_id;
                    cmd4.Parameters.Add("@p4", SqlDbType.NVarChar).Value = textBox1.Text;
                    cmd4.Parameters.Add("@p5", SqlDbType.Int).Value = Int32.Parse(textBox2.Text);
                    cmd4.Parameters.Add("@p6", SqlDbType.Int).Value = Int32.Parse(textBox3.Text);
                    cmd4.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("this author is exist");
                    //cheacking title on uniqueallyty
                    string sql7 = @"select Title from Books where Authorid = @p1";
                    SqlCommand cmd7 = new SqlCommand(sql7, conn);
                    cmd7.Parameters.Add("@p1", SqlDbType.Int).Value = author_id;
                    using (SqlDataReader rdr3 = cmd7.ExecuteReader())
                    {
                        while (rdr3.Read())
                        {
                            if(rdr3[0].ToString() == textBox1.Text)
                            {
                                MessageBox.Show("Same title is exist");
                                return;
                            }
                        }
                    }

                    string sql6 = @"insert into Books (Authorid, Title, PRICE, PAGES) values (@p3, @p4, @p5, @p6)";
                    SqlCommand cmd6 = new SqlCommand(sql6, conn);
                    cmd6.Parameters.Add("@p3", SqlDbType.Int).Value = author_id;
                    cmd6.Parameters.Add("@p4", SqlDbType.NVarChar).Value = textBox1.Text;
                    cmd6.Parameters.Add("@p5", SqlDbType.Int).Value = Int32.Parse(textBox2.Text);
                    cmd6.Parameters.Add("@p6", SqlDbType.Int).Value = Int32.Parse(textBox3.Text);
                    cmd6.ExecuteNonQuery();
                }
            }

        }
    }
}
