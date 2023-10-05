using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using buoi6.Models;

namespace buoi6
{
    public partial class frmDSNV : Form
    {
        public frmDSNV()
        {
            InitializeComponent();
        }

      
        private void FillPhongbanCombobox(List<Phongban> listPhongbans)
        {
            this.cmbPB.DataSource = listPhongbans;
            this.cmbPB.DisplayMember = "TenPB";
            this.cmbPB.ValueMember = "MaPB";
        }
        
        private void BindGrid(List<Nhanvien> listNhanviens)
        {
            dgvDSNV.Rows.Clear();
            foreach (var item in listNhanviens)
            {
                int index = dgvDSNV.Rows.Add();
                dgvDSNV.Rows[index].Cells[0].Value = item.MaNV;
                dgvDSNV.Rows[index].Cells[1].Value = item.TenNV;
                dgvDSNV.Rows[index].Cells[2].Value = item.Ngaysinh;
                dgvDSNV.Rows[index].Cells[3].Value = item.Phongban.TenPB;
            }
        }

        private void frmDSNV_Load_1(object sender, EventArgs e)
        {
           
            Model1 context = new Model1();

            List<Nhanvien> listNhanviens = context.Nhanviens.ToList();
            List<Phongban> listPhongbans = context.Phongbans.ToList();

            FillPhongbanCombobox(listPhongbans);
            BindGrid(listNhanviens);

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr= MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

            if ( dr ==DialogResult.OK)
            {
                Application.Exit();
            }
            
        }

       
        private void btnThem_Click(object sender, EventArgs e)
        {

            if (txtMaNV.Text == "" || txtTenNV.Text == "")
            {
                MessageBox.Show("Vui lòng điền đủ thông tin cần thêm ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                MessageBox.Show("Thêm thành công!","thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                

                string maNV = txtMaNV.Text;
                string hoTen = txtTenNV.Text;
                string tenPB = cmbPB.SelectedValue.ToString();
                DateTime ngaySinh = DateTime.Parse(dtpNS.Text);

                using (var md = new Model1())
                {
                    Nhanvien nv = new Nhanvien();
                    nv.MaNV = maNV;
                    nv.TenNV = hoTen;
                    nv.Ngaysinh = ngaySinh;
                    nv.MaPB = tenPB;
                    md.Nhanviens.Add(nv);
                    md.SaveChanges();

                }
                Model1 context = new Model1();
                // Làm mới bảng dữ liệu
                BindGrid(context.Nhanviens.ToList());
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {



            // Code cập nhật nhân viên
          


            using (var md = new Model1())
            {
                string maNV = txtMaNV.Text;
                string hoTen = txtTenNV.Text;
                string tenPB = cmbPB.SelectedValue.ToString();
                DateTime ngaySinh = DateTime.Parse(dtpNS.Text);



                Nhanvien nv = md.Nhanviens.FirstOrDefault(s => s.MaNV == maNV);
                if (nv != null)
                {
                    // Có tìm thấy nhân viên, tiến hành cập nhật

                    nv.TenNV = hoTen;
                    nv.Ngaysinh = ngaySinh;
                    nv.MaPB = tenPB;

                    md.SaveChanges();
                    MessageBox.Show("Cập nhật thành công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindGrid(md.Nhanviens.ToList());
                }
                else MessageBox.Show("Khong tim thay nhan vien", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
              




            }
        

        }


        private void btnXoa_Click(object sender, EventArgs e)
        {

            dgvDSNV.Rows.RemoveAt(dgvDSNV.SelectedRows[0].Index);
            using (Model1 context = new Model1())
            {
                string maNV = txtMaNV.Text;
                string hoTen = txtTenNV.Text;
                string tenPB = cmbPB.SelectedValue.ToString();
                DateTime ngaySinh = DateTime.Parse(dtpNS.Text);


                Nhanvien nv = context.Nhanviens.FirstOrDefault(s => s.MaNV == maNV);
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                if (result == DialogResult.Yes)
                {

                    
                    context.Nhanviens.Remove(nv);
                    context.SaveChanges();
                }
            }

            
        }

        private void dgvDSNV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataGridViewRow row = dgvDSNV.Rows[e.RowIndex];
            txtMaNV.Text = row.Cells[0].Value.ToString();
            txtTenNV.Text = row.Cells[1].Value.ToString();
            dtpNS.Text = row.Cells[2].Value.ToString();
            cmbPB.Text = row.Cells[3].Value.ToString();
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            /*using (Model1 context = new Model1())
            {
                string maNV = txtMaNV.Text;
                string hoTen = txtTenNV.Text;
                string tenPB = cmbPB.SelectedValue.ToString();
                DateTime ngaySinh = DateTime.Parse(dtpNS.Text);
                Nhanvien nv = context.Nhanviens.Where(s => s.MaNV == maNV).FirstOrDefault();

                if (nv != null)
                {
                    // Có tìm thấy nhân viên, tiến hành cập nhật
                    txtTenNV.Text = nv.TenNV.ToString();
                    dtpNS.Text = nv.Ngaysinh.ToString();
                    cmbPB.Text = nv.Phongban.TenPB.ToString();
                }
                else MessageBox.Show("Khong tim thay sv!", "Thong bao!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }*/
            using (Model1 context = new Model1())
            {
                string maNV = txtMaNV.Text;
                string hoTen = txtTenNV.Text;
                string tenPB = cmbPB.SelectedValue.ToString();
                DateTime ngaySinh = DateTime.Parse(dtpNS.Text);
                Nhanvien nv = context.Nhanviens.Where(s => s.MaNV == maNV).FirstOrDefault();

                if (maNV == "")
                {
                    // Trả về dữ liệu ban đầu của DataGridView
                    BindGrid(context.Nhanviens.ToList());
                    return;
                }

                if (nv != null)
                {
                    // Có tìm thấy nhân viên, tiến hành cập nhật
                    txtTenNV.Text = nv.TenNV.ToString();
                    dtpNS.Text = nv.Ngaysinh.ToString();
                    cmbPB.Text = nv.Phongban.TenPB.ToString();

                    // Xóa tất cả các dòng trong DataGridView
                    dgvDSNV.Rows.Clear();

                    // Thêm dòng mới vào DataGridView
                    int index = dgvDSNV.Rows.Add();
                    dgvDSNV.Rows[index].Cells[0].Value = nv.MaNV;
                    dgvDSNV.Rows[index].Cells[1].Value = nv.TenNV;
                    dgvDSNV.Rows[index].Cells[2].Value = nv.Ngaysinh;
                    dgvDSNV.Rows[index].Cells[3].Value = nv.Phongban.TenPB;
                }
                else
                {


                    MessageBox.Show("Khong tim thay nhan vien", "Thong bao!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    BindGrid(context.Nhanviens.ToList());
                    return;
                }

            }
        }
    }
}
