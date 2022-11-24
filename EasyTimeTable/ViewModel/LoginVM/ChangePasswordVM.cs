using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class ChangePasswordVM
    {
        public ICommand PasswordChangedCM { get; set; }
        public ICommand ConfirmCM { get; set; }

        private string Password;

        public ChangePasswordVM()
        {
            PasswordChangedCM = new RelayCommand<PasswordBox>((p) =>
            {
                Password = p.Password;
            });

            ConfirmCM = new RelayCommand<object>((p) =>
            {
                string AccountChange = ForgotPassViewModel.AccountChange;
                if (Converter.Converter.IsValidPassword(Password))
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("Update taikhoan set matkhau = '" + Converter.Converter.CreateMD5(Password) + "' where MSSV = '" + AccountChange + "'", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đổi mật khẩu thành công, vui lòng đăng nhập lại!");
                    if (LoginWindow.funcTitle != null)
                        LoginWindow.funcTitle.Text = "Đăng nhập";
                    LoginViewModel.MainFrame.Content = new LoginPage();
                }
                else
                {
                    MessageBox.Show("Mật khẩu không hợp lệ");
                }
            });


            
        }

    }
}
