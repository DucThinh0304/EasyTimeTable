using EasyTimeTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace EasyTimeTable.Views.Student.OpenCourse
{
    /// <summary>
    /// Interaction logic for CustomRequestYesNoDialog.xaml
    /// </summary>
    public partial class CustomRequestYesNoDialog : Window
    {

       private string MaYeuCau;
        private string MSSV;

        public CustomRequestYesNoDialog()
        {
            InitializeComponent();
        }

        public CustomRequestYesNoDialog(string MaYeuCau, string name, string MaMon)
        {
            InitializeComponent();
            txt.Text = "Bạn có chắc chắn muốn đăng kí môn " + MaMon + " - " + name;
            this.MaYeuCau = MaYeuCau;
            this.MSSV = LoginViewModel.mssv;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("INSERT INTO SINHVIENYEUCAU VALUES ('" + MaYeuCau + "', '" + MSSV + "')", con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Ghi danh thành công");
            this.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
