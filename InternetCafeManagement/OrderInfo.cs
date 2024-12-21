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
    public partial class OrderInfo : Form
    {
        public OrderInfo()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        public double user_balance {  get; set; }
        public string user_mail { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        void griddoldur()
        {

            con = new SqlConnection(connectionString);
            da = new SqlDataAdapter("Select * from Orders", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "Orders");
            dataGridView1.DataSource = ds.Tables["Orders"];
            con.Close();
        }

        public bool user_role { get; set; }
        private void OrderInfo_Load(object sender, EventArgs e)
        {
            griddoldur();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;  // TextBox1'den email alınır

            if (!string.IsNullOrEmpty(email))
            {
                int userId = GetUserIdByEmail(email); // Email'e göre user_id al
                if (userId > 0)
                {
                    GetOrdersByUserId(userId);  // user_id'ye göre siparişleri al
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

        // Kullanıcı email'e göre user_id'yi almak için metod
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

        // user_id'ye göre Orders tablosundaki siparişleri almak için metod
        private void GetOrdersByUserId(int userId)
        {
            con = new SqlConnection(connectionString);

            // user_id ile Orders tablosundaki verileri çekme
            SqlCommand command = new SqlCommand(
                "SELECT * FROM Orders WHERE CustomerID = @UserId", con
            );
            command.Parameters.AddWithValue("@UserId", userId);

            da = new SqlDataAdapter(command);
            ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds, "Orders");

                // DataGridView'e verileri aktarma
                if (ds.Tables["Orders"].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables["Orders"];
                }
                else
                {
                    MessageBox.Show("Bu kullanıcıya ait sipariş bulunamadı.");
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
            string email = textBox1.Text;  // TextBox1'den email alınır

            if (!string.IsNullOrEmpty(email))
            {
                int userId = GetUserIdByEmail(email); // Email'e göre user_id al
                if (userId > 0)
                {
                    GetOrdersByUserId(userId);  // user_id'ye göre siparişleri al
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
    }
}
