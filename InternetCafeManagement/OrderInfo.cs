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











    }
}
