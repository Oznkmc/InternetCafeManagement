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
    }
}
