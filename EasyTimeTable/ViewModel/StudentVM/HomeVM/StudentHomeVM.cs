using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student.Tuition;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StudentHomeVM

    {
        public ICommand TuitionPageCM { get; set; }
        public ICommand CoursePageCM { get; set; }

        [ObservableProperty]
        private string tuitionCheck;

        [ObservableProperty]
        private Brush colorTuition;

        [ObservableProperty]
        private string courseNumberText;
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
            TuitionCheck = "(đã đóng học phí)";
            ColorTuition = new SolidColorBrush(Colors.Black);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("select * from lophocphansinhvien where ngaythanhtoan IS NULL", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                TuitionCheck = "(chưa đóng học phí)";
                ColorTuition = new SolidColorBrush(Colors.Red);
            }
            dr.Close();
            cmd = new SqlCommand("select Sum(sotclt)+sum(sotcth) from sinhvienmonhoc, monhoc where masv = '20520782' and monhoc.mamon = sinhvienmonhoc.mamon", con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                CourseNumberText = "(Số tín chỉ: " + dr.GetInt32(0).ToString() + ")";
            }


        }

    }
}
