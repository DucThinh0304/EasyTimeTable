using EasyTimeTable.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyTimeTable.Views.Student.Tuition
{
    /// <summary>
    /// Interaction logic for StudentTuitionPage.xaml
    /// </summary>
    public partial class StudentTuitionPage : Page
    {
        public OpenCourseModel current;
        public StudentTuitionPage()
        {
            InitializeComponent();
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            current = new OpenCourseModel();
            var item = sender as ListViewItem;
            if (item != null)
            {
                current = (OpenCourseModel)item.DataContext;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("delete from lophocphansinhvien where mahocphan = @mahocphan and masv = @masv", con);
            cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@mahocphan"].Value = current.MaHocPhan;
            cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@masv"].Value = "20520782";
            var dr = cmd.ExecuteNonQuery();
            MessageBox.Show("Đã hủy đăng kí");
            var viewModel = (TuitionViewModel)DataContext;
            if (viewModel.LoadDB.CanExecute(null))
                viewModel.LoadDB.Execute(null);
            if (viewModel.LoadListDB.CanExecute(null))
                viewModel.LoadListDB.Execute(null);
            return;
        }
    }
}
