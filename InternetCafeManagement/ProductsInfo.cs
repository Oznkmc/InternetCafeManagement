using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InternetCafeManagement
{
    public partial class ProductsInfo : Form
    {
        public ProductsInfo()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand com = new SqlCommand();
        DataSet ds = new DataSet();
        public bool user_role { get; set; }
        public string user_mail { get; set; }
        void griddoldur()
        {

            con = new SqlConnection(connectionString);
            da = new SqlDataAdapter("Select * from products", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "products");
            dataGridView1.DataSource = ds.Tables["products"];
            con.Close();
        }
        void KayıtSil(string name)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand("delete from products where name=@UrunAdı", connection);
                    com.Parameters.AddWithValue("@UrunAdı", txtUrunName.Text);
                    com.ExecuteNonQuery();
                    MessageBox.Show("Kullanıcı Başarıyla Silinmiştir.");
                    griddoldur();
                    connection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata:" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }




        }
        private void ProductsInfo_Load(object sender, EventArgs e)
        {
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    try
            //    {
            //        connection.Open();

            //        // SQL sorgusu
            //        SqlCommand command = new SqlCommand("SELECT * FROM products", connection);
            //        SqlDataReader dr = command.ExecuteReader();

            //        // cmbProducts burada bir ComboBox olarak ele alınmalı
            //        cmbTip.Items.Clear(); // Önce listeyi temizleyin
            //        while (dr.Read())
            //        {
            //            cmbTip.Items.Add(dr["product_type"].ToString());
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Hatanızın Nedeni: " + ex.Message);
            //    }
            //    finally
            //    {
            //        connection.Close();
            //    }
            //}
            cmbTip.Items.Add("Yemek");
            cmbTip.Items.Add("İçecek");


            griddoldur();
        }

        private void pictrureAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Ürün kontrol sorgusu
                    SqlCommand UrunKontrol = new SqlCommand("SELECT * FROM products WHERE name = @ProductName", connection);
                    UrunKontrol.Parameters.AddWithValue("@ProductName", txtUrunName.Text);

                    SqlDataReader sqlDataReader = UrunKontrol.ExecuteReader();

                    if (sqlDataReader.Read()) // Eğer ürün varsa
                    {
                        MessageBox.Show("Böyle bir ürün zaten mevcut");
                    }
                    else
                    {
                        // Ürün yoksa yeni ürün eklemek için okuyucuyu kapat
                        sqlDataReader.Close();

                        SqlCommand com = new SqlCommand("INSERT INTO products(name, price, product_type) VALUES (@ProductName, @ProductPrice, @ProductType)", connection);
                        com.Parameters.AddWithValue("@ProductName", txtUrunName.Text);

                        // Fiyatın sayıya çevrilmesi ve kontrol edilmesi
                        if (decimal.TryParse(txtFiyat.Text, out decimal fiyat))
                        {
                            com.Parameters.AddWithValue("@ProductPrice", fiyat);
                        }
                        else
                        {
                            MessageBox.Show("Lütfen geçerli bir fiyat girin.");
                            return;
                        }

                        com.Parameters.AddWithValue("@ProductType", cmbTip.Text);

                        com.ExecuteNonQuery();
                        MessageBox.Show("Ürün Başarıyla Eklenmiştir.");
                        griddoldur();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hatanızın Nedeni: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        private void pictureDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drow in dataGridView1.SelectedRows)  //Seçili Satırları Silme
            {
                string UrunName = drow.Cells[1].Value.ToString();
                KayıtSil(UrunName);
            }
            griddoldur();
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Tıklanan bir satır olduğundan emin ol
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // TextBoxlara seçili satırın değerlerini aktar
                txtUrunName.Text = row.Cells["name"].Value.ToString();
                txtFiyat.Text = row.Cells["price"].Value.ToString();
                cmbTip.Text = row.Cells["product_type"].Value.ToString();
            }
        }

        private void pictureUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Ürün ID'sini almak için sorgu
                    SqlCommand idgetir = new SqlCommand("SELECT product_id FROM products WHERE name=@ProductName", connection);
                    idgetir.Parameters.AddWithValue("@ProductName", txtUrunName.Text);

                    object result = idgetir.ExecuteScalar(); // Tek bir değer döner (product_id)
                    if (result != null)
                    {
                        int id = (int)result;

                        // Güncelleme komutu
                        SqlCommand updateProduct = new SqlCommand("UPDATE products SET name=@NewProductName, price=@ProductPrice, product_type=@ProductType WHERE product_id=@ProductID", connection);
                        updateProduct.Parameters.AddWithValue("@NewProductName", txtUrunName.Text); // Yeni ürün adı
                        updateProduct.Parameters.AddWithValue("@ProductPrice", Convert.ToDecimal(txtFiyat.Text)); // Fiyat
                        updateProduct.Parameters.AddWithValue("@ProductType", cmbTip.Text); // Ürün tipi
                        updateProduct.Parameters.AddWithValue("@ProductID", id); // ID

                        int rowsAffected = updateProduct.ExecuteNonQuery(); // Güncelleme işlemi

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Ürün bilgileri başarıyla güncellenmiştir.");
                            griddoldur(); // Grid'i güncelle
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme sırasında bir hata oluştu. Satır etkilenmedi.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen ürün bulunamadı. Lütfen ürün adını kontrol edin.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hatanızın Nedeni: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Admin Paneline Dönüyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                admin Admin = new admin();
                Admin.role = this.user_role;

                Admin.usermail = this.user_mail;

                Admin.Show();
                this.Hide();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
