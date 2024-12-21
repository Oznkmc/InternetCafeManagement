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
        public double user_balance { get; set; }
        public string user_mail {  get; set; }
        public bool hediyekullandi {  get; set; }
        public string secilihediye {  get; set; }
        public bool user_role {  get; set; }
        private int GetUserId(string email)
        {
            int userId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT user_id FROM users WHERE email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = Convert.ToInt32(reader["user_id"]);
                        }
                    }
                }
            }
            return userId;
        }

        private void Game_points(string Category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT GameName, PointType, Price FROM GamePoints WHERE GameName = @GameName", connection))
                {
                    sqlCommand.Parameters.AddWithValue("@GameName", Category);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        listView1.Items.Clear();
                        while (reader.Read())
                        {
                            string gameName = reader["GameName"].ToString();
                            string pointType = reader["PointType"].ToString();
                            decimal price = (decimal)reader["Price"];

                            ListViewItem item = new ListViewItem(gameName);
                            item.SubItems.Add(pointType);
                            item.SubItems.Add(price.ToString("C2"));

                            listView1.Items.Add(item);
                        }
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            txtCount.Text += "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtCount.Text += "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtCount.Text += "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtCount.Text += "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtCount.Text += "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtCount.Text += "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtCount.Text += "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtCount.Text += "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtCount.Text += "9";
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
            if (listView2.Items.Count == 0)
            {
                MessageBox.Show("Sipariş eklenmedi. Lütfen önce sipariş oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = GetUserId(user_mail); // Kullanıcı ID'sini bir kez alın.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (!string.IsNullOrEmpty(secilihediye))
                {
                    // Hediye ile işlem yapılacaksa, siparişi oluşturuyoruz
                    foreach (ListViewItem item in listView2.Items)
                    {
                        if (item.SubItems[1].Text == secilihediye)
                        {
                            string gameName = item.SubItems[0].Text;
                            string pointType = new string(secilihediye.Where(char.IsLetter).ToArray());
                            int quantity = int.Parse(new string(secilihediye.Where(char.IsDigit).ToArray())); // Adet sayısı

                            using (SqlCommand insertCommand = new SqlCommand(
                                "INSERT INTO GamePoints_Sales (GameName, PointType, Quantity, Price, TotalPrice, UserID, PaymentMethod) " +
                                "VALUES (@GameName, @PointType, @Quantity, @Price, 0, @UserID, 'Hediye')",
                                connection))
                            {
                                insertCommand.Parameters.AddWithValue("@GameName", gameName);
                                insertCommand.Parameters.AddWithValue("@PointType", pointType);
                                insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                                insertCommand.Parameters.AddWithValue("@Price", 0);  // Hediye olduğu için fiyat 0
                                insertCommand.Parameters.AddWithValue("@UserID", userId);

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    SqlCommand command1 = new SqlCommand("UPDATE gift_wheel SET is_claimed = 1 WHERE user_id = @UserID", connection);
                    command1.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                    int row1 = command1.ExecuteNonQuery();

                    if (row1 > 0)
                    {
                        MessageBox.Show("Hediye kullanılarak sipariş tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    decimal userBalance;

                    // Kullanıcı bakiyesini çekiyoruz
                    using (SqlCommand balanceCommand = new SqlCommand("SELECT Balance FROM Users WHERE user_id = @UserID", connection))
                    {
                        balanceCommand.Parameters.AddWithValue("@UserID", GetUserId(user_mail));

                        using (SqlDataReader reader = balanceCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userBalance = reader.GetDecimal(0);  // Kullanıcı bakiyesini alıyoruz
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı bakiyesi alınamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    decimal totalPrice = 0;

                    // Siparişin toplam fiyatını hesaplıyoruz
                    foreach (ListViewItem item in listView2.Items)
                    {
                        string pointType = item.SubItems[1].Text;  // Puan türünü alıyoruz (örneğin: "475 VP")
                        int quantity = int.Parse(new string(item.SubItems[2].Text.Where(char.IsDigit).ToArray())); // Adet sayısını alıyoruz (örneğin: "2")

                        decimal price = 0;

                        // Puan türüne göre fiyatı alıyoruz
                        using (SqlCommand priceCommand = new SqlCommand("SELECT Price FROM GamePoints WHERE PointType = @PointType", connection))
                        {
                            priceCommand.Parameters.AddWithValue("@PointType", pointType);

                            using (SqlDataReader reader = priceCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    price = reader.GetDecimal(0);  // Puan fiyatını alıyoruz
                                }
                                else
                                {
                                    MessageBox.Show($"Puan türü için fiyat bulunamadı: {pointType}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        // Adet ve fiyatı çarpıyoruz
                        decimal itemTotalPrice = price * quantity;
                        totalPrice += itemTotalPrice;  // Toplam fiyatı biriktiriyoruz
                    }

                    // Toplam fiyatı karşılaştırıyoruz
                    if (userBalance < totalPrice)
                    {
                        MessageBox.Show("Yeterli bakiyeniz yok. Lütfen bakiye yükleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Ödeme işlemi ve bakiye güncellemesi
                    foreach (ListViewItem item in listView2.Items)
                    {
                        string gameName = item.SubItems[0].Text;
                        string pointType = item.SubItems[1].Text; // Puan türünü alıyoruz
                        int quantity = int.Parse(new string(item.SubItems[2].Text.Where(char.IsDigit).ToArray())); // Adet sayısını alıyoruz
                        decimal price = 0;

                        // Puan fiyatını alıyoruz
                        using (SqlCommand priceCommand = new SqlCommand("SELECT Price FROM GamePoints WHERE PointType = @PointType", connection))
                        {
                            priceCommand.Parameters.AddWithValue("@PointType", pointType);

                            using (SqlDataReader reader = priceCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    price = reader.GetDecimal(0);  // Puan fiyatını alıyoruz
                                }
                                else
                                {
                                    MessageBox.Show($"Puan türü için fiyat bulunamadı: {pointType}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        decimal itemTotalPrice = price * quantity;

                        // Veritabanına sipariş bilgilerini ekliyoruz
                        using (SqlCommand insertCommand = new SqlCommand(
                            "INSERT INTO GamePoints_Sales (GameName, PointType, Quantity, Price, TotalPrice, UserID, PaymentMethod) " +
                            "VALUES (@GameName, @PointType, @Quantity, @Price, @TotalPrice, @UserID, @PaymentMethod)",
                            connection))
                        {
                            insertCommand.Parameters.AddWithValue("@GameName", gameName);
                            insertCommand.Parameters.AddWithValue("@PointType", pointType);
                            insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                            insertCommand.Parameters.AddWithValue("@Price", price);
                            insertCommand.Parameters.AddWithValue("@TotalPrice", itemTotalPrice);
                            insertCommand.Parameters.AddWithValue("@UserID", userId);
                            insertCommand.Parameters.AddWithValue("@PaymentMethod", "Nakit");

                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    // Kullanıcı bakiyesini güncelliyoruz
                    using (SqlCommand updateBalanceCommand = new SqlCommand(
                        "UPDATE Users SET Balance = Balance - @TotalPrice WHERE user_id = @UserID",
                        connection))
                    {
                        updateBalanceCommand.Parameters.AddWithValue("@TotalPrice", totalPrice);
                        updateBalanceCommand.Parameters.AddWithValue("@UserID", userId);
                        updateBalanceCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Sipariş başarıyla tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                listView1.Items.Clear();
                listView2.Items.Clear();
            }
        }








        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // Seçilen öğeyi al
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string gameName = selectedItem.Text; // Oyun adı (örneğin, Valorant veya LoL)
                string pointType = selectedItem.SubItems[1].Text; // Puan türü (örneğin, VP veya RP)
                decimal price = decimal.Parse(selectedItem.SubItems[2].Text, System.Globalization.NumberStyles.Currency); // Birim fiyat

                // Adet TextBox'ından değer al
                if (int.TryParse(txtCount.Text, out int quantity) && quantity > 0)
                {
                    decimal totalprice = price * quantity; // Toplam fiyatı hesapla

                    // Sipariş ListView'ine ekle
                    ListViewItem orderItem = new ListViewItem(gameName); // Oyun adı
                    orderItem.SubItems.Add(pointType); // Puan türü
                    orderItem.SubItems.Add(quantity.ToString()); // Adet
                    orderItem.SubItems.Add(price.ToString("C2")); // Birim fiyat
                    orderItem.SubItems.Add(totalprice.ToString("C2")); // Toplam fiyat
                    listView2.Items.Add(orderItem); // Sipariş ListView'ine ekle
                }
                else
                {
                    MessageBox.Show("Lütfen geçerli bir adet giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir ürün seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Game_points("Valorant");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Game_points("League of Legends");
        }

        private void button13_Click(object sender, EventArgs e)
        {

            if (listView2.SelectedItems.Count > 0)
            {
                // Seçili öğeyi kaldır
                listView2.Items.Remove(listView2.SelectedItems[0]);
                MessageBox.Show("Seçili sipariş iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCount.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen iptal etmek için bir sipariş seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GamePoints_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ana Sayfaya Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                AnaSayfa anaSayfa = new AnaSayfa();
                anaSayfa.user_role = this.user_role;
                anaSayfa.user_balance = this.user_balance;
                anaSayfa.user_mail = this.user_mail;

                anaSayfa.Show();
                this.Hide();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
