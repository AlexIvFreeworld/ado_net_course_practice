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
        //private List<User> ListUser = new List<User>();
        //private List<UserShop> ListUser = new List<UserShop>();

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
            comboBox1.Items.Add(TypeBD.XML.ToString());
            comboBox1.Items.Add(TypeBD.Entity.ToString());
            //   ListUser = Helper.LoadUsersFromFile(); 
            //ListUser = Helper.LoadFromSql();
            //BridgeToBD.LoadUserShopFromDB(TypeBD.SQL);
            // single saving XML
            /*
                var xmlReader = new XmlSerializer(typeof(List<UserShop>));
                using (var stream = File.Create(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Users.xml")))
                {
                    xmlReader.Serialize(stream, BridgeToBD.ListUser);
                }
            BridgeToBD.LoadProductMovingFromDB(TypeBD.SQL);
            var xmlReader2 = new XmlSerializer(typeof(List<ProductMoving>));
            using (var stream = File.Create(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ProductsMoving.xml")))
            {
                xmlReader2.Serialize(stream, BridgeToBD.ProductsMoving);
            }
            BridgeToBD.LoadProductShopFromDB(TypeBD.SQL);
            var xmlReader3 = new XmlSerializer(typeof(List<ProductShop>));
            using (var stream = File.Create(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Products.xml")))
            {
                xmlReader3.Serialize(stream, BridgeToBD.Products);
            }
            BridgeToBD.LoadUserRoleShopFromDB(TypeBD.SQL);
            var xmlReader4 = new XmlSerializer(typeof(List<UserRoleShop>));
            using (var stream = File.Create(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "UserRoles.xml")))
            {
                xmlReader4.Serialize(stream, BridgeToBD.ListUserRole);
            }
            */
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
