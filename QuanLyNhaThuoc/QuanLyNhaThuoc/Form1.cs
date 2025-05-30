using System;
using System.Data.SqlClient; // Add this namespace for SQL-related classes
using System.Windows.Forms;

namespace QuanLyNhaThuoc
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=SHANGPC\MSSQLSERVER1;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            LoadComboBoxData();
            LoadTimKiemComboBoxData();
            LoadSanPhamFromDatabase();
            LoadXuatXuComboBoxData();
        }
        private void LoadComboBoxData()
        {
            // Example implementation: Populate the combo box with options
            cbLoaiThuoc.Items.Clear();
            cbLoaiThuoc.Items.Add("Thuốc Viên");
            cbLoaiThuoc.Items.Add("Thuốc Dung Dịch");
            cbLoaiThuoc.Items.Add("Thuốc Bột");
            cbTimKiemLoaiThuoc.Items.Add("Khác");
            cbLoaiThuoc.SelectedIndex = 0; // Default selection
        }
        private void LoadXuatXuComboBoxData()
        {
            cbXuatXu.Items.Clear();
            cbXuatXu.Items.Add("Việt Nam");
            cbXuatXu.Items.Add("Nhật Bản");
            cbXuatXu.Items.Add("Mỹ");
            cbXuatXu.Items.Add("Trung Quốc");
            cbXuatXu.SelectedIndex = 0; 
        }   

        private void LoadTimKiemComboBoxData()
        {
            // Example implementation: Populate the search combo box with options
            cbTimKiemLoaiThuoc.Items.Clear();
            cbTimKiemLoaiThuoc.Items.Add("Thuốc Viên");
            cbTimKiemLoaiThuoc.Items.Add("Thuốc Dung Dịch");
            cbTimKiemLoaiThuoc.Items.Add("Thuốc Bột");
            cbTimKiemLoaiThuoc.Items.Add("Khác");
            cbTimKiemLoaiThuoc.SelectedIndex = 0; // Default selection
        }
        private void LoadSanPhamFromDatabase()
        {
            dgvThuoc.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaThuoc, TenThuoc, LoaiThuoc, DonGiaNhap, DonGiaBan, XuatXu, NgaySanXuat, NgayHetHan FROM SanPham";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvThuoc.Rows.Add(
                        reader["MaThuoc"].ToString(),
                        reader["TenThuoc"].ToString(),
                        reader["LoaiThuoc"].ToString(),
                        reader["DonGiaNhap"].ToString(),
                        reader["DonGiaBan"].ToString(),
                        reader["XuatXu"].ToString(),
                        Convert.ToDateTime(reader["NgaySanXuat"]).ToShortDateString(),
                        Convert.ToDateTime(reader["NgayHetHan"]).ToShortDateString()
                    );
                }
                reader.Close();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO SanPham (MaThuoc, TenThuoc, LoaiThuoc, DonGiaNhap, DonGiaBan, XuatXu, NgaySanXuat, NgayHetHan) " +
                               "VALUES (@MaThuoc, @TenThuoc, @LoaiThuoc, @DonGiaNhap, @DonGiaBan, @XuatXu, @NgaySanXuat, @NgayHetHan)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThuoc", txtMaThuoc.Text);
                cmd.Parameters.AddWithValue("@TenThuoc", txtTenThuoc.Text);
                cmd.Parameters.AddWithValue("@LoaiThuoc", cbLoaiThuoc.Text);
                cmd.Parameters.AddWithValue("@DonGiaNhap", txtDonGiaNhap.Text);
                cmd.Parameters.AddWithValue("@DonGiaBan", txtDonGiaBan.Text);
                cmd.Parameters.AddWithValue("@XuatXu", cbXuatXu.Text);
                cmd.Parameters.AddWithValue("@NgaySanXuat", dtpNgaySanXuat.Value);
                cmd.Parameters.AddWithValue("@NgayHetHan", dtpNgayHetHan.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            LoadSanPhamFromDatabase();
            ClearForm();
        }
        private void ClearForm()
        {
            txtMaThuoc.Clear();
            txtTenThuoc.Clear();
            txtDonGiaNhap.Clear();
            txtDonGiaBan.Clear();
            cbLoaiThuoc.SelectedIndex = 0;
            cbXuatXu.SelectedIndex = -1; // Assuming this combo box allows no selection
            dtpNgaySanXuat.Value = DateTime.Now;
            dtpNgayHetHan.Value = DateTime.Now;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvThuoc.CurrentRow != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE SanPham SET TenThuoc=@TenThuoc, LoaiThuoc=@LoaiThuoc, DonGiaNhap=@DonGiaNhap, DonGiaBan=@DonGiaBan, XuatXu=@XuatXu, NgaySanXuat=@NgaySanXuat, NgayHetHan=@NgayHetHan WHERE MaThuoc=@MaThuoc";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaThuoc", txtMaThuoc.Text);
                    cmd.Parameters.AddWithValue("@TenThuoc", txtTenThuoc.Text);
                    cmd.Parameters.AddWithValue("@LoaiThuoc", cbLoaiThuoc.Text);
                    cmd.Parameters.AddWithValue("@DonGiaNhap", txtDonGiaNhap.Text);
                    cmd.Parameters.AddWithValue("@DonGiaBan", txtDonGiaBan.Text);
                    cmd.Parameters.AddWithValue("@XuatXu", cbXuatXu.Text);
                    cmd.Parameters.AddWithValue("@NgaySanXuat", dtpNgaySanXuat.Value);
                    cmd.Parameters.AddWithValue("@NgayHetHan", dtpNgayHetHan.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadSanPhamFromDatabase();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvThuoc.CurrentRow != null)
            {
                string maThuoc = dgvThuoc.CurrentRow.Cells[0].Value.ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM SanPham WHERE MaThuoc=@MaThuoc";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadSanPhamFromDatabase();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tenThuoc = txtTimKiemTenThuoc.Text.Trim().ToLower();
            string loaiThuoc = cbTimKiemLoaiThuoc.Text;
            dgvThuoc.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaThuoc, TenThuoc, LoaiThuoc, DonGiaNhap, DonGiaBan, XuatXu, NgaySanXuat, NgayHetHan FROM SanPham WHERE 1=1";
                if (!string.IsNullOrEmpty(tenThuoc))
                {
                    query += " AND LOWER(TenThuoc) LIKE @TenThuoc";
                }
                if (loaiThuoc != "Tất cả")
                {
                    query += " AND LoaiThuoc = @LoaiThuoc";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(tenThuoc))
                {
                    cmd.Parameters.AddWithValue("@TenThuoc", "%" + tenThuoc + "%");
                }
                if (loaiThuoc != "Tất cả")
                {
                    cmd.Parameters.AddWithValue("@LoaiThuoc", loaiThuoc);
                }
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvThuoc.Rows.Add(
                        reader["MaThuoc"].ToString(),
                        reader["TenThuoc"].ToString(),
                        reader["LoaiThuoc"].ToString(),
                        reader["DonGiaNhap"].ToString(),
                        reader["DonGiaBan"].ToString(),
                        reader["XuatXu"].ToString(),
                        Convert.ToDateTime(reader["NgaySanXuat"]).ToShortDateString(),
                        Convert.ToDateTime(reader["NgayHetHan"]).ToShortDateString()
                    );
                }
                reader.Close();
            }
        }
    }
}
