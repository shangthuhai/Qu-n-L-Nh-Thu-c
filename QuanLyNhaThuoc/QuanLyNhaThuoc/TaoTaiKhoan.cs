using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Add this namespace for SQL-related classes

namespace QuanLyNhaThuoc
{
    public partial class TaoTaiKhoan : Form
    {
        public TaoTaiKhoan()
        {
            InitializeComponent();
        }

        // Form đăng ký tài khoản nhân viên
        private void butDangKy_Click(object sender, EventArgs e)
        {
            string tenNhanVien = txtTenNhanVien.Text.Trim();
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(tenNhanVien) || string.IsNullOrEmpty(taiKhoan) ||
                string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=.;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Kiểm tra tài khoản đã tồn tại chưa
                    string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE TaiKhoan = @TaiKhoan";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Tài khoản đã tồn tại. Vui lòng chọn tên tài khoản khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = "INSERT INTO NhanVien (TenNhanVien, TaiKhoan, MatKhau, Email) VALUES (@TenNhanVien, @TaiKhoan, @MatKhau, @Email)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenNhanVien", tenNhanVien);
                        cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                        cmd.Parameters.AddWithValue("@Email", email);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Đăng ký tài khoản nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtTenNhanVien.Clear();
                            txtTaiKhoan.Clear();
                            txtMatKhau.Clear();
                            txtEmail.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Đăng ký tài khoản thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
