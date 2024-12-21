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

namespace InternetCafeManagement
{
    public partial class SessionInfo : Form
    {
        public SessionInfo()
        {
            InitializeComponent();
        }
      
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
       public double user_balance { get; set;}
        public string user_mail { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        void griddoldur()
        {

            con = new SqlConnection(connectionString);
            da = new SqlDataAdapter("Select * from sessions", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "sessions");
            dataGridView1.DataSource = ds.Tables["sessions"];
            con.Close();
        }

        public bool user_role { get; set; }
        private void SessionInfo_Load(object sender, EventArgs e)
        {
            griddoldur();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Admin Paneline Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                admin Admin = new admin();
                Admin.role = this.user_role;
                user_balance = this.user_balance;
                Admin.usermail = this.user_mail;

                Admin.Show();
                this.Hide();
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

        private void GetSessionsByUserId(int userId)
        {
            con = new SqlConnection(connectionString);

            // user_id ile Sessions tablosundaki verileri çekme
            SqlCommand command = new SqlCommand(
                "SELECT * FROM sessions WHERE user_id = @UserId", con
            );
            command.Parameters.AddWithValue("@UserId", userId);

            da = new SqlDataAdapter(command);
            ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds, "sessions");

                // DataGridView'e verileri aktarma
                if (ds.Tables["sessions"].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables["sessions"];
                }
                else
                {
                    MessageBox.Show("Bu kullanıcıya ait oturum bulunamadı.");
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
        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;  // TextBox1'den email alınır

            if (!string.IsNullOrEmpty(email))
            {
                int userId = GetUserIdByEmail(email); // Email'e göre user_id al
                if (userId > 0)
                {
                    GetSessionsByUserId(userId);  // user_id'ye göre oturumları al
                }
                else
                {
                    MessageBox.Show("Email'e ait kullanıcı bulunamadı.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir email giriniz.");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;  // TextBox1'den email alınır

            if (!string.IsNullOrEmpty(email))
            {
                int userId = GetUserIdByEmail(email); // Email'e göre user_id al
                if (userId > 0)
                {
                    GetSessionsByUserId(userId);  // user_id'ye göre oturumları al
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
