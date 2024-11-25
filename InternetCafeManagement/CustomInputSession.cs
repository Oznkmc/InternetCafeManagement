using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetCafeManagement
{
    public partial class CustomInputSession : Form
    {
        public CustomInputSession()
        {
            InitializeComponent();
        }

        public string user_role { get; set; }
        public string user_mail { get; set; }
        public double user_balance { get; set; }
        public string secili_pc { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void CustomInputSession_Load(object sender, EventArgs e)
        {


        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Oturum formuna dönmek istediğinize emin misiniz?");
            if (result == DialogResult.OK)
            {
                Sessions sessions = new Sessions();
                sessions.user_role = user_role;
                sessions.user_mail = user_mail;
                sessions.user_balance = user_balance;
                sessions.Show();
                this.Hide();
            }

        }
        public int oturumsuresi;
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (txtInput.Text.ToLower() == "sınırsız")
            {
                 oturumsuresi = 99999;
                DialogResult result = MessageBox.Show("Oturum Süresinden Emin Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    UsersSession usersSession = new UsersSession
                    {
                        oturum_suresi = oturumsuresi,
                        user_role = this.user_role,
                        user_mail = this.user_mail,
                        user_balance = this.user_balance,
                        secili_pc = this.secili_pc
                    };
                    usersSession.Show();
                    this.Hide();
                }
            }

            else if (int.TryParse(txtInput.Text, out int oturumsuresi) && oturumsuresi > 0)
            {
                if ((oturumsuresi * 0.25) < user_balance)
                {
                    DialogResult result = MessageBox.Show("Oturum Süresinden Emin Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        UsersSession usersSession = new UsersSession
                        {
                            oturum_suresi = oturumsuresi,
                            user_role = this.user_role,
                            user_mail = this.user_mail,
                            user_balance = this.user_balance,
                            secili_pc = this.secili_pc
                        };
                        usersSession.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen Geçerli Bakiye Girin.");
                    txtInput.Clear();
                    DialogResult result2 = MessageBox.Show("Bakiye Sayfasına Yönlendirilmek İster Misin?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result2 == DialogResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @secilipc", connection);
                            command.Parameters.AddWithValue("@secilipc", secili_pc);
                            command.ExecuteNonQuery();
                            Balance balance = new Balance
                            {
                                userbalance = this.user_balance,
                                usermail = this.user_mail,
                                userrole = this.user_role
                            };
                            connection.Close();
                            balance.Show();
                            this.Hide();
                        }
                    }
                }
            }
        }
              

            }
        }
    

