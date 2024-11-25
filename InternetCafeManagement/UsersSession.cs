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
    public partial class UsersSession : Form
    {
        public UsersSession()
        {
            InitializeComponent();
        }
        public string user_role { get; set; }
        public string user_mail { get; set; }
        public double user_balance { get; set; }
        public string secili_pc {  get; set; }
        public int oturum_suresi {  get; set; }
        public double sessionBalance = 0;
        public int dakika = 0;
        public int saniye = 0;
        public int saniye2 = 0;
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Order order= new Order();

            order.Show();

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eğer anasayfaya dönmek istersen oturumun sıfırlanacaktır.\n Anasayfaya geçmeye emin misin?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(result==DialogResult.Yes)
            {
                
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                    try
                    {
                         connection.Open();
                        SqlCommand command = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @secilipc", connection);
                        command.Parameters.AddWithValue("@secilipc", secili_pc);
                        command.ExecuteNonQuery();
                        timer1.Stop();
                        AnaSayfa ana = new AnaSayfa();
                        ana.user_mail = user_mail;
                        ana.user_balance = user_balance;
                        ana.user_role = user_role;
                        ana.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata nedeni:" + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
                
               
            
                    

            }

           
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Eğer anasayfaya dönmek istersen oturumun sıfırlanacaktır.\n Anasayfaya geçmeye emin misin?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @secilipc", connection);
                        command.Parameters.AddWithValue("@secilipc", secili_pc);
                        command.ExecuteNonQuery();
                        timer1.Stop();
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata nedeni:" + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }

                }





            }
            

        }
       
        private void UsersSession_Load(object sender, EventArgs e)
        {
            label1.Text += " "+user_mail;
            label2.Text +=" "+ secili_pc;
            label5.Text = "Dakika: 0 Saniye:0 ";
            lblSessionCount.Text = "0";
            oturum_suresi = oturum_suresi * 60;
            timer1.Start();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısını açma
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL sorgusunda parametreyi düzgün kullanma
                    SqlCommand command = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @secilipc", connection);
                    command.Parameters.AddWithValue("@secilipc", secili_pc);
                    int rowsAffected = command.ExecuteNonQuery(); // Etkilenen satır sayısını kontrol edebilirsiniz
                    
                   

                    if (rowsAffected > 0)
                    {
                        timer1.Stop();
                        MessageBox.Show("Bilgisayar durumu başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        user_balance -= sessionBalance;
                        
                        SqlCommand balanceupdate = new SqlCommand("update users SET balance=balance-@SessionBalance where email=@SessionMail", connection);
                        balanceupdate.Parameters.AddWithValue("@SessionBalance", sessionBalance);
                        balanceupdate.Parameters.AddWithValue("@SessionMail", user_mail);
                        balanceupdate.ExecuteNonQuery();
                        
                        MessageBox.Show("Oturum Süresi:" + dakika.ToString() + "\n Seçili Bilgisayar:" + secili_pc + "\n Toplam Tutar:" + sessionBalance.ToString());
                        

                    }
                    else
                    {
                        MessageBox.Show("Bilgisayar bulunamadı veya durum zaten güncel.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    connection.Close();
                }

                // Ana sayfa formuna geçiş
                AnaSayfa anaSayfa = new AnaSayfa
                {
                    user_balance = this.user_balance,
                    user_mail = this.user_mail,
                    user_role = this.user_role
                };
                
                anaSayfa.Show();

                this.Hide(); // Mevcut formu gizle
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
      
        private void timer1_Tick(object sender, EventArgs e)
        {
            saniye++;
            saniye2++;

           
             
            

            if (saniye <= oturum_suresi)
            {
               
                label5.Text = "Dakika: " + dakika.ToString() + " Saniye: " + saniye2.ToString();
                if (saniye % 60 == 0)
                {
                    dakika++;
                    saniye2 = 0;
                     
                    sessionBalance = dakika * 0.25f;
                    lblSessionCount.Text += sessionBalance.ToString();
                }

                lblSessionCount.Text = (dakika * 0.25f).ToString();
                
            }
                 

            else
            {
                timer1.Stop();
                MessageBox.Show("Oturum Süreniz Tükenmiştir.Ana Sayfaya aktarılıyorsunuz...");
                try
                {
                    // Veritabanı bağlantısını açma
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // SQL sorgusunda parametreyi düzgün kullanma
                        SqlCommand command = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @secilipc", connection);
                        command.Parameters.AddWithValue("@secilipc", secili_pc);
                        int rowsAffected = command.ExecuteNonQuery(); // Etkilenen satır sayısını kontrol edebilirsiniz
                        SqlCommand balanceupdate = new SqlCommand("update users SET balance=balance-@SessionBalance where email=@SessionMail", connection);
                        balanceupdate.Parameters.AddWithValue("@SessionBalance", sessionBalance);
                        balanceupdate.Parameters.AddWithValue("@SessionMail", user_mail);
                        user_balance -= sessionBalance;
                        MessageBox.Show("Oturum Süresi:" + dakika.ToString() + "\n Seçili Bilgisayar:" + secili_pc + "\n Toplam Tutar:" + sessionBalance.ToString());
                        
                        balanceupdate.ExecuteNonQuery();
                        if (rowsAffected < 0)
                        {
                            MessageBox.Show("Bilgisayar bulunamadı veya durum zaten güncel.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }

                    // Ana sayfa formuna geçiş


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                AnaSayfa anaSayfa = new AnaSayfa
                {
                    user_role=user_role,
                    user_balance = user_balance,
                    user_mail = user_mail,
                        

                };
               anaSayfa.Show();
                this.Hide();



               
               
            }
            
        }

        
    }
}
