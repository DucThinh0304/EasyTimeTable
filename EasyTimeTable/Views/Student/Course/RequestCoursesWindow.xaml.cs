using EasyTimeTable.Model;
using EasyTimeTable.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
            ComboBoxGiaoVien.Items.Clear();
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
            if (dr.Read())
            {
                t = dr.GetInt32(0);
            }
            dr.Close();
            YeuCauModel yeucau = new YeuCauModel();

            yeucau.MaMon = listMon[comboMon.SelectedIndex].MaMon;
            yeucau.MaGV = listGiaoVien[ComboBoxGiaoVien.SelectedIndex].MaGV;
            yeucau.LyDo = textLyDo.Text;
            if (Thu.Text != "" && Buoi.Text != "")
            {
                t++;
                yeucau.MaYeuCau = t;
                yeucau.Thu = Convert.ToInt32(Thu.Text);
                yeucau.Buoi = (Buoi.Text == "Sáng") ? 0 : 1;
                cmd.CommandText = "INSERT INTO YEUCAUMOLOP VALUES ('" + yeucau.MaYeuCau + "', '" + yeucau.MaMon + "', 60, '" + yeucau.MaGV + "', N'" + yeucau.LyDo + "', " + yeucau.Thu + "," + yeucau.Buoi + ")";
                cmd.ExecuteNonQuery();
                dr.Close();
                cmd.CommandText = "Insert into sinhvienyeucau values ('" + yeucau.MaYeuCau + "', '" + MSSV + "')";
                cmd.ExecuteNonQuery();
                dr.Close();
            }
            if (Thu1.Text != "" && Buoi1.Text != "")
            {
                t++;
                yeucau.MaYeuCau = t;
                yeucau.Thu = Convert.ToInt32(Thu1.Text);
                yeucau.Buoi = (Buoi1.Text == "Sáng") ? 0 : 1;
                cmd.CommandText = "INSERT INTO YEUCAUMOLOP VALUES ('" + yeucau.MaYeuCau + "', '" + yeucau.MaMon + "', 60, '" + yeucau.MaGV + "', N'" + yeucau.LyDo + "', " + yeucau.Thu + "," + yeucau.Buoi + ")";
                cmd.ExecuteNonQuery();
                dr.Close();
                cmd.CommandText = "Insert into sinhvienyeucau values ('" + yeucau.MaYeuCau + "', '" + MSSV + "')";
                cmd.ExecuteNonQuery();
                dr.Close();
            }
            if (Thu2.Text != "" && Buoi2.Text != "")
            {
                t++;
                yeucau.MaYeuCau = t;
                yeucau.Thu = Convert.ToInt32(Thu2.Text);
                yeucau.Buoi = (Buoi2.Text == "Sáng") ? 0 : 1;
                cmd.CommandText = "INSERT INTO YEUCAUMOLOP VALUES ('" + yeucau.MaYeuCau + "', '" + yeucau.MaMon + "', 60, '" + yeucau.MaGV + "', N'" + yeucau.LyDo + "', " + yeucau.Thu + "," + yeucau.Buoi + ")";
                cmd.ExecuteNonQuery();
                dr.Close();
                cmd.CommandText = "Insert into sinhvienyeucau values ('" + yeucau.MaYeuCau + "', '" + MSSV + "')";
                cmd.ExecuteNonQuery();
                dr.Close();
            }

            this.Close();
        }
    }
}
