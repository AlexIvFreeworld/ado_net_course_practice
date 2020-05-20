using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW_1_2
{

    public partial class Form1 : Form
    {
        private string cs = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const string AsyncEnabled = "Asynchronous Processing=true";
            if (!cs.Contains(AsyncEnabled))
            {
                cs = String.Format("{0}; {1}", cs, AsyncEnabled);
            }
            SqlConnection conn = new SqlConnection(cs);
            SqlCommand comm = conn.CreateCommand();
            comm.CommandText = textBox1.Text;
            comm.CommandType = CommandType.Text;
            comm.CommandTimeout = 30;
            SqlConnection conn2 = new SqlConnection(cs);
            SqlCommand comm2 = conn2.CreateCommand();
            comm2.CommandText = textBox2.Text;
            comm2.CommandType = CommandType.Text;
            comm2.CommandTimeout = 30;
            try
            {
                conn.Open();
                conn2.Open();
                IAsyncResult iar = comm.BeginExecuteReader();
                IAsyncResult iar2 = comm2.BeginExecuteReader();
                WaitHandle handle = iar.AsyncWaitHandle;
                WaitHandle handle2 = iar2.AsyncWaitHandle;
                if (handle.WaitOne() && handle2.WaitOne())
                {
                    GetDataProducts(comm, iar);
                    GetDataUsers(comm2, iar2);
                }
                else
                {
                    MessageBox.Show("TimeOut exceeded");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetDataProducts(SqlCommand command, IAsyncResult ia)
        {
            SqlDataReader reader = null;
            try
            {
                reader = command.EndExecuteReader(ia);
                DataTable table = new DataTable();
                dataGridView1.DataSource = null;
                int line = 0;
                do
                {
                    while (reader.Read())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("From GetData:" + ex.Message);
            }
            finally
            {
                try
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }

                }
                catch
                {
                }
            }
        }
        private void GetDataUsers(SqlCommand command, IAsyncResult ia)
        {
            SqlDataReader reader = null;
            try
            {
                reader = command.EndExecuteReader(ia);
                DataTable table = new DataTable();
                dataGridView2.DataSource = null;
                int line = 0;
                do
                {
                    while (reader.Read())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i < reader.FieldCount;
                            i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());
                dataGridView2.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("From GetData:" + ex.Message);
            }
            finally
            {
                try
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                catch
                {
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "WAITFOR DELAY '00:00:01'; SELECT * FROM Products;";
            textBox2.Text = "WAITFOR DELAY '00:00:05'; SELECT * FROM Users;";
        }
    }
}
