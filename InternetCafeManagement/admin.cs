﻿using System;
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
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }
        public bool role {  get; set; }
        public string usermail {  get; set; }
        public decimal balance {  get; set; }


        private void admin_Load(object sender, EventArgs e)
        {

        }

        private void pictureUsers_Click(object sender, EventArgs e)
        {
            Users users = new Users
            {

                user_role = role,
                user_mail = usermail,
                user_balance = balance
            };
            users.Show();
            this.Hide();
        }

        private void ComputersBox_Click(object sender, EventArgs e)
        {
            SessionInfo sessionInfo = new SessionInfo();
            sessionInfo.user_role = role;
            sessionInfo.user_mail = usermail;
            sessionInfo.user_balance = balance;
            sessionInfo.Show();
            this.Hide();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProductsInfo productsInfo = new ProductsInfo
            {
               user_mail=usermail, 
               user_role= role,
               user_balance=this.balance
            };
            productsInfo.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ana Sayfaya Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                AnaSayfa ana = new AnaSayfa();
                ana.user_mail = this.usermail;
                ana.user_balance = this.balance;
                ana.user_role = this.role;

                ana.Show();
                this.Hide();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            WheelInfo wheelInfo = new WheelInfo
            {
                usermail = this.usermail,
                user_role= role,
                user_balance = this.balance
            };
            wheelInfo.Show();
            this.Hide();


        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            GamePointsSalesInfo sales = new GamePointsSalesInfo
            {
                user_mail = usermail,
                user_role = role,
                user_balance= this.balance
            };
            sales.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            OrderInfo order = new OrderInfo
            {
                user_mail = usermail,
                user_role = role,
                user_balance = this.balance
            };
            order.Show();
            this.Hide();
        }
    }
}
