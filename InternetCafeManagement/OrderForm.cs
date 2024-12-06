using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetCafeManagement
{
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
        }
        public string usermail {  get; set; }
        
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void OrderForm_Load(object sender, EventArgs e)
        {
            
        }
        private void btnCompleteSale_Click(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
