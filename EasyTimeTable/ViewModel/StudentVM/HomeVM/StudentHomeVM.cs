using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student.Tuition;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows.Media;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StudentHomeVM

    {
        public ICommand TuitionPageCM { get; set; }
        public ICommand CoursePageCM { get; set; }
        public ICommand LoadDB { get; set; }

        [ObservableProperty]
        private string tuitionCheck;

        [ObservableProperty]
        private Brush colorTuition;

        [ObservableProperty]
        private Brush colorSemester;

        [ObservableProperty]
        private string semester;

        [ObservableProperty]
        private string year;

        [ObservableProperty]
        private bool isEnable;

        [ObservableProperty]
        private int hocPhi;

        private int namhoc = 0;
        private int SoTinChi = 0;
        public StudentHomeVM()
        {

            TuitionPageCM = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new StudentTuitionPage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Thông tin học phí";
            });
            CoursePageCM = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new OpenCourseListPage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Danh sách học phần";
            });
            LoadDB = new RelayCommand<object>((p) =>
            {
                TuitionCheck = "(đã đóng học phí)";
                ColorTuition = new SolidColorBrush(Colors.Black);
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("select * from lophocphansinhvien where ngaythanhtoan IS NULL and masv = '20520782'", con);
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    TuitionCheck = "(chưa đóng học phí)";
                    ColorTuition = new SolidColorBrush(Colors.Red);
                }
                dr.Close();
                cmd = new SqlCommand("SELECT ki from thamso", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr.GetInt32(0) == 0)
                    {
                        Semester = "(Chưa đến thời gian đăng kí học phần)";
                        IsEnable = false;
                        ColorSemester = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        Semester = "(kì mở học phần hiện tại: " + dr.GetInt32(0).ToString() + ")";
                        IsEnable = true;
                        ColorSemester = new SolidColorBrush(Colors.Black);
                    }
                }
                dr.Close();
                cmd = new SqlCommand("SELECT namhoc from thamso", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Year = "Năm học: " + dr.GetInt32(0).ToString() + " - " + (dr.GetInt32(0) + 1).ToString();
                }
                dr.Close();
                cmd = new SqlCommand("SELECT namhoc FROM thamso", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    namhoc = dr.GetInt32(0);
                }
                dr.Close();
                cmd = new SqlCommand("SELECT sum(sotclt) from monhoc, lophocphansinhvien, HOCPHAN where HOCPHAN.mamon= MONHOC.mamon AND " +
                "lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and len(hocphan.mahocphan) = 9", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.IsDBNull(0))
                    {
                        SoTinChi = 0;
                    }
                    else
                    {
                        SoTinChi = dr.GetInt32(0);
                    }
                }
                dr.Close();
                cmd = new SqlCommand("SELECT sum(sotcth) from monhoc, lophocphansinhvien, HOCPHAN where HOCPHAN.mamon= MONHOC.mamon AND " +
               "lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and len(hocphan.mahocphan) = 11", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.IsDBNull(0) == false)
                    {
                        if (SoTinChi != 0)
                        {
                            SoTinChi += dr.GetInt32(0);
                        }
                    }
                }
                dr.Close();
                cmd = new SqlCommand("SELECT kieuhocphan from thamso, hocki where ki=kihoc and hocki.namhoc = '" + (namhoc.ToString() + "-" + (namhoc + 1).ToString()) + "'", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr.GetInt32(0) == 1)
                    {
                        TheoTinChi();
                    }
                    if (dr.GetInt32(0) == 2)
                    {
                        TheoTronGoi();
                    }
                }

            });
        }
        private void TheoTinChi()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("select giatinchi from thamso", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                HocPhi = SoTinChi * dr.GetInt32(0);
            }
        }

        private void TheoTronGoi()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("select giatrongoi from thamso", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                HocPhi = dr.GetInt32(0);
            }
        }

    }
}
