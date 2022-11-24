using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using EasyTimeTable.Views.Student.Course;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StudentOpenCourseVM
    {
        [ObservableProperty]
        private string search;
        [ObservableProperty]
        private OpenCourseModel selectedCourse;
        [ObservableProperty]
        private string buttonContent;
        [ObservableProperty]
        private bool buttonEnable;
        [ObservableProperty]
        private ComboBoxItem selectedCombobox;
        [ObservableProperty]
        private bool isLoading;

        private string MSSV;

        public static Grid? MaskName { get; set; }
        public List<string> ComboboxRequestKhoa { get; set; }
        public ICommand RequestCM { get; set; }
        public ICommand SelectCourseCM { get; set; }
        public ICommand RegionChangedCM { get; set; }
        public ICommand DatagridChangedSelectionCM { get; set; }
        public ICommand SendRequestCM { get; set; }
        public ICommand LoadDataGrid { get; set; }
        public ICommand GetMaskName { get; set; }

        [ObservableProperty]
        private string mon1;
        [ObservableProperty]
        private string tiet1;
        [ObservableProperty]
        private string thu1;
        [ObservableProperty]
        private string giangVien1;
        [ObservableProperty]
        private string siSo1;
        [ObservableProperty]
        private DateTime ngayBatDau1;
        [ObservableProperty]
        private DateTime ngayKetThuc1;
        public ICommand DangKi1 { get; set; }


        [ObservableProperty]
        private string mon2;
        [ObservableProperty]
        private string tiet2;
        [ObservableProperty]
        private string thu2;
        [ObservableProperty]
        private string giangVien2;
        [ObservableProperty]
        private string siSo2;
        [ObservableProperty]
        private DateTime ngayBatDau2;
        [ObservableProperty]
        private DateTime ngayKetThuc2;
        public ICommand DangKi2 { get; set; }


        // Search Textbox
        private bool Filter(OpenCourseModel c)
        {
            return Search == null
                || c.MaHocPhan.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1
                || c.TenMon.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1
                || c.TenGV.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
        }

        public StudentOpenCourseVM()
        {
            MSSV = LoginViewModel.mssv;
            ComboboxRequestKhoa = new List<string>();
            OpenCourse = new ObservableCollection<OpenCourseModel>();
            LoadDataGrid = new RelayCommand<object>(async (p) =>
            {

                OpenCourse.Clear();
                LoadDB(OpenCourse);
            });
            FilteredOpenCourse.Filter = new Predicate<object>(o => Filter(o as OpenCourseModel));
            SelectCourseCM = new RelayCommand<object>((p) =>
            {
                SelectCourseTask();
            });
            // CM mở cửa sổ request
            RequestCM = new RelayCommand<object>((p) =>
            {
                MaskName.Visibility = System.Windows.Visibility.Visible;
                LoadDBRequest();
                RequestCoursesWindow rcw = new RequestCoursesWindow();
                rcw.ShowDialog();
                MaskName.Visibility = Visibility.Collapsed;
            });
            // CM cho thay đổi combobox
            RegionChangedCM = new RelayCommand<object>((p) =>
            {
                RegionChanged();
            });
            // CM cho thay đổi button khi item khác được chọn
            DatagridChangedSelectionCM = new RelayCommand<object>((p) =>
            {
                if (SelectedCourse == null)
                {
                    ButtonContent = "Đăng kí môn học";
                    ButtonEnable = false;
                    return;
                }
                if (SelectedCourse.IsSignUp == false)
                {
                    ButtonContent = "Đăng kí môn học";
                    ButtonEnable = true;
                    return;
                }
                if (SelectedCourse.IsSignUp == true)
                {
                    ButtonContent = "Hủy môn học";
                    ButtonEnable = true;
                    return;
                }
            });
            SendRequestCM = new RelayCommand<object>((p) =>
            {
                MessageBox.Show("Đã gửi");
            });
            GetMaskName = new RelayCommand<Grid>((p) =>
            {
                MaskName = p;
            });
            DangKi1 = new RelayCommand<System.Windows.Window>((p) =>
            {
                DK1(p);
            });
            DangKi2 = new RelayCommand<System.Windows.Window>((p) =>
            {
                DK2(p);
            });
        }

        public void LoadDBRequest()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd1 = new SqlCommand("Select tenkhoa from khoa", con);
            var dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                ComboboxRequestKhoa.Add(dr1.GetString(0));
            }
        }
        public void RegionChanged()
        {
            if (SelectedCombobox.Content.ToString() == "Tự động")
            {
                RefreshDB(OpenCourse);
            }
            if (SelectedCombobox.Content.ToString() == "Môn đã chọn")
            {
                RefreshDBDaChon(OpenCourse);
            }
            if (SelectedCombobox.Content.ToString() == "Tất cả")
            {
                RefreshDBfull(OpenCourse);
            }
            if (SelectedCombobox.Content.ToString() == "Môn chưa chọn")
            {
                RefreshDBChuaChon(OpenCourse);
            }
        }
        public void LoadDB(ObservableCollection<OpenCourseModel> list)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv = GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '" + MSSV + "' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
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
            dr.Close();
            cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam " +
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = '" + MSSV + "') AND tenmon not in (select tenmon FROM lophocphansinhvien, " +
                "HOCPHAN,GIAOVIEN,MONHOC,thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan " +
                "and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam) and len(hocphan.mahocphan) = 9", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    IsSignUp = false,
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
            dr.Close();
            //List<OpenCourseModel> t = SignUpCourse();
            //foreach (var item in t)
            //{
            //    cmd.CommandText = "SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth " +
            //        "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv " +
            //        "and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam AND (HOCPHAN.mahocphan like '" + item.MaHocPhan + "%') AND (HOCPHAN.MAHOCPHAN != '" + item.MaHocPhan + "')";
            //    dr = cmd.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        list.Add(new OpenCourseModel
            //        {
            //            IsSignUp = false,
            //            MaHocPhan = dr.GetString(0),
            //            TenMon = dr.GetString(1),
            //            TenGV = dr.GetString(2),
            //            Nam = dr.GetInt32(3),
            //            Ki = dr.GetInt32(4),
            //            SoPhong = dr.GetString(5),
            //            Toa = dr.GetString(6),
            //            NgayBatDau = dr.GetDateTime(7),
            //            NgayKetThuc = dr.GetDateTime(8),
            //            TietHoc = dr.GetString(9),
            //            Thu = dr.GetInt32(10),
            //            SiSo = dr.GetInt32(11),
            //            SoTinChi = dr.GetInt32(12) + dr.GetInt32(13),
            //        });
            //    }
            //    dr.Close();
            //}    

        }

        public void SelectCourseTask()
        {
            //check coi có trùng lịch hoặc đã đkí môn rồi không
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            if (ButtonContent == "Đăng kí môn học")
            {
                foreach (OpenCourseModel c in SignUpCourse())
                {
                    if ((c.TenMon == SelectedCourse.TenMon) && (SelectedCourse.MaHocPhan.Length != 11))
                    {
                        MessageBox.Show("Bạn đã đăng kí môn " + c.TenMon + " rồi!!!");
                        return;
                    }
                    if (Converter.Converter.Compare(c.TietHoc, SelectedCourse.TietHoc, c.Thu, SelectedCourse.Thu) == false)
                    {
                        MessageBox.Show("Môn này bị trùng lịch với môn " + c.TenMon + " rồi!!!");
                        return;
                    }
                    int tongtc = 0;
                    foreach (OpenCourseModel b in SignUpCourse1())
                    {
                        tongtc += b.SoTinChi;
                    }
                    if (tongtc > 24)
                    {
                        MessageBox.Show("Bạn đã đăng kí quá 24 tín chỉ rồi!!!");
                        return;
                    }
                }


                var cmd = new SqlCommand("SELECT mamontienquyet FROM monhoctienquyet where  mamonphuthuoc = '" + SelectedCourse.MaHocPhan.Substring(0,5) + "'", con);
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (CheckMonTienQuyet() == false) return;
                }
                dr.Close();
                
                int SoHocPhanHienTai = SignUpCourse1().Count();
                 cmd = new SqlCommand("SELECT COUNT(*) FROM hocphan, monhoc where  hocphan.mamon = monhoc.mamon and " +
                    "tenmon = N'" + SelectedCourse.TenMon + "' and len(hocphan.mahocphan) = 11 and mahocphan like '" + SelectedCourse.MaHocPhan + "%'", con);
                 dr = cmd.ExecuteReader();

                

                //Đăng kí môn thực hành
                if (dr.Read())
                {
                    if (dr.GetInt32(0) == 2)
                    {
                        ChoosePraticeCourse choosePraticeCourse = new ChoosePraticeCourse();
                        LoadChoose(choosePraticeCourse);
                        choosePraticeCourse.ShowDialog();
                        if (SoHocPhanHienTai == SignUpCourse1().Count())
                        {
                            MessageBox.Show("Bạn cần phải chọn 1 môn thực hành");
                            return;
                        }
                    }
                    else if (dr.GetInt32(0) == 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Lỗi cơ sở dữ liệu!!!");
                    }
                }
                dr.Close();





                //check số lần môn đã học lại rồi
                int count = 0;
                cmd = new SqlCommand("SELECT * FROM lophocphansinhvien, hocphan, monhoc where masv = '" + MSSV + "' and hocphan.mahocphan = lophocphansinhvien.mahocphan and hocphan.mamon = monhoc.mamon and " +
                    "tenmon = N'" + SelectedCourse.TenMon + "' and len(hocphan.mahocphan) = 9", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    count++;
                }
                dr.Close();
                if (count == 0)
                {
                    cmd = new SqlCommand("Insert into lophocphansinhvien (mahocphan, masv, lanhoc, diem, ngaydangki, daduyet) values " + "(@mahocphan, @masv, @lanhoc, @diem, @ngaydangki, @daduyet)", con);
                    cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan;
                    cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@masv"].Value = MSSV;
                    cmd.Parameters.Add("@lanhoc", System.Data.SqlDbType.Int);
                    cmd.Parameters["@lanhoc"].Value = 1;
                    cmd.Parameters.Add("@ngaydangki", System.Data.SqlDbType.DateTime);
                    cmd.Parameters["@ngaydangki"].Value = DateTime.Now;
                    cmd.Parameters.Add("@diem", System.Data.SqlDbType.Int);
                    cmd.Parameters["@diem"].Value = 0;
                    cmd.Parameters.Add("@daduyet", System.Data.SqlDbType.Int);
                    cmd.Parameters["@daduyet"].Value = 0;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đăng kí thành công");
                    RegionChanged();
                    return;
                }
                else
                {
                    MessageBox.Show("Đây là lần học lại lần thứ " + count.ToString() + "!!");
                    cmd = new SqlCommand("Insert into lophocphansinhvien (mahocphan, masv, lanhoc, diem, ngaydangki, daduyet) values " + "(@mahocphan, @masv, @lanhoc, @diem, @ngaydangki, @daduyet)", con);
                    cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan;
                    cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@masv"].Value = MSSV;
                    cmd.Parameters.Add("@lanhoc", System.Data.SqlDbType.Int);
                    cmd.Parameters["@lanhoc"].Value = count + 1;
                    cmd.Parameters.Add("@ngaydangki", System.Data.SqlDbType.DateTime);
                    cmd.Parameters["@ngaydangki"].Value = DateTime.Now;
                    cmd.Parameters.Add("@diem", System.Data.SqlDbType.Int);
                    cmd.Parameters["@diem"].Value = 0;
                    cmd.Parameters.Add("@daduyet", System.Data.SqlDbType.Int);
                    cmd.Parameters["@daduyet"].Value = 0;
                    cmd.ExecuteNonQuery();
                    RegionChanged();
                    return;
                }
            }

            //hủy môn học
            if (ButtonContent == "Hủy môn học")
            {
                var cmd = new SqlCommand("delete from lophocphansinhvien where mahocphan like @mahocphan and masv = @masv", con);
                cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan+"%";
                cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@masv"].Value = MSSV;
                cmd.ExecuteNonQuery();
                RegionChanged();
                return;
            }
        }

        // Refresh Tự động (Môn cùng tên sẽ không xuất hiện)
        public void RefreshDB(ObservableCollection<OpenCourseModel> list)
        {
            list.Clear();
            LoadDB(list);
        }
        // Refresh Tất cả
        public void RefreshDBfull(ObservableCollection<OpenCourseModel> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = " + MSSV + " and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
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
            dr.Close();
            cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC,thamso where HOCPHAN.mamon= MONHOC.mamon  and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9 " +
                "AND HOCPHAN.magv=GIAOVIEN.Magv AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = " + MSSV + ")", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    IsSignUp = false,
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
        }
        // Refresh Chưa chọn
        public void RefreshDBChuaChon(ObservableCollection<OpenCourseModel> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth " +
               "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon  and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9 " +
               "AND HOCPHAN.magv=GIAOVIEN.Magv AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = " + MSSV + ")", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    IsSignUp = false,
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
            dr.Close();
        }
        //Refesh môn Đã chọn
        public void RefreshDBDaChon(ObservableCollection<OpenCourseModel> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC, thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = " + MSSV + "  and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
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
            dr.Close();
        }
        // List để check những môn đã đăng kí
        public List<OpenCourseModel> SignUpCourse()
        {
            List<OpenCourseModel> list = new List<OpenCourseModel>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = " + MSSV + " and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
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
            dr.Close();
            return list;
        }

        public List<OpenCourseModel> SignUpCourse1()
        {
            List<OpenCourseModel> list = new List<OpenCourseModel>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = " + MSSV + " and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
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
                    SoTinChi = dr.GetInt32(12),
                });
            }
            dr.Close();
            cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = " + MSSV + " and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 11", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
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
                    SoTinChi = dr.GetInt32(13),
                });
            }
            dr.Close();
            return list;
        }

        // Custom ObservableCollection
        public ObservableCollection<OpenCourseModel> OpenCourse { get; }

        private ICollectionView filteredOpenCourse;
        public ICollectionView FilteredOpenCourse
        {
            get
            {
                if (filteredOpenCourse == null)
                {
                    filteredOpenCourse = new NotifiableCollectionView(OpenCourse, this);
                }
                return filteredOpenCourse;
            }
        }

        public bool CheckMonTienQuyet()
        {
            string mamontienquyet = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT mamontienquyet FROM monhoctienquyet where mamonphuthuoc = '" + SelectedCourse.MaHocPhan.Substring(0, 5) + "'", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                mamontienquyet = dr.GetString(0);
            }
            dr.Close();
            cmd = new SqlCommand("SELECT * FROM lophocphansinhvien,hocphan,thamso where mamon = '" + mamontienquyet + "' and hocphan.mahocphan = lophocphansinhvien.mahocphan and masv = '" + MSSV + "' and nam<namhoc", con);
            dr = cmd.ExecuteReader();
            if (dr.Read() == false)
            {
                MessageBox.Show("Bạn cần phải học môn " + mamontienquyet + " trước");
                return false;
            }
            return true;
        }

        public void LoadChoose(ChoosePraticeCourse c)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT * FROM hocphan, monhoc, giaovien where  hocphan.mamon = monhoc.mamon and  hocphan.magv = giaovien.magv and " +
                    "tenmon = N'" + SelectedCourse.TenMon + "' and len(hocphan.mahocphan) = 11 and mahocphan like '" + SelectedCourse.MaHocPhan + "%'", con);
            var dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                i++;
                if (i == 1)
                {
                    Mon1 = dr.GetString(0) + " - " + dr.GetString(13);
                    Tiet1 = dr.GetString(9);
                    Thu1 = dr.GetInt32(10).ToString();
                    GiangVien1 = dr.GetString(18);
                    SiSo1 = dr.GetInt32(11).ToString();
                    NgayBatDau1 = dr.GetDateTime(7);
                    NgayKetThuc1 = dr.GetDateTime(8);
                }
                else
                {
                    Mon2 = dr.GetString(0) + " - " + dr.GetString(13);
                    Tiet2 = dr.GetString(9);
                    Thu2 = dr.GetInt32(10).ToString();
                    GiangVien2 = dr.GetString(18);
                    SiSo2 = dr.GetInt32(11).ToString();
                    NgayBatDau2 = dr.GetDateTime(7);
                    NgayKetThuc2 = dr.GetDateTime(8);
                }
            }
            dr.Close();
        }
        public void DK1(System.Windows.Window c)
        {
            foreach (OpenCourseModel b in SignUpCourse())
            {
                if (Converter.Converter.Compare(b.TietHoc, SelectedCourse.TietHoc, b.Thu, SelectedCourse.Thu) == false)
                {
                    MessageBox.Show("Môn này bị trùng lịch với môn " + b.TenMon + " rồi!!!");
                    return;
                }
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Insert into lophocphansinhvien (mahocphan, masv, lanhoc, diem, ngaydangki, daduyet) values " + "(@mahocphan, @masv, @lanhoc, @diem, @ngaydangki, @daduyet)", con);
            cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan + ".1";
            cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@masv"].Value = MSSV;
            cmd.Parameters.Add("@lanhoc", System.Data.SqlDbType.Int);
            cmd.Parameters["@lanhoc"].Value = 1;
            cmd.Parameters.Add("@ngaydangki", System.Data.SqlDbType.DateTime);
            cmd.Parameters["@ngaydangki"].Value = DateTime.Now;
            cmd.Parameters.Add("@diem", System.Data.SqlDbType.Int);
            cmd.Parameters["@diem"].Value = 0;
            cmd.Parameters.Add("@daduyet", System.Data.SqlDbType.Int);
            cmd.Parameters["@daduyet"].Value = 0;
            cmd.ExecuteNonQuery();
            c.Close();
        }

        public void DK2(System.Windows.Window c)
        {
            foreach (OpenCourseModel b in SignUpCourse())
            {
                if (Converter.Converter.Compare(b.TietHoc, SelectedCourse.TietHoc, b.Thu, SelectedCourse.Thu) == false)
                {
                    MessageBox.Show("Môn này bị trùng lịch với môn " + b.TenMon + " rồi!!!");
                    return;
                }
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Insert into lophocphansinhvien (mahocphan, masv, lanhoc, diem, ngaydangki, daduyet) values " + "(@mahocphan, @masv, @lanhoc, @diem, @ngaydangki, @daduyet)", con);
            cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan + ".2";
            cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@masv"].Value = MSSV;
            cmd.Parameters.Add("@lanhoc", System.Data.SqlDbType.Int);
            cmd.Parameters["@lanhoc"].Value = 1;
            cmd.Parameters.Add("@ngaydangki", System.Data.SqlDbType.DateTime);
            cmd.Parameters["@ngaydangki"].Value = DateTime.Now;
            cmd.Parameters.Add("@diem", System.Data.SqlDbType.Int);
            cmd.Parameters["@diem"].Value = 0;
            cmd.Parameters.Add("@daduyet", System.Data.SqlDbType.Int);
            cmd.Parameters["@daduyet"].Value = 0;
            cmd.ExecuteNonQuery();
            c.Close();
        }
        public class NotifiableCollectionView : ListCollectionView
        {
            public NotifiableCollectionView(IList sourceCollection, object model)
                : base(sourceCollection)
            {
                if (model is INotifyPropertyChanged)
                    (model as INotifyPropertyChanged).PropertyChanged += NotifiableCollectionView_PropertyChanged;
            }

            void NotifiableCollectionView_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Search")
                    this.Refresh();
            }
        }
    }
}
