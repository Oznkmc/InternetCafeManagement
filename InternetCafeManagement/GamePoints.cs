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
                    "SELECT GameName, PointType, Price FROM GamePoints WHERE GameName = @GameName",
                    connection
                );

                // Seçilen oyun adı (örneğin, Valorant veya League of Legends)
                sqlCommand.Parameters.AddWithValue("@GameName", Category);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                listView1.Items.Clear(); // Önceki verileri temizle

                while (reader.Read())
                {
                    // Tablo sütunlarından veri al
                    string gameName = reader["GameName"].ToString();
                    string pointType = reader["PointType"].ToString();
                    decimal price = (decimal)reader["Price"];

                    // ListView'e verileri ekle
                    ListViewItem item = new ListViewItem(gameName); // Oyun adı
                    item.SubItems.Add(pointType);                  // Puan türü
                    item.SubItems.Add(price.ToString("C2"));       // Fiyat (para birimi formatında)

                    listView1.Items.Add(item);
                }

                reader.Close();
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
            if (listView2.Items.Count == 0) // Sipariş listesi boşsa uyarı göster
            {
                MessageBox.Show("Sipariş eklenmedi. Lütfen önce sipariş oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kullanıcının hediyesi var mı kontrol ediliyor
                if (!string.IsNullOrEmpty(secilihediye))
                {
                    // Kullanıcının hediyesi varsa, bakiye kontrolü yapmadan işlemleri gerçekleştir
                    foreach (ListViewItem item in listView2.Items)
                    {
                        if (item.SubItems[1].Text == secilihediye) // Eğer ürün hediyeyle eşleşiyorsa
                        {
                            string gameName = item.SubItems[0].Text; // Oyun Adı
                            string pointType = new string(secilihediye.Where(char.IsLetter).ToArray()); // Puan türü (VP veya RP)
                            string quantityNumberStr = new string(secilihediye.Where(char.IsDigit).ToArray()); // Sayısal kısmı
                            int quantity = int.Parse(quantityNumberStr); // Miktarı sayıya dönüştür

                            // Veritabanına sipariş kaydı
                            SqlCommand insertCommand = new SqlCommand(
                                "INSERT INTO GamePoints_Sales (GameName, PointType, Quantity, Price, TotalPrice, UserID, PaymentMethod) " +
                                "VALUES (@GameName, @PointType, @Quantity, @Price, 0, @UserID, 'Hediye')",
                                connection
                            );
                            insertCommand.Parameters.AddWithValue("@GameName", gameName);
                            insertCommand.Parameters.AddWithValue("@PointType", pointType);
                            insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                            insertCommand.Parameters.AddWithValue("@Price", 0); // Hediye olduğu için fiyat sıfır
                            insertCommand.Parameters.AddWithValue("@UserID", GetUserId(user_mail));

                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Hediye kullanılarak sipariş tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Kullanıcının hediyesi yoksa, bakiye kontrolü yapılır
                    SqlCommand balanceCommand = new SqlCommand("SELECT Balance FROM Users WHERE user_id = @UserID", connection);
                    balanceCommand.Parameters.AddWithValue("@UserID", GetUserId(user_mail));

                    decimal userBalance = 0;
                    using (SqlDataReader reader = balanceCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userBalance = reader.GetDecimal(0);
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bakiyesi alınamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Siparişin toplam fiyatını hesapla
                    decimal totalPrice = listView2.Items.Cast<ListViewItem>()
                        .Sum(item => decimal.Parse(item.SubItems[2].Text) * int.Parse(new string(item.SubItems[1].Text.Where(char.IsDigit).ToArray())));

                    if (userBalance < totalPrice)
                    {
                        MessageBox.Show("Yeterli bakiyeniz yok. Lütfen bakiye yükleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Sipariş detaylarını kaydet ve bakiye güncelle
                    foreach (ListViewItem item in listView2.Items)
                    {
                        string gameName = item.SubItems[0].Text;
                        string pointType = new string(item.SubItems[1].Text.Where(char.IsLetter).ToArray());
                        int quantity = int.Parse(new string(item.SubItems[1].Text.Where(char.IsDigit).ToArray()));
                        decimal price = decimal.Parse(item.SubItems[2].Text);

                        decimal itemTotalPrice = price * quantity;

                        SqlCommand insertCommand = new SqlCommand(
                            "INSERT INTO GamePoints_Sales (GameName, PointType, Quantity, Price, TotalPrice, UserID, PaymentMethod) " +
                            "VALUES (@GameName, @PointType, @Quantity, @Price, @TotalPrice, @UserID, @PaymentMethod)",
                            connection
                        );
                        insertCommand.Parameters.AddWithValue("@GameName", gameName);
                        insertCommand.Parameters.AddWithValue("@PointType", pointType);
                        insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                        insertCommand.Parameters.AddWithValue("@Price", price);
                        insertCommand.Parameters.AddWithValue("@TotalPrice", itemTotalPrice);
                        insertCommand.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                        insertCommand.Parameters.AddWithValue("@PaymentMethod", "Nakit"); // Ödeme yöntemi örnek

                        insertCommand.ExecuteNonQuery();
                    }

                    // Kullanıcı bakiyesini güncelle
                    SqlCommand updateBalanceCommand = new SqlCommand(
                        "UPDATE Users SET Balance = Balance - @TotalPrice WHERE user_id = @UserID",
                        connection
                    );
                    updateBalanceCommand.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    updateBalanceCommand.Parameters.AddWithValue("@UserID", GetUserId(user_mail));
                    updateBalanceCommand.ExecuteNonQuery();

                    MessageBox.Show("Sipariş başarıyla tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Sipariş ekranını temizle
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
    }
}
