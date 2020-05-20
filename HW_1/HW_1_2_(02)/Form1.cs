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
        private DataTable table2 = null;
        private DataTable table3 = null;

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
            comm.CommandText = "WAITFOR DELAY '00:00:01'; SELECT * FROM Products;";
            textBox1.Text = comm.CommandText;
            comm.CommandType = CommandType.Text;
            comm.CommandTimeout = 30;
            SqlConnection conn2 = new SqlConnection(cs);
            SqlCommand comm2 = conn2.CreateCommand();
            comm2.CommandText = "WAITFOR DELAY '00:00:05'; SELECT * FROM Users;";
            textBox2.Text = comm2.CommandText;
            comm2.CommandType = CommandType.Text;
            comm2.CommandTimeout = 30;
            try
            {
                conn.Open();
                conn2.Open();
                /// блок 1
                IAsyncResult iar = comm.BeginExecuteReader();
                IAsyncResult iar2 = comm2.BeginExecuteReader();
                ///
                /// блок 2
                WaitHandle handle = iar.AsyncWaitHandle;
                WaitHandle handle2 = iar2.AsyncWaitHandle;
                ///
                /// блок 3
                if (handle.WaitOne() && handle2.WaitOne())
                {
                    /// блок 4
                    GetDataProducts(comm, iar);
                    GetDataUsers(comm2, iar2);
                    ///
                }
                else
                {
                    MessageBox.Show("TimeOut exceeded");
                }
                ///
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
                     //   MessageBox.Show("GetDataProducts done");
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
                      //  MessageBox.Show("GetDataUsers done");

                    }
                }
                catch
                {
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
/*            const string AsyncEnabled = "Asynchronous Processing=true";
            if (!cs.Contains(AsyncEnabled))
            {
               cs = $"{cs}; {AsyncEnabled}";
            }
            using (var conn2 = new SqlConnection(cs))
            {
                var comm = conn2.CreateCommand();
                comm.CommandText = "WAITFOR DELAY '00:00:05'; SELECT * FROM Products;";
                comm.CommandType = CommandType.Text;
                comm.CommandTimeout = 30;

                try
                {
                    conn2.Open();
                    AsyncCallback callback = GetDataProductsCallback;
                    comm.BeginExecuteReader(callback, comm);
                    MessageBox.Show("Added thread is working ");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }*/
        }

        private void GetDataProductsCallback(IAsyncResult result)
        {
            SqlDataReader reader = null;
            try
            {
                var command = (SqlCommand)result.AsyncState;
                reader = command.EndExecuteReader(result);
                table2 = new DataTable();
                int line = 0;
                while (reader.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            table2.Columns.Add(reader.GetName(i));
                        }
                    }

                    var row = table2.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader[i];
                    }

                    table2.Rows.Add(row);

                    line++;
                }

                DgvAction();
            }
            catch (Exception e)
            {
                MessageBox.Show("From Callback 1:" + e.Message);
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
                catch (Exception e)
                {
                    MessageBox.Show("From Callback 2:" + e.Message);
                }
            }
        }

        private void DgvAction()
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.BeginInvoke(new Action(DgvAction));
            }
            dataGridView1.DataSource = table2;
            Console.WriteLine("It is DgvAction()");
        }
        private void DgvAction2()
        {
            if (dataGridView2.InvokeRequired)
            {
                dataGridView2.BeginInvoke(new Action(DgvAction2));
            }
            dataGridView2.DataSource = table3;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            const string AsyncEnabled = "Asynchronous Processing=true";
            if (!cs.Contains(AsyncEnabled))
            {
                cs = $"{cs}; {AsyncEnabled}";
            }
            using (var conn2 = new SqlConnection(cs))
            {
                var comm = conn2.CreateCommand();
                comm.CommandText = "WAITFOR DELAY '00:00:05'; SELECT * FROM Products;";
                comm.CommandType = CommandType.Text;
                comm.CommandTimeout = 30;
                try
                {
                    conn2.Open();
                    AsyncCallback callback = GetDataProductsCallback;
                    comm.BeginExecuteReader(callback, comm);
                   // MessageBox.Show("Added thread 1 is working ");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
            using (var conn3 = new SqlConnection(cs))
            {
                var comm2 = conn3.CreateCommand();
                comm2.CommandText = "WAITFOR DELAY '00:00:01'; SELECT * FROM Users;";
                comm2.CommandType = CommandType.Text;
                comm2.CommandTimeout = 30;
                try
                {
                    conn3.Open();
                    AsyncCallback callback2 = GetDataUsersCallback;
                    comm2.BeginExecuteReader(callback2, comm2);
                    MessageBox.Show("Added thread 2 is working ");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        private void GetDataUsersCallback(IAsyncResult result)
        {
            SqlDataReader reader = null;
            try
            {
                var command = (SqlCommand)result.AsyncState;
                reader = command.EndExecuteReader(result);
                table3 = new DataTable();
                int line = 0;
                while (reader.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            table3.Columns.Add(reader.GetName(i));
                        }
                    }

                    var row = table3.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader[i];
                    }

                    table3.Rows.Add(row);

                    line++;
                }

                DgvAction2();
            }
            catch (Exception e)
            {
                MessageBox.Show("From Callback 1:" + e.Message);
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
                catch (Exception e)
                {
                    MessageBox.Show("From Callback 2:" + e.Message);
                }
            }
        }
    }
}
