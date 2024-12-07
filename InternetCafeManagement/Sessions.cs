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
    public partial class Sessions : Form
    {
        public Sessions()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";

        public bool user_role { get; set; }
        public string user_mail { get; set; }
        public double user_balance { get; set; }
        public string secili_pc {  get; set; }
        int parsedOturumSuresi;
        private void Sessions_Load(object sender, EventArgs e)
        {
            string pcdurumbelirle;

            // Veritabanı bağlantısını açıyoruz
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 12 Label ve PictureBox için kontrol yapacağız
                for (int i = 1; i <= 12; i++)
                {
                    // Dinamik olarak Label ve PictureBox isimlerini oluşturuyoruz
                    string lblPCName = "lblPC" + i.ToString(); // lblPC1, lblPC2, ..., lblPC12
                    string picBoxName = "picturePC" + i.ToString(); // picBox1, picBox2, ..., picBox12

                    // Veritabanından bilgisayar durumu bilgilerini alıyoruz
                    SqlCommand pcdurum = new SqlCommand("SELECT status FROM computers WHERE name=@lblPC", connection);
                    pcdurum.Parameters.AddWithValue("@lblPC", Controls[lblPCName].Text); // Label'dan bilgisayar adı alıyoruz
                    object result = pcdurum.ExecuteScalar();

                    // Eğer sonuç varsa
                    if (result != null)
                    {
                        pcdurumbelirle = (string)result;

                        // Label'ı duruma göre renklendiriyoruz
                        var labelControl = Controls[lblPCName];
                        if (labelControl is Label label)
                        {
                            if (pcdurumbelirle == "available")
                            {
                                label.ForeColor = Color.Blue; // "available" ise mavi renk
                            }
                            else
                            {
                                label.ForeColor = Color.Red; // "unavailable" ise kırmızı renk
                            }
                        }

                        // PictureBox'ı duruma göre değiştiriyoruz
                        var pictureBoxControl = Controls[picBoxName];
                        if (pictureBoxControl is PictureBox pictureBox)
                        {
                            if (pcdurumbelirle == "unavailable")
                            {
                                pictureBox.Enabled = false; // Bilgisayar doluysa tıklanamaz hale getir
                                pictureBox.BackColor = Color.Gray; // Dolu bilgisayar için gri arka plan
                            }
                            else
                            {
                                pictureBox.Enabled = true; // Bilgisayar mevcutsa tıklanabilir
                                pictureBox.BackColor = Color.Green; // Mevcut bilgisayar için yeşil arka plan
                            }
                        }
                    }
                }
            }
        }
        private void picturePC1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Kullanıcı ID'sini almak için sorgu
                    SqlCommand idgetir = new SqlCommand("SELECT user_id FROM users WHERE email = @UserMail", connection);
                    idgetir.Parameters.AddWithValue("@UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();

                    if (IDresult == null)
                    {
                        MessageBox.Show("Kullanıcı ID'si bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int idtake = (int)IDresult;

                    // Hediye bilgilerini almak için sorgu
                    SqlCommand giftCommand = new SqlCommand("SELECT reward, gift_duration, used_time, is_claimed FROM gift_wheel WHERE user_id = @UserID", connection);
                    giftCommand.Parameters.AddWithValue("@UserID", idtake);

                    SqlDataReader reader = giftCommand.ExecuteReader();
                    if (!reader.Read())
                    {
                        MessageBox.Show("Hediye bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OpenCustomInputSession();
                        return;
                    }

                    string reward = reader["reward"]?.ToString();
                    int giftDuration = reader["gift_duration"] != DBNull.Value ? Convert.ToInt32(reader["gift_duration"]) : 0;
                    int usedTime = reader["used_time"] != DBNull.Value ? Convert.ToInt32(reader["used_time"]) : 0;
                    bool isClaimed = reader["is_claimed"] != DBNull.Value ? Convert.ToBoolean(reader["is_claimed"]) : false;
                    reader.Close();

                    // Eğer hediye zaten kullanıldıysa, CustomInputSession'ı aç
                    if (isClaimed)
                    {
                        MessageBox.Show("Hediyeniz zaten kullanıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OpenCustomInputSession();
                        return;
                    }

                    // Eğer hediye 1 veya 3 saat ücretsiz oturumsa
                    if (reward == "1 saat ücretsiz oturum" || reward == "3 saat ücretsiz oturum")
                    {
                        DialogResult result2 = MessageBox.Show($"Bir Hediyeniz Var ({reward}). Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result2 == DialogResult.Yes)
                        {
                            int remainingTime = giftDuration - usedTime; // Kalan süreyi hesapla
                            if (remainingTime <= 0)
                            {
                                MessageBox.Show("Hediye süresi tamamlanmış.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                OpenCustomInputSession();
                                return;
                            }

                            parsedOturumSuresi = remainingTime * 60; // Kalan süreyi saniyeye çevir

                            // Kullanıcı oturumu başlat
                            UsersSession usersSessionGift = new UsersSession
                            {
                                oturum_suresi = parsedOturumSuresi,
                                user_role = this.user_role,
                                user_mail = this.user_mail,
                                user_balance = this.user_balance,
                                secili_pc = "PC1", // Seçilen bilgisayar
                                hediyekullandi = true,
                            };

                            // Hediye kullanıldıktan sonra 'used_time' güncelleniyor
                            SqlCommand updateUsedTime = new SqlCommand("UPDATE gift_wheel SET used_time = ISNULL(used_time, 0) + @UsedTime, is_claimed = 1 WHERE user_id = @UserID", connection);
                            updateUsedTime.Parameters.AddWithValue("@UsedTime", remainingTime); // Kalan süre kadar kullanılan süreyi ekle
                            updateUsedTime.Parameters.AddWithValue("@UserID", idtake);
                            updateUsedTime.ExecuteNonQuery(); // Güncellemeyi yap

                            this.Hide();
                            usersSessionGift.Show();
                            return;
                        }
                    }

                    // Eğer hediye kabul edilmezse veya başka ödül varsa
                    OpenCustomInputSession();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // CustomInputSession açma işlemi
            void OpenCustomInputSession()
            {
                CustomInputSession customInputSession = new CustomInputSession
                {
                    user_role = this.user_role,
                    user_mail = this.user_mail,
                    user_balance = this.user_balance,
                    secili_pc = lblPC1.Text
                };

                customInputSession.Show();
                this.Hide();
            }




        }


        private void picturePC3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand1 = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand1.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand1.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc ="PC3"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                   
                                    usersSessionGift.Show();
                                    this.Hide();
                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC3.Text;
            customInputSession.Show();

            this.Hide();

        }

        private void picturePC4_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand2 = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand2.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand2.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc ="PC4"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();


                                }
                                return; // Hediye kullanıldığı için işlemi bitir
                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC4.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC5_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand3 = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand3.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand3.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc ="PC5"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC5.Text;
            customInputSession.Show();
            this.Hide();
        }

        private void picturePC6_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand4 = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand4.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand4.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = "PC6"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = "PC6"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC6.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC7_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand5 = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand5.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand5.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = "PC7"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC7.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC8_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc ="PC8"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC8.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC9_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc ="PC9"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC9.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC10_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc ="PC10"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC10.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC11_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = "PC11"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                 if(gift=="3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC11.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC12_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc ="PC12"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC12.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void picturePC2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand idgetir = new SqlCommand("select user_id from  users where email=@UserMail", connection);
                    idgetir.Parameters.AddWithValue("UserMail", user_mail);
                    object IDresult = idgetir.ExecuteScalar();
                    if (IDresult != null)
                    {
                        int idtake = (int)IDresult;

                        // Hediye sorgusu
                        SqlCommand giftCommand = new SqlCommand("SELECT reward FROM gift_wheel WHERE user_id = @UserID", connection);
                        giftCommand.Parameters.AddWithValue("@UserID", idtake); // Kullanıcı ID'si
                        object result = giftCommand.ExecuteScalar();

                        if (result != null)
                        {
                            DialogResult result2 = MessageBox.Show("Bir Hediyeniz Var. Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result2 == DialogResult.Yes)
                            {
                                string gift = (string)result;
                                if (gift == "1 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 60; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 1 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = "PC2"
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }
                                if (gift == "3 saat ücretsiz oturum")
                                {
                                    parsedOturumSuresi = 180; // 1 saat
                                    MessageBox.Show("Hediyeniz olan 3 saatlik oturum kullanılıyor!");

                                    // Oturum başlatma
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi * 60, // Saniye cinsinden
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = this.secili_pc
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();

                                    return; // Hediye kullanıldığı için işlemi bitir
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("Hediyeniz bulunmamaktadır.");
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata nedeni:" + ex.Message);
            }
            CustomInputSession customInputSession = new CustomInputSession();
            customInputSession.user_role = user_role;
            customInputSession.user_mail = user_mail;
            customInputSession.user_balance = user_balance;
            customInputSession.secili_pc = lblPC2.Text;
            customInputSession.Show();

            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result==DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ana Sayfaya Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result==DialogResult.Yes)
            {
                AnaSayfa anaSayfa = new AnaSayfa();
                anaSayfa.user_role = this.user_role;
                anaSayfa.user_balance = this.user_balance;
                anaSayfa.user_mail = this.user_mail;

                anaSayfa.Show();
                this.Hide();
            }
           
        }
    }
}
