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
    public partial class DogrulamaKodu : Form
    {
        private string resetCode;  // Gönderdiğimiz şifre sıfırlama kodu
        public string userEmail;  // Kullanıcının e-posta adresi
        public DogrulamaKodu(string code, string email)
        {
            InitializeComponent();
            resetCode = code;
            userEmail = email;
        }
       

     
        private void DogrulamaKodu_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
        }
        private int secondsRemaining = 180;
        private void timer1_Tick(object sender, EventArgs e)
        {
            secondsRemaining--;
            if (secondsRemaining <= 0)
            {
                timer1.Stop();  // Timer'ı durdur
                MessageBox.Show("Zaman Doldu!");
            }
            else
            {
                // Kalan süreyi dakika:saniye formatında göster
                int minutes = secondsRemaining / 60;
                int seconds = secondsRemaining % 60;
                label2.Text = $"{minutes:D2}:{seconds:D2}";  // Zaman formatını "mm:ss" olarak güncelle
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string enteredCode = textBox1.Text.Trim();

            if (enteredCode == resetCode)
            {
                // Kod doğruysa, yeni şifre formunu göster
                this.Hide();
                NewPasswordForm newPasswordForm = new NewPasswordForm(userEmail);
                newPasswordForm.userEmail = userEmail;
                newPasswordForm.Show();
            }
            else
            {
                MessageBox.Show("Geçersiz kod. Lütfen tekrar deneyin.");
            }
        }
    }
}
