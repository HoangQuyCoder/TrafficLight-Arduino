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

namespace TrafficLight
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            LoadComboBox();
        }

        private string connectionString = "Server=THEODORE;Database=TRAFFIC_LIGHT;Integrated Security=True;";

        private bool AuthenticateUser(string username, string password, out string role)
        {
            role = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Role FROM Users WHERE UserName = @username AND Password = @password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        role = reader["Role"].ToString();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBoxName.Text;
            string password = textBoxPassword.Text;

            if (AuthenticateUser(username, password))
            {
                MessageBox.Show("Login successful!");

                string selectedCOMPort = comboBox1.SelectedItem?.ToString() ?? "COM1";

                Form1 mainForm = new Form1(selectedCOMPort);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT 1 FROM Users WHERE UserName = @username AND Password = @password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = command.ExecuteReader();
                    return reader.HasRows;  // Returns true if a matching record is found
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
        private void LoadComboBox()
        {
            for (int i = 1; i <= 5; i++) // Adjust the range as needed
            {
                comboBox1.Items.Add($"COM{i}");
            }
        }

    }
}
