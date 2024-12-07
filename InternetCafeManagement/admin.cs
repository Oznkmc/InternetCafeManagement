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
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }
        public string role {  get; set; }
        public string usermail {  get; set; }
        public double balance {  get; set; }

        private void admin_Load(object sender, EventArgs e)
        {

        }

        private void pictureUsers_Click(object sender, EventArgs e)
        {
            Users users= new Users
            {
               
                user_role = role,
                user_mail = usermail
            };
            users.Show();
            this.Hide();
        }

        private void ComputersBox_Click(object sender, EventArgs e)
        {
            SessionsInfo sessions= new SessionsInfo
            {
                user_mail= usermail,
                user_role= role,
            };
            sessions.Show();
            this.Hide();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProductsInfo productsInfo = new ProductsInfo
            {
               user_mail=usermail, user_role= role
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
    }
}
