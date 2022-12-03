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

namespace EasyTimeTable.Views.Student.Tuition
{
    /// <summary>
    /// Interaction logic for CustomYesNoDialog.xaml
    /// </summary>
    public partial class CustomYesNoDialog : Window
    {
        private string message;
        private string name;

        public CustomYesNoDialog()
        {
            InitializeComponent();
        }

        public CustomYesNoDialog(string message, string name)
        {
            InitializeComponent();
            txt.Text = "Bạn có chắc hủy môn " + message + " - " + name;
            this.message = message;
            this.name = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("delete from lophocphansinhvien where mahocphan like @mahocphan and masv = @masv", con);
            cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@mahocphan"].Value = message + "%";
            cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@masv"].Value = LoginViewModel.mssv;
            var dr = cmd.ExecuteNonQuery();
            MessageBox.Show("Đã hủy đăng kí môn: " + message + " - " + name);
            var viewModel = (TuitionViewModel)DataContext;
            if (viewModel.LoadDB.CanExecute(null))
                viewModel.LoadDB.Execute(null);
            if (viewModel.LoadListDB.CanExecute(null))
                viewModel.LoadListDB.Execute(null);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
