using EasyTimeTable.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyTimeTable.Views.Staff.Course
{
    /// <summary>
    /// Interaction logic for ManageDotDKHP.xaml
    /// </summary>
    public partial class ManageDotDKHP : Page
    {
        public static List<Model.DotDKHP> list;
        public static List<HocKi> courses;
        public ManageDotDKHP()
        {
            InitializeComponent();
            LoadKiHoc();
            comboDotDKHP.SelectedIndex = 0;
        }
        public void LoadKiHoc()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd1 = new SqlCommand("Select kihoc, namhoc, kieuhocphan from hocki", con);
            var dr1 = cmd1.ExecuteReader();
            courses = new List<HocKi>();
            while (dr1.Read())
            {
                comboDotDKHP.Items.Add("Học kì " + dr1.GetInt32(0).ToString() +" năm học "+dr1.GetString(1));
                courses.Add(new HocKi
                {
                    KiHoc = dr1.GetInt32(0),
                    NamHoc = dr1.GetString(1),
                    KieuHocPhan = dr1.GetInt32(2)
                });
            }
            dr1.Close();
            con.Close();
        }
        public void LoadDB(int KiHoc, string NamHoc)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd1 = new SqlCommand("Select madot, kihoc, namhoc, ngaybatdau, ngayketthuc from dotdkhp where kihoc = " + KiHoc.ToString() + " and namhoc= '" + NamHoc + "'", con);
            var dr1 = cmd1.ExecuteReader();
            list = new List<DotDKHP>();
            int i = 0;
            while (dr1.Read())
            {
                i++;
                list.Add(new DotDKHP
                {
                    STT = i,
                    HocKi = dr1.GetInt32(1),
                    NamHoc = dr1.GetString(2),
                    MaDot = dr1.GetInt32(0),
                    NgayBatDau = dr1.GetDateTime(3),
                    NgayKetThuc = dr1.GetDateTime(4)
                }) ;
            }
            dr1.Close();
            con.Close();
            Grid.ItemsSource = list;
        }

        private void comboDotDKHP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            LoadDB(courses[comboDotDKHP.SelectedIndex].KiHoc, courses[comboDotDKHP.SelectedIndex].NamHoc);

        }

        private void buttonThem_Click(object sender, RoutedEventArgs e)
        {
            AddDotDKHP.HocKiChon = courses[comboDotDKHP.SelectedIndex];
            AddDotDKHP.view = this;
            AddDotDKHP add = new AddDotDKHP();
            add.ShowDialog();
        }

        private void buttonXoa_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult t = MessageBox.Show("Bạn có chắc chắn muốn xóa đợt ĐKHP này không ?", "Cảnh báo xóa học phần", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (t == MessageBoxResult.Yes)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                try
                {
                    con.Open();
                    var cmd = new SqlCommand("DELETE FROM lophocphansinhvien where mahocphan in (select hocphan.mahocphan from hocphan, lophocphansinhvien, hocphanodotdk where hocphan.mahocphan = hocphanodotdk.mahocphan and hocphan.mahocphan = lophocphansinhvien.mahocphan and " +
                        "madot = " + list[Grid.SelectedIndex].MaDot.ToString() + " and kihoc = " + list[Grid.SelectedIndex].HocKi.ToString() + " and namhoc = '"+ list[Grid.SelectedIndex].NamHoc +"')", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("DELETE FROM hocphan where mahocphan in (select hocphan.mahocphan from hocphan, hocphanodotdk where hocphan.mahocphan = hocphanodotdk.mahocphan and " +
                        "madot = " + list[Grid.SelectedIndex].MaDot.ToString() + " and kihoc = " + list[Grid.SelectedIndex].HocKi.ToString() + " and namhoc = '" + list[Grid.SelectedIndex].NamHoc + "')", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("DELETE FROM dotdkhp where " +
                        "madot = " + list[Grid.SelectedIndex].MaDot.ToString() + " and kihoc = " + list[Grid.SelectedIndex].HocKi.ToString() + " and namhoc = '" + list[Grid.SelectedIndex].NamHoc + "'", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã xóa đợt đăng kí học phần thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đợt đăng kí học phần này có học phần này đã được đăng kí, không thể xóa");
                }
                LoadDB(list[comboDotDKHP.SelectedIndex].HocKi, list[comboDotDKHP.SelectedIndex].NamHoc);
            }
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonXoa.IsEnabled = true;
            buttonSua.IsEnabled = true;
        }

        private void buttonSua_Click(object sender, RoutedEventArgs e)
        {
            EditDotDKHP.DotDKHPChon = list[Grid.SelectedIndex];
            EditDotDKHP.view = this;
            EditDotDKHP edit = new EditDotDKHP();
            edit.Show();
        }

        private void buttonXuat_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

