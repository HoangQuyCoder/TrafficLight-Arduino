using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace TrafficLight
{
    public partial class Database : Form
    {
<<<<<<< HEAD
        private string connectionString = "Server=MSI\\MYDATABASE;Database=TRAFFIC_LIGHT;Integrated Security=True;";
=======
        private string connectionString = "Server=THEODORE;Database=TRAFFIC_LIGHT;Integrated Security=True;";
>>>>>>> 3a541463f93918d4cbcbeb34dba05c1466f343bc

        public Database()
        {
            InitializeComponent();
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM UserModeHistory";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        //dataGridView1.Refresh();
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dataGridView1.AutoGenerateColumns = true;
                        // Bind the DataTable directly to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("No data found in the UserModeHistory table.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data from the database: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = (Form1)Application.OpenForms["Form1"];
            form1.Show();
            this.Hide();
        }

    }
}
