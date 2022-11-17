using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Tuition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class TuitionViewModel
    {
        [ObservableProperty]
        private string? soTinChi;
        [ObservableProperty]
        private string? soTinChiLanDau;
        [ObservableProperty]
        private string? soTinChiHocLai;
        [ObservableProperty]
        private string? tongTinChi;

        [ObservableProperty]
        private TabItem selectedItem;
        public ICommand LoadDB { get; set; }
        public ICommand LoadListDB { get; set; }
        public ICommand MouseLeftButtonDownCM { get; set; }
        public ICommand GetListViewCM { get; set; }
        public ICommand TabChangedCM { get; set; }

        public ICommand ThanhToanCM { get; set; }

        [ObservableProperty]
        public ObservableCollection<OpenCourseModel> courseList;

        [ObservableProperty]
        private Visibility tienTronGoi;

        [ObservableProperty]
        private Visibility tienTinChi;

        [ObservableProperty]
        private Visibility tienHocLai;

        [ObservableProperty]
        private Visibility hocKiHe;

        [ObservableProperty]
        private int giaTronGoi;

        [ObservableProperty]
        private double heSoHocLai;

        [ObservableProperty]
        private double heSoHocHe;

        [ObservableProperty]
        private int giaTinChi;

        [ObservableProperty]
        private int sum;

        [ObservableProperty]
        private double sumHocLai;

        [ObservableProperty]
        private double tienThanhToan;

        [ObservableProperty]
        private bool enableThanhToan;

        [ObservableProperty]
        public ObservableCollection<OpenCourseModel> course;

        [ObservableProperty]
        public ObservableCollection<OpenCourseModel> courseHocLai;

        [ObservableProperty]
        public OpenCourseModel selectCourse;

        public TuitionViewModel()
        {
            LoadDB = new RelayCommand<object>((p) =>
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("SELECT sum(sotclt) + sum(sotcth) from monhoc, lophocphansinhvien, HOCPHAN where HOCPHAN.mamon= MONHOC.mamon AND " +
                    "lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782'", con);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.IsDBNull(0))
                    {
                        SoTinChi = "0";
                    }
                    else
                    {
                        SoTinChi = dr.GetInt32(0).ToString();
                    }
                }
                dr.Close();
                cmd = new SqlCommand("Select * from thamso", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    GiaTronGoi = dr.GetInt32(3);
                    HeSoHocHe = dr.GetDouble(2);
                    HeSoHocLai = dr.GetDouble(1);
                    GiaTinChi = dr.GetInt32(0);
                 }

                dr.Close();
                FeeVisibility();
                getCourse();
                getCourseHocLai();
                if (TienThanhToan == 0)
                {
                    EnableThanhToan = false;
                }
                else
                {
                    EnableThanhToan = true;
                }
                
            });
            CourseList = new ObservableCollection<OpenCourseModel>();
            LoadListDB = new RelayCommand<object>((p) =>
            {
                CourseList = new ObservableCollection<OpenCourseModel>();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                    "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and ngaythanhtoan is null " +
                    "and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam", con);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CourseList.Add(new OpenCourseModel
                    {
                        IsSignUp = true,
                        MaHocPhan = dr.GetString(0),
                        TenMon = dr.GetString(1),
                        TenGV = dr.GetString(2),
                        Nam = dr.GetInt32(3),
                        Ki = dr.GetInt32(4),
                        SoPhong = dr.GetString(5),
                        Toa = dr.GetString(6),
                        NgayBatDau = dr.GetDateTime(7),
                        NgayKetThuc = dr.GetDateTime(8),
                        TietHoc = dr.GetString(9),
                        Thu = dr.GetInt32(10),
                        SiSo = dr.GetInt32(11),
                        SoTinChi = dr.GetInt32(12) + dr.GetInt32(13),
                    });
                }
                int TC = 0;
                foreach (OpenCourseModel item in CourseList)
                {
                     TC += item.SoTinChi;
                }
                SoTinChi = TC.ToString();
                getCourse();
                FeeVisibility();
            });

            TabChangedCM = new RelayCommand<object>((p) =>
            {
                switch (SelectedItem.Header)
                {
                    case "Môn đăng kí":
                        CourseList.Clear();
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                        con.Open();
                        var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC, thamso where " +
                            "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and ngaythanhtoan is null " +
                            "and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam", con);
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            CourseList.Add(new OpenCourseModel
                            {
                                IsSignUp = true,
                                MaHocPhan = dr.GetString(0),
                                TenMon = dr.GetString(1),
                                TenGV = dr.GetString(2),
                                Nam = dr.GetInt32(3),
                                Ki = dr.GetInt32(4),
                                SoPhong = dr.GetString(5),
                                Toa = dr.GetString(6),
                                NgayBatDau = dr.GetDateTime(7),
                                NgayKetThuc = dr.GetDateTime(8),
                                TietHoc = dr.GetString(9),
                                Thu = dr.GetInt32(10),
                                SiSo = dr.GetInt32(11),
                                SoTinChi = dr.GetInt32(12) + dr.GetInt32(13),
                            });
                        }
                        int TC = 0;
                        foreach (OpenCourseModel item in CourseList)
                        {
                            TC += item.SoTinChi;
                        }
                        SoTinChi = TC.ToString();
                        break;
                    case "Môn đang chờ":
                        CourseList.Clear();
                        SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                        con1.Open();
                        var cmd1 = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                            "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and ngaythanhtoan is not null and daduyet = 0 " +
                            "and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam", con1);
                        var dr1 = cmd1.ExecuteReader();
                        while (dr1.Read())
                        {
                            CourseList.Add(new OpenCourseModel
                            {
                                IsSignUp = true,
                                MaHocPhan = dr1.GetString(0),
                                TenMon = dr1.GetString(1),
                                TenGV = dr1.GetString(2),
                                Nam = dr1.GetInt32(3),
                                Ki = dr1.GetInt32(4),
                                SoPhong = dr1.GetString(5),
                                Toa = dr1.GetString(6),
                                NgayBatDau = dr1.GetDateTime(7),
                                NgayKetThuc = dr1.GetDateTime(8),
                                TietHoc = dr1.GetString(9),
                                Thu = dr1.GetInt32(10),
                                SiSo = dr1.GetInt32(11),
                                SoTinChi = dr1.GetInt32(12) + dr1.GetInt32(13),
                            });
                        }
                        int TC1 = 0;
                        foreach (OpenCourseModel item in CourseList)
                        {
                            TC1 += item.SoTinChi;
                        }
                        SoTinChi = "Tổng số tín chỉ đang chờ: " + TC1;
                        break;
                    case "Môn đã thanh toán":
                        CourseList.Clear();
                        SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                        con2.Open();
                        var cmd2 = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                            "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and ngaythanhtoan is not null and daduyet = 1 " +
                            "and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam", con2);
                        var dr2 = cmd2.ExecuteReader();
                        while (dr2.Read())
                        {
                            CourseList.Add(new OpenCourseModel
                            {
                                IsSignUp = true,
                                MaHocPhan = dr2.GetString(0),
                                TenMon = dr2.GetString(1),
                                TenGV = dr2.GetString(2),
                                Nam = dr2.GetInt32(3),
                                Ki = dr2.GetInt32(4),
                                SoPhong = dr2.GetString(5),
                                Toa = dr2.GetString(6),
                                NgayBatDau = dr2.GetDateTime(7),
                                NgayKetThuc = dr2.GetDateTime(8),
                                TietHoc = dr2.GetString(9),
                                Thu = dr2.GetInt32(10),
                                SiSo = dr2.GetInt32(11),
                                SoTinChi = dr2.GetInt32(12) + dr2.GetInt32(13),
                            });
                        }
                        int TC2 = 0;
                        foreach (OpenCourseModel item in CourseList)
                        {
                            TC2 += item.SoTinChi;
                        }
                        SoTinChi = "Tổng số tín chỉ đã thanh toán: " + TC2;
                        break;
                    default:
                        MessageBox.Show("Sth wrong happen");
                        break;
                }
            });

            ThanhToanCM = new RelayCommand<object>((p) =>
            {
                foreach (OpenCourseModel item in CourseList) {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("update lophocphansinhvien set ngaythanhtoan = @ngaythanhtoan where mahocphan = '" + item.MaHocPhan + "'", con);
                    cmd.Parameters.Add("@ngaythanhtoan", System.Data.SqlDbType.DateTime);
                    cmd.Parameters["@ngaythanhtoan"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã thanh toán thành công");
                    if (LoadDB.CanExecute(null))
                        LoadDB.Execute(null);
                    if (LoadListDB.CanExecute(null))
                        LoadListDB.Execute(null);
                }
            });
        }
        public void FeeVisibility()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT ki, namhoc FROM thamso", con);
            var dr = cmd.ExecuteReader();
            int namhoc = 0;
            while (dr.Read())
            {
                if (dr.GetInt32(0) != 3)
                {
                    HocKiHe = Visibility.Collapsed;
                }
                namhoc = dr.GetInt32(1);
            }
            dr.Close();
            cmd = new SqlCommand("SELECT kieuhocphan FROM hocki, thamso where kihoc = ki and hocki.namhoc = '" + (namhoc.ToString() + "-" + (namhoc + 1).ToString()) +"'", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr.GetInt32(0) == 1)
                {
                    TienTronGoi = Visibility.Collapsed;
                    TienTinChi = Visibility.Visible;
                }
                else
                {
                    TienTinChi = Visibility.Collapsed;
                    TienTronGoi = Visibility.Visible;
                }
            }
            dr.Close();
        }

        public void getCourse()
        {
            Course = new ObservableCollection<OpenCourseModel>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and ngaythanhtoan is null and lanhoc = 1" +
                "and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Course.Add(new OpenCourseModel
                {
                    IsSignUp = true,
                    MaHocPhan = dr.GetString(0),
                    TenMon = dr.GetString(1),
                    TenGV = dr.GetString(2),
                    Nam = dr.GetInt32(3),
                    Ki = dr.GetInt32(4),
                    SoPhong = dr.GetString(5),
                    Toa = dr.GetString(6),
                    NgayBatDau = dr.GetDateTime(7),
                    NgayKetThuc = dr.GetDateTime(8),
                    TietHoc = dr.GetString(9),
                    Thu = dr.GetInt32(10),
                    SiSo = dr.GetInt32(11),
                    SoTinChi = dr.GetInt32(12) + dr.GetInt32(13),
                });
            }
            int TC = 0;
            foreach (var course in Course)
            {
                TC += course.SoTinChi;
            }
            SoTinChiLanDau = TC.ToString();

            if (TienTinChi == Visibility.Visible)
                Sum = TC * GiaTinChi;
            else
                Sum = GiaTronGoi;


        }

        public void getCourseHocLai()
        {
            CourseHocLai = new ObservableCollection<OpenCourseModel>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '20520782' and ngaythanhtoan is null and lanhoc > 1" +
                "and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CourseHocLai.Add(new OpenCourseModel
                {
                    IsSignUp = true,
                    MaHocPhan = dr.GetString(0),
                    TenMon = dr.GetString(1),
                    TenGV = dr.GetString(2),
                    Nam = dr.GetInt32(3),
                    Ki = dr.GetInt32(4),
                    SoPhong = dr.GetString(5),
                    Toa = dr.GetString(6),
                    NgayBatDau = dr.GetDateTime(7),
                    NgayKetThuc = dr.GetDateTime(8),
                    TietHoc = dr.GetString(9),
                    Thu = dr.GetInt32(10),
                    SiSo = dr.GetInt32(11),
                    SoTinChi = dr.GetInt32(12) + dr.GetInt32(13),
                });
            }
            int TC = 0;
            foreach (var course in CourseHocLai)
            {
                TC += course.SoTinChi;
            }
            if (TC == 0)
            {
                TienHocLai = Visibility.Hidden;
            }
            else
            {
                TienHocLai = Visibility.Visible;
            }
            SoTinChiHocLai = TC.ToString();
            TongTinChi = (Convert.ToInt32(SoTinChiLanDau) + TC).ToString();
            SumHocLai = TC * GiaTinChi * HeSoHocLai;
            if (HocKiHe == Visibility.Visible)
            {
                TienThanhToan = (SumHocLai + Sum) * HeSoHocHe;
            }
            else
                TienThanhToan = SumHocLai + Sum;
            }
    }
}
