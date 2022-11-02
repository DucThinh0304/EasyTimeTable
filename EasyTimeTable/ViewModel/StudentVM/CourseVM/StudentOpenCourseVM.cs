using CommunityToolkit.Mvvm.ComponentModel;
using EasyTimeTable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using System.Collections;
using System.Windows.Input;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using EasyTimeTable.Views.Student.Course;
using System.Windows.Documents;

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

        public ICommand RequestCM { get; set; }
        public ICommand SelectCourse { get; set; }
        public ICommand RegionChangedCM { get; set; }
        public ICommand DatagridChangedSelectionCM { get; set; }

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
            OpenCourse = new ObservableCollection<OpenCourseModel>();
            LoadDB(OpenCourse);
            FilteredOpenCourse.Filter = new Predicate<object>(o => Filter(o as OpenCourseModel));
            SelectCourse = new RelayCommand<object>(p =>
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                if (ButtonContent == "Đăng kí môn học")
                {
                    foreach(OpenCourseModel c in SignUpCourse())
                    {
                        if (c.TenMon == SelectedCourse.TenMon)
                        {
                            MessageBox.Show("Bạn đã đăng kí môn " + c.TenMon + " rồi!!!");
                            return;
                        }
                    }
                    var cmd = new SqlCommand("Insert into lophocphansinhvien (mahocphan, masv) values " + "(@mahocphan, @masv)", con);
                    cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan;
                    cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@masv"].Value = "20520782";
                    var dr = cmd.ExecuteNonQuery();
                    MessageBox.Show("Thành công");
                    RegionChanged();
                    return;
                }
                if (ButtonContent == "Hủy môn học")
                {
                    var cmd = new SqlCommand("delete from lophocphansinhvien where mahocphan = @mahocphan and masv = @masv", con);
                    cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@mahocphan"].Value = SelectedCourse.MaHocPhan;
                    cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@masv"].Value = "20520782";
                    var dr = cmd.ExecuteNonQuery();
                    MessageBox.Show("Thành công");
                    RegionChanged();
                    return;
                }
            });
            // CM mở cửa sổ request
            RequestCM = new RelayCommand<object>((p) =>
            {
                RequestCoursesWindow rcw = new RequestCoursesWindow();
                rcw.ShowDialog();
            });
            // CM cho thay đổi combobox
            RegionChangedCM = new RelayCommand<object>((p) => {
                RegionChanged();
            });
            // CM cho thay đổi thứ datagrid chọn
            DatagridChangedSelectionCM = new RelayCommand<object>((p) =>
            {
                if (SelectedCourse == null)
                {
                    ButtonContent = "Đăng kí môn học";
                    ButtonEnable = false;
                    return;
                }
                if (SelectedCourse.isSignUp == false)
                {
                    ButtonContent = "Đăng kí môn học";
                    ButtonEnable = true;
                    return;
                }
                if (SelectedCourse.isSignUp == true)
                {
                    ButtonContent = "Hủy môn học";
                    ButtonEnable = true;
                    return;
                }
                
            });
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
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    isSignUp = true,
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
            dr.Close();
            cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC where HOCPHAN.mamon= MONHOC.mamon " +
                "AND HOCPHAN.magv=GIAOVIEN.Magv " +
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien) AND tenmon not in (select tenmon FROM lophocphansinhvien, " +
                "HOCPHAN,GIAOVIEN,MONHOC where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan)", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    isSignUp = false,
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
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    isSignUp = true,
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
            dr.Close();
            cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC where HOCPHAN.mamon= MONHOC.mamon " +
                "AND HOCPHAN.magv=GIAOVIEN.Magv AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien)", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    isSignUp = false,
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
        //Refesh môn Đã chọn
        public void RefreshDBDaChon(ObservableCollection<OpenCourseModel> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
             var cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC where HOCPHAN.mamon= MONHOC.mamon " +
                "AND HOCPHAN.magv=GIAOVIEN.Magv AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien)", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    isSignUp = false,
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
            dr.Close();
        }
        // Refresh Chưa chọn
        public void RefreshDBChuaChon(ObservableCollection<OpenCourseModel> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    isSignUp = true,
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
            dr.Close();
        }
        // List để check những môn đã đăng kí
        public List<OpenCourseModel> SignUpCourse()
        {
            List<OpenCourseModel>  list = new List<OpenCourseModel>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OpenCourseModel
                {
                    isSignUp = true,
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
            dr.Close();
            return list;
        }

        // Custom Danh sách
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
