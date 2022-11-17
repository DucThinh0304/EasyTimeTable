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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class ForgotPassViewModel
    {

        public static string? AccountChange;

        [ObservableProperty]
        public string username;
        public ICommand PreviousPageCM { get; set; }

        public ICommand ConfirmCM { get; set; }

        public ForgotPassViewModel()
        {
            PreviousPageCM = new RelayCommand<object>((p) =>
            {
                LoginViewModel.MainFrame.Content = new LoginPage();
            });

            ConfirmCM = new RelayCommand<object>((p) =>
            {
                if (Username != null)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("Select * from taikhoan where MSSV = '" + Username + "'", con);
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        AccountChange = Username;
                        LoginViewModel.MainFrame.Content = new EnterCode();
                    }
                    else
                    {
                        MessageBox.Show("Không có tài khoản này");
                    }
                }
            });


        }
    }
}
