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
       private int remainingTime;
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
        private void SessionGo(string pcsec)
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
                        OpenCustomInputSession(pcsec);
                        return;
                    }

                    string reward = reader["reward"]?.ToString();
                    int giftDuration = reader["gift_duration"] != DBNull.Value ? Convert.ToInt32(reader["gift_duration"]) : 0;
                    int usedTime = reader["used_time"] != DBNull.Value ? Convert.ToInt32(reader["used_time"]) : 0;
                    bool isClaimed = reader["is_claimed"] != DBNull.Value ? Convert.ToBoolean(reader["is_claimed"]) : false;
                    reader.Close();

                    // Eğer hediye zaten kullanıldıysa, CustomInputSession'ı aç
                    //if (isClaimed)
                    //{
                    //    MessageBox.Show("Hediyeniz zaten kullanıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    OpenCustomInputSession();
                    //    return;
                    //}

                    if (reward == "1 saat ücretsiz oturum" || reward == "3 saat ücretsiz oturum")
                    {
                        DialogResult result2 = MessageBox.Show($"Bir Hediyeniz Var ({reward}). Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result2 == DialogResult.Yes)
                        {
                            SqlCommand command = new SqlCommand("select is_claimed from gift_wheel where user_id=@UserID", connection);
                            command.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                            object resultClaimed = command.ExecuteScalar();

                            SqlCommand command1 = new SqlCommand("SELECT (gift_duration * 60) - used_time FROM gift_wheel WHERE user_id=@UserID;", connection);
                            command1.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                            object resultcommand1 = command1.ExecuteScalar();

                            if (resultcommand1 != null && resultcommand1 != DBNull.Value)
                            {
                                int remainingTime = (int)resultcommand1;  // Bu artık doğrudan kalan süreyi döndürecek
                                if (remainingTime > 0)
                                {
                                    parsedOturumSuresi = remainingTime; // Kalan süreyi saniye olarak alıyoruz
                                   
                                        try
                                        {
                                            
                                            //Oturum bilgilerini kaydet
                                            SqlCommand addSession = new SqlCommand(
                                                "INSERT INTO sessions (user_id, computer_id, start_time,status) " +
                                                "VALUES (@UserId, @ComputerId, @StartTime,1)", connection);
                                            addSession.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                                            addSession.Parameters.AddWithValue("@ComputerId", GetComputerId(pcsec));

                                            addSession.Parameters.AddWithValue("@StartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                            addSession.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Hatanız:" + ex.Message);
                                        }
                                        finally
                                        {
                                            connection.Close();
                                        }

                                    
                                    // Kullanıcı oturumu başlat
                                    UsersSession usersSessionGift = new UsersSession
                                    {
                                        oturum_suresi = parsedOturumSuresi,
                                        user_role = this.user_role,
                                        user_mail = this.user_mail,
                                        user_balance = this.user_balance,
                                        secili_pc = pcsec, // Seçilen bilgisayar
                                        hediyekullandi = true,
                                    };
                                    this.Hide();
                                    usersSessionGift.Show();
                                }
                                else if (remainingTime <= 0)
                                {
                                    MessageBox.Show("Hediye süresi tamamlanmış.");
                                    OpenCustomInputSession(pcsec);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Hediye süresi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }



                        }
                    }
                    else
                    {

                        CustomInputSession customInputSession = new CustomInputSession
                        {
                            user_role = this.user_role,
                            user_mail = this.user_mail,
                            user_balance = this.user_balance,
                            secili_pc = pcsec,
                            hediyekullandi = false
                        };

                        customInputSession.Show();
                        this.Hide();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
        private void picturePC1_Click(object sender, EventArgs e)
        {
            SessionGo("PC1");





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

        // CustomInputSession açma işlemi
        private void OpenCustomInputSession(string pcsec2)
        {
            CustomInputSession customInputSession = new CustomInputSession
            {
                user_role = this.user_role,
                user_mail = this.user_mail,
                user_balance = this.user_balance,
                secili_pc = pcsec2
            };

            customInputSession.Show();
            this.Hide();
        }
        private void picturePC3_Click(object sender, EventArgs e)
        {
            SessionGo("PC3");
            
            

        }

        private void picturePC4_Click(object sender, EventArgs e)
        {
            SessionGo("PC4");
            //OpenCustomInputSession("PC4");
        }

        private void picturePC5_Click(object sender, EventArgs e)
        {
            SessionGo("PC5");
            //OpenCustomInputSession("PC5");
        }

        private void picturePC6_Click(object sender, EventArgs e)
        {
            SessionGo("PC6");
            //OpenCustomInputSession("PC6");
        }

        private void picturePC7_Click(object sender, EventArgs e)
        {
            SessionGo("PC7");
            //OpenCustomInputSession("PC7");
        }

        private void picturePC8_Click(object sender, EventArgs e)
        {
            SessionGo("PC8");
            //OpenCustomInputSession("PC8");
        }

        private void picturePC9_Click(object sender, EventArgs e)
        {
          SessionGo("PC9");
            //OpenCustomInputSession("PC9");
        }

        private void picturePC10_Click(object sender, EventArgs e)
        {
            SessionGo("PC10");
            //OpenCustomInputSession("PC10");
        }

        private void picturePC11_Click(object sender, EventArgs e)
        {
            SessionGo("11");
            //OpenCustomInputSession("PC11");
        }

        private void picturePC12_Click(object sender, EventArgs e)
        {
            SessionGo("PC12");
            //OpenCustomInputSession("PC12");
        }

        private void picturePC2_Click(object sender, EventArgs e)
        {
            SessionGo("PC2");
            //OpenCustomInputSession("PC2");
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
