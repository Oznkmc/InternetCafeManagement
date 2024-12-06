using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
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
        public string secili_pc { get; set; }
        public int oturum_suresi { get; set; }
        private DateTime start_time { get; set; }
        private DateTime end_time { get; set; }
        public bool hediyekullandi { get; set; }
        public string hediyeadi { get; set; }

        public double sessionBalance;
        public int dakika;
        public int saniye = 0;
        public int saniye2 = 0;
        public bool dakikasifirla { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Order order = new Order();

            order.Show();

        }
        private void OturumuKapat(bool uygulamayiKapat)
        {
            timer1.Stop(); // Zamanlayıcıyı durdur

            try
            {
                string startTimeString = start_time.ToString("yyyy-MM-dd HH:mm:ss");
                string endTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Bilgisayar durumunu 'available' olarak güncelle
                    SqlCommand updateComputerStatus = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @SelectedPC", connection);
                    updateComputerStatus.Parameters.AddWithValue("@SelectedPC", secili_pc);
                    updateComputerStatus.ExecuteNonQuery();

                    // Kullanıcı ID'sini al
                    SqlCommand userIdCommand = new SqlCommand("SELECT user_id FROM users WHERE email = @UserMail", connection);
                    userIdCommand.Parameters.AddWithValue("@UserMail", user_mail);
                    object userIdResult = userIdCommand.ExecuteScalar();
                    int userId = userIdResult != DBNull.Value ? Convert.ToInt32(userIdResult) : 0;

                    // Bilgisayar ID'sini al
                    SqlCommand computerIdCommand = new SqlCommand("SELECT computer_id FROM computers WHERE name = @SelectedPC", connection);
                    computerIdCommand.Parameters.AddWithValue("@SelectedPC", secili_pc);
                    object computerIdResult = computerIdCommand.ExecuteScalar();
                    int computerId = computerIdResult != DBNull.Value ? Convert.ToInt32(computerIdResult) : 0;

                    // Oturum bilgilerini kaydet
                    SqlCommand saveSession = new SqlCommand(
                        "INSERT INTO sessions (user_id, computer_id, total_price, start_time, end_time) " +
                        "VALUES (@UserId, @ComputerId, @TotalPrice, @StartTime, @EndTime)", connection);
                    saveSession.Parameters.AddWithValue("@UserId", userId);
                    saveSession.Parameters.AddWithValue("@ComputerId", computerId);
                    saveSession.Parameters.AddWithValue("@TotalPrice", sessionBalance);
                    saveSession.Parameters.AddWithValue("@StartTime", startTimeString);
                    saveSession.Parameters.AddWithValue("@EndTime", endTimeString);
                    saveSession.ExecuteNonQuery();

                    // Kullanıcı bakiyesini güncelle
                    SqlCommand updateBalance = new SqlCommand("UPDATE users SET balance = @UserBalance WHERE email = @UserMail", connection);
                    updateBalance.Parameters.AddWithValue("@UserBalance", user_balance);
                    updateBalance.Parameters.AddWithValue("@UserMail", user_mail);
                    updateBalance.ExecuteNonQuery();
                }

                // Kullanıcıya bilgi mesajı
                MessageBox.Show("Oturum başarıyla kapatıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Uygulamayı kapat veya anasayfaya dön
                if (uygulamayiKapat)
                {
                    Application.Exit();
                }
                else
                {
                    AnaSayfa anaSayfa = new AnaSayfa
                    {
                        user_role = user_role,
                        user_balance = user_balance,
                        user_mail = user_mail,
                    };
                    anaSayfa.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anasayfaya dönmek oturumunuzu sonlandıracaktır. Emin misiniz?", "Anasayfaya Dönüş", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                OturumuKapat(false); // Anasayfaya dön seçeneği ile oturumu kapat
            }





        }




        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan çıkmak istediğinizden emin misiniz?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                OturumuKapat(true); // Uygulamayı kapat seçeneği ile oturumu kapat
            }



        }
        private int user_id;
        private void UsersSession_Load(object sender, EventArgs e)
        {

            label1.Text += " " + user_mail;
            label2.Text += " " + secili_pc;
            label5.Text = "Dakika: 0 Saniye:0";
            lblSessionCount.Text += sessionBalance;
            // Kullanıcının kalan gift_duration'ını al
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kullanıcı ID'sini al
                SqlCommand userIdCommand = new SqlCommand("SELECT user_id FROM users WHERE email = @UserMail", connection);
                userIdCommand.Parameters.AddWithValue("@UserMail", user_mail);
                object userIdResult = userIdCommand.ExecuteScalar();
                userid = userIdResult != DBNull.Value ? Convert.ToInt32(userIdResult) : 0;

                // gift_duration'ı al
                SqlCommand giftDurationCommand = new SqlCommand("SELECT gift_duration FROM gift_wheel WHERE user_id = @UserId", connection);
                giftDurationCommand.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                object giftDurationResult = giftDurationCommand.ExecuteScalar();
                double giftDuration = giftDurationResult != DBNull.Value ? Convert.ToDouble(giftDurationResult) : 0;

                // Kalan süreyi hesapla
                label4.Text = "Kalan Hediye Süresi: " + giftDuration.ToString() + " dakika"; // Burada kalan süreyi ekrana yazdırabilirsiniz
            }


            timer1.Start();
            start_time = DateTime.Now;

        }
        private int userid;
        private int computerid;
        public int lasttime;
        int saat = 0;
        private int computeridtake;
        private void timer1_Tick(object sender, EventArgs e)
        {
            saniye++;  // Her tikte saniyeyi arttırıyoruz.
            lasttime = oturum_suresi - saniye;
            label4.Text = "Kalan Süre:" + lasttime.ToString();
            saniye2++;

            // Oturum süresi (dakika olarak) dışarıdan alındığı için saniyeye çevrilmiş olmalı.
            if (saniye <= oturum_suresi) // Oturum süresi saniye cinsinden kontrol ediliyor.
            {
                user_balance -= sessionBalance;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (hediyekullandi == false)
                    {
                        connection.Open();
                        SqlCommand balanceguncelle = new SqlCommand("Update users SET balance=@UserBalance where email=@UserMail", connection);
                        balanceguncelle.Parameters.AddWithValue("@UserBalance", user_balance);
                        balanceguncelle.Parameters.AddWithValue("@UserMail", user_mail);
                        balanceguncelle.ExecuteNonQuery();
                        connection.Close();
                    }

                    label5.Text = "Dakika: " + dakika.ToString() + " Saniye: " + saniye2.ToString();

                    if (saniye % 60 == 0)
                    {
                        dakika++;  // Dakikayı arttır
                        sessionBalance += (dakika * 0.25f);
                        lblSessionCount.Text = "Oturum Süresi: " + sessionBalance.ToString();
                        saniye2 = 0;
                    }

                    if (dakika == 60)
                    {
                        saat++;  // Saati arttır
                        if (dakikasifirla == true)
                        {
                            dakika = 0;  // Dakikayı sıfırla
                        }
                        saniye2 = 0;
                        label5.Text = "Saat: " + saat.ToString() + " Dakika: " + dakika.ToString() + " Saniye: " + saniye2.ToString();
                    }
                }
            }
            else
            {
                // Oturum süresi bitmişse işlemi sonlandır
                timer1.Stop();
                MessageBox.Show("Oturum Süreniz Tükenmiştir. Ana Sayfaya aktarılıyorsunuz...");

                try
                {
                    string startTimeString = start_time.ToString("yyyy-MM-dd HH:mm:ss");
                    string endTimeString = end_time.ToString("yyyy-MM-dd HH:mm:ss");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Kullanıcının ödülünü güncelle (hediye hakkı bittiyse)
                        if (hediyekullandi == false)
                        {
                            SqlCommand giftUpdate = new SqlCommand(
                                "UPDATE gift_wheel SET is_claimed = 1 WHERE user_id = @UserId AND is_claimed = 0", connection);
                            giftUpdate.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                            giftUpdate.ExecuteNonQuery();
                        }

                        // Kullanıcı bakiyesini güncelle
                        if (hediyekullandi == false)
                        {
                            SqlCommand balanceupdate = new SqlCommand("UPDATE users SET balance=balance-@SessionBalance WHERE email=@SessionMail", connection);
                            balanceupdate.Parameters.AddWithValue("@SessionBalance", sessionBalance);
                            balanceupdate.Parameters.AddWithValue("@SessionMail", user_mail);
                            balanceupdate.ExecuteNonQuery();
                        }

                        // Bilgisayarın durumunu 'available' olarak güncelle
                        SqlCommand command = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @secilipc", connection);
                        command.Parameters.AddWithValue("@secilipc", secili_pc);
                        int rowsAffected = command.ExecuteNonQuery();

                        // Oturum bilgilerini kaydet
                        SqlCommand addSessions = new SqlCommand(
                            "INSERT INTO sessions (user_id, computer_id, total_price, start_time, end_time) " +
                            "VALUES (@UserId, @ComputerId, @TotalPrice, @StartTime, @EndTime)", connection);
                        addSessions.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                        SqlCommand computerid = new SqlCommand("select computer_id from computers where name=@ComputerName", connection);
                        computerid.Parameters.AddWithValue("@ComputerName", secili_pc);
                        object ComputerResult = computerid.ExecuteScalar();
                        if (ComputerResult != null)
                        {
                            computeridtake = (int)ComputerResult;
                        }
                        addSessions.Parameters.AddWithValue("@ComputerId", computeridtake);
                        addSessions.Parameters.AddWithValue("@TotalPrice", sessionBalance);
                        addSessions.Parameters.AddWithValue("@StartTime", startTimeString);
                        addSessions.Parameters.AddWithValue("@EndTime", endTimeString);

                        addSessions.ExecuteNonQuery();

                        MessageBox.Show("Oturum Süresi: " + dakika.ToString() + "\n Seçili Bilgisayar: " + secili_pc + "\n Toplam Tutar: " + sessionBalance.ToString());

                        if (rowsAffected < 0)
                        {
                            MessageBox.Show("Bilgisayar bulunamadı veya durum zaten güncel.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    // Ana sayfaya geçiş
                    AnaSayfa anaSayfa = new AnaSayfa
                    {
                        user_role = user_role,
                        user_balance = user_balance,
                        user_mail = user_mail,
                    };
                    anaSayfa.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int GetUserId(string email)
        {
            int userId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT user_id FROM users WHERE email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["user_id"]);
                }
            }
            return userId;
        }

        private int GetComputerId(string computerName)
        {
            int computerId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id FROM computers WHERE name = @ComputerName", connection);
                command.Parameters.AddWithValue("@ComputerName", computerName);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    computerId = Convert.ToInt32(reader["id"]);
                }
            }
            return computerId;
        }




        private void pictureBox5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            MessageBox.Show((lasttime / 60).ToString());
            AddTime add = new AddTime()
            {
                user_balance = user_balance,
                user_mail = user_mail,
                user_role = user_role,
                oturum_suresi = lasttime,
                session_balance = this.sessionBalance,
            };

            add.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Oturumu kapatmak istediğinizden emin misiniz?", "Oturumu Kapat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                timer1.Stop();

                try
                {
                    string startTimeString = start_time.ToString("yyyy-MM-dd HH:mm:ss");
                    string endTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Bilgisayar durumunu güncelle
                        SqlCommand updateComputerStatus = new SqlCommand("UPDATE computers SET status = 'available' WHERE name = @SelectedPC", connection);
                        updateComputerStatus.Parameters.AddWithValue("@SelectedPC", secili_pc);
                        updateComputerStatus.ExecuteNonQuery();

                        // Kullanıcı ID'sini al
                        SqlCommand userIdCommand = new SqlCommand("SELECT user_id FROM users WHERE email = @UserMail", connection);
                        userIdCommand.Parameters.AddWithValue("@UserMail", user_mail);
                        object userIdResult = userIdCommand.ExecuteScalar();
                        int userId = userIdResult != DBNull.Value ? Convert.ToInt32(userIdResult) : 0;

                        // Bilgisayar ID'sini al
                        SqlCommand computerIdCommand = new SqlCommand("SELECT computer_id FROM computers WHERE name = @SelectedPC", connection);
                        computerIdCommand.Parameters.AddWithValue("@SelectedPC", secili_pc);
                        object computerIdResult = computerIdCommand.ExecuteScalar();
                        int computerId = computerIdResult != DBNull.Value ? Convert.ToInt32(computerIdResult) : 0;

                        // Oturum bilgilerini kaydet
                        SqlCommand saveSession = new SqlCommand(
                            "INSERT INTO sessions (user_id, computer_id, total_price, start_time, end_time) " +
                            "VALUES (@UserId, @ComputerId, @TotalPrice, @StartTime, @EndTime)", connection);
                        saveSession.Parameters.AddWithValue("@UserId", userId);
                        saveSession.Parameters.AddWithValue("@ComputerId", computerId);
                        saveSession.Parameters.AddWithValue("@TotalPrice", sessionBalance);
                        saveSession.Parameters.AddWithValue("@StartTime", startTimeString);
                        saveSession.Parameters.AddWithValue("@EndTime", endTimeString);
                        saveSession.ExecuteNonQuery();

                        // Kullanıcı bakiyesini güncelle
                        SqlCommand updateBalance = new SqlCommand("UPDATE users SET balance = @UserBalance WHERE email = @UserMail", connection);
                        updateBalance.Parameters.AddWithValue("@UserBalance", user_balance);
                        updateBalance.Parameters.AddWithValue("@UserMail", user_mail);
                        updateBalance.ExecuteNonQuery();

                        MessageBox.Show("Oturum başarıyla kapatıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Ana sayfaya dön
                    AnaSayfa anaSayfa = new AnaSayfa
                    {
                        user_role = user_role,
                        user_balance = user_balance,
                        user_mail = user_mail,
                    };
                    anaSayfa.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}