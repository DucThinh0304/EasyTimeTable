using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student.Home;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Staff;
using EasyTimeTable.Views.Staff.Home;
using EasyTimeTable.Views.Staff.Course;
using EasyTimeTable.Views.LoginWindow;
using EasyTimeTable.Views.Staff.TuiTion;
using System.Configuration;
using System.Data.SqlClient;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StaffViewModel
    {

        private string MAGV;
        public ICommand LoadStaffHomeCM { get; set; }
        public ICommand LoadStaffTuitionCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public ICommand LoadStaffCourseListCM { get; set; }
        public ICommand LoadListOfRegister { get; set; }
        public static Frame? Frame { get; set; }

        [ObservableProperty]
        private string currentUserName;

        [ObservableProperty]
        public String selectFuncName;

        public StaffViewModel()
        {
            MAGV = LoginViewModel.mssv;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select tengv from giaovien where magv = '" + MAGV + "'", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CurrentUserName = dr.GetString(0);
            }
            LoadStaffHomeCM = new RelayCommand<Frame>((p) =>
            {

                if (StaffWindow.Slidebtn != null)
                    StaffWindow.Slidebtn.IsChecked = false;
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Ngôi nhà chung";
                if (p != null)
                    p.Content = new StaffHomePage();
                Frame = p;
            });

            LoadStaffCourseListCM = new RelayCommand<Frame>((p) =>
            {

                if (StaffWindow.Slidebtn != null)
                    StaffWindow.Slidebtn.IsChecked = false;
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Quản lý học phần";
                if (p != null)
                    p.Content = new ManageCourses();
            });
            LoadListOfRegister = new RelayCommand<Frame>((p) =>
            {
                if (StaffWindow.Slidebtn != null)
                    StaffWindow.Slidebtn.IsChecked = false;
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Quản lý đợt đăng kí học phần";
                if (p != null)
                    p.Content = new ManageDotDKHP();
            });
            LoadStaffTuitionCM = new RelayCommand<Frame>((p) =>
            {
                if (StaffWindow.Slidebtn != null)
                    StaffWindow.Slidebtn.IsChecked = false;
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Quản lý học phí";
                if (p != null)
                    p.Content = new ManageTuition();
            });

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
            
            FrameworkElement GetParentWindow(FrameworkElement p)
            {
                FrameworkElement parent = p;

                while (parent.Parent != null)
                {
                    parent = parent.Parent as FrameworkElement;
                }
                return parent;
            }


        }
    }
}
