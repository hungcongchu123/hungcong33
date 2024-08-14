using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace quản_lý_thư_viện
{
    public partial class Form1 : Form
    {
        private string username;
        private string password;
        private string connectionString = "Data Source=.;Initial Catalog=ABC_LIBRARY;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"; 
        public Form1(string username, string password)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT BranchID, BranchName, Address FROM Branches", connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data_branch.DataSource = dataTable;
                data_branch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void data_branch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = data_branch.Rows[e.RowIndex];
                txt_id.Text = row.Cells["BranchID"].Value.ToString();
                txt_id.Enabled = false;
                txt_name.Text = row.Cells["BranchName"].Value.ToString();
                txt_address.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Branches (BranchID, BranchName, Address) VALUES (@BranchID, @BranchName, @Address)", connection);
                    command.Parameters.AddWithValue("@BranchID", Convert.ToInt32(txt_id.Text));
                    command.Parameters.AddWithValue("@BranchName", txt_name.Text);
                    command.Parameters.AddWithValue("@Address", txt_address.Text);
                    command.ExecuteNonQuery();
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Failure: " + ex.Message);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE Branches SET BranchName = @BranchName, Address = @Address WHERE BranchID = @BranchID", connection);
                    command.Parameters.AddWithValue("@BranchID", Convert.ToInt32(txt_id.Text));
                    command.Parameters.AddWithValue("@BranchName", txt_name.Text);
                    command.Parameters.AddWithValue("@Address", txt_address.Text);
                    command.ExecuteNonQuery();
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Edit Failure: " + ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Branches WHERE BranchID = @BranchID", connection);
                    command.Parameters.AddWithValue("@BranchID", Convert.ToInt32(txt_id.Text));
                    command.ExecuteNonQuery();
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Failure: " + ex.Message);
            }
        }

        private void btn_res_Click(object sender, EventArgs e)
        {
            txt_address.Text = "";
            txt_id.Text = "";
            txt_name.Text = "";
            txt_search.Text = "";
            txt_id.Enabled = true;
            LoadData();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            librarian ac = new librarian(username, password);
            ac.ShowDialog();
            ac = null;
            this.Show();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_search.Text) || !int.TryParse(txt_search.Text, out _))
            {
                MessageBox.Show("No results were found!");
                LoadData();
            }
            else
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT BranchID, BranchName, Address FROM Branches WHERE BranchID = @BranchID", connection);
                        adapter.SelectCommand.Parameters.AddWithValue("@BranchID", Convert.ToInt32(txt_search.Text));
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        data_branch.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search Failure: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
