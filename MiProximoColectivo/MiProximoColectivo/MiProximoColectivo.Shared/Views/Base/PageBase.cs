using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MiProximoColectivo
{
    public class PageBase : Page
    {
        protected ViewModelBase Vm;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Vm = (ViewModelBase)DataContext;
            Vm.SetAppFrame(Frame);
            Vm.SetCurrentPageType(GetType());
#if WINDOWS_PHONE_APP
            Vm.SetPageStatusBar(StatusBar.GetForCurrentView());
#endif
            Vm.OnNavigatedTo(e);
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (!e.Handled && Frame.CurrentSourcePageType == GetType())
            {
                Vm.BackKeyPressed(e);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Vm.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Vm.OnNavigatingFrom(e);
        }

    }
}

