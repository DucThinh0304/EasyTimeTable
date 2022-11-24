using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasyTimeTable.Views.Staff;
using EasyTimeTable.Views.Staff.Course;
using EasyTimeTable.Views.Staff.TuiTion;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class HomeStaffVM
    {
        public ICommand CoursePageCM { get; set; }
        public ICommand TuitionPageCM { get; set; }
        public ICommand DotPageCM { get; set; }
        public HomeStaffVM()
        {
            CoursePageCM = new RelayCommand<object>((p) => {
                StaffViewModel.Frame.Content = new ManageCourses();
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Quản lý học phần";
            });

            TuitionPageCM = new RelayCommand<object>((p) => {
                StaffViewModel.Frame.Content = new ManageDotDKHP();
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Quản lý đợt đăng kí học phần";
            });

            DotPageCM = new RelayCommand<object>((p) => {
                StaffViewModel.Frame.Content = new ManageTuition();
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Quản lý học phí";
            });
        }

    }
}
