using EasyTimeTable.Model;
using EasyTimeTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasyTimeTable.Views.Student.Course
{
    /// <summary>
    /// Interaction logic for RequestCoursesWindow.xaml
    /// </summary>
    public partial class RequestCoursesWindow : Window
    {
        private string MSSV;
        public List<MonHocModel> listMon;
        public List<GiaoVienModel> listGiaoVien;
        public RequestCoursesWindow()
        {
            InitializeComponent();
            MSSV = LoginViewModel.mssv;
            LoadDB();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (grid1.Visibility == Visibility.Collapsed)
            {
                grid1.Visibility = Visibility.Visible;
            }
            else
            {
                grid2.Visibility = Visibility.Visible;
                button.IsEnabled = false;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = true;
            grid1.Visibility = Visibility.Collapsed;
            Thu1.Text = "";
            Buoi1.Text = "";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = true;
            grid2.Visibility = Visibility.Collapsed;
            Thu2.Text = "";
            Buoi2.Text = "";
        }

        private void LoadDB()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT mamon, tenmon from MonHoc", con);
            var dr = cmd.ExecuteReader();
            listMon = new List<MonHocModel>();
            while (dr.Read())
            {
                listMon.Add(new MonHocModel
                {
                    MaMon = dr.GetString(0),
                    TenMon = dr.GetString(1)
                });
            }
            foreach (var item in listMon)
            {
                comboMon.Items.Add(item.MaMon + " - " + item.TenMon);
            }  
        }

        private void comboMon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT giaovien.magv, tengv from giaovien, giangday where giangday.magv = giaovien.magv and mamon = '" + listMon[comboMon.SelectedIndex].MaMon + "'", con);
            var dr = cmd.ExecuteReader();
            listGiaoVien = new List<GiaoVienModel>();
            while (dr.Read())
            {
                listGiaoVien.Add(new GiaoVienModel
                {
                    MaGV = dr.GetString(0),
                    TenGV = dr.GetString(1)
                });
            }
            foreach (var item in listGiaoVien)
            {
                ComboBoxGiaoVien.Items.Add(item.MaGV + " - " + item.TenGV);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT COUNT(*) FROM YEUCAUMOLOP", con);
            var dr = cmd.ExecuteReader();
            int t = 0;
            if (dr.Read()) t = dr.GetInt32(0);
            YeuCauModel yeucau = new YeuCauModel();
            yeucau.MaYeuCau = "YC" + t.ToString();
            yeucau.MaMon = listMon[comboMon.SelectedIndex].MaMon;
            yeucau.SiSo = 0;
            yeucau.MaGV = listGiaoVien[ComboBoxGiaoVien.SelectedIndex].MaGV;
            yeucau.LyDo = textLyDo.Text;
            yeucau.listBuoi = new List<Buoi>();
            if (Thu.Text != "" && Buoi.Text != "") yeucau.listBuoi.Add(new Model.Buoi
            {
                Thu = Convert.ToInt32(Thu.Text),
                BuoiHoc = Buoi.SelectedIndex + 1
            });
            if (Thu1.Text != "" && Buoi1.Text != "") yeucau.listBuoi.Add(new Model.Buoi
            {
                Thu = Convert.ToInt32(Thu1.Text),
                BuoiHoc = Buoi1.SelectedIndex + 1
            });
            if (Thu2.Text != "" && Buoi2.Text != "") yeucau.listBuoi.Add(new Model.Buoi
            {
                Thu = Convert.ToInt32(Thu2.Text),
                BuoiHoc = Buoi2.SelectedIndex + 1
            });
            dr.Close();
            cmd.CommandText = "INSERT INTO YEUCAUMOLOP VALUES ('" + yeucau.MaYeuCau + "', '" + yeucau.MaMon + "', " + yeucau.SiSo.ToString() + ", '" + yeucau.MaGV + "', N'" + yeucau.LyDo + "')";
            cmd.ExecuteNonQuery();
            foreach (Model.Buoi buoi in yeucau.listBuoi)
            {
                cmd.CommandText = "INSERT INTO BUOIYEUCAU VALUES ('" + yeucau.MaYeuCau + "', " + buoi.Thu.ToString() + ", " + buoi.BuoiHoc.ToString() + ")";
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = "INSERT INTO SINHVIENYEUCAU VALUES ('" + yeucau.MaYeuCau + "', '" + MSSV + "')";
            cmd.ExecuteNonQuery();
            this.Close();
        }
    }
}
