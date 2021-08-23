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
    public partial class Form1 : Form
    {

        ContextStudentDB contextDB;
        public bool KiemTraDuLieu()
        {
            if (txtID.Text == "" || txtName.Text == "" || txtDiemTB.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            else if (txtID.TextLength != 10)
            {
                MessageBox.Show("Mã số sinh viên chưa đúng, phải 10 ký tự!", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            else
            {
                float ketQua = 0;
                bool Result = float.TryParse(txtDiemTB.Text, out ketQua);
                if (Result == false)
                {
                    MessageBox.Show("Điểm số không hợp lệ!", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

            }
            return true;
        }
        public Form1()
        {
            InitializeComponent();
            contextDB = new ContextStudentDB();
        }
        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Student> Liststudent = contextDB.Students.ToList();
            List<Faculty> Listkhoa = contextDB.Faculties.ToList();
            doDBSV(Liststudent);
            doDBKhoa(Listkhoa);
        }
        private void doDBKhoa(List<Faculty> list)
        {
            cbbKhoaVien.DataSource = list;
            cbbKhoaVien.DisplayMember = "FacultyName";
            cbbKhoaVien.ValueMember = "FacultyID";
        }

        private void doDBSV(List<Student> list)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in list)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }
        private int CheckIDStudent(string id)
        {
            for (int i = 0; i < dgvStudent.Rows.Count; i++)
            {
                if (dgvStudent.Rows[i].Cells[0].Value != null)
                {
                    if (dgvStudent.Rows[i].Cells[0].Value.ToString() == id)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        private void ReloadDGV()
        {
            List<Student> students = contextDB.Students.ToList();
            doDBSV(students);
        }
        private void ResetForm()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtDiemTB.Text = "";
            cbbKhoaVien.SelectedIndex = 0;
        }
        public bool TestDB()
        {
            if (txtID.Text == "" || txtName.Text == "" || txtDiemTB.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin !", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            if (KiemTraDuLieu())
            {
                if (CheckIDStudent(txtID.Text) == -1)
                {
                    Student student = new Student();
                    student.StudentID = txtID.Text;
                    student.FullName = txtName.Text;
                    student.AverageScore = Convert.ToInt32(txtDiemTB.Text);
                    student.FacultyID = Convert.ToInt32(cbbKhoaVien.SelectedValue.ToString());
                    contextDB.Students.AddOrUpdate(student);
                    contextDB.SaveChanges();
                    ReloadDGV();
                    ResetForm();
                    MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Sinh viên đã tồn tại, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvStudent.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvStudent.CurrentRow.Selected = true;
                    txtID.Text = dgvStudent.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    txtName.Text = dgvStudent.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                    txtDiemTB.Text = dgvStudent.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
                    cbbKhoaVien.SelectedIndex = cbbKhoaVien.FindString(dgvStudent.Rows[e.RowIndex].Cells[3].FormattedValue.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có vẻ bạn đang chọn sai!", "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            List<Faculty> listFaculty = contextDB.Faculties.ToList();
            List<Student> listStudent = contextDB.Students.ToList();
            ReloadDGV();
            ResetForm();
            doDBKhoa(listFaculty);
            doDBSV(listStudent);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            Student studentDel = contextDB.Students.FirstOrDefault(p => p.StudentID == txtID.Text);
            if (studentDel != null)
            {
                DialogResult result = MessageBox.Show($"Bạn có đồng ý xóa sinh viên {studentDel.FullName}", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    contextDB.Students.Remove(studentDel);
                    contextDB.SaveChanges();
                    ReloadDGV();
                    ResetForm();
                    MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (KiemTraDuLieu())
            {
                Student updateStudent = contextDB.Students.FirstOrDefault(
                    p => p.StudentID == txtID.Text);
                if (updateStudent != null)
                {
                    updateStudent.FullName = txtName.Text;
                    updateStudent.AverageScore = Convert.ToInt32(txtDiemTB.Text);
                    updateStudent.FacultyID = Convert.ToInt32(cbbKhoaVien.SelectedValue.ToString());
                    contextDB.Students.AddOrUpdate(updateStudent);
                    ReloadDGV();
                    ResetForm();
                    MessageBox.Show("Chỉnh sửa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần sửa!", "Thông báo", MessageBoxButtons.OK);
                }

            }
        }

        private void khoaViệnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FromKhoaVien formKhoaVien = new FromKhoaVien();
            formKhoaVien.ShowDialog();
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FromTimKiem formTimKiem = new FromTimKiem();
            formTimKiem.ShowDialog();
        }
    }
}
