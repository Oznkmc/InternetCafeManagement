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
            
        }
        private int secondsRemaining = 180;
     

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

        private void label2_Click(object sender, EventArgs e)
        {

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
    }
}
