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

        public static Grid MaskName { get; set; }

        public List<string> ComboboxRequestKhoa { get; set; }
        public ICommand RequestCM { get; set; }
        public ICommand SelectCourseCM { get; set; }
        public ICommand RegionChangedCM { get; set; }
        public ICommand DatagridChangedSelectionCM { get; set; }
        public ICommand SendRequestCM { get; set; }
        public ICommand LoadDataGrid { get; set; }
        public ICommand GetMaskName { get; set; }


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
            SelectCourseCM = new RelayCommand<object>(async (p) =>
            {
                await SelectCourseTask();
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
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv = GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '"+ MSSV + "' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
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
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = '"+ MSSV +"') AND tenmon not in (select tenmon FROM lophocphansinhvien, " +
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

        public async Task SelectCourseTask()
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            if (ButtonContent == "Đăng kí môn học")
            {
                foreach (OpenCourseModel c in SignUpCourse())
                {
                    if ((c.TenMon == SelectedCourse.TenMon) && (SelectedCourse.MaHocPhan.Length!=11))
                    {
                        MessageBox.Show("Bạn đã đăng kí môn " + c.TenMon + " rồi!!!");
                        return;
                    }
                    if (Converter.Converter.Compare(c.TietHoc, SelectedCourse.TietHoc, c.Thu, SelectedCourse.Thu) == false)
                    {
                        MessageBox.Show("Môn này bị trùng lịch với môn " + c.TenMon + " rồi!!!");
                        return;
                    }
                }

                int count = 0;
                var cmd = new SqlCommand("SELECT * FROM lophocphansinhvien, hocphan, monhoc where masv = '"+ MSSV +"' and hocphan.mahocphan = lophocphansinhvien.mahocphan and hocphan.mamon = monhoc.mamon and " +
                    "tenmon = N'" + SelectedCourse.TenMon + "' and len(hocphan.mahocphan) = 9", con);
                var dr = cmd.ExecuteReader();
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
            if (ButtonContent == "Hủy môn học")
            {
                var cmd = new SqlCommand("delete from lophocphansinhvien where mahocphan = @mahocphan and masv = @masv", con);
                cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan;
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
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = "+ MSSV + " and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
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
                "AND HOCPHAN.magv=GIAOVIEN.Magv AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = "+ MSSV +")", con);
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
               "AND HOCPHAN.magv=GIAOVIEN.Magv AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = "+ MSSV +")", con);
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
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = "+ MSSV + "  and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
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
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = "+ MSSV + " and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
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
