using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class ForgotPassViewModel
    {
        public ICommand PreviousPageCM { get; set; }

        public ForgotPassViewModel()
        {
            PreviousPageCM = new RelayCommand<object>((p) =>
            {
                LoginViewModel.MainFrame.Content = new LoginPage();
            });
        }
    }
}
