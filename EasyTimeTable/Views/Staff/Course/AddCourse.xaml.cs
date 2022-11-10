using EasyTimeTable.Model;
using Syncfusion.Windows.Controls;
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

namespace EasyTimeTable.Views.Staff.Course
{
    /// <summary>
    /// Interaction logic for AddCourse.xaml
    /// </summary>
    public partial class AddCourse : Window
    {
        public List<GiaoVienModel> TenGV;
        public List<MonHocModel> TenMon;
        public List<PhongHocModel> PhongHoc;
        public List<PhongHocModel> PhongThucHanh = new List<PhongHocModel>();
        public List<string> Tiet;
        public ManageCourses view;
        public static DotDKHP dotDKHP;
        public AddCourse()
        {
            InitializeComponent();
            LoadTenMon();
            foreach (var item in TenMon)
            {
                comboMonHoc.Items.Add(item.TenMon);
            }
        }
        private void LoadTenMon()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            TenMon = new List<MonHocModel>();
            var cmd = new SqlCommand("SELECT mamon, tenmon, sotclt, sotcth FROM MONHOC", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TenMon.Add(new MonHocModel
                {
                    MaMon = dr.GetString(0),
                    TenMon = dr.GetString(1),
                    SoTCLT = dr.GetInt32(2),
                    SoTCTH = dr.GetInt32(3)
                });

            }
            dr.Close();
            con.Close();
        }
        private void LoadTenGiaoVien(string MaMon)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            TenGV = new List<GiaoVienModel>();
            var cmd = new SqlCommand("SELECT giangday.magv, tengv FROM GIAOVIEN, GIANGDAY WHERE mamon = '" + MaMon + "' AND GIAOVIEN.magv = GIANGDAY.magv", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TenGV.Add(new GiaoVienModel
                {
                    MaGV = dr.GetString(0),
                    TenGV = dr.GetString(1)
                });

            }
            dr.Close();
            con.Close();
        }
        private void LoadTiet(int SoTCLT)
        {
            Tiet = new List<string>();
            switch (SoTCLT)
            {
                case 1:
                    Tiet.Add("1");
                    Tiet.Add("2");
                    Tiet.Add("3");
                    Tiet.Add("4");
                    Tiet.Add("5");
                    Tiet.Add("6");
                    Tiet.Add("7");
                    Tiet.Add("8");
                    Tiet.Add("9");
                    Tiet.Add("0");
                    Tiet.Add("12");
                    Tiet.Add("23");
                    Tiet.Add("34");
                    Tiet.Add("45");
                    Tiet.Add("67");
                    Tiet.Add("78");
                    Tiet.Add("89");
                    Tiet.Add("90");
                    break;
                case 2:
                    Tiet.Add("12");
                    Tiet.Add("23");
                    Tiet.Add("34");
                    Tiet.Add("45");
                    Tiet.Add("67");
                    Tiet.Add("78");
                    Tiet.Add("89");
                    Tiet.Add("90");
                    Tiet.Add("123");
                    Tiet.Add("234");
                    Tiet.Add("345");
                    Tiet.Add("678");
                    Tiet.Add("789");
                    Tiet.Add("890");
                    break;
                case 3:
                    Tiet.Add("123");
                    Tiet.Add("234");
                    Tiet.Add("345");
                    Tiet.Add("678");
                    Tiet.Add("789");
                    Tiet.Add("890");
                    Tiet.Add("1234");
                    Tiet.Add("2345");
                    Tiet.Add("6789");
                    Tiet.Add("7890");
                    break;
                case 4:
                    Tiet.Add("1234");
                    Tiet.Add("2345");
                    Tiet.Add("6789");
                    Tiet.Add("7890");
                    Tiet.Add("12345");
                    Tiet.Add("67890");
                    break;
                default:
                    break;
            }
        }
        private void LoadPhong(int Thu, string Tiet)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            PhongHoc = new List<PhongHocModel>();
            var cmd = new SqlCommand("SELECT sophong, toa, soluongtoida, laphongmay FROM PHONGHOC EXCEPT SELECT phonghoc.sophong, phonghoc.toa, soluongtoida, laphongmay FROM PHONGHOC, HOCPHAN WHERE" +
                " PHONGHOC.sophong = HOCPHAN.sophong AND HOCPHAN.toa = PHONGHOC.toa and laphongmay=0 AND thu= " + Convert.ToString(Thu), con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                PhongHoc.Add(new PhongHocModel
                {
                    SoPhong = dr.GetString(0),
                    Toa = dr.GetString(1),
                    SoLuongToiDa = dr.GetInt32(2),
                    LaPhongmay = false
                });
            }
            dr.Close();
            cmd = new SqlCommand("SELECT phonghoc.sophong, phonghoc.toa, soluongtoida, laphongmay, tiethoc FROM PHONGHOC, HOCPHAN WHERE" +
                " PHONGHOC.sophong = HOCPHAN.sophong AND HOCPHAN.toa = PHONGHOC.toa and laphongmay=0 AND thu= " + Convert.ToString(Thu), con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string t = dr.GetString(4);
                if (Converter.Converter.CommonChars(t, Tiet) == 0)
                {
                    PhongHoc.Add(new PhongHocModel
                    {
                        SoPhong = dr.GetString(0),
                        Toa = dr.GetString(1),
                        SoLuongToiDa = dr.GetInt32(2),
                        LaPhongmay = false
                    });
                }
            }
            dr.Close();
            con.Close();
        }
        private void LoadPhongTH(int Thu, string Tiet)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            PhongHoc = new List<PhongHocModel>();
            var cmd = new SqlCommand("SELECT sophong, toa, soluongtoida, laphongmay FROM PHONGHOC EXCEPT SELECT phonghoc.sophong, phonghoc.toa, soluongtoida, laphongmay FROM PHONGHOC, HOCPHAN WHERE" +
                " PHONGHOC.sophong = HOCPHAN.sophong AND HOCPHAN.toa = PHONGHOC.toa and laphongmay=1 AND thu= " + Convert.ToString(Thu), con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                PhongThucHanh.Add(new PhongHocModel
                {
                    SoPhong = dr.GetString(0),
                    Toa = dr.GetString(1),
                    SoLuongToiDa = dr.GetInt32(2),
                    LaPhongmay = false
                });
            }
            dr.Close();
            cmd = new SqlCommand("SELECT phonghoc.sophong, phonghoc.toa, soluongtoida, laphongmay, tiethoc FROM PHONGHOC, HOCPHAN WHERE" +
                " PHONGHOC.sophong = HOCPHAN.sophong AND HOCPHAN.toa = PHONGHOC.toa AND laphongmay=1 and thu= " + Convert.ToString(Thu), con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string t = dr.GetString(4);
                if (Converter.Converter.CommonChars(t, Tiet) == 0)
                {
                    PhongThucHanh.Add(new PhongHocModel
                    {
                        SoPhong = dr.GetString(0),
                        Toa = dr.GetString(1),
                        SoLuongToiDa = dr.GetInt32(2),
                        LaPhongmay = false
                    });
                }
            }
            dr.Close();
            con.Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ThucHanh.Visibility = Visibility.Hidden;
            textThuTH.Visibility = Visibility.Hidden;
            comboThu_TH.Visibility = Visibility.Hidden;
            textTietTH.Visibility = Visibility.Hidden;
            comboTiet_TH.Visibility = Visibility.Hidden;
            textPhongTH.Visibility = Visibility.Hidden;
            comboPhong_TH.Visibility = Visibility.Hidden;
        }

        private void comboMonHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTenGiaoVien(TenMon[comboMonHoc.SelectedIndex].MaMon);
            foreach (var item in TenGV)
            {
                comboGiaoVien.Items.Add(item.TenGV);
            }
            LoadTiet(TenMon[comboMonHoc.SelectedIndex].SoTCLT);
            comboTiet.ItemsSource = Tiet;
            if (TenMon[comboMonHoc.SelectedIndex].SoTCTH > 0)
            {
                textThucHanh.Visibility = Visibility.Visible;
                HT1.Visibility = Visibility.Visible;
                HT2.Visibility = Visibility.Visible;
                HT2.IsChecked = true;
            }
            else
            {
                textThucHanh.Visibility = Visibility.Hidden;
                HT1.Visibility = Visibility.Hidden;
                HT2.Visibility = Visibility.Hidden;
                ThucHanh.Visibility = Visibility.Hidden;
                textThuTH.Visibility = Visibility.Hidden;
                comboThu_TH.Visibility = Visibility.Hidden;
                textTietTH.Visibility = Visibility.Hidden;
                comboTiet_TH.Visibility = Visibility.Hidden;
                textPhongTH.Visibility = Visibility.Hidden;
                comboPhong_TH.Visibility = Visibility.Hidden;
            }

        }

        private void comboThu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(comboTiet is null))
            {
                LoadPhong(comboThu.SelectedIndex + 2, comboTiet.Text);
                comboPhong.Items.Clear();
                foreach (var item in PhongHoc)
                {

                    comboPhong.Items.Add(item.Toa + item.SoPhong);

                }
            }
        }

        private void comboTiet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadPhong(comboThu.SelectedIndex + 2, comboTiet.Text);
            comboPhong.Items.Clear();
            foreach (var item in PhongHoc)
            {
                comboPhong.Items.Add(item.Toa + item.SoPhong);
            }
        }
        private void comboThuTH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(comboTiet is null))
            {
                LoadPhongTH(comboThu.SelectedIndex + 2, comboTiet.Text);
                comboPhong.Items.Clear();
                foreach (var item in PhongThucHanh)
                {

                    comboPhong_TH.Items.Add(item.Toa + item.SoPhong);

                }
            }
        }

        private void comboTietTH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadPhongTH(comboThu.SelectedIndex + 2, comboTiet.Text);
            comboPhong.Items.Clear();
            foreach (var item in PhongThucHanh)
            {
                comboPhong_TH.Items.Add(item.Toa + item.SoPhong);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CourseModel course = new CourseModel();
            course = createCourse();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            if (course.SiSo > PhongHoc[comboPhong.SelectedIndex].SoLuongToiDa)
            {
                MessageBox.Show("Phòng này chỉ chứa tối đa " + PhongHoc[comboPhong.SelectedIndex].SoLuongToiDa.ToString() + " sinh viên!");
                return;
            }
            var cmd = new SqlCommand("SET DATEFORMAT DMY", con);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("Insert into hocphan values ('" + course.MaHocPhan + "','" + course.TenMon + "','" + course.TenGV + "'," + course.Nam.ToString() + "," + course.Ki.ToString() + ",'" + course.SoPhong + "','" + course.Toa + "','" + course.NgayBatDau.Value.ToString("dd/MM/yyyy") + "','" + course.NgayKetThuc.Value.ToString("dd/MM/yyyy") + "','" + course.TietHoc + "', " + course.Thu.ToString() + "," + course.SiSo.ToString() + ")", con);
            try
            {
                cmd.ExecuteNonQuery();
                cmd.CommandText = "Insert into hocphanodotdk values (" + dotDKHP.MaDot.ToString() + "," + dotDKHP.HocKi.ToString() + ",'" + course.MaHocPhan + "','" + dotDKHP.NamHoc + "')";
                cmd.ExecuteNonQuery();
                if (HT1.IsChecked == true && TenMon[comboMonHoc.SelectedIndex].SoTCTH > 0)
                {
                    cmd = new SqlCommand("Insert into hocphan values ('" + course.MaHocPhan + ".1" + "','" + course.TenMon + "','" + course.TenGV + "'," + course.Nam.ToString() + "," + course.Ki.ToString() + ",'" + PhongThucHanh[comboPhong_TH.SelectedIndex].SoPhong + "','" + PhongThucHanh[comboPhong_TH.SelectedIndex].Toa + "','" + course.NgayBatDau.Value.ToString("dd/MM/yyyy") + "','" + course.NgayKetThuc.Value.ToString("dd/MM/yyyy") + "','" + comboTiet_TH.Text + "', " + comboThu_TH.Text + "," + (course.SiSo / 2).ToString() + ")", con);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Insert into hocphanodotdk values (" + dotDKHP.MaDot.ToString() + "," + dotDKHP.HocKi.ToString() + ",'" + course.MaHocPhan + ".1','" + dotDKHP.NamHoc + "')";
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Insert into hocphan values ('" + course.MaHocPhan + ".2" + "','" + course.TenMon + "','" + course.TenGV + "'," + course.Nam.ToString() + "," + course.Ki.ToString() + ",'" + PhongThucHanh[comboPhong_TH.SelectedIndex].SoPhong + "','" + PhongThucHanh[comboPhong_TH.SelectedIndex].Toa + "','" + course.NgayBatDau.Value.ToString("dd/MM/yyyy") + "','" + course.NgayKetThuc.Value.ToString("dd/MM/yyyy") + "','" + comboTiet_TH.Text + "', " + comboThu_TH.Text + "," + (course.SiSo / 2).ToString() + ")", con);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Insert into hocphanodotdk values (" + dotDKHP.MaDot.ToString() + "," + dotDKHP.HocKi.ToString() + ",'" + course.MaHocPhan + ".2','" + dotDKHP.NamHoc + "')";
                    cmd.ExecuteNonQuery();
                }
                else if (HT2.IsChecked == true && TenMon[comboMonHoc.SelectedIndex].SoTCTH > 0)
                {
                    cmd = new SqlCommand("Insert into hocphan values ('" + course.MaHocPhan + ".1" + "','" + course.TenMon + "','" + course.TenGV + "'," + course.Nam.ToString() + "," + course.Ki.ToString() + ", null, null, '" + course.NgayBatDau.Value.ToString("dd/MM/yyyy") + "','" + course.NgayKetThuc.Value.ToString("dd/MM/yyyy") + "', null, null, " + (course.SiSo / 2).ToString() + ")", con);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "Insert into hocphanodotdk values (" + dotDKHP.MaDot.ToString() + "," + dotDKHP.HocKi.ToString() + ",'" + course.MaHocPhan + ".1','" + dotDKHP.NamHoc + "')";
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Thêm học phần thành công");
                view.LoadDB(ManageCourses.list[view.comboDotDKHP.SelectedIndex].MaDot, ManageCourses.list[view.comboDotDKHP.SelectedIndex].HocKi, ManageCourses.list[view.comboDotDKHP.SelectedIndex].NamHoc);
                this.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }
        public CourseModel createCourse()
        {
            string maHocPhan, tenMon, tenGV, soPhong, Toa, tiet;
            DateTime? ngayBatDau, ngayKetThuc;
            int thu, siSo, nam, ki;
            tenMon = TenMon[comboMonHoc.SelectedIndex].MaMon;
            tenGV = TenGV[comboGiaoVien.SelectedIndex].MaGV;
            tiet = Tiet[comboTiet.SelectedIndex];
            soPhong = PhongHoc[comboPhong.SelectedIndex].SoPhong;
            Toa = PhongHoc[comboPhong.SelectedIndex].Toa;
            ki = dotDKHP.HocKi;
            nam = Convert.ToInt32(dotDKHP.NamHoc.Remove(0, 5));
            thu = Convert.ToInt32(comboThu.Text);
            siSo = Convert.ToInt32(textSiSo.Text);
            ngayBatDau = NgayBatDau.DateTime;
            ngayKetThuc = NgayKetThuc.DateTime;
            CourseModel model = new CourseModel();
            maHocPhan = tenMon + "." + (char)(nam - 2020 + ki + 65);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT MAX(mahocphan) FROM hocphan where mamon = '" + tenMon + "' and ky = " + ki.ToString() + " and nam = " + nam.ToString(), con);
            var dr = cmd.ExecuteReader();
            string t = "";
            if (dr.Read()) t = dr.GetString(0);
            maHocPhan = Converter.Converter.TaoMaHocPhan(maHocPhan, t);
            con.Close();
            dr.Close();
            model.MaHocPhan = maHocPhan;
            model.TenMon = tenMon;
            model.TenGV = tenGV;
            model.Nam = nam;
            model.SiSo = siSo;
            model.Ki = ki;
            model.SoPhong = soPhong;
            model.Toa = Toa;
            model.NgayBatDau = ngayBatDau;
            model.NgayKetThuc = ngayKetThuc;
            model.TietHoc = tiet;
            model.Thu = thu;
            return model;
        }

        private void HT1_Checked(object sender, RoutedEventArgs e)
        {
            ThucHanh.Visibility = Visibility.Visible;
            textThuTH.Visibility = Visibility.Visible;
            comboThu_TH.Visibility = Visibility.Visible;
            textTietTH.Visibility = Visibility.Visible;
            comboTiet_TH.Visibility = Visibility.Visible;
            textPhongTH.Visibility = Visibility.Visible;
            comboPhong_TH.Visibility = Visibility.Visible;
        }

        private void NgayBatDau_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
