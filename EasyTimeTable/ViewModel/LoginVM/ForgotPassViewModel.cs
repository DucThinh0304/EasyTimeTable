using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using System.Threading;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class ForgotPassViewModel
    {

        public static string? AccountChange;
        public static string? MaXacNhan;

        [ObservableProperty]
        public string username;

        [ObservableProperty]
        private Visibility maskVisibility;

        [ObservableProperty]
        private bool isLoading;

        public ICommand PreviousPageCM { get; set; }

        public ICommand ConfirmCM { get; set; }

        public ForgotPassViewModel()
        {
            MaskVisibility = Visibility.Collapsed;
            PreviousPageCM = new RelayCommand<object>((p) =>
            {
                if (LoginWindow.funcTitle != null)
                    LoginWindow.funcTitle.Text = "Đăng nhập";
                LoginViewModel.MainFrame.Content = new LoginPage();
            });

            ConfirmCM = new RelayCommand<object>(async (p) =>
            {
                MaskVisibility = Visibility.Visible;
                IsLoading = true;
                if (Username != null)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("Select * from taikhoan where MSSV = '" + Username + "'", con);
                    var dr = await cmd.ExecuteReaderAsync();
                    if (await dr.ReadAsync())
                    {
                        AccountChange = Username;
                        await ChangePage();
                        LoginViewModel.MainFrame.Content = new EnterCode(); 
                    }
                    else
                    {
                        MessageBox.Show("Không có tài khoản này");
                    }
                }
                MaskVisibility = Visibility.Collapsed;
                IsLoading = false;
            });

        }

        public async Task ChangePage()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select EMAIL from SINHVIEN, taikhoan where MSSV = '" + AccountChange + "' and taikhoan.mssv = sinhvien.masv", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
                await SendMail(dr.GetString(0));
            else
            {
                SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con2.Open();
                var cmd2 = new SqlCommand("Select EMAIL from GiaoVien, taikhoan where MSSV = '" + AccountChange + "' and taikhoan.mssv = giaovien.magv", con2);
                var dr2 = cmd2.ExecuteReader();
                if (dr2.Read())
                    await SendMail(dr2.GetString(0));
            }
        }

        protected async Task SendMail(string CusMail)
        {

            Random Ran = new Random();
            MaXacNhan = Ran.Next(1000, 9999).ToString();

            MailMessage Mess = new MailMessage("20520782@gm.uit.edu.vn", CusMail, "[EASY TIME TABLE]", "Mã xác nhận: " + MaXacNhan);
            Mess.BodyEncoding = System.Text.Encoding.UTF8;
            Mess.SubjectEncoding = System.Text.Encoding.UTF8;
            Mess.IsBodyHtml = true;
            Mess.Sender = new MailAddress("20520782@gm.uit.edu.vn", "[EASY TIME TABLE]");
            SmtpClient Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("20520782@gm.uit.edu.vn", "ranhgheha");
            try
            {
                await Client.SendMailAsync(Mess);
                MessageBox.Show("Đã gửi mã xác nhận. Bạn vui lòng kiểm tra trong hộp thư gmail của bạn.");
            }
            catch (Exception)
            {
                MessageBox.Show("Đã có lỗi xảy ra");
            }

        }
    }
}
