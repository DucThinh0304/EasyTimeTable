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
        public ICommand TuitionPageCM { get; set; }
        public ICommand CoursePageCM { get; set; }
        public StudentHomeVM()
        {
            TuitionPageCM = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new StudentTuitionPage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Thông tin học phí";
            });
            CoursePageCM = new RelayCommand<object>((p) =>
            {
                StudentViewModel.MainFrame.Content = new OpenCourseListPage();
                if (StudentMainWindow.funcTitle != null)
                    StudentMainWindow.funcTitle.Text = "Danh sách học phần";
            });
        }
    }
}
