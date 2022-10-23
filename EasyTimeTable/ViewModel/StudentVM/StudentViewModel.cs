using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student.Home;
using EasyTimeTable.Views.Student.Tuition;
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
        public static Frame? MainFrame { get; set; }


        public StudentViewModel()
        {
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

