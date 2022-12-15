using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using EasyTimeTable.Views.Account;
using Microsoft.Win32;
using Syncfusion.Windows.Shared;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Window = System.Windows.Window;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class ChangeAccountInfoVM
    {

        [ObservableProperty]
        private string mSSV;
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        public string imageURL;
        [ObservableProperty]
        private List<LopModel> listLopHoc;
        [ObservableProperty]
        private List<string> listLop;
        [ObservableProperty]
        private string lopHoc;
        [ObservableProperty]
        private string cMND;
        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string sdt;

        [ObservableProperty]
        private Visibility btn_Visible;

        private string lUserName;
        private string lImageURL;
        private string lLopHoc;
        private string lCMND;
        private string lEmail;
        private string lSdt;
        private bool isImgDelete = true;

        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private Visibility mask;


        public ChangeAccountInfoVM()
        {

        }
        [RelayCommand]
        public async Task LoadDB()
        {
            ListLopHoc = new List<LopModel>();
            ListLop = new List<string>();
            IsLoading = true;
            Mask = Visibility.Visible;
            MSSV = LoginViewModel.mssv;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("Select lophoc, tenkhoa from lophoc, khoa where lophoc.khoa = khoa.makhoa", con);
            var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                ListLopHoc.Add(new LopModel
                {
                    Lophoc = dr.GetString(0),
                    Khoa = dr.GetString(1)
                });
                ListLop.Add(dr.GetString(0));
            }
            dr.Close();
            cmd = new SqlCommand("Select tensv, lophoc ,cmnd,email,sdt from sinhvien where MASV = '" + MSSV + "'", con);
            dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                UserName = dr.GetString(0);
                LopHoc = dr.GetString(1);
                CMND = dr.GetString(2);
                Email = dr.GetString(3);
                Sdt = dr.GetString(4);
                lUserName = dr.GetString(0);
                lLopHoc = dr.GetString(1);
                lCMND = dr.GetString(2);
                lEmail = dr.GetString(3);
                lSdt = dr.GetString(4);
            }
            string path = "../../../Assets/Student - " + MSSV + ".jpg";
            BitmapImage result = new BitmapImage();
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    result = image;
                }
                ChangeAccountInfo.AVT_bor.ImageSource = result;
                Btn_Visible = Visibility.Collapsed;
            }
            else
            {
                Btn_Visible = Visibility.Visible;
            }

            IsLoading = false;
            Mask = Visibility.Collapsed;
        }

        FrameworkElement GetParentWindow(FrameworkElement p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
        [RelayCommand]
        private void Cancel(FrameworkElement p)
        {
            if (lUserName != UserName || lLopHoc != LopHoc || lCMND != CMND || lEmail != Email || lSdt != Sdt)
            {
                if ((MessageBox.Show("Bạn có chắc hủy những thay đổi không", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    UserName = lUserName;
                    LopHoc = lLopHoc;
                    CMND = lCMND;
                    Email = lEmail;
                    Sdt = lSdt;
                };
            }
        }

        [RelayCommand]
        private void Save(FrameworkElement p)
        {
            if (lUserName != UserName || lLopHoc != LopHoc || lCMND != CMND || lEmail != Email || lSdt != Sdt || ImageURL.IsNullOrWhiteSpace() == false)
            {
                if ((MessageBox.Show("Bạn có chắc lưu những thay đổi không", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("update sinhvien set tensv = N'" + UserName + "', lophoc = '" + LopHoc + "' ,cmnd = '" + CMND + "', email = '" + Email + "',sdt = '" + Sdt + "' where MASV = '" + MSSV + "'", con);
                    cmd.ExecuteNonQuery();
                    lUserName = UserName;
                    lLopHoc = LopHoc;
                    lCMND = CMND;
                    lEmail = Email;
                    lSdt = Sdt;
                    if (!isImgDelete)
                    {
                        BitmapImage result = new BitmapImage();
                        if (!string.IsNullOrEmpty(ImageURL) && File.Exists(ImageURL))
                        {
                            using (var stream = File.OpenRead(ImageURL))
                            {
                                var image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = stream;
                                image.EndInit();
                                result = image;
                            }
                        }
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(result));
                        using (var fileStream = new System.IO.FileStream(@"../../../Assets/Student - " + MSSV + ".jpg", System.IO.FileMode.Create))
                        {
                            encoder.Save(fileStream);
                        }
                    }


                };
            }
        }
        [RelayCommand]
        private void Quit(FrameworkElement p)
        {
            if (lUserName != UserName || lLopHoc != LopHoc || lCMND != CMND || lEmail != Email || lSdt != Sdt)
            {
                if ((MessageBox.Show("Những thay đổi của bạn chưa được lưu, bạn có chắc muốn thoát không!", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    FrameworkElement window = GetParentWindow(p);
                    var w = window as Window;
                    if (w != null)
                    {
                        w.Close();
                    }
                };
            }
            else
            {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            }
        }

        [RelayCommand]
        private void AddAVT()
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
            "|PNG Portable Network Graphics (*.png)|*.png" +
            "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
            "|BMP Windows Bitmap (*.bmp)|*.bmp" +
            "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
            "|GIF Graphics Interchange Format (*.gif)|*.gif";
            if (OpenFileDialog.ShowDialog() == true)
            {
                ImageURL = OpenFileDialog.FileName;
                BitmapImage result = new BitmapImage();
                if (!string.IsNullOrEmpty(ImageURL) && File.Exists(ImageURL))
                {
                    using (var stream = File.OpenRead(ImageURL))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        result = image;
                    }
                }
                ChangeAccountInfo.AVT_bor.ImageSource = result;
                Btn_Visible = Visibility.Collapsed;
            }
        }

        [RelayCommand]
        private void DeleteAVT()
        {
            isImgDelete = true;
            ChangeAccountInfo.AVT_bor.ImageSource = null;
            Btn_Visible = Visibility.Visible;
        }

        [RelayCommand]
        private void ChangeAVT()
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
            "|PNG Portable Network Graphics (*.png)|*.png" +
            "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
            "|BMP Windows Bitmap (*.bmp)|*.bmp" +
            "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
            "|GIF Graphics Interchange Format (*.gif)|*.gif";
            if (OpenFileDialog.ShowDialog() == true)
            {
                ImageURL = OpenFileDialog.FileName;
                BitmapImage result = new BitmapImage();
                if (!string.IsNullOrEmpty(ImageURL) && File.Exists(ImageURL))
                {
                    using (var stream = File.OpenRead(ImageURL))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        result = image;
                    }
                }
                ChangeAccountInfo.AVT_bor.ImageSource = result;
                Btn_Visible = Visibility.Collapsed;
            }
        }
    }
}
