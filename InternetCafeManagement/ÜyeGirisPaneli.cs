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
    public partial class ÜyeGirisPaneli : Form
    {
        public ÜyeGirisPaneli()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MailGir mailGir = new MailGir();
            mailGir.Show();
            this.Hide();
        }

        private void ÜyeGirisPaneli_Load(object sender, EventArgs e)
        {
            txtMail.Enter += textBox1_Enter;
            txtMail.Leave += textBox1_Leave;
            txtPassword.Enter += textBox2_Enter;
            txtPassword.Leave += textBox2_Leave;
            ShowPlaceholder();
            ShowPlaceholderMailGiris();
        }

        private void ShowPlaceholder()
        {
            // Eğer TextBox boşsa placeholder göster
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Text = "Şifre giriniz";
                txtPassword.ForeColor = Color.Gray; // Placeholder rengi
                txtPassword.UseSystemPasswordChar = false; // Maskeyi kapat
            }
        }
        private void HidePlaceholder()
        {
            // Eğer placeholder görünüyorsa temizle
            if (txtPassword.Text == "Şifre giriniz")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black; // Yazı rengini normal yap
                txtPassword.UseSystemPasswordChar = true; // Maskeyi aç
            }
        }
        private void ShowPlaceholderMailGiris()
        {
            // Eğer TextBox boşsa placeholder göster
            if (string.IsNullOrEmpty(txtMail.Text))
            {
                txtMail.Text = "Mail Adresi Giriniz";
                txtMail.ForeColor = Color.Gray; // Placeholder rengi

            }
        }
        private void HidePlaceholderMailGiris()
        {
            // Eğer placeholder görünüyorsa temizle
            if (txtMail.Text == "Mail Adresi Giriniz")
            {
                txtMail.Text = "";
                txtMail.ForeColor = Color.Black; // Yazı rengini normal yap

            }
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            HidePlaceholder();
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            ShowPlaceholder();
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            HidePlaceholderMailGiris();
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderMailGiris();
        }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        public string name;
        public bool role; // Kullanıcı rolü
        public string usermail; // Kullanıcı maili
        public double userbalance; // Kullanıcı bakiyesi

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Kullanıcı giriş sorgusu
                    SqlCommand giris = new SqlCommand("SELECT * FROM users WHERE email = @userMail AND password = @userPassword", connection);
                    giris.Parameters.AddWithValue("@userMail", txtMail.Text);
                    giris.Parameters.AddWithValue("@userPassword", txtPassword.Text);
                    SqlDataReader dr = giris.ExecuteReader();

                    bool kullanicigiris = false;

                    // Giriş kontrolü
                    if (dr.Read())
                    {
                        kullanicigiris = true;
                    }
                    dr.Close();

                    if (kullanicigiris)
                    {
                        MessageBox.Show("Kullanıcı Girişi Doğrudur.");
                        SqlCommand balance = new SqlCommand("SELECT balance FROM users WHERE email = @userMail", connection);
                        
                        balance.Parameters.AddWithValue("@userMail", txtMail.Text);

                        object result3 = balance.ExecuteScalar();
                        if (result3 != null && result3 != DBNull.Value)
                        {
                            userbalance = Convert.ToDouble(result3); // Bakiyeyi güncelliyoruz
                        }
                        else
                        {
                            userbalance = 0; // Eğer bakiye NULL ise varsayılan değer olarak 0 atanır
                        }
                        // Kullanıcı rolü sorgulama
                        SqlCommand rol = new SqlCommand("SELECT ISNULL(user_rol, 0) FROM users WHERE email = @userMail", connection);
                        rol.Parameters.AddWithValue("@userMail", txtMail.Text);
                        object result = rol.ExecuteScalar();

                        // `DBNull` kontrolü ve role dönüşümü
                        if (result != DBNull.Value && result is bool)
                        {
                            role = (bool)result;
                        }
                        else
                        {
                            role = false; // Varsayılan değer
                        }

                        MessageBox.Show("Kullanıcının Rolü: " + (role ? "Admin" : "Kullanıcı"));

                        // Ana sayfa geçiş
                        AnaSayfa ana = new AnaSayfa();
                        ana.user_mail = txtMail.Text;
                        ana.user_role =role.ToString();
                        ana.user_balance = userbalance; // Bakiye burada atanabilir

                        // Rol kontrolü: role false ise Ana Sayfa'daki pictureBox1 gizlenir
                     

                        this.Hide();
                        ana.Show();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı Girişi Yanlıştır.");
                        txtMail.Clear();
                        txtPassword.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message);
                }
            }
        }

    }
}
