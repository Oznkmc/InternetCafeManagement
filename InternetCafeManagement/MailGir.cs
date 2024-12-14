using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InternetCafeManagement
{
    public partial class MailGir : Form
    {
        public MailGir()
        {
            InitializeComponent();
        }
        public string user_mail {  get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        public  void SendPasswordResetEmail(string userEmail, string resetCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from users where email=@UserMail", connection);
                command.Parameters.AddWithValue("@UserMail", textBox1.Text);
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    // Gönderen e-posta adresi ve şifresi (Gmail örneği)
                    string fromEmail = "oznkmc21@gmail.com";  // Gönderen e-posta adresini buraya yaz
                    string emailPassword = "nspw nifz ccjm rlza";  // E-posta şifresi (bu bilgiyi güvenli bir şekilde saklamalısınız)

                    // E-posta başlığı ve içeriği
                    string subject = "Şifre Yenileme Kodu";
                    string body = $"Şifrenizi sıfırlamak için aşağıdaki kodu kullanın:\n\n{resetCode}\n\nKodunuz 15 dakika geçerli olacaktır.";

                    // SMTP ayarları (Gmail için)
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,  // TLS portu
                        Credentials = new NetworkCredential(fromEmail, emailPassword),  // Gönderen e-posta ve uygulama şifresi
                        EnableSsl = true  // SSL/TLS bağlantısını etkinleştir
                    };

                    // E-posta mesajı
                    MailMessage mailMessage = new MailMessage(fromEmail, userEmail, subject, body);

                    try
                    {
                        // E-posta gönderimi
                        smtpClient.Send(mailMessage);
                        MessageBox.Show("Kodunuz Başarıyla Gönderildi");
                        DogrulamaKodu codeForm = new DogrulamaKodu(resetCode, userEmail);
                        codeForm.userEmail = textBox1.Text;
                        codeForm.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda mesaj
                        MessageBox.Show("E-posta gönderme hatası: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Böyle bir mail adresi mevcut değil.Tekrar Deneyiniz.");
                    textBox1.Clear();
                    
                  
                }

            }


          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string userEmail = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(userEmail))
            {
                MessageBox.Show("Lütfen geçerli bir e-posta adresi girin.");
                return;
            }

            // Şifre sıfırlama kodu oluştur
            string resetCode = new Random().Next(100000, 999999).ToString();

            // E-posta gönder
            SendPasswordResetEmail(userEmail, resetCode);

            // Kod ve kullanıcı e-posta adresini geçici bir veri yapısına kaydet
            // Örneğin, formu geçici olarak gizleyip kod onaylama formuna yönlendirebilirsin
          
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
