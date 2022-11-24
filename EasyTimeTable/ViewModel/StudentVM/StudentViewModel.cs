using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Calendar;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student.Home;
using EasyTimeTable.Views.Student.Tuition;
using MaterialDesignThemes.Wpf;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StudentViewModel
    {
        public ICommand LoadStudentHomeCM { get; set; }
        public ICommand LoadStudentTuitionCM { get; set; }
        public ICommand LoadOpenCourseListCM { get; set; }
        public ICommand LoadSchedulerCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public static Frame? MainFrame { get; set; }

        private string MSSV;

        [ObservableProperty]
        private string currentUserName;

        private string Name;

        [ObservableProperty]
        private bool isLoading;

        public SnackbarMessageQueue MessageQueueSnackBar { set; get; } = new(TimeSpan.FromSeconds(3));

        public StudentViewModel()
        {

            MSSV = LoginViewModel.mssv;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select tensv from sinhvien where masv = '" + MSSV + "'", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CurrentUserName = dr.GetString(0);
                Name = CurrentUserName.Split(' ').Last();
            }
            Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Xin chào, " + Name));
            LoadStudentTuitionCM = new RelayCommand<Frame>((p) =>
            {
                if (StudentMainWindow.Slidebtn != null)
                    StudentMainWindow.Slidebtn.IsChecked = false;
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Thông tin học phí";
                if (p != null)
                    p.Content = new StudentTuitionPage();
            });
            LoadStudentHomeCM = new RelayCommand<Frame>((p) =>
            {
                
                if (StudentMainWindow.Slidebtn != null)
                    StudentMainWindow.Slidebtn.IsChecked = false;
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Ngôi nhà chung";
                if (p != null)
                    p.Content = new StudentHomePage();
                MainFrame = p;
            });

            LoadOpenCourseListCM = new RelayCommand<Frame>((p) =>
            {
                if (StudentMainWindow.Slidebtn != null)
                    StudentMainWindow.Slidebtn.IsChecked = false;
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Danh sách học phần";
                if (p != null)
                    p.Content = new OpenCourseListPage();
            });

            LoadSchedulerCM = new RelayCommand<Frame>((p) =>
            {
                if (StudentMainWindow.Slidebtn != null)
                    StudentMainWindow.Slidebtn.IsChecked = false;
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Thời khóa biểu";
                if (p != null)
                    p.Content = new SchedulerPage();
            });

            FrameworkElement GetParentWindow(FrameworkElement p)
            {
                FrameworkElement parent = p;

                while (parent.Parent != null)
                {
                    parent = parent.Parent as FrameworkElement;
                }
                return parent;
            }
            SignoutCM = new RelayCommand<FrameworkElement>((p) =>
            {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Hide();
                    LoginWindow w1 = new LoginWindow();
                    w1.ShowDialog();
                    w.Close();
                }
            });


        }

    }

}

