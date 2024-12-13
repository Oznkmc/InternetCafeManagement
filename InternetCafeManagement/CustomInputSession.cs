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
    public partial class CustomInputSession : Form
    {
        public CustomInputSession()
        {
            InitializeComponent();
        }

        public bool user_role { get; set; }
        public string user_mail { get; set; }
        public double user_balance { get; set; }
        public string secili_pc { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        int userID;
        //object giftResult;
        int parsedOturumSuresi;
        private void CustomInputSession_Load(object sender, EventArgs e)
        {
            // Yükleme işlemi varsa buraya eklenebilir
            //user id alacaz


            // Hediye kullanımı kontrolü












        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Oturum formuna dönmek istediğinize emin misiniz?");
            if (result == DialogResult.OK)
            {
                Sessions sessions = new Sessions();
                sessions.user_role = user_role;
                sessions.user_mail = user_mail;
                sessions.user_balance = user_balance;
                sessions.Show();
                this.Hide();
            }
        }

        public int oturumSuresi;
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
                SqlCommand command = new SqlCommand("SELECT computer_id FROM computers WHERE name = @ComputerName", connection);
                command.Parameters.AddWithValue("@ComputerName", computerName);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    computerId = Convert.ToInt32(reader["computer_id"]);
                }
            }
            return computerId;
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string inputText = txtInput.Text.ToLower();

            //sınırsız oturum
            if (inputText == "sınırsız")
            {
                if (user_balance >= 7.5)
                {
                    oturumSuresi = 99999;
                    DialogResult result = MessageBox.Show("Oturum Süresinden Emin Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Veritabanında bilgisayar durumunu "unavailable" olarak güncelle
                        UpdateComputerStatus("unavailable");

                        // Oturum başlatma
                        UsersSession usersSession = new UsersSession
                        {
                            oturum_suresi = oturumSuresi,
                            user_role = this.user_role,
                            user_mail = this.user_mail,
                            user_balance = this.user_balance,
                            secili_pc = this.secili_pc
                        };
                        usersSession.Show();
                        this.Hide();
                    }
                }
                else
                {
                    // Yetersiz bakiye durumunda
                    MessageBox.Show("Lütfen Geçerli Bakiye Girin.");
                    txtInput.Clear();
                    DialogResult result2 = MessageBox.Show("Bakiye Sayfasına Yönlendirilmek İster Misin?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result2 == DialogResult.Yes)
                    {
                        // Bilgisayar durumunu "available" yap
                        UpdateComputerStatus("available");

                        // Balance sayfasına yönlendir
                        Balance balance = new Balance
                        {
                            userbalance = this.user_balance,
                            usermail = this.user_mail,
                            userrole = this.user_role
                        };
                        balance.Show();
                        this.Hide();
                    }
                }
            }
            // Süreli oturum
            if (int.TryParse(txtInput.Text, out int parsedOturumSuresi) && parsedOturumSuresi > 0)
            {
                if (parsedOturumSuresi >= 0)
                {
                    double requiredBalance = parsedOturumSuresi * 0.25;
                    if (user_balance >= requiredBalance)
                    {
                        DialogResult result = MessageBox.Show("Oturum Süresinden Emin Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            using (SqlConnection connection=new SqlConnection(connectionString))
                            {
                                try
                                {
                                    connection.Open();
                                    //Oturum bilgilerini kaydet
                                    SqlCommand addSession = new SqlCommand( 
                                        "INSERT INTO sessions (user_id, computer_id, start_time) " +
                                        "VALUES (@UserId, @ComputerId, @StartTime)", connection);
                                    addSession.Parameters.AddWithValue("@UserId", GetUserId(user_mail));
                                    addSession.Parameters.AddWithValue("@ComputerId", GetComputerId(secili_pc));

                                    addSession.Parameters.AddWithValue("@StartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                    addSession.ExecuteNonQuery();
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show("Hatanız:" + ex.Message);
                                }
                                finally
                                {
                                    connection.Close();
                                }
                              
                            }
                            
                            oturumSuresi = parsedOturumSuresi;
                            UpdateComputerStatus("unavailable");
                            // Oturum başlatma
                            UsersSession usersSession = new UsersSession
                            {
                                oturum_suresi = oturumSuresi*60,
                                user_role = this.user_role,
                                user_mail = this.user_mail,
                                user_balance = this.user_balance,
                                secili_pc = this.secili_pc,
                                hediyekullandi=false
                                
                            };
                            usersSession.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Yetersiz bakiye.");
                        DialogResult result2 = MessageBox.Show("Bakiye Sayfasına Yönlendirilmek İster Misin?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result2 == DialogResult.Yes)
                        {
                            // Balance sayfasına yönlendir
                            Balance balance = new Balance
                            {
                                userbalance = this.user_balance,
                                usermail = this.user_mail,
                                userrole = this.user_role
                            };
                            balance.Show();
                            this.Hide();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Oturum süresi en az 30 dakika olmalı.");
                }
            }
            else
            {
                MessageBox.Show("Geçerli bir değer girin.");
            }
        }

        // Veritabanında bilgisayar durumunu güncelleyen fonksiyon
        private void UpdateComputerStatus(string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE computers SET status = @status WHERE name = @secilipc", connection);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@secilipc", secili_pc);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hatanızın Nedeni: " + ex.Message);
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Oturum Sayfasına Dönüyorsun. Emin Misin?", "Geri Dön", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Sessions sessions = new Sessions
                {
                    user_mail = this.user_mail,
                    user_balance = this.user_balance,
                    user_role = this.user_role,

                };
                sessions.Show();
                this.Hide();
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }
    }
}






