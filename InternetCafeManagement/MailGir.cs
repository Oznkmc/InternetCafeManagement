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

namespace InternetCafeManagement
{
    public partial class MailGir : Form
    {
        public MailGir()
        {
            InitializeComponent();
        }
        public string user_mail {  get; set; }
        public  void SendPasswordResetEmail(string userEmail, string resetCode)
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
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj
               MessageBox.Show("E-posta gönderme hatası: " + ex.Message);
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
            this.Hide();
            DogrulamaKodu codeForm = new DogrulamaKodu(resetCode, userEmail);
            codeForm.userEmail = textBox1.Text;
            codeForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
