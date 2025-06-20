using System;
using System.Collections.Generic;   
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class Form2 : Form
    {
        private string connectionString = @"Data Source=SHANGPC\MSSQLSERVER1;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True";
        private DataTable nhanVienTable = new DataTable();

        public Form2()
        {
            InitializeComponent();
            LoadNhanVien();

        }

        private void LoadNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Email, TenNhanVien, TaiKhoan, MatKhau FROM NhanVien";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                nhanVienTable.Clear();
                adapter.Fill(nhanVienTable);
                dataGridView1.DataSource = nhanVienTable;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int rowIndex = dataGridView1.CurrentRow.Index;
                if (rowIndex >= 0 && rowIndex < nhanVienTable.Rows.Count)
                {
                    DataRow row = nhanVienTable.Rows[rowIndex];
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE NhanVien SET TenNhanVien = @TenNhanVien, Email = @Email, TaiKhoan = @TaiKhoan, MatKhau = @MatKhau WHERE Email = @Email";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@TenNhanVien", row["TenNhanVien"]);
                        cmd.Parameters.AddWithValue("@Email", row["Email"]);
                        cmd.Parameters.AddWithValue("@TaiKhoan", row["TaiKhoan"]);
                        cmd.Parameters.AddWithValue("@MatKhau", row["MatKhau"]);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    LoadNhanVien();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int rowIndex = dataGridView1.CurrentRow.Index;
                if (rowIndex >= 0 && rowIndex < nhanVienTable.Rows.Count)
                {
                    DataRow row = nhanVienTable.Rows[rowIndex];
                    DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            string query = "DELETE FROM NhanVien WHERE Email = @Email";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@Email", row["Email"]);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        LoadNhanVien();
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
