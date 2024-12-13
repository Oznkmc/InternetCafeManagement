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
        public bool user_role { get; set; }
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
        public static Timer timer2;
        public bool dakikasifirla { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            order.secilipc = secili_pc;
            order.user_mail = user_mail;
            order.user_balance = this.user_balance;
           
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
                    //SqlCommand saveSession = new SqlCommand(
                    //    "INSERT INTO sessions (user_id, computer_id, total_price, start_time, end_time) " +
                    //    "VALUES (@UserId, @ComputerId, @TotalPrice, @StartTime, @EndTime)", connection);
                    //saveSession.Parameters.AddWithValue("@UserId", userId);
                    //saveSession.Parameters.AddWithValue("@ComputerId", computerId);
                    //saveSession.Parameters.AddWithValue("@TotalPrice", sessionBalance);
                    //saveSession.Parameters.AddWithValue("@StartTime", startTimeString);
                    //saveSession.Parameters.AddWithValue("@EndTime", endTimeString);
                    //saveSession.ExecuteNonQuery();

                    SqlCommand SessionID = new SqlCommand("select session_id from sessions where user_id=@GetUserID  and status=1", connection);
                    SessionID.Parameters.AddWithValue("@GetUserID", GetUserId(user_mail));
                    object result7 = SessionID.ExecuteScalar();
                    if (result7 != null)
                    {
                        int SessionIDgetir = (int)result7;
                        SqlCommand UpdateSession = new SqlCommand("Update sessions set total_price=@TotalPrice,end_time=@EndDate where sessionid=@SessionID", connection);
                        UpdateSession.Parameters.AddWithValue("@TotalPrice", totalprice);
                        UpdateSession.Parameters.AddWithValue("@EndDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        UpdateSession.Parameters.AddWithValue("@SessionID", SessionIDgetir);
                        int row = UpdateSession.ExecuteNonQuery();
                        if (row != 0)
                        {
                            MessageBox.Show("Oturum Başarıyla Güncellenmiştir.Bir Daha Bekleriz.");
                        }
                    }

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
            if(hediyekullandi)
            {
                pictureBox5.Visible= false;
            }
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
            saniye++;  // Zamanı bir artırıyoruz
            lasttime = oturum_suresi - saniye;  // Kalan süreyi hesaplıyoruz
            label4.Text = "Kalan Süre: " + lasttime.ToString() + " saniye";

            if (saniye % 60 == 0)  // Her dakika başında
            {
                dakika++;
                sessionBalance += 0.25f;  // Dakika başına ücret ekleniyor
            }

            // 60 dakikada bir saat hesaplanır
            if (dakika == 60)
            {
                saat++;
                dakika = 0;  // Dakikalar sıfırlanır
            }

            // Etiketleri günceller
            label5.Text = $"Saat: {saat} Dakika: {dakika} Saniye: {saniye}";
            lblSessionCount.Text = $"Oturum Süresi Ücreti: {sessionBalance:0.00} TL";

            // Hediye durumu kontrolü
            if (hediyekullandi)
            {
                // Hediye kullanılıyorsa sadece zaman işlemleri yapılır
                UpdateUsedGiftTime(1);  // Hediye zamanı 1 saniye artar
            }
            else
            {
                // Hediye kullanılmıyorsa oturum ücretlendirmesi yapılır
                UpdateLabels();
            }

            if (saniye >= oturum_suresi)  // Süre bittiğinde
            {
                timer1.Stop();  // Zamanlayıcıyı durdur
                MessageBox.Show("Oturum süreniz sona ermiştir. İşlemler tamamlanıyor...");
                FinalizeSession();  // Oturumu sonlandır
            }
        }


        private void UpdateLabels()
        {
            // Dakika ve saniye hesaplamalarını güncelle
            if (saniye % 60 == 0)
            {
                dakika++;
                sessionBalance += (dakika * 0.25f); // Dakika başına ücretlendirme
                saniye2 = 0; // Ekran için saniye sıfırlanır
            }

            if (dakika == 60)
            {
                saat++;
                dakika = 0; // Saat tamamlanırsa dakika sıfırlanır
            }

            // Etiketleri güncelle
            label5.Text = $"Saat: {saat} Dakika: {dakika} Saniye: {saniye2}";
            lblSessionCount.Text = $"Oturum Süresi Ücreti: {sessionBalance:0.00} TL";
        }

        private void UpdateUsedGiftTime(int elapsedSeconds)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand updateGiftTime = new SqlCommand(
                        "UPDATE gift_wheel SET used_time = used_time + @ElapsedSeconds WHERE user_id = @UserId AND is_claimed = 1",
                        connection);
                    updateGiftTime.Parameters.AddWithValue("@ElapsedSeconds", elapsedSeconds);
                    updateGiftTime.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                    updateGiftTime.ExecuteNonQuery();  // Sadece 1 saniye güncelleniyor
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hediye zamanı güncellenirken bir hata oluştu: " + ex.Message);
            }
        }

        private void FinalizeSession()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Hediye durumu kontrolü ve güncellenmesi
                    if (!hediyekullandi)
                    {
                        SqlCommand markGiftClaimed = new SqlCommand(
                            "UPDATE gift_wheel SET is_claimed = 1 WHERE user_id = @UserId AND is_claimed = 0", connection);
                        markGiftClaimed.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                        markGiftClaimed.ExecuteNonQuery();
                    }

                    // Bilgisayar durumunu güncelle
                    SqlCommand updateComputerStatus = new SqlCommand(
                        "UPDATE computers SET status = 'available' WHERE name = @ComputerName", connection);
                    updateComputerStatus.Parameters.AddWithValue("@ComputerName", secili_pc);
                    updateComputerStatus.ExecuteNonQuery();

                    // Oturum güncellemesi
                    SqlCommand getSessionId = new SqlCommand(
                        "SELECT session_id FROM sessions WHERE user_id = @UserId AND status = 1", connection);
                    getSessionId.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                    object sessionIdResult = getSessionId.ExecuteScalar();

                    if (sessionIdResult != null)
                    {
                        int sessionId = (int)sessionIdResult;

                        SqlCommand updateSession = new SqlCommand(
                            "UPDATE sessions SET total_price = @TotalPrice, end_time = @EndDate, status = 0 WHERE session_id = @SessionID",
                            connection);
                        updateSession.Parameters.AddWithValue("@TotalPrice", totalprice); // Oturum toplam ücreti
                        updateSession.Parameters.AddWithValue("@EndDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        updateSession.Parameters.AddWithValue("@SessionID", sessionId);

                        int rowsAffected = updateSession.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Oturum başarıyla güncellenmiştir. Bir daha bekleriz.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private int GetUserId(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT user_id FROM users WHERE email = @Email", connection);
                    command.Parameters.AddWithValue("@Email", email);

                    object userId = command.ExecuteScalar();
                    return userId != null ? Convert.ToInt32(userId) : 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcı ID alınırken bir hata oluştu: " + ex.Message);
                return 0;
            }
        }

        private int GetComputerId(string computerName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT computer_id FROM computers WHERE name = @ComputerName", connection);
                    command.Parameters.AddWithValue("@ComputerName", computerName);

                    object computerId = command.ExecuteScalar();
                    return computerId != null ? Convert.ToInt32(computerId) : 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bilgisayar ID alınırken bir hata oluştu: " + ex.Message);
                return 0;
            }
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
        public decimal totalprice;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // Oturum süresi dolmadan kapatma kontrolü
            //    if (timer1.Enabled)
            //    {
            //        timer1.Stop(); // Zamanlayıcıyı durdur
            //    }

            //    // FinalizeSession metodu ile oturum işlemlerini tamamla
            //    FinalizeSession();

            //    MessageBox.Show("Oturum başarıyla kapatıldı.", "Oturum Kapatıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    // Gerekirse başka işlemler yap
            //    // Örneğin, kullanıcıyı ana ekrana yönlendirme
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Oturum kapatılırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            int usedTime = saniye; // Kullanılan süre (saniye cinsinden)
            int remainingTime = 0; // Kalan süre
            int giftDuration = 0;
            int previouslyUsedTime = 0;
            bool isGiftClaimed = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Kullanıcının hediye bilgilerini al
                    SqlCommand giftQuery = new SqlCommand(
                        "SELECT gift_duration, used_time, is_claimed FROM gift_wheel WHERE user_id = @UserID AND is_claimed = 0", connection);
                    giftQuery.Parameters.AddWithValue("@UserID", GetUserId(user_mail));

                    SqlDataReader reader = giftQuery.ExecuteReader();
                    if (reader.Read())
                    {
                        giftDuration = reader["gift_duration"] != DBNull.Value ? Convert.ToInt32(reader["gift_duration"]) : 0;
                        previouslyUsedTime = reader["used_time"] != DBNull.Value ? Convert.ToInt32(reader["used_time"]) : 0;
                        isGiftClaimed = reader["is_claimed"] != DBNull.Value ? Convert.ToBoolean(reader["is_claimed"]) : false;
                        reader.Close();

                        // Eğer hediye kullanıldıysa balance işlemi yapılacak, hediye süresi geçerli değil
                        if (isGiftClaimed)
                        {
                            MessageBox.Show("Hediyeniz zaten kullanıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Kalan süreyi hesaplayalım
                            int remainingGiftTime = giftDuration - previouslyUsedTime; // Kalan süre (hediye süresi - kullanılan süre)

                            if (remainingGiftTime > 0)
                            {
                                // Hediye süresi hala geçerli ve kullanılabilir durumda
                                int newUsedTime = previouslyUsedTime + (usedTime / 60); // Kullanılan süreyi dakikaya çevirerek ekliyoruz

                                if (newUsedTime <= giftDuration)
                                {
                                    // Güncellenmiş used_time'ı kaydediyoruz
                                    SqlCommand updateGift = new SqlCommand(
                                        "UPDATE gift_wheel SET used_time = @UsedTime WHERE user_id = @UserID AND is_claimed = 0", connection);
                                    updateGift.Parameters.AddWithValue("@UsedTime", newUsedTime); // Yeni used_time değerini güncelle
                                    updateGift.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                                    updateGift.ExecuteNonQuery();
                                }
                                else
                                {
                                    MessageBox.Show("Hediye süreniz tamamlanmıştır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                // Hediye süresi bitmişse, 'is_claimed' durumunu güncelle
                                SqlCommand claimGift = new SqlCommand(
                                    "UPDATE gift_wheel SET is_claimed = 1 WHERE user_id = @UserID", connection);
                                claimGift.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                                claimGift.ExecuteNonQuery();

                                MessageBox.Show("Hediye süreniz tamamlanmıştır. Yeni bir oturum açabilirsiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        reader.Close();
                        // Eğer hediye yoksa, balance güncellenecek

                        SqlCommand OrderPrice = new SqlCommand(
     "SELECT TOP 1 o.TotalAmount AS TotalAmount " +
     "FROM Orders o " +
     "INNER JOIN sessions s ON o.SessionID = s.session_id " +
     "INNER JOIN users u ON o.CustomerID = u.user_id " +
     "WHERE u.user_id = @UserID " +
     "AND s.session_id = @session_id " +
     "AND s.start_time = (SELECT MAX(start_time) FROM sessions WHERE session_id = o.SessionID) " +
     "ORDER BY s.start_time DESC;",
     connection
 );
                        SqlCommand sessionidgetir = new SqlCommand("select session_id from sessions where status=1 and computer_id=@ComputerID", connection);
                        sessionidgetir.Parameters.AddWithValue("@ComputerID", GetComputerId(secili_pc));
                        object resultSessionID= sessionidgetir.ExecuteScalar();
                        if (resultSessionID != null)
                        {
                            int sessionid1 = (int)resultSessionID;
                            OrderPrice.Parameters.AddWithValue("@session_id", sessionid1);
                        }
                        OrderPrice.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                        object result8 = OrderPrice.ExecuteScalar();
                        if (result8 != null)
                        {
                            decimal totalamountgetir=(decimal)result8;

                            SqlCommand balanceUpdate = new SqlCommand("UPDATE users SET balance = balance - @SessionBalance WHERE email = @UserMail", connection);

                            balanceUpdate.Parameters.AddWithValue("@SessionBalance", Convert.ToDecimal(sessionBalance)+totalamountgetir); // Kalan süreyi dakika cinsinden balance'dan düş

                            balanceUpdate.Parameters.AddWithValue("@UserMail", user_mail);
                            balanceUpdate.ExecuteNonQuery();
                            
                            MessageBox.Show("Oturum başarıyla kapatıldı. Kullanılabilir süreniz bitti, balance güncellendi.", "Oturum Kapatıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    

                    SqlCommand SessionID = new SqlCommand("select session_id from sessions where user_id=@GetUserID  and status=1", connection);
                    SessionID.Parameters.AddWithValue("@GetUserID", GetUserId(user_mail));
                    object result7 = SessionID.ExecuteScalar();
                    if (result7 != null)
                    {
                        int SessionIDgetir=(int) result7;
                        SqlCommand UpdateSession = new SqlCommand("Update sessions set total_price=@TotalPrice,end_time=@EndDate, status=0 where session_id=@SessionID", connection);
                        UpdateSession.Parameters.AddWithValue("@TotalPrice", Convert.ToDecimal(totalprice));
                        UpdateSession.Parameters.AddWithValue("@EndDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        UpdateSession.Parameters.AddWithValue("@SessionID", SessionIDgetir);
                        int row=UpdateSession.ExecuteNonQuery();
                        if (row != 0)
                        {

                            // Hediye kullanıldıktan sonra 'used_time' güncelleniyor
                            SqlCommand updateUsedTime = new SqlCommand("UPDATE gift_wheel SET used_time = ISNULL(used_time, 0) + @UsedTime, is_claimed = 1 WHERE user_id = @UserID", connection);
                            updateUsedTime.Parameters.AddWithValue("@UsedTime", remainingTime); // Kalan süre kadar kullanılan süreyi ekle
                            updateUsedTime.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                            updateUsedTime.ExecuteNonQuery(); // Güncellemeyi yap
                            MessageBox.Show("Oturum Başarıyla Güncellenmiştir.Bir Daha Bekleriz.");
                        }
                    }

                   










                    // Bilgisayar durumunu güncelle
                    SqlCommand updateComputerStatus = new SqlCommand(
                        "UPDATE computers SET status = 'available' WHERE name = @ComputerName", connection);
                    updateComputerStatus.Parameters.AddWithValue("@ComputerName", secili_pc);
                    updateComputerStatus.ExecuteNonQuery();
                }

                // Oturum kapatma işlemi tamamlandıktan sonra ana menüye dön
                Sessions sessionsForm = new Sessions
                {
                    user_role = this.user_role,
                    user_mail = this.user_mail,
                    user_balance = this.user_balance,
                };
                sessionsForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anasayfaya Döneceksin.Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            try
            {
                // Oturum süresi dolmadan kapatma kontrolü
                if (timer1.Enabled)
                {
                    timer1.Stop(); // Zamanlayıcıyı durdur
                }

                // FinalizeSession metodu ile oturum işlemlerini tamamla
                FinalizeSession();

                MessageBox.Show("Oturum başarıyla kapatıldı.", "Oturum Kapatıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Gerekirse başka işlemler yap
                // Örneğin, kullanıcıyı ana ekrana yönlendirme
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oturum kapatılırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}