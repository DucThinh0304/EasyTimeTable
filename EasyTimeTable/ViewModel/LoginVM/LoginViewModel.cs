using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyTimeTable.Views.LoginWindow;

namespace EasyTimeTable.ViewModel
{
    [ObservableObject]
    public partial class LoginViewModel
    {
        public Button LoginBtn { get; set; }
        public static Frame? MainFrame { get; set; }
        public Window LoginWindow { get; set; }
        public ICommand ShadowMaskCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand MinimizeWindowCM { get; set; }
        public ICommand MouseLeftButtonDownWindowCM { get; set; }
        public ICommand LoadLoginPageCM { get; set; }

        public LoginViewModel()
        {
            MouseLeftButtonDownWindowCM = new RelayCommand<Window>((p) =>
            {
                if (p != null)
                {
                    p.DragMove();
                }
            });
            LoadLoginPageCM = new RelayCommand<Frame>((p) =>
            {
                MainFrame = p;
                p.Content = new LoginPage();
            });
        }

            FrameworkElement GetParentWindow(FrameworkElement p)
        {
            FrameworkElement? parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
