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
        private void ShowPlaceholderPass1()
        {
            // Eğer TextBox boşsa placeholder göster
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = "Şifre giriniz";
                textBox1.ForeColor = Color.Gray; // Placeholder rengi
                textBox1.UseSystemPasswordChar = false; // Maskeyi kapat
            }
        }
        private void HidePlaceholderPass1()
        {
            // Eğer placeholder görünüyorsa temizle
            if (textBox1.Text == "Şifre giriniz")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black; // Yazı rengini normal yap
                textBox1.UseSystemPasswordChar = true; // Maskeyi aç
            }
        }
        private void ShowPlaceholderPass2()
        {
            // Eğer TextBox boşsa placeholder göster
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox2.Text = "Tekrar Şifre giriniz";
                textBox2.ForeColor = Color.Gray; // Placeholder rengi
                textBox2.UseSystemPasswordChar = false; // Maskeyi kapat
            }
        }
        private void HidePlaceholderPass2()
        {
            // Eğer placeholder görünüyorsa temizle
            if (textBox2.Text == "Tekrar Şifre giriniz")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black; // Yazı rengini normal yap
                textBox2.UseSystemPasswordChar = true; // Maskeyi aç
            }
        }
        private void NewPasswordForm_Load(object sender, EventArgs e)
        {
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;
            ShowPlaceholderPass1();
            HidePlaceholderPass1();
            
            ShowPlaceholderPass2();
            HidePlaceholderPass2();

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

        private void textBox1_Enter(object sender, EventArgs e)
        {
            HidePlaceholderPass1();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderPass1();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderPass2();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            HidePlaceholderPass2();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar == true)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
