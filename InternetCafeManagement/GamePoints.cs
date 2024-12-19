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
    public partial class GamePoints : Form
    {
        public GamePoints()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        public string user_balance { get; set; }
        private int GetUserId(string email)
        {
            int userId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT user_id FROM users WHERE email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["user_id"]);
                }
            }
            return userId;
        }
        private void Game_points(string Category)
        {



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // GamePoints tablosundaki oyun puanlarını filtrele ve getir
                SqlCommand sqlCommand = new SqlCommand(
                    "SELECT GameName, PointType, Points, Price FROM GamePoints WHERE GameName = @GameName",
                    connection
                );

                // GroupBox veya başka bir kontrol üzerinden seçilen oyun adı (örneğin, Valorant)
                sqlCommand.Parameters.AddWithValue("@GameName", Category);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                listView1.Items.Clear(); // Önceki verileri temizle

                while (reader.Read())
                {
                    // Tablo sütunlarından veri al
                    string gameName = reader["GameName"].ToString();
                    string pointType = reader["PointType"].ToString();
                    int points = (int)reader["Points"];
                    decimal price = (decimal)reader["Price"];

                    // ListView'e verileri ekle
                    ListViewItem item = new ListViewItem(gameName); // Oyun adı
                    item.SubItems.Add(pointType);                  // Puan türü
                    item.SubItems.Add(points.ToString());          // Puan miktarı
                    item.SubItems.Add(price.ToString("C2"));       // Fiyat (para birimi formatında)

                    listView1.Items.Add(item);
                }

                reader.Close();
            }

        }
        private void GamePoints_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtCount.Clear();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtCount.Text += "0";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count == 0) // Sipariş listesi boşsa uyarı göster
            {
                MessageBox.Show("Sipariş eklenmedi. Lütfen önce sipariş oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Siparişin toplam fiyatını hesaplıyoruz
                decimal totalPrice = 0;

                foreach (ListViewItem item in listView2.Items)
                {
                    string gameName = item.SubItems[0].Text; // Oyun Adı (Valorant veya LoL)
                    int quantity = int.Parse(item.SubItems[1].Text); // Satılan puan miktarı
                    decimal price = decimal.Parse(item.SubItems[2].Text, System.Globalization.NumberStyles.Currency); // Birim fiyat
                    decimal itemTotalPrice = quantity * price; // Miktar x Birim Fiyat
                    totalPrice += itemTotalPrice;
                }

                // Kullanıcı bakiyesi kontrolü
                if (totalPrice > Convert.ToDecimal(user_balance))
                {
                    MessageBox.Show("Hesap bakiyesi yetersiz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    listView1.Items.Clear();
                    listView2.Items.Clear();
                    return;
                }

                // Sipariş detaylarını GamePoints tablosuna kaydediyoruz
                foreach (ListViewItem item in listView2.Items)
                {
                    string gameName = item.SubItems[0].Text; // Oyun Adı
                    string pointType = item.SubItems[1].Text; // Puan Türü (VP veya RP)
                    int quantity = int.Parse(item.SubItems[2].Text); // Miktar
                    decimal price = decimal.Parse(item.SubItems[3].Text, System.Globalization.NumberStyles.Currency); // Birim fiyat

                    SqlCommand insertCommand = new SqlCommand(
                        "INSERT INTO GamePoints (GameName, PointType, Quantity, Price, OrderDate) VALUES (@GameName, @PointType, @Quantity, @Price, @OrderDate)",
                        connection
                    );
                    insertCommand.Parameters.AddWithValue("@GameName", gameName);
                    insertCommand.Parameters.AddWithValue("@PointType", pointType);
                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                    insertCommand.Parameters.AddWithValue("@Price", price);
                    insertCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);

                    insertCommand.ExecuteNonQuery();
                }

                // Kullanıcı bakiyesini güncelleme
                SqlCommand updateBalanceCommand = new SqlCommand(
                    "UPDATE Users SET Balance = Balance - @TotalPrice WHERE UserID = @UserID",
                    connection
                );
                updateBalanceCommand.Parameters.AddWithValue("@TotalPrice", totalPrice);
                updateBalanceCommand.Parameters.AddWithValue("@UserID", userId); // Kullanıcı ID'si burada olmalı
                updateBalanceCommand.ExecuteNonQuery();

                // Sipariş ekranını temizliyoruz
                listView1.Items.Clear();
                listView2.Items.Clear();

                // Kullanıcıya siparişin başarıyla kaydedildiği mesajını gösteriyoruz
                MessageBox.Show("Sipariş başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
