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
                if (Password.Length >= 5)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("Update taikhoan set matkhau = '" + Converter.Converter.CreateMD5(Password) + "' where MSSV = '" + AccountChange + "'", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đổi mật khẩu thành công, vui lòng đăng nhập lại!");
                    LoginViewModel.MainFrame.Content = new LoginPage();
                }
                else
                {
                    MessageBox.Show("Mật khẩu cần có ít nhất 5 chữ cái");
                }
            });
        }

    }
}
