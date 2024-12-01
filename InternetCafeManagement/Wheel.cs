using System;
using System.Collections;
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
    public partial class Wheel : Form
    {
        public Wheel()
        {
            InitializeComponent();
        }
        private int counter = 0;
        public string usermail {  get; set; }
        public double user_balance { get; set; }
        public string user_rol {  get; set; }
        private void Wheel_Load(object sender, EventArgs e)
        {
            

        }
       private Random rnd = new Random();
       private int userid;
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;

            // Rastgele bir hediye seç ve textBox1'e yazdır
            int rastgelesayi = rnd.Next(0, hediyeler.Count);
            textBox1.Text = hediyeler[rastgelesayi].ToString();

            // Eğer counter 10'a ulaştıysa, timer'ı durdur ve kazandığınız hediyeyi göster
            if(user_balance>199)
            {
                if (counter == 10)
                {
                    timer1.Stop();

                    // Kullanıcıyı ve hediyesini veritabanına kaydetme işlemi
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            // Bağlantıyı açıyoruz
                            connection.Open();

                            // Kullanıcının user_id bilgisini alıyoruz
                            SqlCommand cmd = new SqlCommand("select user_id from users where email=@UserMail", connection);
                            cmd.Parameters.AddWithValue("@UserMail", usermail); // Kullanıcı mailine göre user_id alıyoruz.
                            object result = cmd.ExecuteScalar(); // Kullanıcıyı veritabanından sorguluyoruz.

                            // Kullanıcı bulunduysa
                            if (result != null)
                            {
                                userid = (int)result;

                                // Hediyeyi gift_wheel tablosuna ekliyoruz
                                SqlCommand cmd2 = new SqlCommand("insert into gift_wheel(user_id, reward) values(@UserId, @Reward)", connection);
                                cmd2.Parameters.AddWithValue("@UserId", userid);
                                cmd2.Parameters.AddWithValue("@Reward", textBox1.Text); // Kazanılan hediyeyi ekliyoruz
                                cmd2.ExecuteNonQuery(); // Veriyi kaydediyoruz.
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı bulunamadı.");
                            }

                            // İşlem başarılı olduğunda kullanıcıya kazandığı hediyeyi gösteriyoruz
                            MessageBox.Show("Kazandığınız Hediye: " + textBox1.Text);
                            user_balance -= 200;
                            SqlCommand balance_update = new SqlCommand("Update users SET balance=@UserBalance where user_id=@Userid", connection);
                            balance_update.Parameters.AddWithValue("@UserBalance", user_balance);
                            balance_update.Parameters.AddWithValue("Userid", userid);
                            balance_update.ExecuteNonQuery();


                        }
                        catch (Exception exception)
                        {
                            // Hata mesajını ekrana yazdırıyoruz
                            MessageBox.Show("Hata nedeni: " + exception.Message);
                        }
                        finally
                        {
                            // Bağlantıyı kapatıyoruz
                            connection.Close();
                        }
                    }

                    // TextBox'u temizliyoruz
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Bakiyenizde en az 200 lira bulunmalıdır.");
            }
           

        }
        private   ArrayList hediyeler = new ArrayList();
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            hediyeler.Clear(); // Eğer sürekli ekleme yapılıyorsa listeyi temizleyin
            hediyeler.Add("1 saat ücretsiz oturum");
            hediyeler.Add("3 saat ücretsiz oturum");
            hediyeler.Add("1 ay ücretsiz oturum");
            hediyeler.Add("Oyuncu Klavyesi");
            hediyeler.Add("Bluetooth hoparlörler");
            hediyeler.Add("Oyuncu kulaklığı");
            hediyeler.Add("Oyuncu Mouse");
            hediyeler.Add("Mousepad");
            hediyeler.Add("USB bellek");
            hediyeler.Add("Ülker çikolatalı gofret");
            hediyeler.Add("200 liralık içecek hakkı");
            hediyeler.Add("200 liralık atıştırmalık hakkı");
            hediyeler.Add("Beypazarı Soda");
            hediyeler.Add("Kutu Kola");
            hediyeler.Add("1000 Valorant Point");
            hediyeler.Add("130 Valorant Point");
            hediyeler.Add("475 Valorant Point");
            hediyeler.Add("2050 Valorant Point");
            hediyeler.Add("575 Riot Points");
            hediyeler.Add("1380 Riot Points");
            hediyeler.Add("2800 Riot Points");

            if (!timer1.Enabled)
            {
                counter = 0; // Timer tekrar başlatıldığında sayaç sıfırlansın
                if(user_balance>199)
                {
                  timer1.Start(); // Timer başlat
                }
                else
                {
                    MessageBox.Show("Bakiyenizde en az 200 lira bulunmalıdır.\n Sizin Bakiyeniz:"+user_balance);
                }
            }
            else
            {
               
                timer1.Stop(); // Timer durdur
                counter = 0; // Sayaç sıfırlansın
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {


            AnaSayfa ana = new AnaSayfa();
            ana.user_mail = usermail;
            ana.user_balance = user_balance;
            ana.user_role = user_rol;
            ana.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
    }
    }

