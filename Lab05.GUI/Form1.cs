using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab05.BUS;
using Lab05.DAL.Entities;

namespace Lab05.GUI
{
    public partial class Form1 : Form
    {
        private StudentService studentService = new StudentService();
        private FacultyService facultyService = new FacultyService();
        private string selectedImagePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadFaculties();
                LoadStudents();
                
                // Đăng ký sự kiện cho checkbox
                chkChuaCoChuyenNganh.CheckedChanged += chkUnregisterMajor_CheckedChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}");
            }
        }

        private void LoadFaculties()
        {
            try
            {
                var faculties = facultyService.GetAll();
                cmbKhoa.DataSource = faculties;
                cmbKhoa.DisplayMember = "FacultyName";
                cmbKhoa.ValueMember = "FacultyID";
                
                // Set default selection
                if (faculties.Count > 0)
                {
                    cmbKhoa.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database: " + ex.Message);
            }
        }

        private void LoadStudents()
        {
            try
            {
                // Lấy danh sách sinh viên từ CSDL
                var students = chkChuaCoChuyenNganh.Checked ? 
                    studentService.GetAllHasNoMajor() : 
                    studentService.GetAll();
                
                // Fill vào DataGridView
                dgvStudents.DataSource = students;
                
                // Cấu hình hiển thị cột
                if (dgvStudents.Columns.Count > 0)
                {
                    if (dgvStudents.Columns["StudentID"] != null)
                        dgvStudents.Columns["StudentID"].HeaderText = "MSSV";
                    if (dgvStudents.Columns["FullName"] != null)
                        dgvStudents.Columns["FullName"].HeaderText = "Họ Tên";
                    if (dgvStudents.Columns["FacultyID"] != null)
                        dgvStudents.Columns["FacultyID"].HeaderText = "Mã Khoa";
                    if (dgvStudents.Columns["MajorID"] != null)
                        dgvStudents.Columns["MajorID"].HeaderText = "Mã Chuyên Ngành";
                    if (dgvStudents.Columns["AverageScore"] != null)
                        dgvStudents.Columns["AverageScore"].HeaderText = "Điểm TB";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFile.FileName;
                picAvatar.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!");
                    return;
                }
                if (string.IsNullOrEmpty(txtDTB.Text) || !double.TryParse(txtDTB.Text, out double score))
                {
                    MessageBox.Show("Vui lòng nhập điểm trung bình hợp lệ!");
                    return;
                }

                int studentId;
                if (txtMSSV.Tag != null)
                {
                    studentId = (int)txtMSSV.Tag;
                }
                else
                {
                    if (!int.TryParse(txtMSSV.Text, out studentId))
                    {
                        MessageBox.Show("Vui lòng nhập MSSV hợp lệ!");
                        return;
                    }
                }

                var student = new Student
                {
                    StudentID = studentId,
                    FullName = txtHoTen.Text,
                    FacultyID = (int)cmbKhoa.SelectedValue,
                    MajorID = null, // Mặc định chưa có chuyên ngành
                    AverageScore = score
                };

                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    string imagesDir = Path.Combine(Application.StartupPath, "Images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);
                        
                    string fileName = student.StudentID + Path.GetExtension(selectedImagePath);
                    string destPath = Path.Combine(imagesDir, fileName);
                    File.Copy(selectedImagePath, destPath, true);
                    student.Avatar = fileName;
                }

                studentService.InsertUpdate(student);
                LoadStudents();
                ClearForm();
                MessageBox.Show("Thêm/cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow != null)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        var student = (Student)dgvStudents.CurrentRow.DataBoundItem;
                        studentService.Delete(student.StudentID);
                        LoadStudents();
                        ClearForm();
                        MessageBox.Show("Xóa thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi xóa: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!");
            }
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var student = (Student)dgvStudents.Rows[e.RowIndex].DataBoundItem;
                txtMSSV.Text = student.StudentID.ToString();
                txtMSSV.Tag = student.StudentID;
                txtHoTen.Text = student.FullName;
                cmbKhoa.SelectedValue = student.FacultyID;
                txtDTB.Text = student.AverageScore.ToString();

                // Load avatar
                string imagePath = Path.Combine(Application.StartupPath, "Images", student.StudentID + ".jpg");
                if (File.Exists(imagePath))
                {
                    picAvatar.Image = Image.FromFile(imagePath);
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }

        private void chkChuaCoChuyenNganh_CheckedChanged(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void chkUnregisterMajor_CheckedChanged(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void ClearForm()
        {
            txtMSSV.Text = "";
            txtMSSV.Tag = null;
            txtHoTen.Text = "";
            txtDTB.Text = "";
            picAvatar.Image = null;
            selectedImagePath = "";
        }
    }
}
