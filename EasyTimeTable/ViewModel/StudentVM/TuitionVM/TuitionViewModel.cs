using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
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
        public ICommand LoadDB { get; set; }
        public ICommand LoadListDB { get; set; }
        public ICommand MouseLeftButtonDownCM { get; set; }
        public ICommand GetListViewCM { get; set; }

        [ObservableProperty]
        public ObservableCollection<OpenCourseModel> courseList;

        [ObservableProperty]
        public OpenCourseModel selectCourse;

        public TuitionViewModel()
        {
            LoadDB = new RelayCommand<object>((p) =>
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("SELECT sum(sotclt) + sum(sotcth) from monhoc, lophocphansinhvien, HOCPHAN where HOCPHAN.mamon= MONHOC.mamon AND " +
                    "lophocphansinhvien.mahocphan = hocphan.mahocphan", con);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.IsDBNull(0))
                    {
                        SoTinChi = "Số tín chỉ đã đăng ký: 0";
                    }
                    else
                    {
                        SoTinChi = "Số tín chỉ đã đăng ký: " + dr.GetInt32(0);
                    }
                }
                dr.Close();
            });
            CourseList = new ObservableCollection<OpenCourseModel>();
            LoadListDB = new RelayCommand<object>((p) =>
            {
                CourseList = new ObservableCollection<OpenCourseModel>();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC where " +
                    "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan", con);
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
            });
        }
    }
}
