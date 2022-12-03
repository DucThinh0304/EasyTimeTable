using EasyTimeTable.Model;
using EasyTimeTable.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace EasyTimeTable.Views.Student.OpenCourse
{
    /// <summary>
    /// Interaction logic for RequestList.xaml
    /// </summary>
    public partial class RequestList : Window
    {

        public Request current;
        public RequestList()
        {
            InitializeComponent();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            current = new Request();
            var item = sender as ListViewItem;
            if (item != null)
            {
                current = (Request)item.DataContext;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CustomRequestYesNoDialog customRequestYesNoDialog = new CustomRequestYesNoDialog(current.MaYeuCau, current.MaMon, current.TenMon);
            customRequestYesNoDialog.ShowDialog();
            var viewModel = (ListRequestVM)DataContext;
            if (viewModel.LoadListCommand.CanExecute(null))
                viewModel.LoadListCommand.Execute(null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
