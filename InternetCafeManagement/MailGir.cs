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
    public partial class MailGir : Form
    {
        public MailGir()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DogrulamaKodu dogrulamaKodu=new DogrulamaKodu();
            dogrulamaKodu.Show();   
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
