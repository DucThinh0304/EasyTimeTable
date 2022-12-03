using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student.Tuition;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using EasyTimeTable.Views.OpenCourse;

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
        private Visibility mask;

        [ObservableProperty]
        private bool isLoading;

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
            IsLoading = false;
            Mask = Visibility.Collapsed;
            TuitionPageCM = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new StudentTuitionPage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Thông tin học phí";
            });
            CoursePageCM = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new OpenCoursePage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Danh sách học phần";
            });
            LoadDB = new RelayCommand<object>(async (p) =>
            {
                Mask = Visibility.Visible;
                IsLoading = true;
                TuitionCheck = "(đã đóng học phí)";
                ColorTuition = new SolidColorBrush(Colors.Black);
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("select * from lophocphansinhvien where ngaythanhtoan IS NULL and masv = '"+ LoginViewModel.mssv + "'", con);
                var dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    TuitionCheck = "(chưa đóng học phí)";
                    ColorTuition = new SolidColorBrush(Colors.Red);
                }
                dr.Close();
                cmd = new SqlCommand("SELECT ki from thamso", con);
                dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
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
                dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    Year = "Năm học: " + dr.GetInt32(0).ToString() + " - " + (dr.GetInt32(0) + 1).ToString();
                }
                dr.Close();
                cmd = new SqlCommand("SELECT namhoc FROM thamso", con);
                dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    namhoc = dr.GetInt32(0);
                }
                dr.Close();
                cmd = new SqlCommand("SELECT sum(sotclt) from monhoc, lophocphansinhvien, HOCPHAN where HOCPHAN.mamon= MONHOC.mamon AND " +
                "lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '"+ LoginViewModel.mssv + "' and len(hocphan.mahocphan) = 9", con);
                dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
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
               "lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '"+ LoginViewModel.mssv + "' and len(hocphan.mahocphan) = 11", con);
                dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
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
                dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    if (dr.GetInt32(0) == 1)
                    {
                        await TheoTinChi();
                    }
                    if (dr.GetInt32(0) == 2)
                    {
                        await TheoTronGoi();
                    }
                }
                IsLoading = false;
                Mask = Visibility.Collapsed;
            });
        }
        private async Task TheoTinChi()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("select giatinchi from thamso", con);
            var dr =await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                HocPhi = SoTinChi * dr.GetInt32(0);
            }
        }

        private async Task TheoTronGoi()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("select giatrongioi from thamso", con);
            var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                HocPhi = dr.GetInt32(0);
            }
        }

    }
}
