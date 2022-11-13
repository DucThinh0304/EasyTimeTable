using EasyTimeTable.Model;
using Syncfusion.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace EasyTimeTable.Views.Staff.Course
{
    /// <summary>
    /// Interaction logic for AddDotDKHP.xaml
    /// </summary>
    public partial class AddDotDKHP : System.Windows.Window
    {
        public static HocKi HocKiChon;
        public static ManageDotDKHP view;
        public static List<CourseModel> courses;
        public AddDotDKHP()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            view.LoadDB(HocKiChon.KiHoc, HocKiChon.NamHoc);
        }

        private void buttonThem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string filename="";
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog()==true) filename = dialog.FileName;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            courses = new List<CourseModel>();
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
           // try
            {


                int j = 0;
                for (int i = 2; i <= rowCount; i++)
                {
                    j++;
                    courses.Add(new CourseModel
                    {
                        STT = j,
                        MaHocPhan = xlRange.Cells[i, 1].Value2.ToString(),
                        TenMon = xlRange.Cells[i, 2].Value2.ToString(),
                        TenGV = xlRange.Cells[i, 3].Value2.ToString(),
                        Nam = Convert.ToInt32(xlRange.Cells[i, 4].Value2.ToString()),
                        Ki = Convert.ToInt32(xlRange.Cells[i, 5].Value2.ToString()),
                        SoPhong = xlRange.Cells[i, 6].Value2.ToString(),
                        Toa = xlRange.Cells[i, 7].Value2.ToString(),
                        NgayBatDau = DateTime.Parse(xlRange.Cells[i, 8].Value2.ToString()),
                        NgayKetThuc = DateTime.Parse(xlRange.Cells[i, 9].Value2.ToString()),
                        TietHoc = xlRange.Cells[i, 10].Value2.ToString(),
                        Thu = Convert.ToInt32(xlRange.Cells[i, 11].Value2.ToString()),
                        SiSo = Convert.ToInt32(xlRange.Cells[i, 12].Value2.ToString())
                    }) ;  
                }
                Grid.ItemsSource = courses;
            } //catch (Exception ex)
            {
          //      MessageBox.Show("File không đúng định dạng, vui lòng xem lại hướng dẫn");
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }

        private void buttonXuat_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd1 = new SqlCommand("SET DATEFORMAT DMY", con);
            cmd1.ExecuteNonQuery();
            cmd1.CommandText = "SELECT MAX(Madot) from dotdkhp where kihoc = " + HocKiChon.KiHoc.ToString() + " and namhoc = '" + HocKiChon.NamHoc + "'";
            var dr = cmd1.ExecuteReader();
            int i = 0;
            if (dr.Read()) i = dr.GetInt32(0);
            cmd1.CommandText = "INSERT INTO dotdkhp values (" + (i + 1).ToString() + "," + HocKiChon.KiHoc.ToString() + ", '" + HocKiChon.NamHoc + "', '" + NgayBatDau.Text + "','" + NgayKetThuc.Text + "')";
            dr.Close();
            cmd1.ExecuteNonQuery();
            MessageBox.Show("Thêm đợt đăng kí học phần thành công");
            view.LoadDB(HocKiChon.KiHoc, HocKiChon.NamHoc);
            this.Close();
        }
    }
}
