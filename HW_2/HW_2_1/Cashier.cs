using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Lesson_6_Cashier
{
    public partial class Cashier : Form
    {
        private BindingList<ProductCashier> ProductCashiers = new BindingList<ProductCashier>();

        public Cashier()
        {
            InitializeComponent();
            dataGridView1.DataSource = ProductCashiers;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            listBox1.Items.Clear();
            ProductCashiers.Clear();
            foreach (var productMoving in BridgeToBD.ProductsMoving.Where(it => it.CountShop > 0))
            {
                var product = BridgeToBD.Products.First(it => it.ID == productMoving.IDproduct);
                listBox1.Items.Add(product.Name);
            }
        }

        private void Cashier_Load(object sender, EventArgs e)
        {
            BridgeToBD.LoadProductShopFromDB(BridgeToBD.ChoiceBD);
            BridgeToBD.LoadProductMovingFromDB(BridgeToBD.ChoiceBD);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            var product = BridgeToBD.Products.First(it => it.Name == listBox1.SelectedItem.ToString());
            var productCashier = new ProductCashier() { Name = product.Name, Price = product.PriceSell, UoM = product.UoM, CountByPrices = 1.0 };
            if (ProductCashiers.Any(it => it.Name == productCashier.Name))
            {
                var productCashierTemp = ProductCashiers.First(it => it.Name == productCashier.Name);
                productCashierTemp.CountByPrices += productCashier.CountByPrices;
                ProductCashiers.First(it => it.Name == productCashier.Name).CountByPrices = productCashierTemp.CountByPrices;
                dataGridView1.DataSource = ProductCashiers;
            }
            else
                ProductCashiers.Add(productCashier);
        }

        private void dataGridView1_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateSum()
        {
            textBox1.Text = ProductCashiers.Sum(it => it.CountByPrices * it.Price).ToString();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateSum();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSum();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BridgeToBD.LoadProductShopFromDB(BridgeToBD.ChoiceBD);
            BridgeToBD.LoadProductMovingFromDB(BridgeToBD.ChoiceBD);
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var product = BridgeToBD.Products.First(it => it.Name == row.Cells[0].Value.ToString());
                var productMoving = BridgeToBD.ProductsMoving.First(it => it.IDproduct == product.ID);
                if (productMoving.CountShop < Convert.ToDouble(row.Cells[2].Value.ToString()))
                {
                    MessageBox.Show("Товара " + row.Cells[0].Value.ToString() + " недостаточно в магазине", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    continue;
                }
                productMoving.CountShop -= Convert.ToDouble(row.Cells[2].Value.ToString());
                productMoving.Sold += Convert.ToDouble(row.Cells[2].Value.ToString());
                BridgeToBD.SaveProductMovingToBD(productMoving, BridgeToBD.ChoiceBD);
            }
            panel1.Visible = false;
        }

        private void UpdateProduct(Product product)
        {
            product.CountMarket -= ProductCashiers.Where(it => it.Name == product.Name).Sum(it => it.CountByPrices);
            panel1.Visible = false;
        }
    }
}
