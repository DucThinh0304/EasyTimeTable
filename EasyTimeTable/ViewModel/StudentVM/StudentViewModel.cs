using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Calendar;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.OpenCourse;
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
using System.Runtime.CompilerServices;
using EasyTimeTable.Views.Account;
using System.Runtime.Intrinsics.X86;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StudentViewModel
    {

        [ObservableProperty]
        private Visibility mask;
        public ICommand LoadStudentHomeCM { get; set; }
        public ICommand LoadStudentTuitionCM { get; set; }
        public ICommand LoadOpenCourseListCM { get; set; }
        public ICommand LoadOpenCourseCM { get; set; }
        public ICommand LoadSchedulerCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public static Frame? MainFrame { get; set; }

        private string MSSV;

        [ObservableProperty]
        private string currentUserName;
        [ObservableProperty]
        private Visibility noAvt;
        [ObservableProperty]
        private Visibility avt;
        [ObservableProperty]
        private ImageSource img;

        private string Name;

        [ObservableProperty]
        private bool isLoading;

        public SnackbarMessageQueue MessageQueueSnackBar { set; get; } = new(TimeSpan.FromSeconds(3));

        public StudentViewModel()
        {
            Mask = Visibility.Collapsed;
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

            LoadOpenCourseCM = new RelayCommand<Frame>((p) =>
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("Select ngaythanhtoan from lophocphansinhvien where masv = '" + MSSV + "' and ngaythanhtoan is not null" , con);
                var dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    if (StudentMainWindow.Slidebtn != null)
                        StudentMainWindow.Slidebtn.IsChecked = false;
                    if (StudentMainWindow.funcTitle != null)
                        StudentMainWindow.funcTitle.Text = "Danh sách học phần";
                    if (p != null)
                        p.Content = new OpenCoursePage();
                }
                else
                {
                    MessageBox.Show("Bạn đã thanh toán học phí");
                }
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
        

        [RelayCommand]
        private void LoadDB()
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

            string path = "../../../Assets/Student - " + LoginViewModel.mssv + ".jpg";
            if (File.Exists(path))
            {
                NoAvt = Visibility.Collapsed;
                Avt = Visibility.Visible;
                BitmapImage result = new BitmapImage();
                string ImageURL = "../../../Assets/Student - " + LoginViewModel.mssv + ".jpg";
                if (!string.IsNullOrEmpty(ImageURL) && File.Exists(ImageURL))
                {
                    using (var stream = File.OpenRead(ImageURL))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        result = image;
                    }
                }
                Img = result;
            }
            else
            {
                Avt = Visibility.Collapsed;
                NoAvt = Visibility.Visible;
            }

            Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Xin chào, " + Name));
        }

        [RelayCommand]
        private void ChangeInfo()
        {
            Mask = Visibility.Visible;
            ChangeAccountInfo changeAccountInfo = new ChangeAccountInfo();
            changeAccountInfo.Tag = "changeAccountInfo";
            changeAccountInfo.ShowDialog();
            LoadDBInfo();
            Mask = Visibility.Collapsed;
        }

        private void LoadDBInfo()
        {
            MSSV = LoginViewModel.mssv;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select tensv from sinhvien where masv = '" + MSSV + "'", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CurrentUserName = dr.GetString(0);
            }
            string path = "../../../Assets/Student - " + LoginViewModel.mssv + ".jpg";
            if (File.Exists(path))
            {
                NoAvt = Visibility.Collapsed;
                Avt = Visibility.Visible;
                BitmapImage result = new BitmapImage();
                string ImageURL = "../../../Assets/Student - " + LoginViewModel.mssv + ".jpg";
                if (!string.IsNullOrEmpty(ImageURL) && File.Exists(ImageURL))
                {
                    using (var stream = File.OpenRead(ImageURL))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        result = image;
                    }
                }
                Img = result;
            }
            else
            {
                Avt = Visibility.Collapsed;
                NoAvt = Visibility.Visible;
            }

        }

    }

}

