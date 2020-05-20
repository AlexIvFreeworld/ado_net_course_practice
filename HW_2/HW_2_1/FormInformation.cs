using System;
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
    public partial class FormInformation : Form
    {
        public FormInformation()
        {
            InitializeComponent();
            textBox1.Text = "1. Заполните новый логин пользователя, его новый пароль и новую должность ";
            textBox1.Text += "\n" + "2. Выберите пользователя для редактирования из списка ";
            textBox1.Text += "\n" + "3. Нажмите кнопку 'Редактировать выбранного пользователя' ";
            textBox1.Select(0,0);
        }
    }
}
