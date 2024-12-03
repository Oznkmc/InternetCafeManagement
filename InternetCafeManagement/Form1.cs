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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace InternetCafeManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ÜyeGirisPaneli uyepanel=new ÜyeGirisPaneli();
            uyepanel.Show();
            this.Hide();

        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            txtName.Enter += txtName_Enter;
            txtName.Leave += txtName_Leave;

            txtLastName.Enter += txtLastName_Enter;
            txtLastName.Leave += txtLastName_Leave;

            txtPassword.Enter += txtPassword_Enter;
            txtPassword.Leave += txtPassword_Leave;

            txtAgainPassword.Enter += txtAgainPassword_Enter;
            txtAgainPassword.Leave += txtAgainPassword_Leave;

            txtMail.Enter += txtMail_Enter;
            txtMail.Leave += txtMail_Leave;
            txtphone.Enter += txtphone_Enter;
            txtphone.Leave += txtphone_Leave;
            ShowPlaceholderAd();
            ShowPlaceholderSoyad();
            ShowPlaceholderMail();
            ShowPlaceholderSif();
            ShowPlaceholderTekrarSif();
            ShowPlaceholderPhone();
          
        }

        private void ShowPlaceholderAd()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = "Ad giriniz";
                txtName.ForeColor = Color.Gray; // Placeholder rengi
            }
        }

        private void HidePlaceholderAd()
        {
            if (txtName.Text == "Ad giriniz")
            {
                txtName.Text = "";
                txtName.ForeColor = Color.Black; // Yazı rengini normal yap
            }
        }

        private void ShowPlaceholderSoyad()
        {
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                txtLastName.Text = "Soyad giriniz";
                txtLastName.ForeColor = Color.Gray;
            }
        }

        private void HidePlaceholderSoyad()
        {
            if (txtLastName.Text == "Soyad giriniz")
            {
                txtLastName.Text = "";
                txtLastName.ForeColor = Color.Black;
            }
        }

        private void ShowPlaceholderMail()
        {
            if (string.IsNullOrEmpty(txtMail.Text))
            {
                txtMail.Text = "Mail giriniz:(@gmail.com)";
                txtMail.ForeColor = Color.Gray;
            }
        }

        private void HidePlaceholderMail()
        {
            if (txtMail.Text == "Mail giriniz:(@gmail.com)")
            {
                txtMail.Text = "";
                txtMail.ForeColor = Color.Black;
            }
        }
        private void ShowPlaceholderPhone()
        {
            if (string.IsNullOrEmpty(txtphone.Text))
            {
                txtphone.Text = "Telefon Numarası Giriniz";
                txtphone.ForeColor = Color.Gray;
            }
        }
        private void HidePlaceholderPhone()
        {
            if (txtphone.Text == "Telefon Numarası Giriniz")
            {
                txtphone.Text = "";
                txtphone.ForeColor = Color.Black;
            }
        }

        private void ShowPlaceholderSif()
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Text = "Şifre giriniz";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void HidePlaceholderSif()
        {
            if (txtPassword.Text == "Şifre giriniz")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void ShowPlaceholderTekrarSif()
        {
            if (string.IsNullOrEmpty(txtAgainPassword.Text))
            {
                txtAgainPassword.Text = "Şifreyi tekrar giriniz";
                txtAgainPassword.ForeColor = Color.Gray;
                txtAgainPassword.UseSystemPasswordChar = false;
            }
        }

        private void HidePlaceholderTekrarSif()
        {
            if (txtAgainPassword.Text == "Şifreyi tekrar giriniz")
            {
                txtAgainPassword.Text = "";
                txtAgainPassword.ForeColor = Color.Black;
                txtAgainPassword.UseSystemPasswordChar = true;
            }
        }






        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                   
                    string mailkontrol = txtMail.Text;
                    bool mailsyntax = false;
                    if(mailkontrol.EndsWith("@gmail.com"))
                    {
                        //MessageBox.Show("gmail uyumlu");
                        mailsyntax = true;
                    }
                    else
                    {
                        //MessageBox.Show("uyumsuz");
                        mailsyntax = false;
                    }





                    // E-posta kontrolü
                    SqlCommand kontrol = new SqlCommand("SELECT COUNT(*) FROM users WHERE email = @userMail", connection);
                    kontrol.Parameters.AddWithValue("@userMail", txtMail.Text);
                    int MevcutMailSayisi = (int)kontrol.ExecuteScalar();  // Sayıyı alıyoruz

                    // Eğer e-posta zaten varsa, kullanıcıya uyarı veriyoruz
                    if (MevcutMailSayisi > 0)
                    {
                        MessageBox.Show("Bu mail zaten kullanılmaktadır.");
                        return;  // Burada işlemi sonlandırıyoruz, çünkü e-posta zaten var
                    }

                    // Diğer kontrolleri yapalım
                    if (txtPassword.Text.Length >= 4)
                    {


                        if (txtPassword.Text == txtAgainPassword.Text && txtphone.Text.Length == 12 || txtphone.Text.Length == 10 &&
                            !string.IsNullOrEmpty(txtName.Text.Trim()) && !string.IsNullOrEmpty(txtLastName.Text.Trim()) &&
                            !string.IsNullOrEmpty(txtPassword.Text.Trim()) && !string.IsNullOrEmpty(txtMail.Text.Trim()) &&
                            !string.IsNullOrEmpty(txtphone.Text.Trim()) && mailsyntax == true)
                        {
                            // Kullanıcıyı veritabanına ekleyelim
                            SqlCommand sqlkomut = new SqlCommand("INSERT INTO users(firstname, lastname, password, email, phonenumber) " +
                                                                 "VALUES (@userFirstname, @userLastname, @userPassword, @userMail, @userPhonenumber)", connection);
                            sqlkomut.Parameters.AddWithValue("@userFirstname", txtName.Text);
                            sqlkomut.Parameters.AddWithValue("@userLastname", txtLastName.Text);
                            sqlkomut.Parameters.AddWithValue("@userPassword", txtPassword.Text);
                            sqlkomut.Parameters.AddWithValue("@userMail", txtMail.Text);
                            sqlkomut.Parameters.AddWithValue("@userPhonenumber", txtphone.Text);
                           



                            sqlkomut.ExecuteNonQuery();
                            SqlCommand idgetir = new SqlCommand("select user_id from users where email=@UserMail",connection);
                            idgetir.Parameters.AddWithValue("@UserMail", txtMail.Text);
                            object resultID= idgetir.ExecuteScalar();
                            if(resultID!=null)
                            {
                                int userid=(int) resultID;
                                SqlCommand gift= new SqlCommand("insert into gift_wheel(user_id) values(@UserId)", connection);
                                gift.Parameters.AddWithValue("@UserID", userid);
                                gift.ExecuteNonQuery();
                            }

                            
                            MessageBox.Show("Kullanıcı başarıyla eklendi!");

                            // Formu temizleyelim
                            txtName.Clear();
                            txtLastName.Clear();
                            txtPassword.Clear();
                            txtAgainPassword.Clear();
                            txtMail.Clear();
                            txtphone.Clear();
                            connection.Close();
                            DialogResult result2 = MessageBox.Show("Ana Sayfaya Dönmek İster Misin?", "Üye Kayıt İşlem Formu", MessageBoxButtons.YesNo);
                            if (result2 == DialogResult.Yes)
                            {
                                ÜyeGirisPaneli üyeGirisPaneli = new ÜyeGirisPaneli();
                                üyeGirisPaneli.Show();
                                this.Close();
                            }


                        }
                        else if (txtphone.Text.Length != 12 || txtphone.Text.Length != 10)
                        {
                            MessageBox.Show("Telefon numarası 10 haneli olmalıdır!");
                        }
                        else if (txtPassword.Text != txtAgainPassword.Text)
                        {
                            MessageBox.Show("Şifreler uyuşmuyor!");
                        }
                        else if (mailsyntax == false)
                        {
                            MessageBox.Show("Mailinizi doğru girdiğinizden emin olun.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Şifreniz En Az Dört Karakterli Olmalı...");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            HidePlaceholderAd();
        }
        private void txtName_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderAd();
        }

        private void txtLastName_Enter(object sender, EventArgs e)
        {
            HidePlaceholderSoyad();
        }
        private void txtLastName_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderSoyad();
        }

        private void txtMail_Enter(object sender, EventArgs e)
        {
            HidePlaceholderMail();
        }
        private void txtMail_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderMail();
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            HidePlaceholderSif();
        }
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderSif();
        }

        private void txtAgainPassword_Enter(object sender, EventArgs e)
        {
            HidePlaceholderTekrarSif();
        }
        private void txtAgainPassword_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderTekrarSif();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
            

        }

        private void txtphone_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderPhone();
            string telnum = txtphone.Text;

            // Telefon numarasını sayılara indirgemek için sadece rakamları alalım
            telnum = new string(telnum.Where(char.IsDigit).ToArray());

            StringBuilder duzenlitelnum = new StringBuilder();

            // Eğer telefon numarası 10 haneli ise
            if (telnum.Length == 10)
            {
                for (int i = 0; i < telnum.Length; i++)
                {
                    duzenlitelnum.Append(telnum[i]);

                    // Formatlama için karakter aralarına "-" ekleyelim
                    if (i == 2 || i == 5)
                    {
                        duzenlitelnum.Append("-");
                    }
                }
            }

            // Eğer 10 haneli değilse, olduğu gibi bırakabiliriz veya bir hata mesajı verebiliriz
            else
            {
                // Hatalı format için bir işlem yapabilirsiniz (örneğin kullanıcıya uyarı göstermek gibi)
                MessageBox.Show("Telefon numarası geçersiz.");
            }

            // Düzenlenmiş numarayı TextBox'a aktaralım
            txtphone.Text = duzenlitelnum.ToString();
        }


        private void txtphone_Enter(object sender, EventArgs e)
        {
            HidePlaceholderPhone();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ÜyeGirisPaneli üyeGirisPaneli =new ÜyeGirisPaneli();
            üyeGirisPaneli.Show();
            this.Close();
        }
    }
}
