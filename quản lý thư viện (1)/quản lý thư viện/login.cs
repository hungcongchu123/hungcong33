using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace quản_lý_thư_viện
{
    public partial class login : Form
    {
        private string connectionString = "Data Source=.;Initial Catalog=ABC_LIBRARY;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public login()
        {
            InitializeComponent();
        }

        public string username = "";
        public string password = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string us = txt_us.Text;
            string ps = txt_ps.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT UserName, PassWord FROM Login WHERE UserName = @UserName AND PassWord = @PassWord";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", us);
                        command.Parameters.AddWithValue("@PassWord", ps);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.username = us;
                                this.password = ps;

                                this.Hide();
                                librarian ac = new librarian(username, password);
                                ac.ShowDialog();
                                ac = null;
                                this.Show();
                            }
                            else
                            {
                                MessageBox.Show("Account or password is incorrect");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Register ac = new Register();
            ac.ShowDialog();
            ac = null;
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
