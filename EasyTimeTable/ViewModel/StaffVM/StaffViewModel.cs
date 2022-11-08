﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student.Home;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Staff;
using EasyTimeTable.Views.Staff.Course;
using MaterialDesignThemes.Wpf;
using EasyTimeTable.Views.Student.Course;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StaffViewModel
    {
        public ICommand LoadStaffHomeCM { get; set; }
        public ICommand LoadStaffTuitionCM { get; set; }
        public ICommand LoadOpenCourseListCM { get; set; }

        [ObservableProperty]
        public String selectFuncName;
        public StaffViewModel()
        {
            FrameworkElement GetParentWindow(FrameworkElement p)
            {
                FrameworkElement parent = p;

                while (parent.Parent != null)
                {
                    parent = parent.Parent as FrameworkElement;
                }
                return parent;
            }
            LoadStaffHomeCM = new RelayCommand<Frame>((p) =>
            {
                if (StaffWindow.Slidebtn != null)
                    StaffWindow.Slidebtn.IsChecked = false;
                if (StaffWindow.funcTitle != null)
                    StaffWindow.funcTitle.Text = "Ngôi nhà chung";
            });
            LoadOpenCourseListCM = new RelayCommand<object>((p) =>
            {
                ManageCourses m = new ManageCourses();
                m.Show();
                ManageCourses.Window.Hide();
            });
        }
    }
}
