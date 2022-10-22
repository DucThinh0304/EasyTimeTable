﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Staff;


namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class LoginViewModel
    {
        public Button LoginBtn { get; set; }
        public static Frame? MainFrame { get; set; }
        public Window LoginWindow { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand MinimizeWindowCM { get; set; }
        public ICommand MouseLeftButtonDownWindowCM { get; set; }
        public ICommand LoadLoginPageCM { get; set; }
        public ICommand LoginCM { get; set; }
        public ICommand SaveLoginWindowNameCM { get; set; }
        public ICommand LoadForgotPassCM { get; set; }
        public ICommand LoginPageCM { get; set; }
        public ICommand PasswordChangedCM { get; set; }

        [ObservableProperty]
        private String password;

        [ObservableProperty]
        private String username;

        [ObservableProperty]
        private String currentPage;


        public LoginViewModel()
        {
            MouseLeftButtonDownWindowCM = new RelayCommand<Window>((p) =>
            {
                if (p != null)
                {
                    p.DragMove();
                }
            });

            SaveLoginWindowNameCM = new RelayCommand<Window>((p) =>
            {
                LoginWindow = p;
            });


            LoadLoginPageCM = new RelayCommand<Frame>((p) =>
            {
                CurrentPage = "Đăng nhập";
                MainFrame = p;
                p.Content = new LoginPage();

            });

            LoadForgotPassCM = new RelayCommand<object>((p) =>
            {
                CurrentPage = "Quên mật khẩu";
                MainFrame.Content = new ForgotPasswordPage();
            });

            LoginCM = new RelayCommand<Label>((p) =>
            {
                if (Password == "s" && Username == "s")
                {
                    LoginWindow.Hide();
                    StudentMainWindow studentMainWindow = new StudentMainWindow();
                    studentMainWindow.Show();
                    LoginWindow.Close();
                }
                else if (Password == "a" && Username == "a")
                {
                    LoginWindow.Hide();
                    StaffWindow staffWindow = new StaffWindow();
                    staffWindow.Show();
                    LoginWindow.Close();
                }
                else
                {
                    MessageBox.Show("Không có tài khoản này tồn tại");
                }
            });

            PasswordChangedCM = new RelayCommand<PasswordBox>((p) =>
            {
                Password = p.Password;
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
