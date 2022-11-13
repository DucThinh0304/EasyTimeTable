using EasyTimeTable.Model;
using EasyTimeTable.Views.Staff.Course;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyTimeTable.Views.Staff
{
    /// <summary>
    /// Interaction logic for EditDotDKHP.xaml
    /// </summary>
    public partial class EditDotDKHP : Window
    {
        public static DotDKHP DotDKHPChon;
        public static ManageDotDKHP view;
        public EditDotDKHP()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
            LoadDB();
        }
        public void LoadDB()
        {
            NgayBatDau.SelectedDate = DotDKHPChon.NgayBatDau;
            NgayKetThuc.SelectedDate = DotDKHPChon.NgayKetThuc;
            textDot.Text += " " + DotDKHPChon.MaDot.ToString() + " học kì " + DotDKHPChon.HocKi + " năm học " + DotDKHPChon.NamHoc;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SET DATEFORMAT DMY", con);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "UPDATE dotdkhp set ngaybatdau = '" + NgayBatDau.Text + "', ngayketthuc = '" + NgayKetThuc.Text + "' where madot = " + DotDKHPChon.MaDot.ToString() + " and kihoc = " + DotDKHPChon.HocKi.ToString() + " and namhoc = '" + DotDKHPChon.NamHoc + "'";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Cập nhật thông tin thành công");
            this.Close();
            view.LoadDB(DotDKHPChon.HocKi, DotDKHPChon.NamHoc);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            view.LoadDB(DotDKHPChon.HocKi, DotDKHPChon.NamHoc);
        }
    }
}
