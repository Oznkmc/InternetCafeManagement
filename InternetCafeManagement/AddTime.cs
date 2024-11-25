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
    public partial class AddTime : Form
    {
        public AddTime()
        {
            InitializeComponent();
        }
        public string user_role { get; set; }
        public string user_mail { get; set; }
        public double user_balance { get; set; }
        public string secili_pc { get; set; }
        public int oturum_suresi { get; set; }
        private void AddTime_Load(object sender, EventArgs e)
        {
            //burada kullanıcı dakika arttırmak isterse dakika arttıracak.
        }
    }
}
