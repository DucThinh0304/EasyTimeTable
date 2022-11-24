using EasyTimeTable.Model;
using EasyTimeTable.Views.Staff.TuiTion;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyTimeTable.ViewModel.StudentVM.TuitionVM
{
    /// <summary>
    /// Interaction logic for EditTuition.xaml
    /// </summary>
    public partial class EditTuition : Window
    {
        public static HocKi hk;
        public static ManageTuition view;
        private int GiaTinChi;
        private int GiaTronGoi;
        public EditTuition()
        {
            InitializeComponent();
            LoadDB();
        }
        public void LoadDB()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT giatinchi, hesohoclai, hesohoche, giatrongioi from thamso", con);
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                GiaTinChi = dr.GetInt32(0);
                GiaTronGoi = dr.GetInt32(3);
                textGiaTinChi.Text = string.Format("{0:#,##0}" + " VND", double.Parse(Convert.ToString(dr.GetInt32(0))));
                TextHeSoHocLai.Text = Convert.ToString(dr.GetDouble(1));
                TextHeSoHocHe.Text = Convert.ToString(dr.GetDouble(2));
                TextGiaTronGoi.Text = string.Format("{0:#,##0}" + " VND", double.Parse(Convert.ToString(dr.GetInt32(3))));
            }
            if (hk.KieuHocPhan == 1) comboKieuHocPhi.SelectedIndex = 0;
            else comboKieuHocPhi.SelectedIndex = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("UPDATE THAMSO SET giatinchi = " + GiaTinChi + ", hesohoclai = " + TextHeSoHocLai.Text + ", " +
                "hesohoche = " + TextHeSoHocHe.Text + ", giatrongioi = " + GiaTronGoi, con);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "update hocki set kieuhocphan = " + (comboKieuHocPhi.SelectedIndex + 1).ToString() + " where kihoc = " + hk.KiHoc.ToString() + " and namhoc = '" + hk.NamHoc + "'";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Cập nhật thông tin thành công");
            view.LoadSinhVien(hk.KiHoc, hk.NamHoc, comboKieuHocPhi.SelectedIndex + 1);
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textGiaTinChi_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextGiaTronGoi_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]{0,1})?$");
            string str = txt.Text + e.Text.ToString();
            int cntPrc = 0;
            if (str.Contains('.'))
            {
                string[] tokens = str.Split('.');
                if (tokens.Count() > 0)
                {
                    string result = tokens[1];
                    char[] prc = result.ToCharArray();
                    cntPrc = prc.Count();
                }
            }
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)) && (cntPrc < 3))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TextHeSoHocLai_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]{0,1})?$");
            string str = txt.Text + e.Text.ToString();
            int cntPrc = 0;
            if (str.Contains('.'))
            {
                string[] tokens = str.Split('.');
                if (tokens.Count() > 0)
                {
                    string result = tokens[1];
                    char[] prc = result.ToCharArray();
                    cntPrc = prc.Count();
                }
            }
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)) && (cntPrc < 3))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TextHeSoHocHe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textGiaTinChi_GotFocus(object sender, RoutedEventArgs e)
        {
            textGiaTinChi.Text = GiaTinChi.ToString();
        }

        private void textGiaTinChi_LostFocus(object sender, RoutedEventArgs e)
        {
            GiaTinChi = int.Parse(textGiaTinChi.Text);
            textGiaTinChi.Text = string.Format("{0:#,##0}" + " VND", double.Parse(Convert.ToString(textGiaTinChi.Text)));
        }

        private void TextGiaTronGoi_GotFocus(object sender, RoutedEventArgs e)
        {
            TextGiaTronGoi.Text = GiaTronGoi.ToString();
        }

        private void TextGiaTronGoi_LostFocus(object sender, RoutedEventArgs e)
        {
            GiaTronGoi = int.Parse(TextGiaTronGoi.Text);
            TextGiaTronGoi.Text = string.Format("{0:#,##0}" + " VND", double.Parse(Convert.ToString(TextGiaTronGoi.Text)));
        }
    }
}
