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
            cbLoaiThuoc.Items.Clear();
            cbLoaiThuoc.Items.Add("Thuốc Viên");
            cbLoaiThuoc.Items.Add("Thuốc Dung Dịch");
            cbLoaiThuoc.Items.Add("Thuốc Bột");
            cbTimKiemLoaiThuoc.Items.Add("Khác");
            cbLoaiThuoc.SelectedIndex = 0; 
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
                string query = "SELECT MaThuoc, TenThuoc, SoLuong, LoaiThuoc, DonGiaNhap, DonGiaBan, XuatXu, NgaySanXuat, NgayHetHan FROM SanPham";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                  
                    dgvThuoc.Rows.Add(
                        reader["MaThuoc"].ToString(),
                        reader["TenThuoc"].ToString(),
                        reader["LoaiThuoc"].ToString(), 
                        reader["SoLuong"].ToString(),  
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
                string query = "INSERT INTO SanPham (MaThuoc, TenThuoc, SoLuong, LoaiThuoc, DonGiaNhap, DonGiaBan, XuatXu, NgaySanXuat, NgayHetHan) " +
                               "VALUES (@MaThuoc, @TenThuoc, @SoLuong, @LoaiThuoc, @DonGiaNhap, @DonGiaBan, @XuatXu, @NgaySanXuat, @NgayHetHan)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThuoc", txtMaThuoc.Text);
                cmd.Parameters.AddWithValue("@TenThuoc", txtTenThuoc.Text);
                cmd.Parameters.AddWithValue("@SoLuong", txtSoLuong.Text);
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
            txtSoLuong.Clear();
            cbLoaiThuoc.SelectedIndex = 0;
            cbXuatXu.SelectedIndex = -1; 
            dtpNgaySanXuat.Value = DateTime.Now;
            dtpNgayHetHan.Value = DateTime.Now;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvThuoc.CurrentRow != null)
            {

                dgvThuoc.CellClick += dgvThuoc_CellClick;
                string maThuoc = dgvThuoc.CurrentRow.Cells[0].Value.ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE SanPham SET TenThuoc=@TenThuoc, SoLuong=@SoLuong, LoaiThuoc=@LoaiThuoc, DonGiaNhap=@DonGiaNhap, DonGiaBan=@DonGiaBan, XuatXu=@XuatXu, NgaySanXuat=@NgaySanXuat, NgayHetHan=@NgayHetHan WHERE MaThuoc=@MaThuoc";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenThuoc", txtTenThuoc.Text);
                    cmd.Parameters.AddWithValue("@SoLuong", txtSoLuong.Text);
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
                string query = "SELECT MaThuoc, TenThuoc, SoLuong, LoaiThuoc, DonGiaNhap, DonGiaBan, XuatXu, NgaySanXuat, NgayHetHan FROM SanPham WHERE 1=1";
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
                        reader["SoLuong"].ToString(),
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            TaoTaiKhoan taoTaiKhoanForm = new TaoTaiKhoan();
            taoTaiKhoanForm.Show();
        }
        private void dgvThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvThuoc.Rows[e.RowIndex].Cells[0].Value != null)
            {
                DataGridViewRow row = dgvThuoc.Rows[e.RowIndex];
                txtMaThuoc.Text = row.Cells[0].Value.ToString();
                txtTenThuoc.Text = row.Cells[1].Value.ToString();
                cbLoaiThuoc.Text = row.Cells[2].Value.ToString();
                txtSoLuong.Text = row.Cells[3].Value.ToString();
                txtDonGiaNhap.Text = row.Cells[4].Value.ToString();
                txtDonGiaBan.Text = row.Cells[5].Value.ToString();
                cbXuatXu.Text = row.Cells[6].Value.ToString();
                DateTime ngaySanXuat, ngayHetHan;
                if (DateTime.TryParse(row.Cells[7].Value.ToString(), out ngaySanXuat))
                    dtpNgaySanXuat.Value = ngaySanXuat;
                if (DateTime.TryParse(row.Cells[8].Value.ToString(), out ngayHetHan))
                    dtpNgayHetHan.Value = ngayHetHan;
            }
        }
    }
}
