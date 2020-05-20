using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lesson_6_Cashier
{
    public partial class Manager : Form
    {
       // private List<Product> Products = new List<Product>();
        private List<ProductSell> ProductsSell = new List<ProductSell>();
        private List<User> Users = new List<User>();
        private string hashPassword => Helper.CreateMD5(textBox8.Text);
        private FormInformation F = null;
        private List<ProductShop> Products = new List<ProductShop>();
        private List<ProductMoving> ProductsMoving = new List<ProductMoving>();
        public Manager()
        {
            InitializeComponent();
        }

        private void dataGridView1ReLoad()
        {
            double totalCost = 0;
            double totalProfit = 0;
            // Products = HelperProduct.LoadUsersFromFile();
            Products = BridgeToBD.LoadProductShopFromDB(TypeBD.SQL);
            ProductsMoving = BridgeToBD.LoadProductMovingFromDB(TypeBD.SQL);
            string[] str = new string[7]; 
             dataGridView1.Rows.Clear();
            foreach (var item in Products)
            {
                str[0] = item.Name;
                str[1] = ProductsMoving.First(it => it.IDproduct == item.ID).CountStore.ToString();
                str[2] = ProductsMoving.First(it => it.IDproduct == item.ID).CountShop.ToString();
                str[3] = item.PriceBuy.ToString();
                str[4] = item.PriceSell.ToString();
                str[5] = ((double.Parse(str[1]) + double.Parse(str[2])) * double.Parse(str[4])).ToString();
                str[6] = ((double.Parse(str[1]) + double.Parse(str[2])) * (double.Parse(str[4]) - double.Parse(str[3]))).ToString();
                dataGridView1.Rows.Add(str);
                totalCost += double.Parse(str[5]);
                totalProfit += double.Parse(str[6]);
            }
              textBoxTotalCost.Text = totalCost.ToString() + " " + "руб.";
              textBoxTotalProfit.Text = totalProfit.ToString() + " " + "руб.";
        }
        private void UsersViewReload()
        {
            //Users = Helper.LoadUsersFromFile();
            Users = Helper.LoadFromSql();
            listView1.Items.Clear();
            foreach (var user in Users)
            {
                listView1.Items.Add(user.Login).SubItems.Add(user.UserRole.ToString());
            }
        }
        private void Manager_Load(object sender, EventArgs e)
        {
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Название товара";
            column1.Width = 100;
            column1.ReadOnly = true;
            column1.Name = "name";
            column1.Frozen = true;
            column1.CellTemplate = new DataGridViewTextBoxCell();

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Количество товара на складе";
            column2.Width = 100;
            column2.ReadOnly = true;
            column2.Name = "countStore";
            column2.Frozen = true;
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Количество товара в магазине";
            column3.Width = 100;
            column3.ReadOnly = true;
            column3.Name = "countShop";
            column3.Frozen = true;
            column3.CellTemplate = new DataGridViewTextBoxCell();

            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Цена закупки";
            column4.Width = 100;
            column4.ReadOnly = true;
            column4.Name = "priceBuy";
            column4.Frozen = true;
            column4.CellTemplate = new DataGridViewTextBoxCell();

            var column5 = new DataGridViewColumn();
            column5.HeaderText = "Цена продажи";
            column5.Width = 100;
            column5.ReadOnly = true;
            column5.Name = "priceSell";
            column5.Frozen = true;
            column5.CellTemplate = new DataGridViewTextBoxCell();

            var column6 = new DataGridViewColumn();
            column6.HeaderText = "Общая стоимость товара";
            column6.Width = 100;
            column6.ReadOnly = true;
            column6.Name = "totalCost";
            column6.Frozen = true;
            column6.CellTemplate = new DataGridViewTextBoxCell();

            var column7 = new DataGridViewColumn();
            column7.HeaderText = "Плановая маржа";
            column7.Width = 100;
            column7.ReadOnly = true;
            column7.Name = "profitPlan";
            column7.Frozen = true;
            column7.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            dataGridView1.Columns.Add(column4);
            dataGridView1.Columns.Add(column5);
            dataGridView1.Columns.Add(column6);
            dataGridView1.Columns.Add(column7);

            // var row = new DataGridViewRow();
            // var cell = new DataGridView();
            dataGridView1ReLoad();

            double totalCost2 = 0;
            double totalProfit2 = 0;
            // ProductsSell = HelperProductSell.LoadFromFile();
/*            ProductsSell = HelperProductSell.LoadFromSql();
            foreach (var item in ProductsSell)
            {
                dataGridView2.Rows.Add(item.setGridView2());
                totalCost2 += item.SumSell;
                totalProfit2 += item.ProfitSell;
            }
            textBox1.Text = totalCost2.ToString() + " " + "руб.";
            textBox2.Text = totalProfit2.ToString() + " " + "руб.";*/

            //UsersViewReload();
            comboBox3.Items.Add(UserRole.Cashier);
            comboBox3.Items.Add(UserRole.Manager);

            comboBox1.Items.Add(UoM.Piece);
            comboBox1.Items.Add(UoM.Kg);
            comboBox1.Items.Add(UoM.Liter);
            comboBox1.Items.Add(UoM.Cans);

            comboBox2.Items.Add("Shop");
            comboBox2.Items.Add("Warehouse");
        }

        private void button1_Click(object sender, EventArgs e)
        {
             if (textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Вы не заполнили некоторые поля", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // it need to check name
            Products = BridgeToBD.LoadProductShopFromDB(TypeBD.SQL);
            ProductsMoving = BridgeToBD.LoadProductMovingFromDB(TypeBD.SQL);
            if (!Products.Any(it => it.Name == textBox3.Text)) {
                ProductShop pr = new ProductShop();
                pr.ID = Products.Last().ID + 1;
                pr.Name = textBox3.Text;
                pr.PriceSell = (float)Convert.ToDouble(textBox5.Text);
                pr.PriceBuy = (float)Convert.ToDouble(textBox4.Text);
                pr.UoM = (UoM)Enum.Parse(typeof(UoM), comboBox1.Text);
                ProductMoving prmove = new ProductMoving();
                prmove.IDproduct = pr.ID;
                prmove.Sold = 0;
                if (comboBox2.Text == "Warehouse")
                {
                   prmove.CountStore = Convert.ToDouble(textBox6.Text);
                }
                else if (comboBox2.Text == "Shop")
                {
                    prmove.CountShop = Convert.ToDouble(textBox6.Text);
                }
                else
                {
                    MessageBox.Show("Некорректно выбран склад");
                }
                Products.Add(pr);
                ProductsMoving.Add(prmove);
                //HelperProduct.SaveUsersFromFile(Products);
                // this.Update(); // it doesn't work
                //HelperProduct.InsertToSql(pr);
                BridgeToBD.SaveProductShopToBD(Products, TypeBD.SQL);
                BridgeToBD.SaveProductMovingToSQL(ProductsMoving);
                dataGridView1ReLoad();
            }
            else
            {
                MessageBox.Show("Товар с таким именем уже есть на складе");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                foreach(DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // MessageBox.Show(Products.First(it => it.Name == row.Cells[0].Value.ToString()).Name);
                    // Products.Remove(Products.First(it => it.Name == row.Cells[0].Value.ToString()));
                    HelperProduct.DeleteFromSql(row.Cells[0].Value.ToString());
                }
               // HelperProduct.SaveUsersFromFile(Products);
                dataGridView1ReLoad();
            }
            else
            {
                MessageBox.Show("Вы не выбрали строку с товаром для удаления", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox7.Text == "" || comboBox3.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Поля заполнены не полностью", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(Users.Any(it => String.Equals(it.Login, textBox7.Text, StringComparison.OrdinalIgnoreCase))) // it need case insensitive
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Users.Add(new User() { Login = textBox7.Text, Passwword = hashPassword, UserRole = (UserRole)Enum.Parse(typeof(UserRole), comboBox3.Text)});
            User us = new User() { Login = textBox7.Text, Passwword = hashPassword, UserRole = (UserRole)Enum.Parse(typeof(UserRole), comboBox3.Text)};
            // Helper.SaveUsersFromFile(Users);
            Helper.InsertToSql(us);
            UsersViewReload();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Ни один пользователь не выбран", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Users.Count(it => it.UserRole.ToString() == "Manager") <= 1 && listView1.SelectedItems[0].SubItems[1].Text == "Manager")
            {
                MessageBox.Show("Остался последний пользователь с должностью Менеджер, удаление невозможно!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Users.Count(it => it.UserRole.ToString() == "Cashier") <= 1 && listView1.SelectedItems[0].SubItems[1].Text == "Cashier")
            {
              var result = MessageBox.Show("Вы точно хотите удалить последнего пользователя с должностью кассир?", "Form Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            //MessageBox.Show("Удаление этого пользователя возможно" + listView1.SelectedItems[0].SubItems[1].Text);
            //Users.Remove(Users.First(it => it.Login == listView1.SelectedItems[0].Text));
            //Helper.SaveUsersFromFile(Users);
            Helper.DeleteFromSql(listView1.SelectedItems[0].Text);
            UsersViewReload();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Ни один пользователь не выбран", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Users = Helper.LoadFromSql();
            if (Users.Count(it => it.UserRole.ToString() == "Manager") <= 1 && listView1.SelectedItems[0].SubItems[1].Text == "Manager")
            {
                MessageBox.Show("Редактирование единственного пользователя Менеджер невозможно!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBox7.Text == "" || comboBox3.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Поля заполнены не полностью", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //var user =  Users.First(it => it.Login == listView1.SelectedItems[0].SubItems[0].Text);
            string currentLogin = listView1.SelectedItems[0].SubItems[0].Text;
            User user = new User();
            user.Login = textBox7.Text;
            user.Passwword = hashPassword;
            user.UserRole = (UserRole)Enum.Parse(typeof(UserRole), comboBox3.Text);
            //Helper.SaveUsersFromFile(Users);
            Helper.UpdateToSql(currentLogin, user);
            UsersViewReload();
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
           // F = new FormInformation();
          //  F.Show();
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
           // F?.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            F = new FormInformation();
            F.Show();
        }
    }
}
