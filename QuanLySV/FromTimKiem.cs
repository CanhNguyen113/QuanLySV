using QuanLySV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySV
{
    public partial class FromTimKiem : Form
    {
        ContextStudentDB ContextDB;
        public FromTimKiem()
        {
            InitializeComponent();
            ContextDB = new ContextStudentDB();
        }

        private void FromTimKiem_Load(object sender, EventArgs e)
        {
        }
        private bool kiemTraDB()
        {
            if (txtID.Text == "" && txtName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Thông báo");
                return false;
            }
            return true;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            List<Student> listStudent = ContextDB.Students.ToList();
            List<Student> listStudentSeach = (from s in listStudent
                                              where s.StudentID == txtID.Text ||
                                                    s.FullName == txtName.Text ||
                                                    s.Faculty.FacultyID == Convert.ToInt32(cbbKhoaVien.SelectedValue)
                                                    select s).ToList();
            txtKetQua.Text = (listStudentSeach.Count()).ToString();
            FillDataGV(listStudentSeach);
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void ResetForm()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtKetQua.Text = "0";
            cbbKhoaVien.SelectedIndex = 0;
            dgvTimKiem.Rows.Clear();
        }
        private void Reload()
        {
            List<Student> listStudent = ContextDB.Students.ToList();
            FillDataGV(listStudent);
        }

        private void FillDataGV(List<Student> listStudent )
        {
            dgvTimKiem.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvTimKiem.Rows.Add();
                dgvTimKiem.Rows[index].Cells[0].Value = item.StudentID;
                dgvTimKiem.Rows[index].Cells[1].Value = item.FullName;
                dgvTimKiem.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvTimKiem.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }
        private void dgvTimKiem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvTimKiem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvTimKiem.CurrentRow.Selected = true;

                    txtID.Text = dgvTimKiem.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    txtName.Text = dgvTimKiem.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có vẻ bạn chọn sai chỗ!", "Lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
