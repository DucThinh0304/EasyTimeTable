using EasyTimeTable.Model;
using EasyTimeTable.ViewModel.StudentVM.TuitionVM;
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

namespace EasyTimeTable.Views.Staff.TuiTion
{
    /// <summary>
    /// Interaction logic for ManageTuition.xaml
    /// </summary>
    public partial class ManageTuition : Page { 
        public static List<HocKi> hocKis;
        public static List<SinhVienHocPhi> students;
        public ManageTuition()
        {
            InitializeComponent();
            LoadComboBox();
            comboDotDKHP.SelectedIndex = 0;
        }
        public void LoadComboBox()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT kihoc, namhoc, kieuhocphan FROM HOCKI", con);
            hocKis = new List<HocKi>();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboDotDKHP.Items.Add("Học kì " + dr.GetInt32(0).ToString() + " năm học " + dr.GetString(1));
                hocKis.Add(new HocKi
                {
                    KiHoc = dr.GetInt32(0),
                    NamHoc = dr.GetString(1),
                    KieuHocPhan = dr.GetInt32(2)
                });
            }    
        }
        public void LoadSinhVien(int KiHoc, string NamHoc, int KieuHocPhi)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT masv, tensv, lophoc from sinhvien", con);
            students = new List<SinhVienHocPhi>();
            var dr = cmd.ExecuteReader();
            int n = 0;
            while (dr.Read())
            {
                n++;
                students.Add(new SinhVienHocPhi
                {
                    STT = n,
                    MaSV = dr.GetString(0),
                    HoTen = dr.GetString(1),
                    Lop = dr.GetString(2)
                });
            }
            dr.Close();
            if (KieuHocPhi == 1)
            {
                int giaTinChi = 0;
                double HeSo = 0;
                double HeSoHe = 0;
                cmd.CommandText = "SELECT giatinchi, hesohoclai, hesohoche FROM THAMSO";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    giaTinChi = dr.GetInt32(0);
                    HeSo = dr.GetDouble(1);
                    HeSoHe = dr.GetDouble(2);
                }

                dr.Close();
                for (int i = 0; i < n; i++)
                {
                    cmd.CommandText = "SELECT SUM(Sotclt), SUM(sotcth), lanhoc FROM hocphan, monhoc, lophocphansinhvien where hocphan.mamon = monhoc.mamon and lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '" + students[i].MaSV + "' group by lanhoc";
                    dr = cmd.ExecuteReader();
                    double h = 0;
                    while (dr.Read())
                    {
                        int t = dr.GetInt32(2);
                        if (t > 1) h = h + HeSo * dr.GetInt32(0) + HeSo * dr.GetInt32(1);
                        else h = h + dr.GetInt32(0) + dr.GetInt32(1);
                    }
                    if (KiHoc == 3) h = h * HeSoHe;
                    students[i].HocPhi = (int)h * giaTinChi;
                    h = 0;
                    dr.Close();
                    cmd.CommandText = "SELECT SUM(Sotclt), SUM(sotcth), lanhoc FROM hocphan, monhoc, lophocphansinhvien where hocphan.mamon = monhoc.mamon and lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '" + students[i].MaSV + "' and ngaythanhtoan is not null and daduyet = 1 group by lanhoc";
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int t = dr.GetInt32(2);
                        if (t > 1) h = h + HeSo * dr.GetInt32(0) + HeSo * dr.GetInt32(1);
                        else h = h + dr.GetInt32(0) + dr.GetInt32(1);
                    }

                    dr.Close();
                    if (KiHoc == 3) h = h * HeSoHe;
                    students[i].HocPhiDaDong = (int)h * giaTinChi;
                    if (students[i].HocPhiDaDong == students[i].HocPhi) students[i].TinhTrangHocPhi = "Đã đóng";
                    else
                    {
                        h = 0;
                        cmd.CommandText = "SELECT count(*) FROM lophocphansinhvien where masv = '" + students[i].MaSV + "' and ngaythanhtoan is not null and daduyet = 0";
                        dr = cmd.ExecuteReader();
                        if (dr.Read()) h = dr.GetInt32(0);
                        if (h > 0) students[i].TinhTrangHocPhi = "Đang chờ xác nhận";
                        else students[i].TinhTrangHocPhi = "Chưa đóng";
                    }

                    dr.Close();

                }
            }
            else if (KieuHocPhi==2)
            {
                int giaTinChi = 0;
                double HeSo = 0;
                int GiaTronGoi = 0;
                cmd.CommandText = "SELECT giatinchi, hesohoclai, giatrongioi FROM THAMSO";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    giaTinChi = dr.GetInt32(0);
                    HeSo = dr.GetDouble(1);
                    GiaTronGoi = dr.GetInt32(2);
                }

                dr.Close();
                for (int i = 0; i < n; i++)
                {
                    cmd.CommandText = "SELECT SUM(Sotclt), SUM(sotcth), lanhoc FROM hocphan, monhoc, lophocphansinhvien where hocphan.mamon = monhoc.mamon and lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '" + students[i].MaSV + "' group by lanhoc";
                    dr = cmd.ExecuteReader();
                    double h = 0;
                    while (dr.Read())
                    {
                        int t = dr.GetInt32(2);
                        if (t > 1) h = h + HeSo * dr.GetInt32(0) + HeSo * dr.GetInt32(1);
                    }

                    dr.Close();
                    students[i].HocPhi = (int)h * giaTinChi + GiaTronGoi;
                    h = 0;
                    cmd.CommandText = "SELECT SUM(Sotclt), SUM(sotcth), lanhoc FROM hocphan, monhoc, lophocphansinhvien where hocphan.mamon = monhoc.mamon and lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '" + students[i].MaSV + "' and ngaythanhtoan is not null and daduyet = 1 group by lanhoc";
                    dr = cmd.ExecuteReader();
                    bool flag = false;
                    while (dr.Read())
                    {
                        int t = dr.GetInt32(2);
                        if (t > 1) h = h + HeSo * dr.GetInt32(0) + HeSo * dr.GetInt32(1);
                        else flag = true;
                    }

                    dr.Close();
                    if (flag) students[i].HocPhiDaDong = (int)h * giaTinChi + GiaTronGoi;
                    else students[i].HocPhiDaDong = (int)h * giaTinChi;
                    if (students[i].HocPhiDaDong == students[i].HocPhi) students[i].TinhTrangHocPhi = "Đã đóng";
                    else
                    {
                        h = 0;
                        cmd.CommandText = "SELECT count(*) FROM lophocphansinhvien where masv = '" + students[i].MaSV + "' and ngaythanhtoan is not null and daduyet = 0";
                        dr = cmd.ExecuteReader();
                        if (dr.Read()) h = dr.GetInt32(0);
                        if (h > 0) students[i].TinhTrangHocPhi = "Đang chờ xác nhận";
                        else students[i].TinhTrangHocPhi = "Chưa đóng";
                    }

                    dr.Close();
                }
            }    
            Grid.ItemsSource = students;
        }
        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonSua.IsEnabled = true;
        }

        private void comboDotDKHP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSinhVien(hocKis[comboDotDKHP.SelectedIndex].KiHoc, hocKis[comboDotDKHP.SelectedIndex].NamHoc, hocKis[comboDotDKHP.SelectedIndex].KieuHocPhan);
        }

        private void buttonThem_Click(object sender, RoutedEventArgs e)
        {
            EditTuition.view = this;
            EditTuition.hk = hocKis[comboDotDKHP.SelectedIndex];
            EditTuition m = new EditTuition();
            m.Show();
        }

        private void buttonSua_Click(object sender, RoutedEventArgs e)
        {
            EditStudentTuition.view = this;
            EditStudentTuition.ki = hocKis[comboDotDKHP.SelectedIndex];
            EditStudentTuition.sv = students[Grid.SelectedIndex];
            EditStudentTuition m = new EditStudentTuition();
            m.Show();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<SinhVienHocPhi> course2 = new List<SinhVienHocPhi>();
            foreach (var item in students) course2.Add(item);
            int i = 0;
            while (i < course2.Count)
            {
                if (!(course2[i].MaSV.Contains(SearchBox.Text) || course2[i].HoTen.Contains(SearchBox.Text) || course2[i].Lop.Contains(SearchBox.Text) || course2[i].TinhTrangHocPhi.Contains(SearchBox.Text)))
                    course2.RemoveAt(i);
                else i++;
            }
            Grid.ItemsSource = course2;
        }
    }
}
