using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InternetCafeManagement
{
    public partial class WheelInfo : Form
    {
        public WheelInfo()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        public string usermail { get; set; }
        public bool user_role { get; set; }
        public decimal user_balance {  get; set; }
        void griddoldur()
        {

            con = new SqlConnection(connectionString);
            da = new SqlDataAdapter("Select * from gift_wheel", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "gift_wheel");
            dataGridView1.DataSource = ds.Tables["gift_wheel"];
            con.Close();
        }
        void KayıtSil(int userid)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand("delete from gift_wheel where user_id=@userid", connection);
                    com.Parameters.AddWithValue("@userid", userid);
                    com.ExecuteNonQuery();
                    MessageBox.Show("Kullanıcı Başarıyla Silinmiştir.");
                    griddoldur();
                    connection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata:" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }




        }
        private void WheelInfo_Load(object sender, EventArgs e)
        {
            griddoldur();
            cmbIsClaimed.Items.Add("True");
            cmbIsClaimed.Items.Add("False");

        }

        private void pictrureAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {


                    connection.Open();
                    SqlCommand user_id = new SqlCommand("select spin_id from users where user_id=@UserID", connection);
                    SqlCommand IDcommand = new SqlCommand("select user_id from users email=@UserMail", connection);
                    IDcommand.Parameters.AddWithValue("@UserMail", usermail);
                    object result = IDcommand.ExecuteScalar();
                    if (result != null)
                    {
                        int userid = (int)result;
                        user_id.Parameters.AddWithValue("userID", userid);


                        object result1 = user_id.ExecuteScalar();
                        if (result1 == null)
                        {

                            SqlCommand AddGift = new SqlCommand("insert into gift_wheel(user_id,reward,is_claimed,gift_duration,used_time) values(@userid,@reward,@is_claimed,@gift_duration,used_time) ", connection);
                            AddGift.Parameters.AddWithValue("@userid", Convert.ToInt32(txtKullanıcıID.Text));
                            AddGift.Parameters.AddWithValue("@reward", txtReward.Text);
                            AddGift.Parameters.AddWithValue("@is_claimed", Convert.ToBoolean(cmbIsClaimed.Text));
                            AddGift.Parameters.AddWithValue("@gift_duration", Convert.ToInt32(txtGiftDuration.Text));
                            AddGift.Parameters.AddWithValue("@used_time", Convert.ToInt32(txtUsedTime.Text));
                            AddGift.ExecuteNonQuery();
                            MessageBox.Show("Kullanıcı Başarıyla Eklenmiştir.");
                            griddoldur();
                        }
                        else
                        {
                            MessageBox.Show("Bu kullanıcıya ait tanımlı bir hediye zaten mevcut.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hatanızın Nedeni:" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Tıklanan bir satır olduğundan emin ol
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // TextBoxlara seçili satırın değerlerini aktar
                txtKullanıcıID.Text = row.Cells["user_id"].Value.ToString();
                txtReward.Text = row.Cells["reward"].Value.ToString();
                cmbIsClaimed.Text = row.Cells["is_claimed"].Value.ToString();
                txtGiftDuration.Text = row.Cells["gift_duration"].Value.ToString();
                txtUsedTime.Text = row.Cells["used_time"].Value.ToString();
            }
        }

        private void pictureDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT user_id FROM users WHERE email=@UserMail", connection);
                    command.Parameters.AddWithValue("@UserMail", usermail);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        int id = (int)result;
                        KayıtSil(id);
                        griddoldur();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hatanızın Nedeni: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void pictureUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL komutunu hazırla
                    SqlCommand command = new SqlCommand(
                        "UPDATE gift_wheel SET reward=@reward, is_claimed=@claimed, gift_duration=@duration, used_time=@used_time WHERE user_id=@user_id",
                        connection);

                    // Parametreleri ekle
                    if (!int.TryParse(txtKullanıcıID.Text, out int userId) ||
                        !int.TryParse(txtGiftDuration.Text, out int duration) ||
                        !int.TryParse(txtUsedTime.Text, out int usedTime))
                    {
                        MessageBox.Show("Lütfen geçerli bir sayı giriniz.");
                        return;
                    }

                    command.Parameters.AddWithValue("@user_id", userId);
                    command.Parameters.AddWithValue("@reward", txtReward.Text);
                    command.Parameters.AddWithValue("@claimed", cmbIsClaimed.Text.Trim().ToLower() == "evet"); // True veya False
                    command.Parameters.AddWithValue("@duration", duration);
                    command.Parameters.AddWithValue("@used_time", usedTime);

                    command.ExecuteNonQuery();
                    griddoldur();

                    MessageBox.Show("Kullanıcının Hediyesi Başarıyla Güncellenmiştir.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hatanızın Nedeni: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Admin Sayfasına Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                admin Admin = new admin
                {
                    role = this.user_role,
                    usermail = this.usermail,
                    balance=this.user_balance
                    
                };

                this.Hide();
                Admin.Show(); // Admin formunu göster
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private int GetUserIdByEmail(string email)
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

        // user_id'ye göre gift_wheel tablosundaki verileri almak için metod
        private void GetGiftWheelDataByUserId(int userId)
        {
            con = new SqlConnection(connectionString);

            // user_id ile gift_wheel tablosundaki verileri çekme
            SqlCommand command = new SqlCommand(
                "SELECT * FROM gift_wheel WHERE user_id = @UserId", con
            );
            command.Parameters.AddWithValue("@UserId", userId);

            da = new SqlDataAdapter(command);
            ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds, "gift_wheel");

                // DataGridView'e verileri aktarma
                if (ds.Tables["gift_wheel"].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables["gift_wheel"];
                }
                else
                {
                    MessageBox.Show("Bu kullanıcıya ait hediye çarkı bilgisi bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri çekilirken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string email = txtUserName.Text;  // TextBox1'den email alınır

            if (!string.IsNullOrEmpty(email))
            {
                int userId = GetUserIdByEmail(email); // Email'e göre user_id al
                if (userId > 0)
                {
                    GetGiftWheelDataByUserId(userId);  // user_id'ye göre gift_wheel verilerini al
                }
                else
                {
                    MessageBox.Show("Email'e ait kullanıcı bulunamadı.");
                }
            }
            else
            {
                griddoldur();
            }
        }
    }
}


