using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using EasyTimeTable.Views.Staff.Course;
using EasyTimeTable.Views.Student.Course;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace EasyTimeTable.ViewModel
{
    public partial class ManageCoursesVM
    {
        public ObservableCollection<string> ComboboxDotDKHP { get; set; }
        public ObservableCollection<CourseModel> list { get; set; }

        public ManageCoursesVM()
        {
            ComboboxDotDKHP = new ObservableCollection<string>();
            list = new ObservableCollection<CourseModel>();
            LoadHocPhan();
        }
        public void LoadHocPhan()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT hocphan.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso FROM HOCPHAN,GIAOVIEN, Monhoc where HOCPHAN.mamon = MONHOC.mamon AND HOCPHAN.magv = GIAOVIEN.Magv", con);
            var dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                i++;
                list.Add(new CourseModel
                {
                    STT = i,
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
                    SiSo = dr.GetInt32(11)
                }); 
            }
        }
    }
}
