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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MembersForm mf = new MembersForm();
            mf.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoansForm lf = new LoansForm();
            lf.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReportsForm lf = new ReportsForm();
            lf.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=library.db;Version=3;"))
            {
                try
                {
                    conn.Open();
                    // كود النسف النووي
                    string query = @"
            DELETE FROM Loans;
            DELETE FROM Members;
            DELETE FROM Books;
            DELETE FROM sqlite_sequence;";

                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("All data deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Error " + ex.Message);
                }
            }
        }
    }
}
