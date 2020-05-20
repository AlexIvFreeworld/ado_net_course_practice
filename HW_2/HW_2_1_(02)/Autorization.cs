using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using Lesson_6_Cashier;

namespace Lesson_6_Casher
{
    public partial class Autorization : Form
    {
        //private List<User> ListUser = new List<User>();
        private List<UserShop> ListUser = new List<UserShop>();

        private string Login => logintextBox.Text;
        private string Password => Helper.CreateMD5(passwordTextBox.Text);

        public Autorization()
        {
            InitializeComponent();
        }

        private void Autorization_Load(object sender, EventArgs e)
        {
            //   ListUser = Helper.LoadUsersFromFile(); 
            //ListUser = Helper.LoadFromSql();
            ListUser = BridgeToBD.LoadUserShopFromDB(TypeBD.SQL);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = ListUser.FirstOrDefault(it =>
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
    }
}
