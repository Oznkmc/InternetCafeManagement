using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InternetCafeManagement
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa()
        {
            InitializeComponent();
        }
        public bool user_role { get; set; }
        public string user_mail { get; set; }
        public double user_balance { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";



        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            picAdmin.Visible = user_role;
        }

        private void SessionBox_Click(object sender, EventArgs e)
        {
            Sessions sessions = new Sessions();
            sessions.user_balance = user_balance;
            sessions.user_role = user_role;
            sessions.user_mail = user_mail;

            sessions.Show();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            order.user_balance = user_balance;
            order.user_mail = user_mail;

            order.Show();

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Balance balance = new Balance();
            balance.usermail = user_mail;
            balance.userbalance = user_balance;
            balance.userrole = user_role;
            balance.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            Wheel wheel = new Wheel();
            wheel.usermail = user_mail;
            wheel.user_balance = user_balance;
            wheel.user_rol = user_role;
            wheel.Show();
            this.Hide();
        }

        private void ComputersBox_Click(object sender, EventArgs e)
        {

        }

        private void pictureUsers_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            admin Admin = new admin();
            Admin.role = user_role;
            Admin.usermail = user_mail;
            Admin.balance = user_balance;
            Admin.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
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
        public string secilihediye;
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
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

                    int userId = (int)IDresult;

                    // Hediye bilgilerini almak için sorgu
                    string reward = null;
                    bool isClaimed = false;

                    SqlCommand giftCommand = new SqlCommand("SELECT reward, is_claimed FROM gift_wheel WHERE user_id = @UserID", connection);
                    giftCommand.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = giftCommand.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Hediye bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        reward = reader["reward"]?.ToString();
                        isClaimed = reader["is_claimed"] != DBNull.Value && Convert.ToBoolean(reader["is_claimed"]);
                    } // Reader burada kapandı

                    if (reward == null)
                    {
                        MessageBox.Show("Hediye bilgisi eksik.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (isClaimed)
                    {
                        MessageBox.Show("Hediye zaten kullanılmış.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Hediye bilgisi ile kullanıcıya onay sorusu
                    DialogResult result2 = MessageBox.Show($"Bir Hediyeniz Var ({reward}). Kullanmak İster Misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result2 == DialogResult.Yes)
                    {
                        // Hediye kullanımı
                        secilihediye = reward;
                        SqlCommand command = new SqlCommand("UPDATE gift_wheel SET is_claimed = 1 WHERE user_id = @UserId", connection);
                        command.Parameters.AddWithValue("@UserId", userId);
                        int row = command.ExecuteNonQuery();

                        if (row == 0)
                        {
                            MessageBox.Show("Başarısız Güncelleme", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        GamePoints points = new GamePoints
                        {
                            user_balance = user_balance,
                            user_mail = user_mail,
                            hediyekullandi = true,
                            secilihediye = secilihediye
                        };
                        points.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Hediye kullanmama
                        GamePoints points = new GamePoints
                        {
                            user_balance = user_balance,
                            user_mail = user_mail,
                            hediyekullandi = false
                        };
                        points.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }



        }
    }
}
