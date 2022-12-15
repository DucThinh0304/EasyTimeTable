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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyTimeTable.Views.LoginWindow
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        public static PasswordBox pass;
        public static PasswordBox conpass;
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void FloatingPasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            pass = FloatingPasswordBox;
        }

        private void ConfirmPasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            conpass = ConfirmPasswordBox;
        }
    }
}
