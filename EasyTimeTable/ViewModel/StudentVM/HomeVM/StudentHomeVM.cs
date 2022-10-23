using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.Student;
using EasyTimeTable.Views.Student.Course;
using EasyTimeTable.Views.Student.Tuition;
using System.Windows.Input;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class StudentHomeVM

    {
        public ICommand TuitionPage { get; set; }
        public ICommand CoursePage { get; set; }
        public StudentHomeVM()
        {
            TuitionPage = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new StudentTuitionPage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Thông tin học phí";
            });
            CoursePage = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new OpenCourseListPage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Danh sách học phần";
            });
        }
    }
}
