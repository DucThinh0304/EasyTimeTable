using EasyTimeTable.Converter;
using EasyTimeTable.Model;
using EasyTimeTable.ViewModel;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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
        private string bestMatch;
        private string bestMatch1;
        List<Buoi> choiceIgnore = new List<Buoi>();


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
            EnableButton();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = true;
            grid1.Visibility = Visibility.Collapsed;
            Thu1.SelectedIndex = -1;
            Buoi1.SelectedIndex = -1;
            EnableButton();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = true;
            grid2.Visibility = Visibility.Collapsed;
            Thu2.SelectedIndex = -1;
            Buoi2.SelectedIndex = -1;
            EnableButton();
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

        private void comboMon_SelectionChanged(object sender, TextChangedEventArgs e)
        {
            if (comboMon.Items.Contains(comboMon.Text))
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
                if (ComboBoxGiaoVien.SelectedIndex != -1 && comboMon.SelectedIndex != -1)
                {
                    EnableAll();
                }
                else
                {
                    DisableAll();
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, TextChangedEventArgs e)
        {
            if (ComboBoxGiaoVien.Items.Contains(ComboBoxGiaoVien.Text))
            {
                if (ComboBoxGiaoVien.SelectedIndex != -1 && comboMon.SelectedIndex != -1)
                {
                    EnableAll();
                }
                else
                {
                    DisableAll();
                }
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("SELECT thu, buoi from yeucaumolop where magvdexuat = '" + ComboBoxGiaoVien.Text.Substring(0, 5) + "' and mamon = '" + listMon[comboMon.SelectedIndex].MaMon + "'", con);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    choiceIgnore.Add(new Model.Buoi
                    {
                        Thu = dr.GetInt32(0),
                        BuoiHoc = dr.GetInt32(1)
                    });
                }
            }
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

        private void Thu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Buoi.SelectedIndex = -1;
            string text = (sender as ComboBox).SelectedItem as string;
            if (text == Thu1.Text)
            {
                Buoi.Items.Remove(Buoi1.Text);
            }
            if (text == Thu2.Text)
            {
                Buoi.Items.Remove(Buoi2.Text);
            }
            EnableButton();
        }

        private void Buoi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Buoi.SelectedIndex != -1)
            {
                string text = (sender as ComboBox).SelectedItem as string;
                foreach (Buoi buoi in choiceIgnore)
                {
                    if (buoi.Thu == Convert.ToInt32(Thu.Text) && buoi.BuoiHoc == ((text == "Sáng") ? 0 : 1))
                    {
                        MessageBox.Show("Đã có yêu cầu này rồi");
                        Buoi.SelectedIndex = -1;
                    }
                }
            }
            EnableButton();
        }
        private void Thu1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Buoi1.SelectedIndex = -1;
            string text = (sender as ComboBox).SelectedItem as string;
            if (text == Thu.Text)
            {
                Buoi1.Items.Remove(Buoi.Text);
            }
            if (text == Thu2.Text)
            {
                Buoi1.Items.Remove(Buoi2.Text);
            }
            EnableButton();
        }

        private void Buoi1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Buoi1.SelectedIndex != -1)
            {
                string text = (sender as ComboBox).SelectedItem as string;
                foreach (Buoi buoi in choiceIgnore)
                {
                    if (buoi.Thu == Convert.ToInt32(Thu1.Text) && buoi.BuoiHoc == ((text == "Sáng") ? 0 : 1))
                    {
                        MessageBox.Show("Đã có yêu cầu này rồi");
                        Buoi1.SelectedIndex = -1;
                    }
                }
            }
            EnableButton();
        }

        private void Thu2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Buoi2.SelectedIndex = -1;
            string text = (sender as ComboBox).SelectedItem as string;
            if (text == Thu.Text)
            {
                Buoi2.Items.Remove(Buoi.Text);
            }
            if (text == Thu1.Text)
            {
                Buoi2.Items.Remove(Buoi1.Text);
            }
            EnableButton();
        }

        private void Buoi2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Buoi2.SelectedIndex != -1)
            {
                string text = (sender as ComboBox).SelectedItem as string;
                foreach (Buoi buoi in choiceIgnore)
                {
                    if (buoi.Thu == Convert.ToInt32(Thu2.Text) && buoi.BuoiHoc == ((text == "Sáng") ? 0 : 1))
                    {
                        MessageBox.Show("Đã có yêu cầu này rồi");
                        Buoi2.SelectedIndex = -1;
                    }
                }
            }
            EnableButton();
        }
        private void EnableAll()
        {
            button.IsEnabled = true;
            Thu.IsEnabled = true;
            Buoi.IsEnabled = true;
            button1.IsEnabled = true;
            Thu1.IsEnabled = true;
            Buoi1.IsEnabled = true;
            button2.IsEnabled = true;
            Buoi2.IsEnabled = true;
            Thu2.IsEnabled = true;
            Thu.Items.Clear();
            Thu1.Items.Clear();
            Thu2.Items.Clear();
            Buoi.Items.Clear();
            Buoi1.Items.Clear();
            Buoi2.Items.Clear();
            for (int i = 2; i <= 7; i++)
            {
                Thu.Items.Add(i.ToString());
                Thu1.Items.Add(i.ToString());
                Thu2.Items.Add(i.ToString());
            }
            for (int i = 0; i <= 1; i++)
            {
                Buoi.Items.Add((i == 0) ? "Sáng" : "Chiều");
                Buoi1.Items.Add((i == 0) ? "Sáng" : "Chiều");
                Buoi2.Items.Add((i == 0) ? "Sáng" : "Chiều");
            }
            textLyDo.IsEnabled = true;

        }
        private void DisableAll()
        {
            button.IsEnabled = false;
            Thu.IsEnabled = false;
            Buoi.IsEnabled = false;
            button1.IsEnabled = false;
            Thu1.IsEnabled = false;
            Buoi1.IsEnabled = false;
            button2.IsEnabled = false;
            Buoi2.IsEnabled = false;
            Thu2.IsEnabled = false;
            Buoi1.SelectedIndex = -1;
            Thu1.SelectedIndex = -1;
            Buoi2.SelectedIndex = -1;
            Thu2.SelectedIndex = -1;
            grid1.Visibility = Visibility.Collapsed;
            grid2.Visibility = Visibility.Collapsed;
            textLyDo.IsEnabled = false;
            textLyDo.Text = "";
            buttonOK.IsEnabled = false;
        }


        private bool IsEnableButton()
        {
            if (ComboBoxGiaoVien.SelectedIndex == -1) return false;
            if (comboMon.SelectedIndex == -1) return false;
            if (grid2.Visibility == Visibility.Visible && (Thu2.SelectedIndex == -1 || Buoi2.SelectedIndex == -1)) return false;
            if (grid1.Visibility == Visibility.Visible && (Thu1.SelectedIndex == -1 || Buoi1.SelectedIndex == -1)) return false;
            if (Thu.SelectedIndex == -1 || Buoi.SelectedIndex == -1) return false;
            if (textLyDo.Text.IsNullOrWhiteSpace()) return false;
            return true;
        }

        private void EnableButton()
        {
            if (!IsEnableButton()) buttonOK.IsEnabled = false;
            else buttonOK.IsEnabled = true;
        }

        private void textLyDo_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }
    }
}
