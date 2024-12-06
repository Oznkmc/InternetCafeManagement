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
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }
        public int user_balance { get; set; }
        private void Order_Load(object sender, EventArgs e)
        {
            radioAtistirmalik.Visible = false;
            radioSicakYiyecek.Visible = false;
            radioSicakIcecek.Visible = false;
            radioSogukIcecek.Visible = false;
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;
            checkBox6.Visible = false;
            checkBox7.Visible = false;
            checkBox8.Visible = false;
            checkBox9.Visible = false;
            checkBox10.Visible = false;
            checkBox11.Visible = false;
            checkBox12.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioYemek.Checked)
            {
               
               
                radioAtistirmalik.Visible = true;
                radioSicakYiyecek.Visible= true;
                radioSicakIcecek.Visible = false;
                radioSogukIcecek.Visible = false;
            }
            
        }

        private void radioIcecek_CheckedChanged(object sender, EventArgs e)
        {
            if (radioIcecek.Checked)
            {
               
                radioSicakYiyecek.Visible = false;
                radioSicakIcecek.Visible = true;
                radioSogukIcecek.Visible = true;

            }
        }

        private void radioAtistirmalik_CheckedChanged(object sender, EventArgs e)
        {
          
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox1.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\ruffles.jpg");
            checkBox1.Text = "Ruffless Ketçaplı";

            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox2.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\canga.jpg");
            checkBox2.Text = "Eti Canga";

            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox3.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\eticikolatalıbisküvi.png");
            checkBox3.Text = "Eti Çikolatalı Bisküvi";


            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox4.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\etikombo.jpg");
            checkBox4.Text = "Eti Kombo";


            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox5.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\gonghardalli.jpg");
            checkBox5.Text = "Gong Hardallı";


            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox6.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\gongpeynir.jpg");
            checkBox6.Text = "Gong Peynir";


            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox7.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\rufflessade.jpg");
            checkBox7.Text = "Ruffless Sade";


            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox8.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\snickers.jpg");
            checkBox8.Text = "Snickers";


            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox9.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\ülkercikolata.jpg");
            checkBox9.Text = "Ülker Cikolata";


            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox10.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\etihosbes.jpg");
            checkBox10.Text = "Eti Hosbes";


            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox11.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\rufflespeynir.jpg");
            checkBox11.Text = "Ruffless Peynirli";


            pictureBox12.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox12.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik\browni.png");
            checkBox12.Text = "Browni Kek";
            checkBox1.Visible = true;
            checkBox2.Visible = true;
            checkBox3.Visible =true;
            checkBox4.Visible = true;
            checkBox5.Visible = true;
            checkBox6.Visible = true;
            checkBox7.Visible = true;
            checkBox8.Visible = true;
            checkBox9.Visible = true;
            checkBox10.Visible = true;
            checkBox11.Visible = true;
            checkBox12.Visible = true;

        }

        private void radioSicakYiyecek_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Visible = true;
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            checkBox4.Visible = true;
            checkBox5.Visible = true;
            checkBox6.Visible = true;
           
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox1.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik/kaşarlıtost.jpg");
            checkBox1.Text = "Kaşarlı Tost";
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox2.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik/sucuklutost.jpg");
            checkBox2.Text = "Sucuklu Tost";
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox3.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik/karisiktost.jpg");
            checkBox3.Text = "Karışık Tost";
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox4.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik/patso.png");
            checkBox4.Text = "Patso";
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox5.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik/sandvic.jpg");
            checkBox5.Text = "Sandvic";
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox6.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\atistirmalik/şinitzel.jpg");
            pictureBox7.Image = null;
            checkBox7.Visible = false;
            pictureBox8.Image = null;
            checkBox8.Visible = false;
            pictureBox9.Image = null;
            checkBox9.Visible = false;
            pictureBox10.Image = null;
            checkBox10.Visible = false;
            pictureBox11.Image = null;
           checkBox11.Visible = false;
            pictureBox12.Image = null;
            checkBox12.Visible = false;
            
        }

        private void radioSicakIcecek_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Visible = true;
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            checkBox4.Visible = true;
            checkBox5.Visible = true;
            checkBox6.Visible = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox1.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\Icecekler\cay.png");
            checkBox1.Text = "Çay";
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox2.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\Icecekler\ıhlamur.jpg");
            checkBox2.Text = "Ihlamur";
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox3.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\kiwi.jpg");
            checkBox3.Text = "Kiwi";
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox4.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\sahlep.png");
            checkBox4.Text = "Sahlep";
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox5.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\SıcakCikolata.jpg");
            checkBox5.Text = "Sıcak Çikolata";
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü sığdırır
            pictureBox6.Image = Image.FromFile(@"C:\Users\oznkm\OneDrive\Desktop\c# proje icin iconlar\Turk-Kahvesi.jpg");
            checkBox6.Text = "Turk-Kahvesi";
            pictureBox7.Image = null;
            checkBox7.Visible = false;
            pictureBox8.Image = null;
            checkBox8.Visible = false;
            pictureBox9.Image = null;
            checkBox9.Visible = false;
            pictureBox10.Image = null;
            checkBox10.Visible = false;
            pictureBox11.Image = null;
            pictureBox11.Visible = false;
            pictureBox12.Image = null;
            pictureBox12.Visible = false;


        }
    }
}
