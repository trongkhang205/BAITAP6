using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Lab05.BUS;
using Lab05.DAL.Entities;

namespace Lab05.GUI
{
    public partial class frmStudent : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();

        public frmStudent()
        {
            InitializeComponent();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty { FacultyID = 0, FacultyName = "-- Chọn khoa --" });
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore;

                if (item.Major != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name;
                    
                ShowAvatar(item.Avatar);
            }
        }

        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                picAvatar.Image = null;
            }
            else
            {
                string imagePath = Path.Combine(Application.StartupPath, "Images", ImageName);
                if (File.Exists(imagePath))
                {
                    picAvatar.Image = Image.FromFile(imagePath);
                    picAvatar.Refresh();
                }
            }
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void chkUnregisterMajor_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = chkUnregisterMajor.Checked ? 
                studentService.GetAllHasNoMajor() : 
                studentService.GetAll();
            BindGrid(listStudents);
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var studentId = dgvStudent.Rows[e.RowIndex].Cells[0].Value.ToString();
                var student = studentService.FindById(int.Parse(studentId));
                
                if (student != null)
                {
                    txtStudentID.Text = student.StudentID.ToString();
                    txtFullName.Text = student.FullName;
                    txtAverageScore.Text = student.AverageScore.ToString();
                    cmbFaculty.SelectedValue = student.FacultyID;
                    ShowAvatar(student.Avatar);
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                picAvatar.Image = Image.FromFile(openFile.FileName);
                picAvatar.Tag = openFile.FileName;
            }
        }

        private void btnAddUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtStudentID.Text) || string.IsNullOrEmpty(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                var student = new Student
                {
                    StudentID = int.Parse(txtStudentID.Text),
                    FullName = txtFullName.Text,
                    AverageScore = double.Parse(txtAverageScore.Text),
                    FacultyID = (int)cmbFaculty.SelectedValue,
                    MajorID = null
                };

                // Xử lý ảnh
                if (picAvatar.Tag != null)
                {
                    string sourceFile = picAvatar.Tag.ToString();
                    string extension = Path.GetExtension(sourceFile);
                    string fileName = student.StudentID + extension;
                    
                    string imagesDir = Path.Combine(Application.StartupPath, "Images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);
                        
                    string destPath = Path.Combine(imagesDir, fileName);
                    File.Copy(sourceFile, destPath, true);
                    student.Avatar = fileName;
                }

                studentService.InsertUpdate(student);
                var listStudents = chkUnregisterMajor.Checked ? 
                    studentService.GetAllHasNoMajor() : 
                    studentService.GetAll();
                BindGrid(listStudents);
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
            if (dgvStudent.CurrentRow != null)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        var studentId = dgvStudent.CurrentRow.Cells[0].Value.ToString();
                        studentService.Delete(int.Parse(studentId));
                        
                        var listStudents = chkUnregisterMajor.Checked ? 
                            studentService.GetAllHasNoMajor() : 
                            studentService.GetAll();
                        BindGrid(listStudents);
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

        private void ClearForm()
        {
            txtStudentID.Text = "";
            txtFullName.Text = "";
            txtAverageScore.Text = "";
            cmbFaculty.SelectedIndex = 0;
            picAvatar.Image = null;
            picAvatar.Tag = null;
        }
    }
}