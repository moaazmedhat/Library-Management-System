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
    public partial class LoansForm : Form
    {
        public LoansForm()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=library.db;Version=3;";

        private void LoansForm_Load(object sender, EventArgs e)
        {
            LoadLoans();
        }

        // دالة عرض الاستعارات
        private void LoadLoans()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // هنجيب بيانات الاستعارة
                    string query = "SELECT * FROM Loans";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in loading loans" + ex.Message);
                }
            }
        }

        // زرار الاستعارة (Borrow Book) - لفل الوحش
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtBookISBN.Text == "" || txtMemberID.Text == "")
            {
                MessageBox.Show("Enter book's number and member's id");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // الخطوة 1: نتأكد إن الكتاب متاح أصلاً
                    string checkQuery = "SELECT Available_Copies FROM Books WHERE ISBN = @isbn";
                    int availableCopies = 0;

                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@isbn", txtBookISBN.Text);
                        object result = checkCmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Book doesnt exist in the library");
                            return;
                        }

                        availableCopies = Convert.ToInt32(result);
                    }

                    // الخطوة 2: لو مفيش نسخ، اكرشه
                    if (availableCopies <= 0)
                    {
                        MessageBox.Show("All book's copies have been borrowed.");
                        return;
                    }

                    // الخطوة 3: لو متاح، نسجل الاستعارة ونخصم النسخة (جوه Transaction عشان لو حاجة باظت التانية تقف)
                    using (SQLiteTransaction tr = conn.BeginTransaction())
                    {
                        // أ: إضافة الاستعارة (تاريخ النهاردة، وتاريخ الإرجاع بعد 14 يوم)
                        string insertQuery = @"INSERT INTO Loans (ISBN, Member_ID, Borrow_Date, Due_Date) 
                                       VALUES (@isbn, @member, DATE('now'), DATE('now', '+14 days'))";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn, tr))
                        {
                            insertCmd.Parameters.AddWithValue("@isbn", txtBookISBN.Text);
                            insertCmd.Parameters.AddWithValue("@member", txtMemberID.Text);
                            insertCmd.ExecuteNonQuery();
                        }

                        // ب: تقليل عدد النسخ المتاحة من جدول الكتب
                        string updateQuery = "UPDATE Books SET Available_Copies = Available_Copies - 1 WHERE ISBN = @isbn";
                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn, tr))
                        {
                            updateCmd.Parameters.AddWithValue("@isbn", txtBookISBN.Text);
                            updateCmd.ExecuteNonQuery();
                        }

                        tr.Commit(); // أكد العمليتين مع بعض
                    }

                    MessageBox.Show("The borrowing was successful.");
                    txtBookISBN.Clear();
                    txtMemberID.Clear();
                    LoadLoans(); // حدث الجدول
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured " + ex.Message);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (txtBookISBN.Text == "" || txtMemberID.Text == "")
            {
                MessageBox.Show("Enter book's number and member's id!");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // الخطوة 1: نتأكد إن الكتاب متاح أصلاً
                    string checkQuery = "SELECT Available_Copies FROM Books WHERE ISBN = @isbn";
                    int availableCopies = 0;

                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@isbn", txtBookISBN.Text);
                        object result = checkCmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Book doesn't exist in the library");
                            return;
                        }

                        availableCopies = Convert.ToInt32(result);
                    }

                    // الخطوة 2: لو مفيش نسخ، اكرشه
                    if (availableCopies <= 0)
                    {
                        MessageBox.Show("all copies have been borrowed.");
                        return;
                    }

                    // الخطوة 3: لو متاح، نسجل الاستعارة ونخصم النسخة (جوه Transaction عشان لو حاجة باظت التانية تقف)
                    using (SQLiteTransaction tr = conn.BeginTransaction())
                    {
                        // 
                        string insertQuery = @"INSERT INTO Loans (ISBN, Member_ID, Borrow_Date, Due_Date) 
                                       VALUES (@isbn, @member, DATE('now'), DATE('now', '+14 days'))";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn, tr))
                        {
                            insertCmd.Parameters.AddWithValue("@isbn", txtBookISBN.Text);
                            insertCmd.Parameters.AddWithValue("@member", txtMemberID.Text);
                            insertCmd.ExecuteNonQuery();
                        }

                        // ب: تقليل عدد النسخ المتاحة من جدول الكتب
                        string updateQuery = "UPDATE Books SET Available_Copies = Available_Copies - 1 WHERE ISBN = @isbn";
                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn, tr))
                        {
                            updateCmd.Parameters.AddWithValue("@isbn", txtBookISBN.Text);
                            updateCmd.ExecuteNonQuery();
                        }

                        tr.Commit(); // أكد العمليتين مع بعض
                    }

                    MessageBox.Show("Book borrowed succsessfully.");
                    txtBookISBN.Clear();
                    txtMemberID.Clear();
                    LoadLoans(); // حدث الجدول
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured " + ex.Message);
                }
            }
        }
    }
    }

