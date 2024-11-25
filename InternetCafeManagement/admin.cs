using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetCafeManagement
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }
        public string role {  get; set; }
        public string usermail {  get; set; }
        public double balance {  get; set; }

        private void admin_Load(object sender, EventArgs e)
        {

        }
    }
}
