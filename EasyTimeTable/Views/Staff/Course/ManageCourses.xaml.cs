using EasyTimeTable.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace EasyTimeTable.Views.Staff.Course
{
    /// <summary>
    /// Interaction logic for ManageCourses.xaml
    /// </summary>
    public partial class ManageCourses : Page
    {
        public static List<Model.DotDKHP> list;
        public static List<CourseModel> courses;
        public static StaffWindow Window { get; set; }
        public ManageCourses()
        {
            InitializeComponent();
            LoadDotDKHP();
        }

        public void LoadDotDKHP()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd1 = new SqlCommand("Select madot, kihoc, namhoc from dotdkhp", con);
            var dr1 = cmd1.ExecuteReader();
            ManageCourses.list = new List<DotDKHP>();
            while (dr1.Read())
            {
                comboDotDKHP.Items.Add("Đợt " + Convert.ToString(dr1.GetInt32(0)) + " Kì " + Convert.ToString(dr1.GetInt32(1)) + " Năm học " + dr1.GetString(2));
                ManageCourses.list.Add(new DotDKHP
                {
                    HocKi = dr1.GetInt32(1),
                    NamHoc = dr1.GetString(2),
                    MaDot = dr1.GetInt32(0),
                });
            }
            dr1.Close();
            con.Close();
            comboDotDKHP.SelectedIndex = 0;
        }
        public void LoadDB(int MaDot, int KiHoc, string NamHoc)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT hocphan.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso FROM HOCPHAN,GIAOVIEN, Monhoc, hocphanodotdk where HOCPHAN.mamon = MONHOC.mamon AND HOCPHAN.magv = GIAOVIEN.Magv and " +
                "hocphanodotdk.mahocphan = hocphan.mahocphan and sophong is not null and madot = " + Convert.ToString(MaDot) + " and kihoc = " + Convert.ToString(KiHoc) + " and namhoc = '" + NamHoc + "'", con);
            var dr = cmd.ExecuteReader();
            int i = 0;

            courses = new List<CourseModel>();
            while (dr.Read())
            {
                i++;
                courses.Add(new CourseModel
                {
                    STT = i,
                    MaHocPhan = dr.GetString(0),
                    TenMon = dr.GetString(1),
                    TenGV = dr.GetString(2),
                    Nam = dr.GetInt32(3),
                    Ki = dr.GetInt32(4),
                    SoPhong = dr.GetString(5),
                    Toa = dr.GetString(6),
                    NgayBatDau = dr.GetDateTime(7),
                    NgayKetThuc = dr.GetDateTime(8),
                    TietHoc = dr.GetString(9),
                    Thu = dr.GetInt32(10),
                    SiSo = dr.GetInt32(11)
                });

            }
            Grid.ItemsSource = courses;
        }

        private void comboDotDKHP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            LoadDB(list[comboDotDKHP.SelectedIndex].MaDot, list[comboDotDKHP.SelectedIndex].HocKi, list[comboDotDKHP.SelectedIndex].NamHoc);

        }

        private void buttonThem_Click(object sender, RoutedEventArgs e)
        {
            AddCourse.dotDKHP = list[comboDotDKHP.SelectedIndex];
            AddCourse add = new AddCourse();
            add.view = this;
            add.ShowDialog();
        }   

        private void buttonXoa_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult t = MessageBox.Show("Bạn có chắc chắn muốn xóa học phần " + courses[Grid.SelectedIndex].MaHocPhan + "?", "Cảnh báo xóa học phần", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (t == MessageBoxResult.Yes)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("DELETE FROM lophocphansinhvien where mahocphan = '" + courses[Grid.SelectedIndex].MaHocPhan + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("DELETE FROM hocphanodotdk where mahocphan = '" + courses[Grid.SelectedIndex].MaHocPhan + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("DELETE FROM hocphan where mahocphan = '" + courses[Grid.SelectedIndex].MaHocPhan + "'", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Đã xóa học phần thành công");
                LoadDB(list[comboDotDKHP.SelectedIndex].MaDot, list[comboDotDKHP.SelectedIndex].HocKi, list[comboDotDKHP.SelectedIndex].NamHoc);
            }
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonXoa.IsEnabled = true;
            buttonSua.IsEnabled = true;
            buttonXuat.IsEnabled = true;
        }

        private void buttonSua_Click(object sender, RoutedEventArgs e)
        {
            EditCourse.HocPhan = courses[Grid.SelectedIndex];
            EditCourse.TenMon = new MonHocModel();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd1 = new SqlCommand("Select monhoc.mamon, tenmon, sotclt, sotcth from monhoc, hocphan where monhoc.mamon = hocphan.mamon and mahocphan = '" + EditCourse.HocPhan.MaHocPhan + "'", con);
            var dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                EditCourse.TenMon.MaMon = dr1.GetString(0);
                EditCourse.TenMon.TenMon = dr1.GetString(1);
                EditCourse.TenMon.SoTCLT = dr1.GetInt32(2);
                EditCourse.TenMon.SoTCTH = dr1.GetInt32(3);
            }
            dr1.Close();
            con.Close();
            EditCourse.MaMonHoc = EditCourse.TenMon.MaMon;
            EditCourse.dotDKHP = list[comboDotDKHP.SelectedIndex];
            EditCourse edit = new EditCourse();
            edit.Show();
            edit.view = this;
        }

        private void buttonXuat_Click(object sender, RoutedEventArgs e)
        {
            PrintListStudentCourse.HocPhanChon = courses[Grid.SelectedIndex].MaHocPhan;
            PrintListStudentCourse print = new PrintListStudentCourse();
            print.Show();
            PrintListStudentCourse.view = this;
        }
    }
}