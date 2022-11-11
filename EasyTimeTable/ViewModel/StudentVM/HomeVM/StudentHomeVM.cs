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

        public ICommand LoadDB { get; set; }

        [ObservableProperty]
        private string tuitionCheck;

        [ObservableProperty]
        private Brush colorTuition;

        [ObservableProperty]
        private string semester;

        [ObservableProperty]
        private string year;
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
                var cmd = new SqlCommand("select * from lophocphansinhvien where ngaythanhtoan IS NULL", con);
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
                    Semester = "(kì mở học phần hiện tại: " + dr.GetInt32(0).ToString() + ")";
                }
                dr.Close();
                cmd = new SqlCommand("SELECT namhoc from thamso", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Year = "Năm học: " + dr.GetInt32(0).ToString() + " - " + (dr.GetInt32(0) + 1).ToString();
                }
                dr.Close();
            });


        }

    }
}
