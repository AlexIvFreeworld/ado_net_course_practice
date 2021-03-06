﻿using System;
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
        private string hashPassword => Helper.CreateMD5(textBox8.Text);
        private FormInformation F = null;
        public Manager()
        {
            InitializeComponent();
        }

        private void dataGridView1ReLoad()
        {
            double totalCost = 0;
            double totalProfit = 0;
            BridgeToBD.LoadProductShopFromDB(BridgeToBD.ChoiceBD);
            BridgeToBD.LoadProductMovingFromDB(BridgeToBD.ChoiceBD);
            string[] str = new string[7];
            double[] result = new double[4];
             dataGridView1.Rows.Clear();
            foreach (var item in BridgeToBD.Products)
            {
                str[0] = item.Name;
                str[1] = BridgeToBD.ProductsMoving.First(it => it.IDproduct == item.ID).CountStore.ToString() + " " + item.UoM.GetDescription();
                result[0] = BridgeToBD.ProductsMoving.First(it => it.IDproduct == item.ID).CountStore;
                str[2] = BridgeToBD.ProductsMoving.First(it => it.IDproduct == item.ID).CountShop.ToString() + " " + item.UoM.GetDescription();
                result[1] = BridgeToBD.ProductsMoving.First(it => it.IDproduct == item.ID).CountShop;
                str[3] = item.PriceBuy.ToString();
                str[4] = item.PriceSell.ToString();
                str[5] = ((result[0] + result[1]) * double.Parse(str[4])).ToString();
                result[2] = ((result[0] + result[1]) * double.Parse(str[4]));
                str[6] = ((result[0] + result[1]) * (double.Parse(str[4]) - double.Parse(str[3]))).ToString();
                result[3] = ((result[0] + result[1]) * (double.Parse(str[4]) - double.Parse(str[3])));
                dataGridView1.Rows.Add(str);
                totalCost += result[2];
                totalProfit += result[3];
            }
              textBoxTotalCost.Text = totalCost.ToString() + " " + "руб.";
              textBoxTotalProfit.Text = totalProfit.ToString() + " " + "руб.";
        }

        private void dataGridView2ReLoad()
        {
            double totalCost = 0;
            double totalProfit = 0;
            BridgeToBD.LoadProductShopFromDB(BridgeToBD.ChoiceBD);
            BridgeToBD.LoadProductMovingFromDB(BridgeToBD.ChoiceBD);
            string[] str = new string[6];
            double[] result = new double[3];
            dataGridView2.Rows.Clear();
            foreach (var item in BridgeToBD.Products)
            {
                str[0] = item.Name;
                str[1] = BridgeToBD.ProductsMoving.First(it => it.IDproduct == item.ID).Sold.ToString() + " " + item.UoM.GetDescription();
                result[0] = BridgeToBD.ProductsMoving.First(it => it.IDproduct == item.ID).Sold;
                str[2] = item.PriceSell.ToString();
                str[3] = item.PriceBuy.ToString();
                str[4] = (result[0] * double.Parse(str[2])).ToString();
                result[1] = (result[0] * double.Parse(str[2]));
                str[5] = ((double.Parse(str[2]) - double.Parse(str[3])) * result[0]).ToString();
                result[2] = ((double.Parse(str[2]) - double.Parse(str[3])) * result[0]);
                dataGridView2.Rows.Add(str);
                totalCost += result[1];
                totalProfit += result[2];
            }
            textBox1.Text = totalCost.ToString() + " " + "руб.";
            textBox2.Text = totalProfit.ToString() + " " + "руб.";
        }
        private void UsersViewReload()
        {
            BridgeToBD.LoadUserShopFromDB(BridgeToBD.ChoiceBD);
            BridgeToBD.LoadUserRoleShopFromDB(BridgeToBD.ChoiceBD);
            listView1.Items.Clear();
            foreach (var user in BridgeToBD.ListUser)
            {
                string us_role = BridgeToBD.ListUserRole.First(it => it.ID == user.IDrole).Role.ToString();
                listView1.Items.Add(user.Login).SubItems.Add(us_role);
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

            dataGridView1ReLoad();
            dataGridView2ReLoad();
            UsersViewReload();
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
            if (textBox3.Text == string.Empty)
            {
                MessageBox.Show("Вы не заполнили поле 'Название товара'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Вы не заполнили поле 'Единицы измерения'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Цена закупки'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBox5.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Цена продажи'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBox6.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Количество товара'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Склад'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
              BridgeToBD.LoadProductShopFromDB(BridgeToBD.ChoiceBD);
            if (!BridgeToBD.Products.Any(it => it.Name == textBox3.Text)) {
                ProductShop pr = new ProductShop();
                pr.Name = textBox3.Text;
                pr.PriceSell = (float)Convert.ToDouble(textBox5.Text);
                pr.PriceBuy = (float)Convert.ToDouble(textBox4.Text);
                pr.UoM = (UoM)Enum.Parse(typeof(UoM), comboBox1.Text);
               
                ProductMoving prmove = new ProductMoving();
                prmove.Sold = 0;
                if (comboBox2.Text == "Warehouse")
                {
                   prmove.CountStore = Convert.ToDouble(textBox6.Text);
                   prmove.CountShop = 0;
                }
                else if (comboBox2.Text == "Shop")
                {
                    prmove.CountShop = Convert.ToDouble(textBox6.Text);
                    prmove.CountStore = 0;
                }
                else
                {
                    MessageBox.Show("Некорректно выбран склад");
                }
                BridgeToBD.SaveProductShopToBD(pr, BridgeToBD.ChoiceBD);
                BridgeToBD.LoadProductShopFromDB(BridgeToBD.ChoiceBD);
                prmove.IDproduct = BridgeToBD.Products.Last().ID;
                BridgeToBD.SaveProductMovingToBD(prmove, BridgeToBD.ChoiceBD);
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
                    int id = BridgeToBD.Products.First(it => it.Name == row.Cells[0].Value.ToString()).ID;
                    int id_prmove = BridgeToBD.ProductsMoving.First(it => it.IDproduct == id).ID;
                    BridgeToBD.SaveProductMovingToBD(id_prmove, BridgeToBD.ChoiceBD);
                    BridgeToBD.SaveProductShopToBD(id,BridgeToBD.ChoiceBD);
                }
                dataGridView1ReLoad();
            }
            else
            {
                MessageBox.Show("Вы не выбрали строку с товаром для удаления", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox7.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Логин'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(textBox8.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Пароль'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(comboBox3.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Должность'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(BridgeToBD.ListUser.Any(it => String.Equals(it.Login, textBox7.Text, StringComparison.OrdinalIgnoreCase))) // it need case insensitive
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            int id_role = BridgeToBD.ListUserRole.First(it => it.Role.ToString() == comboBox3.Text).ID;
            UserShop us = new UserShop() { Login = textBox7.Text, Passwword = hashPassword, IDrole = id_role};
            BridgeToBD.SaveUserShopToBD(us, BridgeToBD.ChoiceBD);
            UsersViewReload();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Ни один пользователь не выбран", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            BridgeToBD.LoadUserShopFromDB(BridgeToBD.ChoiceBD);
            BridgeToBD.LoadUserRoleShopFromDB(BridgeToBD.ChoiceBD);
            int us_role_manager = BridgeToBD.ListUserRole.First(it => it.Role.ToString() == "Manager").ID;
            int us_role_cashier = BridgeToBD.ListUserRole.First(it => it.Role.ToString() == "Cashier").ID;
            if (BridgeToBD.ListUser.Count(it => it.IDrole == us_role_manager) <= 1 && listView1.SelectedItems[0].SubItems[1].Text == "Manager")
            {
                MessageBox.Show("Остался последний пользователь с должностью Менеджер, удаление невозможно!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (BridgeToBD.ListUser.Count(it => it.IDrole == us_role_cashier) <= 1 && listView1.SelectedItems[0].SubItems[1].Text == "Cashier")
            {
              var result = MessageBox.Show("Вы точно хотите удалить последнего пользователя с должностью кассир?", "Form Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            int id = BridgeToBD.ListUser.First(it => it.Login == listView1.SelectedItems[0].Text).ID;
            BridgeToBD.SaveUserShopToBD(id, BridgeToBD.ChoiceBD);
            UsersViewReload();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Ни один пользователь не выбран", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            BridgeToBD.LoadUserShopFromDB(BridgeToBD.ChoiceBD);
            int us_role_manager = BridgeToBD.ListUserRole.First(it => it.Role.ToString() == "Manager").ID;
            if (BridgeToBD.ListUser.Count(it => it.IDrole == us_role_manager) <= 1 && listView1.SelectedItems[0].SubItems[1].Text == "Manager")
            {
                MessageBox.Show("Редактирование единственного пользователя Менеджер невозможно!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(textBox7.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Логин'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(textBox8.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Пароль'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(comboBox3.Text == "")
            {
                MessageBox.Show("Вы не заполнили поле 'Должность'", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string currentLogin = listView1.SelectedItems[0].SubItems[0].Text;
            UserShop user = BridgeToBD.ListUser.First(it => it.Login == currentLogin);
            user.Login = textBox7.Text;
            user.Passwword = hashPassword;
            user.IDrole = BridgeToBD.ListUserRole.First(it => it.Role.ToString() == comboBox3.Text).ID;
            BridgeToBD.SaveUserShopToBD(user, BridgeToBD.ChoiceBD);
            UsersViewReload();
            
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            F = new FormInformation();
            F.Show();
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "priceBuy")
            {
                e.Value = $"{e.Value:F} руб";
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "priceSell")
            {
                e.Value = $"{e.Value:F} руб";
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "totalCost")
            {
                e.Value = $"{e.Value:F} руб";
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "profitPlan")
            {
                e.Value = $"{e.Value:F} руб";
            }
        }
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "priceBuy")
            {
                e.Value = $"{e.Value:F} руб";
            }
            if (dataGridView2.Columns[e.ColumnIndex].Name == "priceSell")
            {
                e.Value = $"{e.Value:F} руб";
            }
            if (dataGridView2.Columns[e.ColumnIndex].Name == "totalSell")
            {
                e.Value = $"{e.Value:F} руб";
            }
            if (dataGridView2.Columns[e.ColumnIndex].Name == "allProfit")
            {
                e.Value = $"{e.Value:F} руб";
            }
        }
    }
}
