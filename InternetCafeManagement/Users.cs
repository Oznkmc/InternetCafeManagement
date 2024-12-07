using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InternetCafeManagement
{
    public partial class Users : Form
    {
        public Users()
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
        public double user_balance { get; set; }
        void griddoldur()
        {
            con = new SqlConnection(connectionString);
            da = new SqlDataAdapter("Select * from users", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "users");
            dataGridView1.DataSource = ds.Tables["users"];
            con.Close();
        }
        void KayıtSil(string email)
        {
           
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                try
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand("delete from users where email=@UserMail", connection);
                    com.Parameters.AddWithValue("@UserMail", email);
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
        private void Users_Load(object sender, EventArgs e)
        {
            griddoldur();
            txtFirstName.Enter += txtFirstName_Enter;
            txtFirstName.Leave += txtFirstName_Leave;

            txtLastName.Enter += txtLastName_Enter;
            txtLastName.Leave += txtLastName_Leave;

            txtPassword.Enter += txtPassword_Enter;
            txtPassword.Leave += txtPassword_Leave;

            Balance.Enter += Balance_Enter;
            Balance.Leave += Balance_Leave;

            txtMail.Enter += txtMail_Enter;
            txtMail.Leave += txtMail_Leave;
            txtPhone.Enter += txtPhone_Enter;
            txtPhone.Leave += txtPhone_Leave;
        }
        private void ShowPlaceholderAd()
        {
            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                txtFirstName.Text = "Ad giriniz";
                txtFirstName.ForeColor = Color.Gray; // Placeholder rengi
            }
        }

        private void HidePlaceholderAd()
        {
            if (txtFirstName.Text == "Ad giriniz")
            {
                txtFirstName.Text = "";
                txtFirstName.ForeColor = Color.Black; // Yazı rengini normal yap
            }
        }

        private void ShowPlaceholderSoyad()
        {
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                txtLastName.Text = "Soyad giriniz";
                txtLastName.ForeColor = Color.Gray;
            }
        }

        private void HidePlaceholderSoyad()
        {
            if (txtLastName.Text == "Soyad giriniz")
            {
                txtLastName.Text = "";
                txtLastName.ForeColor = Color.Black;
            }
        }
        private void ShowPlaceholderMail()
        {
            if (string.IsNullOrEmpty(txtMail.Text))
            {
                txtMail.Text = "Mail giriniz:(@gmail.com)";
                txtMail.ForeColor = Color.Gray;
            }
        }

        private void HidePlaceholderMail()
        {
            if (txtMail.Text == "Mail giriniz:(@gmail.com)")
            {
                txtMail.Text = "";
                txtMail.ForeColor = Color.Black;
            }
        }
        private void ShowPlaceholderPhone()
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                txtPhone.Text = "Telefon Numarası Giriniz";
                txtPhone.ForeColor = Color.Gray;
            }
        }
        private void HidePlaceholderPhone()
        {
            if (txtPhone.Text == "Telefon Numarası Giriniz")
            {
                txtPhone.Text = "";
                txtPhone.ForeColor = Color.Black;
            }
        }
        private void ShowPlaceholderSif()
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Text = "Şifre giriniz";
                txtPassword.ForeColor = Color.Gray;
               
            }
        }

        private void HidePlaceholderSif()
        {
            if (txtPassword.Text == "Şifre giriniz")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                
            }
        }

        private void ShowPlaceholderBalance()
        {
            if (string.IsNullOrEmpty(Balance.Text))
            {
                Balance.Text = "Bakiye Giriniz";
                Balance.ForeColor = Color.Gray;
            }
        }
        private void HidePlaceholderBalance()
        {
            if (txtPhone.Text == "Bakiye Giriniz")
            {
                Balance.Text = "";
                Balance.ForeColor = Color.Black;
            }
        }

        private void txtFirstName_Click(object sender, EventArgs e)
        {
            
        }

        private void txtFirstName_Enter(object sender, EventArgs e)
        {
            HidePlaceholderAd();
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderAd();
        }

        private void txtLastName_Enter(object sender, EventArgs e)
        {
            HidePlaceholderSoyad();
        }

        private void txtLastName_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderSoyad();
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            HidePlaceholderSif();
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderSif();
        }

        private void txtMail_Enter(object sender, EventArgs e)
        {
            HidePlaceholderMail();
        }

        private void txtMail_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderMail();
        }

        private void txtPhone_Enter(object sender, EventArgs e)
        {
            HidePlaceholderPhone();
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
               ShowPlaceholderPhone();
        }

        private void Balance_Enter(object sender, EventArgs e)
        {
            HidePlaceholderPhone();
        }

        private void Balance_Leave(object sender, EventArgs e)
        {
            ShowPlaceholderPhone();
        }

        private void pictureAdd_Click(object sender, EventArgs e)
        {
            string mailkontrol = txtMail.Text;
            bool mailsyntax = false;

            if (mailkontrol.EndsWith("@gmail.com"))
            {
                mailsyntax = true;
            }
            else
            {
                mailsyntax = false;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Mail kontrolü için sorgu
                    SqlCommand MailKontrol = new SqlCommand("SELECT * FROM users WHERE email = @UserMail", connection);
                    MailKontrol.Parameters.AddWithValue("@UserMail", txtMail.Text);

                    SqlDataReader dr = MailKontrol.ExecuteReader();

                    if (dr.Read())
                    {
                        MessageBox.Show("Böyle bir mail adresi zaten mevcut.");
                    }
                    else
                    {
                        // DataReader'ı kapat
                        dr.Close();

                        if (mailsyntax && txtPhone.Text.Length == 10)
                        {
                            // Yeni kullanıcı ekleme
                            SqlCommand AddUser = new SqlCommand(
                                "INSERT INTO users(firstname, lastname, password, email, phonenumber, balance) " +
                                "VALUES(@FirstName, @LastName, @Password, @Email, @Phone, @Balance)",
                                connection);

                            AddUser.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                            AddUser.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            AddUser.Parameters.AddWithValue("@Password", txtPassword.Text);
                            AddUser.Parameters.AddWithValue("@Email", txtMail.Text);
                            AddUser.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            AddUser.Parameters.AddWithValue("@Balance", Balance.Text);

                            AddUser.ExecuteNonQuery();

                            MessageBox.Show("Kullanıcı Başarıyla Eklenmiştir.");

                            
                        }
                        else if (!mailsyntax)
                        {
                            MessageBox.Show("Mail Adresiniz Uyumsuz.");
                        }

                        if (txtPhone.Text.Length != 10)
                        {
                            MessageBox.Show("Telefon Numarası en az 10 haneli olmalı.");
                        }
                        griddoldur();

                        // Form alanlarını temizle
                        txtFirstName.Clear();
                        txtLastName.Clear();
                        txtPassword.Clear();
                        txtMail.Clear();
                        txtPhone.Clear();
                        Balance.Clear();
                    }

                    // DataReader açık kalmışsa kapat
                    if (!dr.IsClosed)
                    {
                        dr.Close();
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
                string mail = drow.Cells[5].Value.ToString();
                KayıtSil(mail);
            }
            griddoldur();
        }

        private void pictureUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand user_id = new SqlCommand("select user_id from users where email=@UserMail", connection);
                user_id.Parameters.AddWithValue("@UserMail", txtMail.Text);
                object result = user_id.ExecuteScalar();
                if(result!=null)
                {
                    try
                    {
                        int idgetir = (int)result;
                        SqlCommand UpdateUser = new SqlCommand("UPDATE users set firstname=@FirstName,lastname=@LastName,password=@password,email=@email,phonenumber=@Phonenumber,balance=@balance where user_id=@userid", connection);
                        UpdateUser.Parameters.AddWithValue("userid", idgetir);
                        UpdateUser.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        UpdateUser.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        UpdateUser.Parameters.AddWithValue("@password", txtPassword.Text);
                        UpdateUser.Parameters.AddWithValue("@email", txtMail.Text);
                        UpdateUser.Parameters.AddWithValue("@phonenumber", txtPhone.Text);
                        UpdateUser.Parameters.AddWithValue("@balance", Convert.ToInt32(Balance.Text));
                        UpdateUser.ExecuteNonQuery();
                        griddoldur();
                        MessageBox.Show("Kullanıcı Başarıyla Güncellenmiştir.");
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Hatanızın Nedeni:" + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
             

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            //string ad = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            //string soyad= dataGridView1.Rows[secilialan].Cells[3].Value.ToString();
            //string password= dataGridView1.Rows[secilialan].Cells[4].Value.ToString();
            //string email = dataGridView1.Rows[secilialan].Cells[5].Value.ToString();
            //string phonenumber = dataGridView1.Rows[secilialan].Cells[6].Value.ToString();
            //int balance = Convert.ToInt32(dataGridView1.Rows[secilialan].Cells[8].Value);
            //txtFirstName.Text = ad; 
            //txtLastName.Text=soyad;
            //txtPassword.Text = password;
            //txtMail.Text = email;
            //txtPhone.Text= phonenumber;
            //Balance.Text = balance.ToString();
            if (e.RowIndex >= 0) // Tıklanan bir satır olduğundan emin ol
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // TextBoxlara seçili satırın değerlerini aktar
                txtFirstName.Text = row.Cells["firstname"].Value.ToString();
                txtLastName.Text = row.Cells["lastname"].Value.ToString();
                txtPassword.Text = row.Cells["password"].Value.ToString();
                txtMail.Text = row.Cells["email"].Value.ToString();
                txtPhone.Text = row.Cells["phonenumber"].Value.ToString();
                Balance.Text = row.Cells["balance"].Value.ToString();
                
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
          
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamadan Çıkıyorsun. Emin Misin?", "Uygulama Çıkışı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
