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
    public partial class NewPasswordForm : Form
    {
        public string userEmail;  // Kullanıcının e-posta adresi
         
        public NewPasswordForm(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
           
            
        }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void UpdatePassword(string email, string newPassword)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();


                SqlCommand UpdateCommand = new SqlCommand("Update users SET password=@NewPassword where email=@UserMail", connection);
                UpdateCommand.Parameters.AddWithValue("@UserMail", email);
                UpdateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                int row = UpdateCommand.ExecuteNonQuery();
                if (row > 0)
                {
                    
                    ÜyeGirisPaneli üyeGirisPaneli=new ÜyeGirisPaneli();
                    üyeGirisPaneli.Show();
                    this.Close();
                }
                
            }
        }
        private void SetPlaceholder(TextBox textBox, string placeholderText)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholderText;
                textBox.ForeColor = Color.Gray;
                textBox.UseSystemPasswordChar = false; // Placeholder gösterildiğinde maske kapatılır
            }
        }

        private void RemovePlaceholder(TextBox textBox, string placeholderText)
        {
            if (textBox.Text == placeholderText)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
                textBox.UseSystemPasswordChar = true; // Kullanıcı şifre yazmaya başlayınca maske açılır
            }
        }
        private void NewPasswordForm_Load(object sender, EventArgs e)
        {
            SetPlaceholder(textBox1, "Şifre giriniz");
            SetPlaceholder(textBox2, "Tekrar Şifre giriniz");

            // Placeholder Yönetimi için Olaylar
            textBox1.Enter += (s, ev) => RemovePlaceholder(textBox1, "Şifre giriniz");
            textBox1.Leave += (s, ev) => SetPlaceholder(textBox1, "Şifre giriniz");

            textBox2.Enter += (s, ev) => RemovePlaceholder(textBox2, "Tekrar Şifre giriniz");
            textBox2.Leave += (s, ev) => SetPlaceholder(textBox2, "Tekrar Şifre giriniz");

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string newPassword = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Yeni şifreyi girin.");
                return;
            }
            if(textBox1.Text==textBox2.Text)
            {
                // Şifreyi veritabanına güncelle
                UpdatePassword(userEmail, newPassword);

                MessageBox.Show("Şifreniz başarıyla güncellendi.");
            }
            else
            {
                MessageBox.Show("Şifre uyumu başarısız.");
                textBox2.Clear();
                textBox1.Clear();
            }
           
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = !textBox1.UseSystemPasswordChar;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;
        }

        ////private void textBox1_Enter(object sender, EventArgs e)
        ////{
        ////    HidePlaceholderPass1();
        ////}

        ////private void textBox1_Leave(object sender, EventArgs e)
        ////{
        ////    ShowPlaceholderPass1();
        ////}

        ////private void textBox2_Enter(object sender, EventArgs e)
        ////{
        ////    HidePlaceholderPass2();
        ////}

        ////private void textBox2_Leave(object sender, EventArgs e)
        ////{
        ////    ShowPlaceholderPass2();
        //}
        // textBox1 Enter ve Leave Olayları
        private void textBox1_Enter(object sender, EventArgs e)
        {
            RemovePlaceholder(textBox1, "Şifre giriniz");
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            SetPlaceholder(textBox1, "Şifre giriniz");
        }

        // textBox2 Enter ve Leave Olayları
        private void textBox2_Enter(object sender, EventArgs e)
        {
            RemovePlaceholder(textBox2, "Tekrar Şifre giriniz");
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            SetPlaceholder(textBox2, "Tekrar Şifre giriniz");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ÜyeGirisPaneli üyeGirisPaneli = new ÜyeGirisPaneli();
            DialogResult result = MessageBox.Show("Üye Giriş Sayfasına Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {


                this.Hide();
                üyeGirisPaneli.Show(); // Admin formunu göster
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void textBox1_Enter_1(object sender, EventArgs e)
        {
            RemovePlaceholder(textBox1, "Şifre giriniz");
        }

        private void textBox1_Leave_1(object sender, EventArgs e)
        {
            SetPlaceholder(textBox1, "Şifre giriniz");
        }

        private void textBox2_Leave_1(object sender, EventArgs e)
        {
            SetPlaceholder(textBox2, "Tekrar Şifre giriniz");
        }

        private void textBox2_Enter_1(object sender, EventArgs e)
        {
            RemovePlaceholder(textBox2, "Tekrar Şifre giriniz");
        }
    }
}
