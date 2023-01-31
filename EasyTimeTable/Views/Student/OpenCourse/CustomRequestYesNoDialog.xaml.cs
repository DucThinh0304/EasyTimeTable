using EasyTimeTable.ViewModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace EasyTimeTable.Views.Student.OpenCourse
{
    /// <summary>
    /// Interaction logic for CustomRequestYesNoDialog.xaml
    /// </summary>
    public partial class CustomRequestYesNoDialog : Window
    {

        private string MaYeuCau;
        private string MSSV;
        bool huy;

        public CustomRequestYesNoDialog()
        {
            InitializeComponent();
        }

        public CustomRequestYesNoDialog(string MaYeuCau, string name, string MaMon, bool huy)
        {
            InitializeComponent();
            this.huy = huy;
            if (!huy)
            {
                txt.Text = "Bạn có chắc chắn muốn ghi danh môn " + MaMon + " - " + name;
                this.MaYeuCau = MaYeuCau;
                this.MSSV = LoginViewModel.mssv;

            }
            else
            {
                txt.Text = "Bạn có chắc chắn muốn hủy ghi danh môn " + MaMon + " - " + name;
                this.MaYeuCau = MaYeuCau;
                this.MSSV = LoginViewModel.mssv;
                this.Titlee.Text = "Xác nhận hủy ghi danh";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            if (!huy)
            {
                var cmd = new SqlCommand("INSERT INTO SINHVIENYEUCAU VALUES ('" + MaYeuCau + "', '" + MSSV + "')", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ghi danh thành công");
                this.Close();
            }
            else
            {
                var cmd = new SqlCommand("DELETE From sinhvienyeucau where Masv = '" + MSSV + "' and MAYC = '"+ MaYeuCau +"'" , con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Hủy ghi danh thành công");
                this.Close();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
