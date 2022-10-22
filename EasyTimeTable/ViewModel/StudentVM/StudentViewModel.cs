using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Home;
using EasyTimeTable.Views.Student.Tuition;
using EasyTimeTable.Views.Student.Course;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StudentViewModel
    {
        public ICommand LoadStudentHomeCM { get; set; }
        public ICommand LoadStudentTuitionCM { get; set; }
        public ICommand LoadOpenCourseListCM { get; set; }

        [ObservableProperty]
        public String selectFuncName;
        public StudentViewModel()
        {
            LoadStudentTuitionCM = new RelayCommand<Frame>((p) =>
            {
                if (StudentMainWindow.Slidebtn != null)
                    StudentMainWindow.Slidebtn.IsChecked = false;
                SelectFuncName = "Thông tin học phí";
                if (p != null)
                    p.Content = new StudentTuitionPage();
            });
            LoadStudentHomeCM = new RelayCommand<Frame>((p) =>
            {
                if (StudentMainWindow.Slidebtn != null)
                    StudentMainWindow.Slidebtn.IsChecked = false;
                SelectFuncName = "Ngôi nhà chung";
                if (p != null)
                    p.Content = new StudentHomePage();
            });

            LoadOpenCourseListCM = new RelayCommand<Frame>((p) =>
            {
                if (StudentMainWindow.Slidebtn != null)
                    StudentMainWindow.Slidebtn.IsChecked = false;
                SelectFuncName = "Danh sách học phần";
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
            SelectFuncName = "Ngôi nhà chung";
        }

    }
}

