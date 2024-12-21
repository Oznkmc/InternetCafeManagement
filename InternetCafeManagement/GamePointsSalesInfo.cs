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

namespace InternetCafeManagement
{
    public partial class GamePointsSalesInfo : Form
    {
        public GamePointsSalesInfo()
        {
            InitializeComponent();
        }
        //SqlConnection con = new SqlConnection();
        //SqlDataAdapter da = new SqlDataAdapter();
        //SqlCommand com = new SqlCommand();
        //DataSet ds = new DataSet();

        public string user_mail { get; set; }
        public bool user_role {  get; set; }
        public double user_balance {  get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();

        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanıcı maili üzerinden user_id alınması

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
        private void GetAllGamePointsData()
        {
            con = new SqlConnection(connectionString);

            // GamePoints_Sales tablosundaki tüm verileri çekme
            SqlCommand command = new SqlCommand(
                "SELECT * FROM GamePoints_Sales", con
            );

            da = new SqlDataAdapter(command);
            ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds, "GamePoints_Sales");
                dataGridView1.DataSource = ds.Tables["GamePoints_Sales"];
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
        private void GetGamePointsData(int userId)
        {
            con = new SqlConnection(connectionString);

            // user_id ile GamePoints_Sales tablosundan veri çekme
            SqlCommand command = new SqlCommand(
                "SELECT * FROM GamePoints_Sales WHERE UserID = @UserId", con
            );
            command.Parameters.AddWithValue("@UserId", userId);

            da = new SqlDataAdapter(command);
            ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds, "GamePoints_Sales");
                dataGridView1.DataSource = ds.Tables["GamePoints_Sales"];
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

        private void GamePointsSalesInfo_Load(object sender, EventArgs e)
        {
            GetAllGamePointsData();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Admin Sayfasına Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                admin Admin = new admin
                {
                    role = this.user_role,
                    usermail = this.user_mail,
                    balance = this.user_balance
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int userId = GetUserId(textBox1.Text);

            if (userId > 0)
            {
                // user_id ile GamePoints_Sales tablosundan veri çekme
                GetGamePointsData(userId);
            }
            else
            {
                MessageBox.Show("Kullanıcı ID bulunamadı.");
            }
        }
    }
}
