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
        public double session_balance { get; set; }
        public string secili_pc { get; set; }
        public int oturum_suresi { get; set; }
        public double oturum_ucret { get; set; }
        public double oturum_siparis_ucret { get; set; }


        private void AddTime_Load(object sender, EventArgs e)
        {
            

            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int dakika=Convert.ToInt32(textBox1.Text);
            int dakikacevir = dakika * 60;
            int toplamzaman=oturum_suresi+dakikacevir;
            MessageBox.Show(dakikacevir.ToString());
            DialogResult result = MessageBox.Show("Oturum Süresi:"+toplamzaman/60, "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
               
                UsersSession usersSession = new UsersSession()
                {
                    user_balance = user_balance,
                    user_mail = user_mail,
                    user_role = user_role,
                    oturum_suresi = toplamzaman,
                    sessionBalance=this.session_balance,
                    
                    
                };
                usersSession.Show();
                this.Hide();
            }
        }
    }
}
