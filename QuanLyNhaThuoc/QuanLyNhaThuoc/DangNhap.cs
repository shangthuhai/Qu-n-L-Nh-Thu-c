using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class DangNhap : Form
    {
        private string connectionString = @"Data Source=SHANGPC\SQLEXPRESS;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True";
        public DangNhap()
        {
            InitializeComponent();
        }

        private bool KiemTraDangNhap(string taiKhoan, string matKhau)
        {
            bool ketQua = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM NhanVien WHERE TaiKhoan = @TaiKhoan AND MatKhau = @MatKhau";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    ketQua = count > 0;
                }
            }
            return ketQua;
        }

        private async void btnDangNhap_Click_1(object sender, EventArgs e)
        {
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool dangNhapThanhCong = false;
            await System.Threading.Tasks.Task.Run(() =>
            {
                dangNhapThanhCong = KiemTraDangNhap(taiKhoan, matKhau);
            });

            if (dangNhapThanhCong)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập sai! Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Clear();
                txtMatKhau.Focus();
            }
        }
    }
}
