using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyTimeTable.Views.Account
{
    /// <summary>
    /// Interaction logic for ChangePasswordAccount.xaml
    /// </summary>
    public partial class ChangePasswordAccount : Window
    {
        static public PasswordBox OldPassword;
        static public PasswordBox NewPassword;
        static public PasswordBox RePassword;
        public ChangePasswordAccount()
        {
            InitializeComponent();
        }

        private void MatKhauCu_Loaded(object sender, RoutedEventArgs e)
        {
            OldPassword = MatKhauCu;
        }

        private void MatKhauMoi_Loaded(object sender, RoutedEventArgs e)
        {
            NewPassword = MatKhauMoi;
        }

        private void NhapLaiMatKhau_Loaded(object sender, RoutedEventArgs e)
        {
            RePassword = NhapLaiMatKhau;
        }
    }
}
