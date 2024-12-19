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
    public partial class ControlOrder : Form
    {
        public ControlOrder()
        {
            InitializeComponent();
        }
        public string user_mail {  get; set; }
        public int secili_oturum {  get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        void griddoldur()
        {

            con = new SqlConnection(connectionString);
            SqlCommand OrderPrice = new SqlCommand(
    "SELECT TOP 1 o.TotalAmount AS TotalAmount " +
    "FROM Orders o " +
    "INNER JOIN sessions s ON o.SessionID = s.session_id " +
    "INNER JOIN users u ON o.CustomerID = u.user_id " +
    "WHERE u.user_id = @UserID " +
    "AND s.session_id = @session_id " +
    "AND s.start_time = (SELECT MAX(start_time) FROM sessions WHERE session_id = o.SessionID) " +
    "ORDER BY s.start_time DESC;",
    con
);
            OrderPrice.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
            OrderPrice.Parameters.AddWithValue("@session_id", secili_oturum);
            SqlDataAdapter da =new SqlDataAdapter(OrderPrice);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "Orders");
            dataGridView1.DataSource = ds.Tables["Orders"];
            con.Close();
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
        private void ControlOrder_Load(object sender, EventArgs e)
        {
           griddoldur();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
