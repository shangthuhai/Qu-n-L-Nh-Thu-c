using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyNhaThuoc
{
    public partial class TaoTaiKhoan : Form
    {
        private string connectionString = @"Data Source=SHANGPC\MSSQLSERVER1;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True";

        public TaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void btnTao_Click_1(object sender, EventArgs e)
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
                            
                            this.Hide();
                            DangNhap dangNhapForm = new DangNhap();
                            dangNhapForm.Show();
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            DangNhap dangNhapForm = new DangNhap();
            dangNhapForm.Show();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
