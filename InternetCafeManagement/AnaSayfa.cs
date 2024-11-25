using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetCafeManagement
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa()
        {
            InitializeComponent();
        }
        public string user_role { get; set; }
        public string user_mail {  get; set; }
        public double user_balance { get; set; }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
         
        }

        private void SessionBox_Click(object sender, EventArgs e)
        {
            Sessions sessions = new Sessions();
            sessions.user_balance = user_balance;
            sessions.user_role = user_role;
            sessions.user_mail = user_mail;
            sessions.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Order order= new Order();
            order.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Balance balance = new Balance();
            balance.usermail = user_mail;
            balance.userbalance = user_balance;
            balance.userrole = user_role;
            balance.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            Wheel wheel = new Wheel();
            wheel.usermail=user_mail;
            wheel.user_balance = user_balance;
            wheel.user_rol = user_role;
            wheel.Show();
            this.Hide();
        }

        private void ComputersBox_Click(object sender, EventArgs e)
        {

        }

        private void pictureUsers_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            admin Admin = new admin();
            Admin.role = user_role;
            Admin.usermail = user_mail;
            Admin.balance = user_balance;
            Admin.Show();
            this.Hide();
        }
    }
}
