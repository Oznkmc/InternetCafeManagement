using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InternetCafeManagement
{
    public partial class SifreYenileme : Form
    {
        public SifreYenileme()
        {
            InitializeComponent();
        }

        private void SifreYenileme_Load(object sender, EventArgs e)
        {
           
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;
            textBox3.Enter += textBox3_Enter;
            textBox3.Leave += textBox3_Leave;
            // Form açıldığında placeholder gösterelim
            ShowPlaceholder();
            ShowPlaceholder2();
        }
       
        
        private void ShowPlaceholder()
        {
            // Eğer TextBox boşsa placeholder göster
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = "Şifre giriniz";
                textBox2.ForeColor = Color.Gray; // Placeholder rengi
                textBox2.UseSystemPasswordChar = false; // Maskeyi kapat
            }
        }
        private void HidePlaceholder()
        {
            // Eğer placeholder görünüyorsa temizle
            if (textBox2.Text == "Şifre giriniz")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black; // Yazı rengini normal yap
                textBox2.UseSystemPasswordChar = true; // Maskeyi aç
            }
        }
        private void ShowPlaceholder2()
        {
            // Eğer TextBox boşsa placeholder göster
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                textBox3.Text = "Şifre Tekrar Giriniz";
                textBox3.ForeColor = Color.Gray; // Placeholder rengi
                textBox3.UseSystemPasswordChar = false; // Maskeyi kapat
            }
        }
        private void HidePlaceholder2()
        {
            // Eğer placeholder görünüyorsa temizle
            if (textBox3.Text == "Şifre Tekrar Giriniz")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black; // Yazı rengini normal yap
                textBox3.UseSystemPasswordChar = true; // Maskeyi aç
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           




        }







        private void textBox1_Click(object sender, EventArgs e)
        {
           
           
        }

      
       
        

        private void textBox1_Leave(object sender, EventArgs e)
        {
            


            }
        
        private void textBox2_Enter(object sender, EventArgs e)
        {

            HidePlaceholder();


        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            ShowPlaceholder();




        }

        private void textBox3_Leave(object sender, EventArgs e)
        {

            ShowPlaceholder2();

        }

       public int bosarttir = 0;
        private void textBox3_Enter(object sender, EventArgs e)
        {
            HidePlaceholder2();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            
            

        }
    }
}
