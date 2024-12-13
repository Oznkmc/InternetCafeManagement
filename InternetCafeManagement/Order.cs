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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InternetCafeManagement
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }
        public double user_balance { get; set; }

       
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

        //private void SaveOrderDetailsToDatabase(int orderId, System.Windows.Forms.ListView listViewOrders)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        foreach (ListViewItem item in listViewOrders.Items)
        //        {
        //            // Ürün bilgilerini ListView'den al
        //            string menuItemName = item.Text; // Ürün adı
        //            int quantity = int.Parse(item.SubItems[1].Text); // Adet
        //            decimal price = decimal.Parse(item.SubItems[2].Text, System.Globalization.NumberStyles.Currency); // Birim fiyat

        //            // MenuItemID'yi bulmak için bir sorgu yap
        //            string getMenuItemIdQuery = "SELECT MenuItemID FROM MenuItems WHERE Name = @Name";
        //            int menuItemId;
        //            using (SqlCommand getMenuItemCommand = new SqlCommand(getMenuItemIdQuery, connection))
        //            {
        //                getMenuItemCommand.Parameters.AddWithValue("@Name", menuItemName);
        //                menuItemId = Convert.ToInt32(getMenuItemCommand.ExecuteScalar());
        //            }

        //            // OrderDetails tablosuna veri ekle
        //            string insertDetailsQuery = @"
        //        INSERT INTO OrderDetails (OrderID, MenuItemID, Quantity, Price) 
        //        VALUES (@OrderID, @MenuItemID, @Quantity, @Price)";

        //            using (SqlCommand insertCommand = new SqlCommand(insertDetailsQuery, connection))
        //            {
        //                insertCommand.Parameters.AddWithValue("@OrderID", orderId);
        //                insertCommand.Parameters.AddWithValue("@MenuItemID", menuItemId);
        //                insertCommand.Parameters.AddWithValue("@Quantity", quantity);
        //                insertCommand.Parameters.AddWithValue("@Price", price * quantity); // Toplam fiyat
        //                insertCommand.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //}
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
                       decimal totalprice = price * quantity;

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
        private void button12_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Sipariş eklenmedi. Lütfen önce sipariş oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Masa numarasını almak
            // Masa numarasını kullanıcıdan alıyoruz
           

            // Masa ID'sini veritabanından almak
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand secilipcCommand = new SqlCommand("select computer_id from computers where name=@secilipc", connection);
                secilipcCommand.Parameters.AddWithValue("@secilipc", secilipc);
                object result5=secilipcCommand.ExecuteScalar();
                if(result5!=null)
                {
                    seciliid = (int)result5;
                }
                SqlCommand sqlCommand = new SqlCommand("SELECT session_id FROM sessions WHERE computer_id = @ComputerID", connection);
                sqlCommand.Parameters.AddWithValue("@ComputerID", seciliid);
                object result = sqlCommand.ExecuteScalar();

                if (result != null)
                {
                    sessionidgetir = (int)result; // Veritabanından alınan TableID
                }
                else
                {
                    MessageBox.Show("Seçilen oturum mevcut değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            decimal totalPrice = 0;

            foreach (ListViewItem item in listView2.Items)
            {
                int quantity = int.Parse(item.SubItems[1].Text); // Adet
                decimal price = decimal.Parse(item.SubItems[2].Text, System.Globalization.NumberStyles.Currency); // Birim fiyat
                 itemTotalPrice = quantity * price; // Adet x Birim Fiyat

                totalPrice += itemTotalPrice;
            }
            if(totalPrice>Convert.ToDecimal(user_balance))
            {
                MessageBox.Show("Hesap Bakiyesi Yetersiz.");
                listView1.Items.Clear();
                listView2.Items.Clear();
                //return;
            }
            else
            {
                DialogResult result6 = MessageBox.Show("Ödeme Tipiniz Nakit Mi?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result6 == DialogResult.Yes)
                {
                    payment_method = "Nakit";
                }
                else
                {
                    payment_method = "Banka Kartı";
                }
                // Siparişi Orders tablosuna kaydet ve OrderID'yi al
                int orderId = SaveOrderToDatabase(sessionidgetir, totalPrice, payment_method);
                UsersSession usersSession = new UsersSession();
                usersSession.user_balance = Convert.ToDouble(totalPrice);
                // Ödeme durumu
              



                // Sipariş ekranını temizle

                MessageBox.Show("Sipariş başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
            }
           

        }
    }
}
