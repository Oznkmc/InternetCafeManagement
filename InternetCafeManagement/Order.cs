using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InternetCafeManagement
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }
        public decimal user_balance { get; set; }

       
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        public string MusteriAdi { get; set; }
        public string secilipc {  get; set; }
        public string user_mail {  get; set; }
        
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
        private int GetComputerId(string computerName)
        {
            int computerId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT computer_id FROM computers WHERE name = @ComputerName", connection);
                command.Parameters.AddWithValue("@ComputerName", computerName);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    computerId = Convert.ToInt32(reader["computer_id"]);
                }
            }
            return computerId;
        }
        private int SaveOrderToDatabase(int tableID, decimal totalPrice,string payment_method)
        {
            int orderId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
    INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod,SessionID)
    VALUES (@CustomerID, @OrderDate, @TotalPrice, @PaymentMethod,@SessionID);
    SELECT SCOPE_IDENTITY();";
                SqlCommand sessionid = new SqlCommand("select session_id from sessions where computer_id=@ComputerID and status=1", connection);
                sessionid.Parameters.AddWithValue("@ComputerID", GetComputerId(secilipc));
                object result9= sessionid.ExecuteScalar();
                if (result9 != null)
                {
                    int sessionidgetir=(int) result9;
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametreleri ekleyin
                        command.Parameters.AddWithValue("@OrderDate", DateTime.Now);   // Siparişin tarihini ekliyoruz
                        command.Parameters.AddWithValue("@TotalPrice", totalPrice);    // Toplam tutar
                        command.Parameters.AddWithValue("@PaymentMethod", payment_method); // Ödeme şekli
                        command.Parameters.AddWithValue("@CustomerID", GetUserId(user_mail)); // Kullanıcı ID'si
                        command.Parameters.AddWithValue("@SessionID", sessionidgetir);
                        // OrderID'yi almak
                        orderId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
             
            }
                     

            return orderId;
        }

        private int SaveOrderToDatabaseInActiveUsersSession(decimal totalPrice, string payment_method)
        {
            int orderId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
        INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod)
        VALUES (@CustomerID, @OrderDate, @TotalPrice, @PaymentMethod);
        SELECT SCOPE_IDENTITY();"; // Yeni siparişin ID'sini alıyoruz

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreleri ekleyin
                    command.Parameters.AddWithValue("@OrderDate", DateTime.Now);         // Siparişin tarihini ekliyoruz
                    command.Parameters.AddWithValue("@TotalPrice", totalPrice);         // Toplam tutar
                    command.Parameters.AddWithValue("@PaymentMethod", payment_method);  // Ödeme şekli
                    command.Parameters.AddWithValue("@CustomerID", GetUserId(user_mail)); // Kullanıcı ID'si

                    // OrderID'yi almak
                    orderId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return orderId;
        }

        

        private void LoadAnaYemekItemsToListView(string Category)
        {



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT Name, Price FROM products WHERE product_type =@Category", connection); // Ana yemekleri getir
                sqlCommand.Parameters.AddWithValue("@Category", Category);


                SqlDataReader reader = sqlCommand.ExecuteReader();
                listView1.Items.Clear(); // Önceki verileri temizle
                while (reader.Read())
                {

                    string itemName = reader["Name"].ToString();
                    decimal price = (decimal)reader["Price"];

                    // ListView'e ekleyin
                    ListViewItem item = new ListViewItem(itemName);
                    item.SubItems.Add(price.ToString("C2")); // Fiyatı para birimi formatında ekleyin
                    listView1.Items.Add(item);
                }
                reader.Close();
            }
        }
        private void ShowPlaceholderMailGiris()
        {
            // Eğer TextBox boşsa placeholder göster
            if (string.IsNullOrEmpty(txtCount.Text))
            {
                txtCount.Text = "Adet Giriniz";
                txtCount.ForeColor = Color.Gray; // Placeholder rengi

            }
        }
        private void HidePlaceholderMailGiris()
        {
            // Eğer placeholder görünüyorsa temizle
            if (txtCount.Text == "Adet Giriniz")
            {
                txtCount.Text = "";
                txtCount.ForeColor = Color.Black; // Yazı rengini normal yap

            }
        }
        private void Order_Load(object sender, EventArgs e)
        {
          
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadAnaYemekItemsToListView("Yemek");
        }
        private decimal totalprice;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoadAnaYemekItemsToListView("İçecek");
        }
 
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
                // Seçili menü öğesini al
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = listView1.SelectedItems[0];
                    string itemName = selectedItem.Text; // Ürün adı
                    decimal price = decimal.Parse(selectedItem.SubItems[1].Text, System.Globalization.NumberStyles.Currency); // Fiyat
                    
                    // Adet TextBox'ından değer al
                    if (int.TryParse(txtCount.Text, out int quantity) && quantity > 0)
                    {
                        totalprice = price * quantity;

                        // Sipariş ListView'ine ekle
                        ListViewItem orderItem = new ListViewItem(itemName); // Ürün adı
                        orderItem.SubItems.Add(quantity.ToString()); // Adet
                        orderItem.SubItems.Add(price.ToString("C2")); // Birim fiyat
                        orderItem.SubItems.Add(totalprice.ToString("C2")); // Toplam fiyat
                        listView2.Items.Add(orderItem);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir adet giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            
        }
        public int seciliid;
        private int sessionidgetir;
        string payment_method;
        decimal itemTotalPrice;
        public bool UsersSessionActive {  get; set; }
        public void UpdateUserBalance()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Bakiyeyi güncelleyen sorgu
                    SqlCommand updateBalance = new SqlCommand("UPDATE users SET balance = balance - @TotalPrice WHERE email = @userMail", connection);
                    updateBalance.Parameters.AddWithValue("@userMail", user_mail);

                    // totalprice doğrudan double'a çevrilmeden önce kontrol ediliyor
                    double totalPriceValue;
                    if (!double.TryParse(totalprice.ToString(), out totalPriceValue))
                    {
                        throw new Exception("Geçerli bir toplam fiyat değeri sağlanmadı.");
                    }
                    updateBalance.Parameters.AddWithValue("@TotalPrice", totalPriceValue);

                    // Güncelleme işlemini gerçekleştir
                    int rowsAffected = updateBalance.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Yeni bakiye sorgusu
                        SqlCommand command1 = new SqlCommand("SELECT balance FROM users WHERE email = @userMail", connection);
                        command1.Parameters.AddWithValue("@userMail", user_mail);

                        object result = command1.ExecuteScalar();
                        if (result != null && double.TryParse(result.ToString(), out double balance))
                        {
                            MessageBox.Show("Bakiyeniz Güncellendi. Yeni Bakiyeniz: " + balance);
                        }
                        else
                        {
                            MessageBox.Show("Bakiyeniz alınamadı.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme işlemi başarısız oldu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata Nedeni: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }


        }
    
        private void button12_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Sipariş eklenmedi. Lütfen önce sipariş oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (UsersSessionActive == true)
                {
                    // Seçilen bilgisayarın ID'sini almak için SQL komutunu oluşturuyoruz
                    SqlCommand secilipcCommand = new SqlCommand("SELECT computer_id FROM computers WHERE name = @secilipc", connection);
                    secilipcCommand.Parameters.AddWithValue("@secilipc", secilipc);
                    object result5 = secilipcCommand.ExecuteScalar();

                    if (result5 != null)
                    {
                        seciliid = (int)result5;
                    }
                    else
                    {
                        MessageBox.Show("Seçilen bilgisayar bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }



                    // Seçilen bilgisayarın oturum bilgisini alıyoruz
                    SqlCommand sqlCommand = new SqlCommand("SELECT session_id FROM sessions WHERE computer_id = @ComputerID", connection);
                    sqlCommand.Parameters.AddWithValue("@ComputerID", seciliid);
                    object result = sqlCommand.ExecuteScalar();

                    if (result != null)
                    {
                        sessionidgetir = (int)result;
                    }
                    else
                    {
                        MessageBox.Show("Seçilen oturum mevcut değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // Siparişin toplam fiyatını hesaplıyoruz
            decimal totalPrice = 0;

            foreach (ListViewItem item in listView2.Items)
            {
                int quantity = int.Parse(item.SubItems[1].Text); // Adet
                decimal price = decimal.Parse(item.SubItems[2].Text, System.Globalization.NumberStyles.Currency); // Birim fiyat
                decimal itemTotalPrice = quantity * price; // Adet x Birim Fiyat
                totalPrice += itemTotalPrice;
            }
            using (SqlConnection sqlConnection=new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand1 = new SqlCommand("select balance from users where user_id=@userID", sqlConnection);
                    sqlCommand1.Parameters.AddWithValue("@userID", GetUserId(user_mail));
                    object resultsqlcommand = sqlCommand1.ExecuteScalar();
                    if (resultsqlcommand != null)
                    {
                        decimal balance = (decimal)resultsqlcommand;
                        if (totalPrice > Convert.ToDecimal(balance))
                        {
                            MessageBox.Show("Hesap bakiyesi yetersiz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            listView1.Items.Clear();
                            listView2.Items.Clear();
                            return;
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Hatanız:" + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
               
            }
             


            // Kullanıcı bakiyesi kontrolü
          

            // Ödeme tipini soruyoruz
            DialogResult result6 = MessageBox.Show("Ödeme Tipiniz Nakit Mi?", "Uygulama Çıkışı", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result6 == DialogResult.Cancel)
            {
                // Kullanıcı vazgeçti, işlem yapılmıyor
                MessageBox.Show("İşlem iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Kullanıcının seçimine göre ödeme tipi belirleniyor
                string payment_method = result6 == DialogResult.Yes ? "Nakit" : "Banka Kartı";
                int orderId;

                if (UsersSessionActive)
                {
                    // Kullanıcı oturumu aktifken siparişi kaydet
                    orderId = SaveOrderToDatabase(sessionidgetir, totalPrice, payment_method);
                    MessageBox.Show("Sipariş başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateUserBalance();
                }
                else
                {
                    // Kullanıcı oturumu aktif değilken siparişi kaydet
                    orderId = SaveOrderToDatabaseInActiveUsersSession(totalPrice, payment_method);
                    MessageBox.Show("Sipariş başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateUserBalance();
                }
            }



            // Sipariş ekranını temizliyoruz
            listView1.Items.Clear();
            listView2.Items.Clear();

            // Kullanıcıya siparişin başarıyla kaydedildiği mesajını gösteriyoruz
        
            
            Order order=new Order();
            order.Hide();
           



        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtCount_Leave(object sender, EventArgs e)
        {
            
        }

        private void txtCount_Enter(object sender, EventArgs e)
        {
           
        }
    }
}
