using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Model;
using EasyTimeTable.Views.Account;
using EasyTimeTable.Views.LoginWindow;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private string lLopHoc;
        private string lCMND;
        private string lEmail;
        private string lSdt;
        private string lPassword;
        private bool isImgDelete = false;

        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private Visibility mask;

        private string OldPassword;
        private string NewPassword;
        private string RePassword;

        [ObservableProperty]
        private bool isOldPasswordFocus;
        [ObservableProperty]
        private bool isNewPasswordFocus;
        [ObservableProperty]
        private bool isRePasswordFocus;

        public SnackbarMessageQueue MessageQueueSnackBar { set; get; } = new(TimeSpan.FromSeconds(3));


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
            var cmd = new SqlCommand("Select lophoc, khoa from lophoc", con);
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
            dr.Close();
            cmd = new SqlCommand("Select matkhau from taikhoan where MSSV = '" + MSSV + "'", con);
            dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                lPassword = dr.GetString(0);
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
            if (lUserName != UserName || lLopHoc != LopHoc || lCMND != CMND || lEmail != Email || lSdt != Sdt || ImageURL.IsNullOrWhiteSpace() == false || isImgDelete == true)
            {
                if ((MessageBox.Show("Bạn có chắc hủy những thay đổi không", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    UserName = lUserName;
                    LopHoc = lLopHoc;
                    CMND = lCMND;
                    Email = lEmail;
                    Sdt = lSdt;
                    isImgDelete = false;
                    ImageURL = "";
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
                };
            }
        }

        [RelayCommand]
        private void Save(FrameworkElement p)
        {
            if (lUserName != UserName || lLopHoc != LopHoc || lCMND != CMND || lEmail != Email || lSdt != Sdt || ImageURL.IsNullOrWhiteSpace() == false || isImgDelete == true)
            {
                if ((MessageBox.Show("Bạn có chắc lưu những thay đổi không", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    string khoa = "";
                    foreach (var item in ListLopHoc)
                    {
                        if (item.Lophoc == LopHoc)
                        {
                            khoa = item.Khoa;
                        }
                    }
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();
                    var cmd = new SqlCommand("update sinhvien set tensv = N'" + UserName + "', lophoc = '" + LopHoc + "' ,cmnd = '" + CMND + "', email = '" + Email + "',sdt = '" + Sdt + "', makhoa='"+khoa+"' where MASV = '" + MSSV + "'", con);
                    cmd.ExecuteNonQuery();
                    lUserName = UserName;
                    lLopHoc = LopHoc;
                    lCMND = CMND;
                    lEmail = Email;
                    lSdt = Sdt;

                    if (!isImgDelete)
                    {
                        if (!ImageURL.IsNullOrWhiteSpace())
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
                            ImageURL = "";
                        }
                    }
                    else
                    {
                        File.Delete(@"../../../Assets/Student - " + MSSV + ".jpg");
                        isImgDelete = false;
                    }


                };
            }
        }
        [RelayCommand]
        private void Quit(FrameworkElement p)
        {
            if (lUserName != UserName || lLopHoc != LopHoc || lCMND != CMND || lEmail != Email || lSdt != Sdt || ImageURL.IsNullOrWhiteSpace() == false || isImgDelete == true)
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
        [RelayCommand]
        private void ChangePasswordButton()
        {
            if (lUserName != UserName || lLopHoc != LopHoc || lCMND != CMND || lEmail != Email || lSdt != Sdt || ImageURL.IsNullOrWhiteSpace() == false || isImgDelete == true)
            {
                if ((MessageBox.Show("Những thay đổi của bạn sẽ không được lưu, bạn có muốn lưu trước khi đổi mật khẩu không", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
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
                    ChangePasswordAccount changePasswordAccount = new ChangePasswordAccount();
                    changePasswordAccount.Tag = "changePasswordAccount";
                    changePasswordAccount.ShowDialog();
                }
                else
                {
                    ChangePasswordAccount changePasswordAccount = new ChangePasswordAccount();
                    changePasswordAccount.Tag = "changePasswordAccount";
                    changePasswordAccount.ShowDialog();
                }
            }
            else
            {
                ChangePasswordAccount changePasswordAccount = new ChangePasswordAccount();
                changePasswordAccount.Tag = "changePasswordAccount";
                changePasswordAccount.ShowDialog();
            }
        }
        [RelayCommand]
        private void ChangeOldPassword(PasswordBox p)
        {
            OldPassword = p.Password;
        }

        [RelayCommand]
        private void ChangeNewPassword(PasswordBox p)
        {
            NewPassword = p.Password;
        }

        [RelayCommand]
        private void ChangeRePassword(PasswordBox p)
        {
            RePassword = p.Password;
        }
        [RelayCommand]
        private void CancelChangePassword(FrameworkElement p)
        {
            FrameworkElement window = GetParentWindow(p);
            var w = window as Window;
            if (w != null)
            {
                w.Close();
            }
        }

        [RelayCommand]
        private async Task SubmitChangePassword(FrameworkElement p)
        {
            IsOldPasswordFocus = false;
            IsNewPasswordFocus = false;
            IsRePasswordFocus = false;
            if (OldPassword.IsNullOrWhiteSpace() == false && RePassword.IsNullOrWhiteSpace() == false && NewPassword.IsNullOrWhiteSpace() == false)
            {
                if (Converter.Converter.CreateMD5(OldPassword) == lPassword)
                {
                    if (Converter.Converter.IsValidPassword(NewPassword) && RePassword == NewPassword)
                    {
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                        con.Open();

                        if (lPassword == Converter.Converter.CreateMD5(NewPassword))
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu trùng với mật khẩu cũ, hãy chọn mật khẩu mới"));
                            IsNewPasswordFocus = true;
                            ChangePasswordAccount.NewPassword.Clear();
                            ChangePasswordAccount.RePassword.Clear();
                            return;
                        }

                        var cmd = new SqlCommand("Update taikhoan set matkhau = '" + Converter.Converter.CreateMD5(NewPassword) + "' where MSSV = '" + MSSV + "'", con);
                        cmd.ExecuteNonQuery();
                        await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Đổi mật khẩu thành công, vui lòng đăng nhập lại!"));
                        await Task.Delay(4000);
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Tag = "loginWindow";
                        loginWindow.Show();
                        foreach (Window win in App.Current.Windows)
                        {
                            if (win.Tag == null)
                            {
                                win.Close();
                            }
                            else if (win.Tag.ToString() != "loginWindow")
                            {
                                win.Close();
                            }
                        }

                    }
                    else if (RePassword != NewPassword)
                    {
                        ChangePasswordAccount.RePassword.Clear();
                        IsRePasswordFocus = true;
                        await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Xác nhận mật khẩu phải giống mật khẩu"));
                    }
                    else
                    {
                        if (NewPassword.Length < 6)
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu ít nhất phải có 6 chữ cái"));
                        }
                        else if (NewPassword.Any(c => Converter.Converter.IsLetter(c)) == false)
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu phải có ít nhất 1 chữ cái thường và in hoa"));
                        }
                        else if (NewPassword.Any(c => Converter.Converter.IsDigit(c)) == false)
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu phải có ít nhất 1 chữ số"));
                        }
                        else
                        {
                            await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu phải có ít nhất 1 kí tự đặc biệt"));
                        }
                        IsNewPasswordFocus = true;
                        ChangePasswordAccount.NewPassword.Clear();
                        ChangePasswordAccount.RePassword.Clear();
                        
                    }
                }
                else
                {
                    ChangePasswordAccount.OldPassword.Clear();
                    ChangePasswordAccount.NewPassword.Clear();
                    ChangePasswordAccount.RePassword.Clear();
                    IsOldPasswordFocus = true;
                    await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Sai mật khẩu cũ"));
                }
            }
            else if (OldPassword.IsNullOrWhiteSpace() == true)
            {
                ChangePasswordAccount.RePassword.Clear();
                IsOldPasswordFocus = true;
                await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu cũ không được để trống"));
            }
            else if (NewPassword.IsNullOrWhiteSpace() == true)
            {
                ChangePasswordAccount.RePassword.Clear();
                IsNewPasswordFocus = true;
                await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Mật khẩu mới không được để trống"));
            }
            else
            {
                ChangePasswordAccount.NewPassword.Clear();
                IsNewPasswordFocus = true;
                await Task.Factory.StartNew(() => MessageQueueSnackBar.Enqueue("Xác nhận mật khẩu không được để trống"));
            }
        }
    }
}
