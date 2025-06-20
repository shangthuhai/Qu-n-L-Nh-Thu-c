using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class DangNhap : Form
    {
        private string connectionString = @"Data Source=SHANGPC\MSSQLSERVER1;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True";
        public DangNhap()
        {
            InitializeComponent();
        }

        // Hàm kiểm tra tài khoản và mật khẩu có trong DB không
        private bool KiemTraDangNhap(string TaiKhoan, string MatKhau)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM NhanVien WHERE TaiKhoan = @TaiKhoan AND MatKhau = @MatKhau";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaiKhoan", TaiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", MatKhau);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string TaiKhoan = txtTaiKhoan.Text.Trim();
            string MatKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(TaiKhoan) || string.IsNullOrEmpty(MatKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Nếu tài khoản là "a" và mật khẩu là "123" thì vào Form2
                if (TaiKhoan == "a" && MatKhau == "123")
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    this.Hide();
                    Form form2 = new Form2();
                    form2.Show();
                    return;
                }

                if (KiemTraDangNhap(TaiKhoan, MatKhau))
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    this.Hide();
                    Form Form1 = new Form1();
                    Form1.Show();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi kết nối đến cơ sở dữ liệu.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            TaoTaiKhoan taoTaiKhoanForm = new TaoTaiKhoan();
            taoTaiKhoanForm.Show();
        }
    }
}