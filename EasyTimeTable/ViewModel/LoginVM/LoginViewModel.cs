using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using EasyTimeTable.Views.Staff;
using EasyTimeTable.Views.Student;
using MaterialDesignThemes.Wpf;
using Syncfusion.Windows.Shared;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Button = System.Windows.Controls.Button;
using Window = System.Windows.Window;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class LoginViewModel
    {
        public Button LoginBtn { get; set; }
        public static Frame? MainFrame { get; set; }
        public Window Login { get; set; }
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
        private bool isMSSVFocus;

        [ObservableProperty]
        private bool isPasswordFocus;

        [ObservableProperty]
        private String username;

        [ObservableProperty]
        private String currentPage;

        public static String mssv;

        public SnackbarMessageQueue MessageQueueSnackBar { set; get; } = new(TimeSpan.FromSeconds(3));


        public LoginViewModel()
        {
            Password = "";
            MouseLeftButtonDownWindowCM = new RelayCommand<Window>((p) =>
            {
                if (p != null)
                {
                    p.DragMove();
                }
            });

            SaveLoginWindowNameCM = new RelayCommand<Window>((p) =>
            {
                Login = p;
            });


            LoadLoginPageCM = new RelayCommand<Frame>((p) =>
            {
                if (LoginWindow.funcTitle != null)
                    LoginWindow.funcTitle.Text = "Đăng nhập";
                MainFrame = p;
                p.Content = new LoginPage();
                IsMSSVFocus = true;

            });

            LoadForgotPassCM = new RelayCommand<object>((p) =>
            {
                if (LoginWindow.funcTitle != null)
                    LoginWindow.funcTitle.Text = "Quên mật khẩu";
                MainFrame.Content = new ForgotPasswordPage();
            });

            LoginCM = new RelayCommand<object>((p) =>
            {
                IsMSSVFocus = false;
                IsPasswordFocus = false;
                if (Username.IsNullOrWhiteSpace() == false && Password.IsNullOrWhiteSpace() == false)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("SELECT * FROM Taikhoan WHERE mssv = @mssv", con);
                    cmd.Parameters.Add("@mssv", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@mssv"].Value = Username;
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                        con1.Open();
                        var cmd1 = new SqlCommand("SELECT * FROM sinhvien WHERE masv = @mssv", con1);
                        cmd1.Parameters.Add("@mssv", System.Data.SqlDbType.VarChar);
                        cmd1.Parameters["@mssv"].Value = Username;
                        var dr1 = cmd1.ExecuteReader();
                        if (dr1.Read())
                        {
                            if (Converter.Converter.CreateMD5(Password) == dr.GetString(1))
                            {
                                Login.Hide();
                                mssv = Username;
                                StudentMainWindow studentMainWindow = new StudentMainWindow();
                                studentMainWindow.Show();
                                Login.Close();
                            }
                            else
                            {
                                Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Sai mật khẩu"));
                                LoginPage.password.Clear();
                                IsPasswordFocus = true;
                                IsMSSVFocus = false;

                            }
                        }
                        else
                        {
                            if (Converter.Converter.CreateMD5(Password) == dr.GetString(1))
                            {
                                Login.Hide();
                                mssv = Username;
                                StaffWindow staffWindow = new StaffWindow();
                                staffWindow.Show();
                                Login.Close();
                            }
                            else
                            {
                                Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Sai mật khẩu"));
                                IsPasswordFocus = true;
                                IsMSSVFocus = false;

                            }
                        }

                    }
                    else
                    {
                        Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Không có tài khoản này tồn tại"));
                        LoginPage.password.Clear();
                        IsMSSVFocus = true;
                        IsPasswordFocus = false;

                    }
                }
                else
                {
                    if (Username.IsNullOrWhiteSpace())
                    {
                        Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Bạn cần nhập MSSV"));
                        IsMSSVFocus = true;
                        IsPasswordFocus = false;
                    }
                    else
                    {
                        Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Bạn cần nhập mật khẩu"));
                        IsMSSVFocus = false;
                        IsPasswordFocus = true;
                    }
                }
            });

            PasswordChangedCM = new RelayCommand<PasswordBox>((p) =>
            {
                Password = p.Password;
            });

            FrameworkElement GetParentWindow(FrameworkElement p)
            {
                FrameworkElement? parent = p;

                while (parent.Parent != null)
                {
                    parent = parent.Parent as FrameworkElement;
                }
                return parent;
            }

        }
    }
}
