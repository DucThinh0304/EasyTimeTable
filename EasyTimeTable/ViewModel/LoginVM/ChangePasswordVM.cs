using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using MaterialDesignThemes.Wpf;
using Syncfusion.Windows.Shared;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

        public SnackbarMessageQueue MessageQueueSnackBar { set; get; } = new(TimeSpan.FromSeconds(3));

        [ObservableProperty]
        private bool isPasswordFocus;

        public ChangePasswordVM()
        {
            IsPasswordFocus = true;
            PasswordChangedCM = new RelayCommand<PasswordBox>((p) =>
            {
                Password = p.Password;
            });

            ConfirmCM = new RelayCommand<object>(async (p) =>
            {
                IsPasswordFocus = false;
                string AccountChange = ForgotPassViewModel.AccountChange;
                if (Password.IsNullOrWhiteSpace() == false)
                    if (Converter.Converter.IsValidPassword(Password))
                    {
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                        con.Open();
                        var cmd = new SqlCommand("Update taikhoan set matkhau = '" + Converter.Converter.CreateMD5(Password) + "' where MSSV = '" + AccountChange + "'", con);
                        cmd.ExecuteNonQuery();
                        await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Đổi mật khẩu thành công, vui lòng đăng nhập lại!"));
                        await Task.Delay(4000);
                        if (LoginWindow.funcTitle != null)
                            LoginWindow.funcTitle.Text = "Đăng nhập";
                        LoginViewModel.MainFrame.Content = new LoginPage();
                    }
                    else
                    {
                        if (Password.Length < 6)
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu ít nhất phải có 6 chữ cái"));
                        }
                        else if (Password.Any(c => Converter.Converter.IsLetter(c)) == false)
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu phải có ít nhất 1 chữ cái thường và in hoa"));
                        }
                        else if (Password.Any(c => Converter.Converter.IsDigit(c)) == false)
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu phải có ít nhất 1 chữ số"));
                        }
                        else
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu phải có ít nhất 1 kí tự đặc biệt"));
                        }
                        IsPasswordFocus = true;
                        ChangePassword.pass.Clear();
                    }
                else
                {
                    IsPasswordFocus = true;
                    await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu không được để trống"));
                }
            });



        }

    }
}
