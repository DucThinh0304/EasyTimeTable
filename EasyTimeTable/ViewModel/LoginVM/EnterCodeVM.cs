using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Security.Policy;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class EnterCodeVM
    {
        public ICommand PreviousPageCM { get; set; }
        public ICommand LoadCM { get; set; }
        public ICommand PasswordChangedCM { get; set; }

        public ICommand ConfirmCM { get; set; }

        public ICommand GetCodeCM { get; set; }


        private string Code;

        private string MaXacNhan;
        public EnterCodeVM()
        {
            PreviousPageCM = new RelayCommand<object>((p) =>
            {
                LoginViewModel.MainFrame.Content = new ForgotPasswordPage();
            });

            LoadCM = new RelayCommand<object>((p) =>
            {
                string AccountChange = ForgotPassViewModel.AccountChange;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("Select EMAIL from SINHVIEN, taikhoan where MSSV = '" + AccountChange + "' and taikhoan.mssv = sinhvien.masv", con);
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                    SendMail(dr.GetString(0));
                else
                {
                    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con2.Open();
                    var cmd2 = new SqlCommand("Select EMAIL from GiaoVien, taikhoan where MSSV = '" + AccountChange + "' and taikhoan.mssv = giaovien.magv", con2);
                    var dr2 = cmd2.ExecuteReader();
                    if (dr2.Read())
                        SendMail(dr2.GetString(0));
                }
            });

            PasswordChangedCM = new RelayCommand<PasswordBox>((p) =>
            {
                Code = p.Password;
            });

            ConfirmCM = new RelayCommand<object>((p) =>
            {
                if (MaXacNhan == Code)
                {
                    LoginViewModel.MainFrame.Content = new ChangePassword();
                }
                else
                {
                    MessageBox.Show("Mã xác nhận không chính xác");
                }
            });

            GetCodeCM = new RelayCommand<object>((p) =>
            {
                string AccountChange = ForgotPassViewModel.AccountChange;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("Select EMAIL from SINHVIEN, taikhoan where MSSV = '" + AccountChange + "' and taikhoan.mssv = sinhvien.masv", con);
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                    SendMail(dr.GetString(0));
                else
                {
                    cmd = new SqlCommand("Select EMAIL from GiaoVien, taikhoan where MSSV = '" + AccountChange + "' and taikhoan.mssv = giaovien.magv", con);
                    var dr2 = cmd.ExecuteReader();
                    if (dr2.Read())
                        SendMail(dr2.GetString(0));
                }
            });


        }

        protected void SendMail(string CusMail)
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
                Client.Send(Mess);
                MessageBox.Show("Đã gửi mã xác nhận. Bạn vui lòng kiểm tra trong hộp thư gmail của bạn.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LoginViewModel.MainFrame.Content = new ForgotPasswordPage();
            }


        }
    }
}
