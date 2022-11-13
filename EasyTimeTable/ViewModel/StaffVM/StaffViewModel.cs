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

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StaffViewModel
    {
        public ICommand LoadStaffHomeCM { get; set; }
        public ICommand LoadStaffTuitionCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public ICommand LoadStaffCourseListCM { get; set; }
        public ICommand LoadListOfRegister { get; set; }
        public static Frame? MainFrame { get; set; }

        [ObservableProperty]
        public String selectFuncName;

        public StaffViewModel()
        {
            LoadStaffHomeCM = new RelayCommand<Frame>((p) =>
            {

                if (StaffWindow.Slidebtn != null)
                    StaffWindow.Slidebtn.IsChecked = false;
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Ngôi nhà chung";
                if (p != null)
                    p.Content = new StaffHomePage();
                MainFrame = p;
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
                    StaffWindow.funcTitle.Text = "Quản lý đợt Đăng kí học phần";
                if (p != null)
                    p.Content = new ManageDotDKHP();
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
