using EasyTimeTable.Model;
using EasyTimeTable.Views.Staff.Course;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.ComponentModel;


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

        private void Window_Closed(object sender, EventArgs e)
        {
            view.Show();
        }

        private void buttonThem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            comboHocPhan.Text = HocPhanChon;
        }
    }
}

