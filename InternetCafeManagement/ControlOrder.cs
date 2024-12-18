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
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        private void ControlOrder_Load(object sender, EventArgs e)
        {
            void griddoldur()
            {

                con = new SqlConnection(connectionString);
                da = new SqlDataAdapter(" SqlCommand OrderPrice = new SqlCommand(\r\n     \"SELECT TOP 1 o.TotalAmount AS TotalAmount \" +\r\n     \"FROM Orders o \" +\r\n     \"INNER JOIN sessions s ON o.SessionID = s.session_id \" +\r\n     \"INNER JOIN users u ON o.CustomerID = u.user_id \" +\r\n     \"WHERE u.user_id = @UserID \" +\r\n     \"AND s.session_id = @session_id \" +\r\n     \"AND s.start_time = (SELECT MAX(start_time) FROM sessions WHERE session_id = o.SessionID) \" +\r\n     \"ORDER BY s.start_time DESC;\",\r\n     connection\r\n );", con);
                ds = new DataSet();
                con.Open();
                da.Fill(ds, "sessions");
                dataGridView1.DataSource = ds.Tables["sessions"];
                con.Close();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
