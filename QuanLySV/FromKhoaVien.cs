using QuanLySV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySV
{
    public partial class FromKhoaVien : Form
    {
        ContextStudentDB ContextDB;
        public FromKhoaVien()
        {
            InitializeComponent();
            ContextDB = new ContextStudentDB();
        }

        private void FromKhoaVien_Load(object sender, EventArgs e)
        {
        }
        private bool kiemTraDB()
        {
            if (txtMaKhoa.Text == "" && txtTenKhoa.Text == "" && txtTongGS.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Thông báo");
                return false;
            }
            return true;
        }
        private int CheckKhoaVien(string id)
        {
            for (int i = 0; i < dgvKhoaVien.Rows.Count; i++)
            {
                if (dgvKhoaVien.Rows[i].Cells[0].Value != null)
                {
                    if (dgvKhoaVien.Rows[i].Cells[0].Value.ToString() == id)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnThem_Sua_Click(object sender, EventArgs e)
        {
            if (kiemTraDB())
            {
                if (CheckKhoaVien(txtMaKhoa.Text) == -1)
                {
                    Faculty faculty = new Faculty();
                    faculty.FacultyID = int.Parse(txtMaKhoa.Text);
                    faculty.FacultyName = txtTenKhoa.Text;
                    faculty.TotalProfessor = int.Parse(txtTongGS.Text);
                    ContextDB.Faculties.AddOrUpdate(faculty);
                    MessageBox.Show($"Thêm khoa{faculty.FacultyName} thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    int MaKhoa = int.Parse(txtMaKhoa.Text);
                    Faculty faculty = ContextDB.Faculties.FirstOrDefault(p => p.FacultyID == MaKhoa);
                    if (faculty != null)
                    {
                        faculty.FacultyID = int.Parse(txtMaKhoa.Text);
                        faculty.FacultyName = txtTenKhoa.Text;
                        faculty.TotalProfessor = int.Parse(txtTongGS.Text);
                        ContextDB.Faculties.AddOrUpdate(faculty);
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK);
                    }
                }
                ContextDB.SaveChanges();
                Reload();
                ResetForm();
            }
        }
        private void ResetForm()
        {
            txtMaKhoa.Text = "";
            txtTenKhoa.Text = "";
            txtTongGS.Text = "";
        }

        private void Reload()
        {
            List<Faculty> listFaculty = ContextDB.Faculties.ToList();
            BillDataGV(listFaculty);
        }

        private void BillDataGV(List<Faculty> listFaculty)
        {
            dgvKhoaVien.Rows.Clear();
            foreach (var item in listFaculty)
            {
                int index = dgvKhoaVien.Rows.Add();
                dgvKhoaVien.Rows[index].Cells[0].Value = item.FacultyID;
                dgvKhoaVien.Rows[index].Cells[1].Value = item.FacultyName;
                dgvKhoaVien.Rows[index].Cells[2].Value = item.TotalProfessor;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int MaKhoa = int.Parse(txtMaKhoa.Text);
            var countStudent = ContextDB.Students.Count(p => p.FacultyID == MaKhoa);
            Faculty faculty = ContextDB.Faculties.FirstOrDefault(p => p.FacultyID == MaKhoa);
            if (faculty != null && countStudent == 0)
            {
                DialogResult dialogResult = MessageBox.Show($"Bạn có muốn xóa{faculty.FacultyName}", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    ContextDB.Faculties.Remove(faculty);
                    ContextDB.SaveChanges();
                    Reload();
                    ResetForm();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            else
            {
                if (countStudent != 0)
                {
                    MessageBox.Show("Có sinh viên trong khoa, xóa không được!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dgvKhoaVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvKhoaVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvKhoaVien.CurrentRow.Selected = true;

                    txtMaKhoa.Text = dgvKhoaVien.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    txtTenKhoa.Text = dgvKhoaVien.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                    txtTongGS.Text = dgvKhoaVien.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có vẻ bạn chọn sai chỗ!", "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
