using Syncfusion.Windows.Shared;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Window = System.Windows.Window;

namespace EasyTimeTable.Views.Account
{
    /// <summary>
    /// Interaction logic for ChangeAccountInfo.xaml
    /// </summary>
    public partial class ChangeAccountInfo : Window
    {
        public static ImageBrush AVT_bor;

        private string lTen;
        private string lLop;
        private string lEmail;
        private string lCMND;
        private string lSDT;
        public ChangeAccountInfo()
        {
            InitializeComponent();

        }

        private void Ten_btn_Click(object sender, RoutedEventArgs e)
        {
            lTen = Ten_txt.Text;
            if (Ten_txt.IsEnabled == false)
            {
                CloseAll();
                Ten_txt.IsEnabled = true;
                Ten_btn.Background = new SolidColorBrush(Colors.Green);
                Ten_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                Tenxoa_btn.Visibility = Visibility.Visible;
                Ten_txt.Focus();
                NoEnableButton();
                Ten_btn.IsEnabled = true;
            }
            else
            {
                if (!Ten_txt.Text.IsNullOrWhiteSpace())
                {
                    EnableButton();
                    CloseAll();
                }
                else
                {
                    if (Snackbar.MessageQueue is { } messageQueue)
                    {
                        var message = "Không được để trống";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
                }
            }
        }

        private void Lop_btn_Click(object sender, RoutedEventArgs e)
        {
            lLop = Lop_txt.Text;
            if (Lop_txt.IsEnabled == false)
            {
                CloseAll();
                Lop_txt.IsEnabled = true;
                Lop_btn.Background = new SolidColorBrush(Colors.Green);
                Lop_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                Lopxoa_btn.Visibility = Visibility.Visible;
                Lop_txt.Focus();
                NoEnableButton();
                Lop_btn.IsEnabled = true;
            }
            else
            {
                EnableButton();
                CloseAll();
            }
        }

        private void Email_btn_Click(object sender, RoutedEventArgs e)
        {
            lEmail = Email_txt.Text;
            if (Email_txt.IsEnabled == false)
            {
                CloseAll();
                Email_txt.IsEnabled = true;
                Email_btn.Background = new SolidColorBrush(Colors.Green);
                Email_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                Emailxoa_btn.Visibility = Visibility.Visible;
                Email_txt.Focus();
                NoEnableButton();
                Email_btn.IsEnabled = true;
            }
            else
            {
                if (!Email_txt.Text.IsNullOrWhiteSpace())
                {
                    EnableButton();
                    CloseAll();
                }
                else
                {
                    if (Snackbar.MessageQueue is { } messageQueue)
                    {
                        var message = "Không được để trống";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
                }
            }
        }
        private void CMND_btn_Click(object sender, RoutedEventArgs e)
        {
            lCMND = CMND_txt.Text;
            if (CMND_txt.IsEnabled == false)
            {
                CloseAll();
                CMND_txt.IsEnabled = true;
                CMND_btn.Background = new SolidColorBrush(Colors.Green);
                CMND_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                CMNDxoa_btn.Visibility = Visibility.Visible;
                CMND_txt.Focus();
                NoEnableButton();
                CMND_btn.IsEnabled = true;
            }
            else
            {
                if (!CMND_txt.Text.IsNullOrWhiteSpace())
                {
                    EnableButton();
                    CloseAll();
                }
                else
                {
                    if (Snackbar.MessageQueue is { } messageQueue)
                    {
                        var message = "Không được để trống";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
                }
            }
        }

        private void SDT_btn_Click(object sender, RoutedEventArgs e)
        {
            lSDT = SDT_txt.Text;
            if (SDT_txt.IsEnabled == false)
            {
                CloseAll();
                SDT_txt.IsEnabled = true;
                SDT_btn.Background = new SolidColorBrush(Colors.Green);
                SDT_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                SDTxoa_btn.Visibility = Visibility.Visible;
                SDT_txt.Focus();
                NoEnableButton();
                SDT_btn.IsEnabled = true;
            }
            else
            {
                if (!SDT_txt.Text.IsNullOrWhiteSpace())
                {
                    EnableButton();
                    CloseAll();
                }
                else
                {
                    if (Snackbar.MessageQueue is { } messageQueue)
                    {
                        var message = "Không được để trống";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
                }
            }
        }

        private void Avt_bor_Loaded(object sender, RoutedEventArgs e)
        {
            AVT_bor = imagebrush;
        }

        private void imagebrush_Changed(object sender, System.EventArgs e)
        {
            if (AVT_bor.ImageSource != null)
            {
                Avt_bor.MouseEnter += Avt_btn_MouseEnter;
                Avt_bor.MouseLeave += Avt_btn_MouseLeave;
            }
            else
            {
                Avt_bor.MouseEnter -= Avt_btn_MouseEnter;
                Avt_bor.MouseLeave -= Avt_btn_MouseLeave;
                AvtMask.Visibility = Visibility.Collapsed;
                AvtChange_btn.Visibility = Visibility.Collapsed;
                AvtDelete_btn.Visibility = Visibility.Collapsed;
            }
        }

        private void Avt_btn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AvtMask.Visibility = Visibility.Visible;
            AvtChange_btn.Visibility = Visibility.Visible;
            AvtDelete_btn.Visibility = Visibility.Visible;
        }
        private void Avt_btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AvtMask.Visibility = Visibility.Collapsed;
            AvtChange_btn.Visibility = Visibility.Collapsed;
            AvtDelete_btn.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CloseAll();
        }

        private void CloseAll()
        {
            Ten_txt.IsEnabled = false;
            Ten_btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#783293");
            Ten_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            Tenxoa_btn.Visibility = Visibility.Collapsed;

            Lop_txt.IsEnabled = false;
            Lop_btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#783293");
            Lop_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            Lopxoa_btn.Visibility = Visibility.Collapsed;

            Email_txt.IsEnabled = false;
            Email_btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#783293");
            Email_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            Emailxoa_btn.Visibility = Visibility.Collapsed;

            CMND_txt.IsEnabled = false;
            CMND_btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#783293");
            CMND_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            CMNDxoa_btn.Visibility = Visibility.Collapsed;

            SDT_txt.IsEnabled = false;
            SDT_btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#783293");
            SDT_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            SDTxoa_btn.Visibility = Visibility.Collapsed;
        }

        private void Tenxoa_btn_Click(object sender, RoutedEventArgs e)
        {
            Ten_txt.Text = lTen;
            CloseAll();
            EnableButton();
        }


        private void Lopxoa_btn_Click(object sender, RoutedEventArgs e)
        {

            Lop_txt.SelectedValue = lLop;
            CloseAll();
            EnableButton();
        }

        private void Emailxoa_btn_Click(object sender, RoutedEventArgs e)
        {

            Email_txt.Text = lEmail;
            CloseAll();
            EnableButton();
        }


        private void CMNDxoa_btn_Click(object sender, RoutedEventArgs e)
        {

            CMND_txt.Text = lCMND;
            CloseAll();
            EnableButton();
        }

        private void SDTxoa_btn_Click(object sender, RoutedEventArgs e)
        {

            SDT_txt.Text = lSDT;
            CloseAll();
            EnableButton();
        }


        private void NoEnableButton()
        {
            Ten_btn.IsEnabled = false;
            Lop_btn.IsEnabled = false;
            Email_btn.IsEnabled = false;
            CMND_btn.IsEnabled = false;
            SDT_btn.IsEnabled = false;
            Huy_btn.IsEnabled = false;
            Luu_btn.IsEnabled = false;
            Dong_btn.IsEnabled = false;
            AvtChange_btn.IsEnabled = false;
            Avt_btn.IsEnabled = false;
            AvtDelete_btn.IsEnabled = false;
        }

        private void EnableButton()
        {
            Lop_btn.IsEnabled = true;
            Ten_btn.IsEnabled = true;
            Email_btn.IsEnabled = true;
            CMND_btn.IsEnabled = true;
            SDT_btn.IsEnabled = true;
            Huy_btn.IsEnabled = true;
            Luu_btn.IsEnabled = true;
            Dong_btn.IsEnabled = true;
            AvtChange_btn.IsEnabled = true;
            Avt_btn.IsEnabled = true;
            AvtDelete_btn.IsEnabled = true;
        }
    }
}

