using EasyTimeTable.Model;
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
using System.Xml.Linq;

namespace EasyTimeTable.Views.Staff.TuiTion
{
    /// <summary>
    /// Interaction logic for EditStudentTuition.xaml
    /// </summary>
    public partial class EditStudentTuition : Window
    {
        public static SinhVienHocPhi sv;
        public static ManageTuition view;
        public static HocKi ki;
        public EditStudentTuition()
        {
            InitializeComponent();
            LoadDB();
        }
        public void LoadDB()
        {
            MSSV.Text = sv.MaSV;
            HoTen.Text = sv.HoTen;
            HocPhiNo.Text = (sv.HocPhi - sv.HocPhiDaDong).ToString();
            TinhTrang.Text = sv.TinhTrangHocPhi;
            if (TinhTrang.Text=="Đang chờ xác nhận")
            {
                XacNhan.IsEnabled = true;
                TuChoi.IsEnabled = true;
            }    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = MessageBox.Show("Bạn có chắc muốn từ chối xác nhận học phí cho sinh viên này?", "Từ chối", MessageBoxButton.YesNo);
            if (t == MessageBoxResult.Yes)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("UPDATE lophocphansinhvien set ngaythanhtoan = null where masv = '" + sv.MaSV + "' and daduyet = 0", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Từ chối xác nhận thành công");
                this.Close();
                view.LoadSinhVien(ki.KiHoc, ki.NamHoc, ki.KieuHocPhan);
            }
        }

        private void XacNhan_Click(object sender, RoutedEventArgs e)
        {
            var t = MessageBox.Show("Bạn có chắc muốn xác nhận sinh viên này đã hoàn thành học phí?", "Xác nhận", MessageBoxButton.YesNo);
            if (t == MessageBoxResult.Yes)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("UPDATE lophocphansinhvien set daduyet = 1 where masv = '" + sv.MaSV + "'", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật trạng thái thành công");
                this.Close();
                view.LoadSinhVien(ki.KiHoc, ki.NamHoc, ki.KieuHocPhan);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
