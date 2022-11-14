using EasyTimeTable.Model;
using EasyTimeTable.Views.Staff.Course;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Office.Interop.Excel;

namespace EasyTimeTable.Views
{
    /// <summary>
    /// Interaction logic for PrintListStudentCourse.xaml
    /// </summary>
    public partial class PrintListStudentCourse : System.Windows.Window
    {
        public static List<SinhVien> SinhViens;
        public static List<CourseModel> courses;
        public static string HocPhanChon;
        public static ManageCourses view;
        public PrintListStudentCourse()
        {
            InitializeComponent();
            LoadHocPhan();
        }
        public void LoadHocPhan()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT Mahocphan FROM HOCPHAN", con);
            var dr = cmd.ExecuteReader();
            courses = new List<CourseModel>();
            while (dr.Read())
            {
                courses.Add(new CourseModel
                {
                    MaHocPhan = dr.GetString(0)
                });
            }
            dr.Close();
            con.Close();
            foreach (var item in courses)
            {
                comboHocPhan.Items.Add(item.MaHocPhan);
            }

        }
        public void LoadSinhVien(string MaHocPhan)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT sinhvien.masv, tensv, lophoc from sinhvien, lophocphansinhvien where lophocphansinhvien.masv = sinhvien.masv and mahocphan = '" + MaHocPhan + "'", con);
            var dr = cmd.ExecuteReader();
            int i = 0;
            SinhViens = new List<SinhVien>();
            while (dr.Read())
            {
                i++;
                SinhViens.Add(new SinhVien
                {
                    STT = i,
                    MaSV = dr.GetString(0),
                    TenSV = dr.GetString(1),
                    LopHoc = dr.GetString(2)
                });
            }
            Grid.ItemsSource = SinhViens;
            textSiSo.Text = SinhViens.Count.ToString();
        }
        private void comboDotDKHP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSinhVien(comboHocPhan.SelectedItem.ToString());
        }


        private void buttonThem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(1);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            for (int i = 0; i < 4; i++)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[3, i + 1];
                myRange.Value2 = Grid.Columns[i].Header;
            }
            sheet1.Range[sheet1.Cells[1, 1], sheet1.Cells[1, 7]].Merge();
            Microsoft.Office.Interop.Excel.Range TitleExcel = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[1, 1];
            TitleExcel.Value2 = "Danh sách sinh viên lớp " + HocPhanChon;
            sheet1.get_Range("A1", "G3").Cells.HorizontalAlignment =
                 Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            int r = 3;
            foreach (var item in SinhViens)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[r + 1, 1];
                myRange.Value2 = item.STT;
                myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[r + 1, 2];
                myRange.Value2 = item.MaSV;
                myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[r + 1, 3];
                myRange.Value2 = item.TenSV;
                myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[r + 1, 4];
                myRange.Value2 = item.LopHoc;
                r++;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            comboHocPhan.Text = HocPhanChon;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<SinhVien> course2 = new List<SinhVien>();
            foreach (var item in SinhViens) course2.Add(item);
            int i = 0;
            while (i < course2.Count)
            {
                if (!(course2[i].MaSV.Contains(SearchBox.Text) || course2[i].TenSV.Contains(SearchBox.Text) || course2[i].LopHoc.Contains(SearchBox.Text)))
                    course2.RemoveAt(i);
                else i++;
            }
            Grid.ItemsSource = course2;
        }

    }
}

