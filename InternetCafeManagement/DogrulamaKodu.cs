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
    public partial class DogrulamaKodu : Form
    {
        public DogrulamaKodu()
        {
            InitializeComponent();
        }

        private void DogrulamaKodu_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
        }
        private int secondsRemaining = 180;
        private void timer1_Tick(object sender, EventArgs e)
        {
            secondsRemaining--;
            if (secondsRemaining <= 0)
            {
                timer1.Stop();  // Timer'ı durdur
                MessageBox.Show("Zaman Doldu!");
            }
            else
            {
                // Kalan süreyi dakika:saniye formatında göster
                int minutes = secondsRemaining / 60;
                int seconds = secondsRemaining % 60;
                label2.Text = $"{minutes:D2}:{seconds:D2}";  // Zaman formatını "mm:ss" olarak güncelle
            }
        }
    }
}
