using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student.OpenCourse;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
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
    public partial class OpenCourseVM
    {

        public SnackbarMessageQueue MessageQueueSnackBar { set; get; } = new(TimeSpan.FromSeconds(3));

        [ObservableProperty]
        private string search;
        [ObservableProperty]
        private string soTinChi;
        [ObservableProperty]
        private OpenCourseModel selectedCourse;
        [ObservableProperty]
        private string buttonContent;
        [ObservableProperty]
        private ComboBoxItem selectedCombobox;
        [ObservableProperty]
        private bool isLoading;
        private bool confirm;

        private string MSSV;

        public static Grid? MaskName { get; set; }
        public ICommand RequestCM { get; set; }
        public ICommand SelectCourseCM { get; set; }
        public ICommand RegionChangedCM { get; set; }
        public ICommand LoadDataGrid { get; set; }
        public ICommand GetMaskName { get; set; }
        public ICommand OKDialog { get; set; }

        [ObservableProperty]
        private List<string> listResult;

        [ObservableProperty]
        public List<string> listError;

        [ObservableProperty]
        public List<string> listSelect;

        [ObservableProperty]
        private int numberError;


        [ObservableProperty]
        private string mon1;
        [ObservableProperty]
        private string dialogTitle;
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
        private bool Filter(Course c)
        {
            return Search == null
                || c.MaHocPhan.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1
                || c.TenMon.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1
                || c.TenGV.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
        }

        public OpenCourseVM()
        {
            ListResult = new List<string>();
            ListError = new List<string>();
            ListSelect = new List<string>();
            MSSV = LoginViewModel.mssv;
            OpenCourse = new ObservableCollection<Course>();
            OpenCourseSelect = new ObservableCollection<Course>();
            OpenCourseSelecttemp = new ObservableCollection<Course>();
            LoadDataGrid = new RelayCommand<object>(async (p) =>
            {
                MaskName.Visibility = System.Windows.Visibility.Visible;
                IsLoading = true;
                OpenCourse.Clear();
                OpenCourseSelect.Clear();
                await LoadDBDK(OpenCourseSelect);
                await LoadDB(OpenCourse);
                SoTinChi = "Số tín chỉ: " + TongTC().ToString();
                IsLoading = false;
                MaskName.Visibility = Visibility.Collapsed;
            });

            OKDialog = new RelayCommand<object>((p) =>
            {
                SoTinChi = "Số tín chỉ: " + TongTC().ToString();
                RegionChanged();
                IsLoading = false;
                int i = 0;
                foreach (var c in ListError)
                {
                    i++;

                }
                ListError.Clear();
                NumberError = i;
                MaskName.Visibility = Visibility.Collapsed;
                ListResult.Clear();
            });
            FilteredOpenCourse.Filter = new Predicate<object>(o => Filter(o as Course));
            FilteredOpenCourseSelect.Filter = new Predicate<object>(o => Filter(o as Course));
            SelectCourseCM = new RelayCommand<object>(async (p) =>
            {
                await SelectCourseTask();
            });
            // CM mở cửa sổ request
            RequestCM = new RelayCommand<object>((p) =>
            {
                MaskName.Visibility = System.Windows.Visibility.Visible;
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

            GetMaskName = new RelayCommand<Grid>((p) =>
            {
                MaskName = p;
            });
            DangKi1 = new RelayCommand<System.Windows.Window>((p) =>
            {

            });
            DangKi2 = new RelayCommand<System.Windows.Window>((p) =>
            {

            });
        }

        public async Task RegionChanged()
        {
            MaskName.Visibility = System.Windows.Visibility.Visible;
            IsLoading = true;
            if (SelectedCombobox.Content.ToString() == "Tự động")
            {
                await RefreshDB(OpenCourse);
            }
            if (SelectedCombobox.Content.ToString() == "Môn chính trị")
            {
                await RefreshDBChinhTri(OpenCourse);
            }
            if (SelectedCombobox.Content.ToString() == "Lý thuyết")
            {
                await RefreshDBLyThuyet(OpenCourse);
            }
            if (SelectedCombobox.Content.ToString() == "Thực hành")
            {
                await RefreshDBThucHanh(OpenCourse);
            }
            IsLoading = false;
            MaskName.Visibility = Visibility.Collapsed;
        }
        public async Task LoadDBDK(ObservableCollection<Course> list)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv = GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '" + MSSV + "' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 9", con);
            var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new Course
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
                    SoTinChi = dr.GetInt32(12),
                    LanHoc = CountLanHoc(dr.GetString(1)).ToString(),
                    SDK = DemSNDK(dr.GetString(0))
                });

            }
            dr.Close();
            cmd = new SqlCommand("SELECT lophocphansinhvien.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth, hinhthuc FROM lophocphansinhvien, HOCPHAN,GIAOVIEN,MONHOC,thamso where " +
                "HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv = GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan and masv = '" + MSSV + "' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam and len(hocphan.mahocphan) = 11", con);
            dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                if (dr.GetInt32(14) == 1)
                {
                    list.Add(new Course
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
                        SoTinChi = dr.GetInt32(13),
                        LanHoc = "_",
                        SDK = DemSNDK(dr.GetString(0))
                    });
                }
                else
                {
                    list.Add(new Course
                    {
                        IsSignUp = false,
                        MaHocPhan = dr.GetString(0),
                        TenMon = dr.GetString(1),
                        TenGV = dr.GetString(2),
                        Nam = dr.GetInt32(3),
                        Ki = dr.GetInt32(4),
                        NgayBatDau = dr.GetDateTime(7),
                        NgayKetThuc = dr.GetDateTime(8),
                        SiSo = dr.GetInt32(11),
                        TietHoc = "0",
                        SoTinChi = dr.GetInt32(13),
                        LanHoc = "_",
                        SDK = DemSNDK(dr.GetString(0))
                    });
                }
            }


        }
        public async Task LoadDB(ObservableCollection<Course> list)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth, hinhthuc " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam " +
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = '" + MSSV + "') and len(hocphan.mahocphan) = 9", con);
            var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new Course
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
                    SoTinChi = dr.GetInt32(12),
                    LanHoc = CountLanHoc(dr.GetString(1)).ToString(),
                    SDK = DemSNDK(dr.GetString(0))
                });
            }
            dr.Close();

            cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth, hinhthuc " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam " +
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = '" + MSSV + "') and len(hocphan.mahocphan) = 11", con);
            dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                if (dr.GetInt32(14) == 1)
                {
                    list.Add(new Course
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
                        SoTinChi = dr.GetInt32(13),
                        LanHoc = "_",
                        SDK = DemSNDK(dr.GetString(0))
                    });
                }
            }
            foreach (Course course in OpenCourseSelect)
            {
                FindAndRemove(list, course);
            }
        }

        public async Task SelectCourseTask()
        {
            MaskName.Visibility = System.Windows.Visibility.Visible;
            IsLoading = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd;
            SqlDataReader dr;

            if (ButtonContent == "Đăng kí môn học")
            {
                ObservableCollection<Course> OpenCoursetemp = new ObservableCollection<Course>();
                foreach (Course c in OpenCourse)
                {
                    if (c.IsSignUp == true)
                    {
                        OpenCoursetemp.Add(c);
                    }
                }
                if (OpenCoursetemp.Count == 0)
                {
                    MaskName.Visibility = System.Windows.Visibility.Collapsed;
                    IsLoading = false;
                    await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Hãy chọn ít nhất 1 môn"));
                }

                foreach (Course c in OpenCoursetemp)
                {
                    if (CheckCourse(c) == false) continue;

                    cmd = new SqlCommand("SELECT mamontienquyet FROM monhoctienquyet where  mamonphuthuoc = '" + c.MaHocPhan.Substring(0, 5) + "'", con);
                    dr = await cmd.ExecuteReaderAsync();
                    if (await dr.ReadAsync())
                    {
                        if (CheckMonTienQuyet(c.MaHocPhan.Substring(0, 5), c) == false)
                        {
                            dr.Close();
                            c.IsSignUp = false;
                            continue;
                        }
                    }
                    dr.Close();
                    cmd = new SqlCommand("Insert into lophocphansinhvien (mahocphan, masv, lanhoc, diem, ngaydangki, daduyet) values " + "(@mahocphan, @masv, @lanhoc, @diem, @ngaydangki, @daduyet)", con);
                    cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                    cmd.Parameters["@mahocphan"].Value = c.MaHocPhan;
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
                    await cmd.ExecuteNonQueryAsync();
                    c.IsSignUp = false;

                    if (CheckHT2(c))
                    {
                        cmd = new SqlCommand("Insert into lophocphansinhvien (mahocphan, masv, lanhoc, diem, ngaydangki, daduyet) values " + "(@mahocphan, @masv, @lanhoc, @diem, @ngaydangki, @daduyet)", con);
                        cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                        cmd.Parameters["@mahocphan"].Value = c.MaHocPhan + ".1";
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
                        await cmd.ExecuteNonQueryAsync();
                    }
                    OpenCourseSelect.Add(c);
                    if (CheckHT2(c))
                    {
                        OpenCourseSelect.Add(new Course
                        {
                            IsSignUp = false,
                            MaHocPhan = c.MaHocPhan + ".1",
                            TenMon = c.TenMon,
                            TenGV = c.TenGV,
                            Nam = c.Nam,
                            Ki = c.Ki,
                            NgayBatDau = c.NgayBatDau,
                            NgayKetThuc = c.NgayKetThuc,
                            SiSo = c.SiSo,
                            TietHoc = "0",
                            SoTinChi = 1,
                            LanHoc = "_",
                            SDK = DemSNDK(c.MaHocPhan + ".1")
                        });
                    }
                    ListResult.Add(c.MaHocPhan + " - " + c.TenMon);
                    if (CheckHT2(c))
                    {
                        ListResult.Add(c.MaHocPhan + ".1 - " + c.TenMon);
                    }
                    dr.Close();
                }
                if (ListResult.Count != 0)
                {
                    DialogTitle = "Đăng kí thành công";
                    DangKiThanhCongDialog dangKiThanhCongDialog = new DangKiThanhCongDialog();
                    dangKiThanhCongDialog.ShowDialog();
                }
                else
                {
                    DialogTitle = "Đăng kí môn không thành công";
                    ListResult.Add("Không có môn nào được đăng kí");
                    DangKiThanhCongDialog dangKiThanhCongDialog = new DangKiThanhCongDialog();
                    dangKiThanhCongDialog.ShowDialog();
                }
            }
            else if (ButtonContent == "Hủy môn học")
            {
                OpenCourseSelecttemp.Clear();
                foreach (Course c in OpenCourseSelect)
                {
                    if (c.IsSignUp == true)
                    {
                        OpenCourseSelecttemp.Add(c);
                    }
                }
                if (OpenCourseSelecttemp.Count == 0)
                {
                    MaskName.Visibility = System.Windows.Visibility.Collapsed;
                    IsLoading = false;
                    await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Hãy chọn ít nhất 1 môn"));
                }
                CustomYesNoDialog customYesNoDialog = new CustomYesNoDialog();
                customYesNoDialog.ShowDialog();
                if (confirm == true)
                {
                    foreach (Course c in OpenCourseSelect)
                    {
                        if (c.IsSignUp == true)
                        {
                            if (!CheckCancelTH(c))
                            {
                                ListError.Add(c.MaHocPhan + " - " + c.TenMon + ": Bạn cần phải xóa cả lý thuyết và thực hành");
                            }
                            else
                            {
                                cmd = new SqlCommand("delete from lophocphansinhvien where mahocphan = @mahocphan and masv = @masv", con);
                                cmd.Parameters.Add("@mahocphan", System.Data.SqlDbType.VarChar);
                                cmd.Parameters["@mahocphan"].Value = c.MaHocPhan;
                                cmd.Parameters.Add("@masv", System.Data.SqlDbType.VarChar);
                                cmd.Parameters["@masv"].Value = MSSV;
                                cmd.ExecuteNonQuery();

                                ListResult.Add(c.MaHocPhan + " - " + c.TenMon);
                            }
                        }
                    }
                    if (ListResult.Count != 0)
                    {
                        foreach (string s in ListResult)
                        {
                            OpenCourseSelect.Remove(OpenCourseSelect.Where(i => i.MaHocPhan == Converter.Helper.GetUntilOrEmpty(s)).Single());
                        }
                        DialogTitle = "Hủy môn thành công";
                        DangKiThanhCongDialog dangKiThanhCongDialog = new DangKiThanhCongDialog();
                        dangKiThanhCongDialog.ShowDialog();
                    }
                    else
                    {
                        DialogTitle = "Hủy môn không thành công";
                        ListResult.Add("Không có môn nào được hủy đăng kí");
                        DangKiThanhCongDialog dangKiThanhCongDialog = new DangKiThanhCongDialog();
                        dangKiThanhCongDialog.ShowDialog();
                    }
                }
                else
                {

                }
            }
        }
        public int TongTC()
        {
            int t = 0;
            foreach (Course s in OpenCourseSelect)
            {
                t += s.SoTinChi;
            }
            return t;
        }

        public bool CheckCourse(Course c)
        {
            if (TongTC() > 24)
            {
                ListError.Add(c.TenMon + " - " + c.MaHocPhan + ": Bạn đã đăng kí quá 24 tín chỉ rồi!!!");
                return false;
            }



            if (CheckLTTH(c) == false && CheckHT2(c) == false)
            {
                string mahocphan = c.MaHocPhan.Substring(0, 9);
                int dem = 0;
                foreach (Course s in OpenCourse)
                {
                    if ((s.MaHocPhan.Substring(0, 9) == mahocphan) && s.IsSignUp == true) dem++;
                }
                if (!(dem >= 2) && c.MaHocPhan.Length == 9)
                {
                    ListError.Add(c.TenMon + " - " + c.MaHocPhan + ": Bạn chưa đăng kí thực hành cho học phần!!!");
                    return false;
                }
                if ((dem == 1) && c.MaHocPhan.Length == 11)
                {
                    bool flag = false;
                    foreach (Course s in OpenCourseSelect)
                    {
                        if (s.MaHocPhan.Contains(c.MaHocPhan.Substring(0, 9)) && c.MaHocPhan != s.MaHocPhan) flag = true;
                    }
                    if (flag == false)
                    {
                        ListError.Add(c.TenMon + " - " + c.MaHocPhan + ": Bạn chưa đăng kí lý thuyết cho học phần!!!");
                        return false;
                    }
                }
            }

            foreach (Course s in OpenCourseSelect)
            {
                if ((c.TenMon == s.TenMon) && (c.MaHocPhan.Length != 11))
                {
                    ListError.Add(c.TenMon + " - " + c.MaHocPhan + ": Bạn đã đăng kí 1 môn cùng tên rồi (" + s.MaHocPhan + ")");
                    return false;
                }
            }
            foreach (Course s in OpenCourseSelect)
            {
                if ((c.TenMon == s.TenMon) && (c.MaHocPhan.Length == 11) && (s.MaHocPhan.Length != 11) && (c.MaHocPhan.Substring(0, 9) != s.MaHocPhan.Substring(0, 9)))
                {
                    ListError.Add(c.TenMon + " - " + c.MaHocPhan + ": Bạn cần phải đăng kí môn thực hành có mã môn tương tự môn lý thuyết bạn đã đăng kí (" + s.MaHocPhan + ")");
                    return false;
                }
            }

            foreach (Course s in OpenCourseSelect)
            {
                if ((c.TenMon == s.TenMon) && (c.MaHocPhan.Length == 11) && (s.MaHocPhan.Length == 11))
                {
                    ListError.Add(c.TenMon + " - " + c.MaHocPhan + ": Bạn đã đăng kí môn thực hành cho môn này rồi (" + s.MaHocPhan + ")");
                    return false;
                }
            }

            foreach (Course s in OpenCourseSelect)
            {
                if (Converter.Converter.Compare(c.TietHoc, s.TietHoc, c.Thu, s.Thu) == false)
                {
                    ListError.Add(c.TenMon + " - " + c.MaHocPhan + ": Bị trùng lịch với môn " + s.TenMon + " - " + s.MaHocPhan);
                    return false;
                }
            }
            return true;
        }
        public int DemSNDK(string s)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT COUNT(DISTINCT MASV) FROM LOPHOCPHANSINHVIEN WHERE MAHOCPHAN='" + s + "' GROUP BY MAHOCPHAN", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
                return dr.GetInt32(0);
            return 0;
        }
        public bool CheckLTTH(Course c)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            string s = c.MaHocPhan.Substring(0, 9);
            SqlCommand cmd = new SqlCommand("Select SoTCTH, hinhthuc from monhoc, hocphan where monhoc.mamon = hocphan.mamon and hocphan.mahocphan = '" + s + "'", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetInt32(0) == 0 && dr.GetInt32(1) != 2) return true;
            }
            return false;
        }
        public bool CheckHT2(Course c)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            string s = c.MaHocPhan.Substring(0, 9);
            SqlCommand cmd = new SqlCommand("Select hinhthuc from hocphan where mahocphan = '" + s + ".1'", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetInt32(0) == 2) return true;
            }
            return false;
        }



        public bool CheckMonTienQuyet(string c, Course s)
        {
            string mamontienquyet = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT mamontienquyet FROM monhoctienquyet where mamonphuthuoc = '" + c + "'", con);
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
                ListError.Add((s.TenMon + " - " + s.MaHocPhan + ": Bạn cần phải học môn " + mamontienquyet + " trước"));
                return false;
            }
            return true;
        }


        public bool CheckCancelTH(Course c)
        {
            int dem = 0;
            foreach (var t in OpenCourseSelect)
            {
                if (t.MaHocPhan.Substring(0, 9) == c.MaHocPhan.Substring(0, 9)) dem++;
            }
            if (dem != 2) return true;
            else
            {
                int f = 0;
                foreach (var t in OpenCourseSelect)
                {
                    if (t.MaHocPhan.Substring(0, 9) == c.MaHocPhan.Substring(0, 9) && t.IsSignUp == true) f++;
                }
                if (f == 2) return true;
                else return false;
            }
            return false;
        }

        public int CountLanHoc(string TenMon)
        {
            int count = 1;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT * FROM lophocphansinhvien, hocphan, monhoc, thamso where masv = '" + MSSV + "' and hocphan.mahocphan = lophocphansinhvien.mahocphan and hocphan.mamon = monhoc.mamon and " +
                "tenmon = N'" + TenMon + "' and len(hocphan.mahocphan) = 9 and hocphan.nam <> thamso.namhoc", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                count++;
            }
            dr.Close();
            return count;
        }

        public void FindAndRemove(ObservableCollection<Course> list1, Course course)
        {
            ObservableCollection<Course> list = new ObservableCollection<Course>();
            foreach (var c in list1)
            {
                list.Add(c);
            }
            if (course.MaHocPhan.Length == 9)
            {
                foreach (Course s in list)
                {
                    if ((s.TenMon == course.TenMon) && (s.MaHocPhan.Length == 9))
                    {
                        list1.Remove(s);
                    }
                }
            }
            if (course.MaHocPhan.Length == 11)
            {
                foreach (Course s in list)
                {
                    if ((s.TenMon == course.TenMon) && (s.MaHocPhan.Length == 11))
                    {
                        list1.Remove(s);
                    }
                }
            }
        }



        // Refresh Tự động (Môn cùng tên sẽ không xuất hiện)
        public async Task RefreshDB(ObservableCollection<Course> list)
        {
            list.Clear();
            await LoadDB(list);
        }
        // Refresh chính trị
        public async Task RefreshDBChinhTri(ObservableCollection<Course> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam " +
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = '" + MSSV + "') AND tenmon not in (select tenmon FROM lophocphansinhvien, " +
                "HOCPHAN,GIAOVIEN,MONHOC,thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan " +
                "and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam) and len(hocphan.mahocphan) = 9 and monhoc.mamon like 'SS0%'", con);
            var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new Course
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
                    LanHoc = CountLanHoc(dr.GetString(1)).ToString()
                });
            }
        }
        // Refresh Lý Thuyết
        public async Task RefreshDBLyThuyet(ObservableCollection<Course> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam " +
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = '" + MSSV + "') AND tenmon not in (select tenmon FROM lophocphansinhvien, " +
                "HOCPHAN,GIAOVIEN,MONHOC,thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan " +
                "and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam) and len(hocphan.mahocphan) = 9 and monhoc.mamon not like 'SS0%'", con);
            var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new Course
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
                    SoTinChi = dr.GetInt32(12),
                    LanHoc = CountLanHoc(dr.GetString(1)).ToString()
                });
            }
            dr.Close();
        }
        //Refesh môn Đã chọn
        public async Task RefreshDBThucHanh(ObservableCollection<Course> list)
        {
            list.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT HOCPHAN.mahocphan, tenmon, tengv, nam, ky, sophong,toa,ngaybatdau,ngayketthuc,tiethoc,thu,siso,sotclt,sotcth " +
                "FROM HOCPHAN,GIAOVIEN,MONHOC, thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam " +
                "AND HOCPHAN.mahocphan not in (select mahocphan from lophocphansinhvien where masv = '" + MSSV + "') AND tenmon not in (select tenmon FROM lophocphansinhvien, " +
                "HOCPHAN,GIAOVIEN,MONHOC,thamso where HOCPHAN.mamon= MONHOC.mamon AND HOCPHAN.magv=GIAOVIEN.Magv AND lophocphansinhvien.mahocphan = hocphan.mahocphan " +
                "and masv = '20520782' and thamso.ki = hocphan.ky and thamso.namhoc = hocphan.nam) and len(hocphan.mahocphan) = 11 and monhoc.mamon not like 'SS0%'", con);
            var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new Course
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
                    SoTinChi = dr.GetInt32(13),
                    LanHoc = "_"
                });
            }
            dr.Close();
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



        public ObservableCollection<Course> OpenCourseSelecttemp { get; }
        // Custom ObservableCollection
        public ObservableCollection<Course> OpenCourse { get; }
        public ObservableCollection<Course> OpenCourseSelect { get; }

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

        private ICollectionView filteredOpenCourseSelect;
        public ICollectionView FilteredOpenCourseSelect
        {
            get
            {
                if (filteredOpenCourseSelect == null)
                {
                    filteredOpenCourseSelect = new NotifiableCollectionView(OpenCourseSelect, this);
                }
                return filteredOpenCourseSelect;
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

        [RelayCommand]
        private void CoDialog()
        {
            confirm = true;
        }

        [RelayCommand]
        private void KhongDialog()
        {
            confirm = false;
            IsLoading = false;
            MaskName.Visibility = Visibility.Collapsed;
        }
    }
}
