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
            

            dgvDSNV.Rows.Add(txtMaNV.Text, txtTenNV.Text, Convert.ToDateTime(dtpNS.Text), cmbPB.Text);

            

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

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSNV.SelectedRows.Count > 0)
            {
                // Code cập nhật nhân viên
                string maNV = txtMaNV.Text;
                string hoTen = txtTenNV.Text;
                string tenPB = cmbPB.SelectedValue.ToString();
                DateTime ngaySinh = DateTime.Parse(dtpNS.Text);

                using (var db = new Model1())
                {
                    Phongban pb = db.Phongbans.FirstOrDefault();
                    Nhanvien nv = db.Nhanviens.Find(maNV);
                    if (nv != null)
                    {
                        // Có tìm thấy nhân viên, tiến hành cập nhật
                        nv.MaNV = maNV;
                        nv.TenNV = hoTen;
                        nv.Ngaysinh = ngaySinh;
                        nv.MaPB = tenPB;
                        db.SaveChanges();
                    }
                    Model1 context = new Model1();
                    BindGrid(context.Nhanviens.ToList());


                }

            }


        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            if (dgvDSNV.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo);
                string maNV = dgvDSNV.SelectedRows[0].Cells[0].Value.ToString();
                if (result == DialogResult.Yes)
                {
                    dgvDSNV.Rows.RemoveAt(dgvDSNV.SelectedRows[0].Index);
                    using (Model1 context = new Model1())
                    {
                        
                        Nhanvien nhanvien = context.Nhanviens.Find(maNV);
                        context.Nhanviens.Remove(nhanvien);
                        context.SaveChanges();
                    }
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
    }
}
