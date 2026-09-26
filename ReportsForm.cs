using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace DB_library_system
{
    public partial class ReportsForm : Form
    {
        // سطر الاتصال العمدة بتاعنا
        string connectionString = "Data Source=library.db;Version=3;";

        public ReportsForm()
        {
            InitializeComponent();
        }
        private void ReportsForm_Load(object sender, EventArgs e)
        {
            // سايبينها فاضية بس عشان الفيجوال ستوديو يبطل صياح
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                  
                    string query = @"
                        SELECT Books.Title AS 'اسم الكتاب', Members.Name AS 'اسم العضو', Loans.Borrow_Date AS 'تاريخ الاستعارة'
                        FROM Loans
                        INNER JOIN Books ON Loans.ISBN = Books.ISBN
                        INNER JOIN Members ON Loans.Member_ID = Members.Member_ID";

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // اعرض النتيجة في الشبكة
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in borrowing report " + ex.Message);
                }
            }
        }

        // --- ده صندوق الزرار التاني (اربطه بزرار الإحصائيات من الديزاين) ---
        private void button2_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT '  total books' AS 'البيان', COUNT(ISBN) AS 'value' FROM Books
                        UNION
                        SELECT 'copies left in storage', SUM(Available_Copies) FROM Books
                        UNION
                        SELECT 'number of registered members', COUNT(Member_ID) FROM Members";

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // اعرض النتيجة   
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in statistics report " + ex.Message);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
            SELECT 'إجمالي أنواع الكتب' AS 'البيان', COUNT(ISBN) AS 'القيمة' FROM Books
            UNION
            SELECT 'النسخ المتاحة في المخزن', SUM(Available_Copies) FROM Books
            UNION
            SELECT 'عدد الأعضاء المسجلين', COUNT(Member_ID) FROM Members";

                    System.Data.SQLite.SQLiteDataAdapter adapter = new System.Data.SQLite.SQLiteDataAdapter(query, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("إيرور في تقرير الإحصائيات: " + ex.Message);
                }
            }
        }
    }
}