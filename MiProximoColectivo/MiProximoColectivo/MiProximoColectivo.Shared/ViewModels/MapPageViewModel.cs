﻿using MiProximoColectivo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Phone.UI.Input;
using MiProximoColectivo.Functions;

namespace MiProximoColectivo.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        public override System.Threading.Tasks.Task OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            return null;
        }

        public override System.Threading.Tasks.Task OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs args)
        {
            return null;
        }

        public override System.Threading.Tasks.Task OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            /*RequestTask<BankCard> requestAddCardTask = new RequestTask<BankCard>(() => Requests.RequestAddCard(Card), true);
            requestAddCardTask.TryStart();*/
            return null;
        }

        public override System.Threading.Tasks.Task BackKeyPressed(BackPressedEventArgs args)
        {
            return null;         
        }

        public override void CleanViewModel()
        {
        }
    }
}
