using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using MiProximoColectivo.ViewModels.Base;

namespace MiProximoColectivo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Tasks de navegación
        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }
        public override Task OnNavigatingFrom(NavigatingCancelEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            return null;
        }

        #endregion

        #region Funciones generales

        public override void CleanViewModel()
        {

        }

#if WINDOWS_PHONE_APP

        public override Task BackKeyPressed(BackPressedEventArgs navigationArgs)
        {
            return null;
        }      
        
#endif

        #endregion

    }
}
