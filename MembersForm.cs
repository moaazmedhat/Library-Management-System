using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_library_system
{
    public partial class MembersForm : Form
    {
        public MembersForm()
        {
            InitializeComponent();
        }
        string connectionString = "Data Source=library.db;Version=3;";
        private void MembersForm_Load(object sender, EventArgs e)
        {
            LoadMembers();
        }

        private void LoadMembers()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Members";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in loading members " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Enter member's name");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Members (Name, Phone, Email) VALUES (@name, @phone, @email)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Member added successfully");
                    txtName.Clear(); txtPhone.Clear(); txtEmail.Clear();
                    LoadMembers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Email already registered " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadMembers();
        }
    }
    }
    
