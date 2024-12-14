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
    public partial class NewPasswordForm : Form
    {
        public string userEmail;  // Kullanıcının e-posta adresi
         
        public NewPasswordForm(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string newPassword = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Yeni şifreyi girin.");
                return;
            }

            // Şifreyi veritabanına güncelle
            UpdatePassword(userEmail, newPassword);

            MessageBox.Show("Şifreniz başarıyla güncellendi.");
            
        }
        string connectionString = "Data Source=DESKTOP-AGLHO45\\SQLEXPRESS;Initial Catalog=InternetCafeManagement;Integrated Security=True";
        private void UpdatePassword(string email, string newPassword)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();


                SqlCommand UpdateCommand = new SqlCommand("Update users SET password=@NewPassword where email=@UserMail", connection);
                UpdateCommand.Parameters.AddWithValue("@UserMail", email);
                UpdateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                int row = UpdateCommand.ExecuteNonQuery();
                if (row > 0)
                {
                    
                    ÜyeGirisPaneli üyeGirisPaneli=new ÜyeGirisPaneli();
                    üyeGirisPaneli.Show();
                    this.Close();
                }
                
            }
        }
    }
}
