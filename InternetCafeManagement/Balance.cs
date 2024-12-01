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
    public partial class Balance : Form
    {
        public Balance()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        public string usermail { get; set; }
        public double userbalance {  get; set; }
        public string userrole { get; set; }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Bağlantıyı açıyoruz
                    connection.Open();

                    // Kullanıcıdan alınan bakiye değerini güvenli bir şekilde parse ediyoruz
                    if (!decimal.TryParse(txtBalance.Text, out decimal bakiye) || bakiye <= 0)
                    {
                        MessageBox.Show("Lütfen geçerli bir bakiye girin.");
                        return;
                    }

                    // SQL sorgusu oluşturuluyor
                    SqlCommand balanceTopUp = new SqlCommand(
                        "UPDATE users SET balance = balance + @BalanceTopUp WHERE email = @UserMail", connection);

                    // Parametreler ekleniyor
                    balanceTopUp.Parameters.AddWithValue("@BalanceTopUp", bakiye);
                    balanceTopUp.Parameters.AddWithValue("@UserMail", usermail);

                    // Sorguyu çalıştırıyoruz
                    int rowsAffected = balanceTopUp.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Bakiyeniz başarıyla güncellenmiştir.");

                        // Kullanıcı bakiyesini yeniden sorguluyoruz
                        SqlCommand balance = new SqlCommand("SELECT balance FROM users WHERE email = @UserMail", connection);
                        balance.Parameters.AddWithValue("@UserMail", usermail);

                        object result2 = balance.ExecuteScalar();
                        if (result2 != null && result2 != DBNull.Value)
                        {
                            userbalance =Convert.ToDouble(result2); // Bakiyeyi güncelliyoruz
                        }
                        else
                        {
                            userbalance = 0; // Eğer bakiye NULL ise varsayılan değer olarak 0 atanır
                        }

                        // AnaSayfa formuna güncel bakiyeyi aktarıyoruz
                        AnaSayfa ana = new AnaSayfa
                        {
                            user_balance = userbalance,
                            user_role = userrole,
                            user_mail=usermail
                        };

                        txtBalance.Clear();
                    }
                    else
                    {
                        MessageBox.Show("E-posta adresine sahip bir kullanıcı bulunamadı.");
                    }
                }
                catch (SqlException sqlEx)
                {
                    // SQL hataları için özelleştirilmiş mesaj
                    MessageBox.Show("Veritabanı hatası oluştu: " + sqlEx.Message);
                }
                catch (Exception exception)
                {
                    // Diğer hatalar için genel mesaj
                    MessageBox.Show("Bir hata oluştu. Lütfen tekrar deneyin. Detay: " + exception.Message);
                }
            }






        }

        private void Balance_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            AnaSayfa ana = new AnaSayfa();
            ana.user_mail = usermail;
            ana.user_balance = userbalance;
            ana.user_role = userrole;
            ana.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand BilgiVer = new SqlCommand("select balance from users where email=@userMail",connection);
                    BilgiVer.Parameters.AddWithValue("@userMail", usermail);
                    object result = BilgiVer.ExecuteScalar();
                    if (result != null)
                    {
                        decimal bilgigoster = (decimal)result;
                        MessageBox.Show("Mevcut Bakiyeniz:" + bilgigoster.ToString());
                    }
                    connection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hatanız: " + ex.Message);
                }
            }
        }
    }
}

    

        
    
