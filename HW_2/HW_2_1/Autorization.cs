using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Xml.Serialization;
using Lesson_6_Cashier;

namespace Lesson_6_Casher
{
    public partial class Autorization : Form
    {
        private string Login => logintextBox.Text;
        private string Password => Helper.CreateMD5(passwordTextBox.Text);

        public Autorization()
        {
            InitializeComponent();
        }

        private void Autorization_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            comboBox1.Text = TypeBD.SQL.ToString();
            comboBox1.Items.Add(TypeBD.SQL.ToString());
            comboBox1.Items.Add(TypeBD.XML.ToString());
            comboBox1.Items.Add(TypeBD.Entity.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = BridgeToBD.ListUser.FirstOrDefault(it =>
                string.Equals(it.Login, Login, StringComparison.CurrentCultureIgnoreCase) && it.Passwword == Password);
            if (user == null)
            {
                MessageBox.Show("Неверные учетные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.Visible = false;
            Form form;
            switch (user.IDrole)
            {
                case 1:
                    form = new Cashier();
                    break;
                case 2:
                    form = new Manager();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            form.Closed += (s, args) => this.Close();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "")
            {
                MessageBox.Show("Не выбрана база данных");
                return;
            }
            BridgeToBD.ChoiceBD = (TypeBD)Enum.Parse(typeof(TypeBD), comboBox1.Text);
            BridgeToBD.LoadUserShopFromDB(BridgeToBD.ChoiceBD);
            panel1.Visible = true;
        }
    }
}
