using EasyTimeTable.Model;
using EasyTimeTable.Views.Staff.TuiTion;
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
                textGiaTinChi.Text = dr.GetInt32(0).ToString();
                TextHeSoHocLai.Text = dr.GetDouble(1).ToString();
                TextHeSoHocHe.Text = dr.GetDouble(2).ToString();
                TextGiaTronGoi.Text = dr.GetInt32(3).ToString();
            }
            if (hk.KieuHocPhan == 1) comboKieuHocPhi.SelectedIndex = 0;
            else comboKieuHocPhi.SelectedIndex = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("UPDATE THAMSO SET giatinchi = " + textGiaTinChi.Text + ", hesohoclai = " + TextHeSoHocLai.Text + ", " +
                "hesohoche = " + TextHeSoHocHe.Text + ", giatrongioi = " + TextGiaTronGoi.Text, con);
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
            Regex regex = new Regex("[^0-9]+.");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextGiaTronGoi_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextGiaTronGoi_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+.");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextHeSoHocLai_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+.");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextHeSoHocHe_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+.");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
