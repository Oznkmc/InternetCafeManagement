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
        public string user_mail { get; set; }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();

        void griddoldur()
        {
            con = new SqlConnection(connectionString);

            // Aktif oturumun session_id değerini almak için sorgu
            // Aktif oturumun session_id değerini almak için sorgu
            SqlCommand activeSessionQuery = new SqlCommand(
                "SELECT TOP 1 session_id FROM sessions WHERE user_id = @UserID AND status =1 ORDER BY start_time DESC", con
            );

            activeSessionQuery.Parameters.AddWithValue("@UserID", GetUserId(user_mail));

            con.Open();
            object sessionIdObj = activeSessionQuery.ExecuteScalar();
            con.Close();

            if (sessionIdObj != null)
            {
                int activeSessionId = Convert.ToInt32(sessionIdObj);

                // Aktif oturum ile ilgili siparişleri çekmek için sorgu
                SqlCommand orderQuery = new SqlCommand(
                    "SELECT o.TotalAmount, o.OrderDate " +
                    "FROM Orders o " +
                    "INNER JOIN sessions s ON o.SessionID = s.session_id " +
                    "INNER JOIN users u ON o.CustomerID = u.user_id " +
                    "WHERE s.session_id = @session_id " +
                    "ORDER BY s.start_time DESC;", con
                );

                orderQuery.Parameters.AddWithValue("@session_id", activeSessionId);

                // Sipariş verilerini çek ve DataGridView'e aktar
                da = new SqlDataAdapter(orderQuery);
                ds = new DataSet();
                con.Open();
                da.Fill(ds, "Orders");
                dataGridView1.DataSource = ds.Tables["Orders"];
                con.Close();
            }
            else
            {
                MessageBox.Show("Aktif oturum bulunamadı.");
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
