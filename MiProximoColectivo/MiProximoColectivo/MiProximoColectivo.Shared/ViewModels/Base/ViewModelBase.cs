using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;


namespace MiProximoColectivo.ViewsModel.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Frame _appFrame;
        private StatusBar _pageStatusBar;
        private string _statusBarMessage;
        private Type _typeOfCurrentPage;
        private bool _isBusy;

        private ContentDialog _loadingPopUp;
        private bool _loadingPopUpCloseEnabled;
        private bool _isloadingPopUpOpen;

        public ViewModelBase()
        {
            
        }

        public Frame AppFrame
        {
            get { return _appFrame; }
        }


        public StatusBar PageStatusBar
        {
            get { return _pageStatusBar; }
        }

        public virtual bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        public abstract Task OnNavigatedFrom(NavigationEventArgs args);

        public abstract Task OnNavigatingFrom(NavigatingCancelEventArgs args);

        public abstract Task OnNavigatedTo(NavigationEventArgs args);

        public abstract Task BackKeyPressed(BackPressedEventArgs args);
 
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                handler(this, new PropertyChangedEventArgs(propertyName)));
        }

        internal void SetAppFrame(Frame viewFrame)
        {
            _appFrame = viewFrame;
        }

        internal void SetPageStatusBar(StatusBar bar)
        {
            _pageStatusBar = bar;
        }

        internal void SetCurrentPageType(Type pageType)
        {
            _typeOfCurrentPage = pageType;
        }

        public void ShowDialog(string message, string title = null)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                try
                {
                    /*if (CommonMethods.ValidateString(title))
                    {
                        MessageDialog md = new MessageDialog(message, title);
                        await md.ShowAsync();
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog(message);
                        await md.ShowAsync();
                    }*/
                }
                catch
                {
                    
                }
            });
        }

        public void ShowDialogCommands(string message, UICommand command1, string title = null, UICommand command2 = null)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                try
                {
                    /*if (CommonMethods.ValidateString(title))
                    {
                        MessageDialog md = new MessageDialog(message, title);
                        md.Commands.Add(command1);
                        if (command2 != null)
                        {
                            md.Commands.Add(command2);
                        }
                        md.CancelCommandIndex = 1;
                        await md.ShowAsync();
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog(message);
                        await md.ShowAsync();
                    }*/
                }
                catch
                {

                }
            });
        }

        public void ShowLoadingPopUp(string body = "cargando...", bool showProgressIndicator = true, string progressIndicatorMessage = "Cargando...", bool cancelOnBackKeyPressed = false)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                if (!_isloadingPopUpOpen)
                {
                    _loadingPopUp = new ContentDialog();
                    _loadingPopUp.Content = body;
                    _loadingPopUp.VerticalAlignment = VerticalAlignment.Stretch;
                    _loadingPopUp.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _loadingPopUp.VerticalContentAlignment = VerticalAlignment.Stretch;
                    _loadingPopUp.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    _loadingPopUp.FullSizeDesired = true;
                    _loadingPopUp.Background = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
                    _loadingPopUp.Foreground = new SolidColorBrush(Colors.White);
                    _loadingPopUp.Closing += _loadingPopUp_Closing;

                    if (showProgressIndicator)
                        ShowStatusBarProgressIndicator(Colors.White, progressIndicatorMessage);

                    _isloadingPopUpOpen = true;

                    await _loadingPopUp.ShowAsync();

                    _loadingPopUpCloseEnabled = cancelOnBackKeyPressed;
                }
                else
                {
                    _loadingPopUp.Content = body;

                    if (!showProgressIndicator)
                        HideStatusBarProgressIndicator();
                    else if (progressIndicatorMessage != _statusBarMessage)
                        ShowStatusBarProgressIndicator(Colors.White, progressIndicatorMessage);
                        
                    _loadingPopUpCloseEnabled = cancelOnBackKeyPressed;
                }
            });
        }

        public void HideLoadingPopUp(bool hideStatusBarProgressIndicator = false)
        {
            if (_loadingPopUp != null)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _loadingPopUpCloseEnabled = true;
                    _loadingPopUp.Hide();
                    _isloadingPopUpOpen = false;
                });
            }
            if (hideStatusBarProgressIndicator)
                HideStatusBarProgressIndicator();
        }

        private void _loadingPopUp_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            //Si no esta habilitado que se pueda ocultar el pop up, cancelo el Closing
            if(!_loadingPopUpCloseEnabled)
                args.Cancel = true;
        }

        /*public virtual bool Navigate(Type destinationPage, NavigationContext navigationParameter = null)
        {
            bool resultado = false;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (AppFrame != null && destinationPage != null)
                {
                    resultado = true;
                    AppFrame.Navigate(destinationPage, navigationParameter);
                }
            });
            return resultado;
        }*/

        public virtual bool NavigateBack()
        {
            bool resultado = false;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (AppFrame != null && AppFrame.CanGoBack)
                {
                    resultado = true;
                    AppFrame.GoBack();
                }
            });
            return resultado;
        }

        /// <summary>
        /// Returns the number of deleted pages from backstack.
        /// </summary>
        /// <param name="numberOfPages">Number of Pages to delete.</param>
        /// <returns></returns>
        public int RemoveBackStackPages(int numberOfPages)
        {
            int amountDeletedPages = 0;
            if (numberOfPages > 0)
            {
                for (int i = 0; i < numberOfPages; i++)
                {
                    try
                    {
                        if (AppFrame.BackStack.Count > 0)
                        {
                            AppFrame.BackStack.RemoveAt(AppFrame.BackStack.Count - 1);
                            amountDeletedPages++;
                        }
                        else
                            break;
                    }
                    catch
                    {
                        
                    }
                }
            }
            return amountDeletedPages;
        }

        public void ShowStatusBarProgressIndicator(string message = "MercadoPago")
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                PageStatusBar.ForegroundColor = Colors.Gray;
                PageStatusBar.ProgressIndicator.Text = message;
                PageStatusBar.ProgressIndicator.ProgressValue = null;
                await PageStatusBar.ShowAsync();
                await PageStatusBar.ProgressIndicator.ShowAsync();
            });
        }

        public void ShowStatusBarProgressIndicator(Color foregroundColor, string message = "MercadoPago")
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                PageStatusBar.ForegroundColor = foregroundColor;
                PageStatusBar.ProgressIndicator.Text = message;
                PageStatusBar.ProgressIndicator.ProgressValue = null;
                _statusBarMessage = message;

                await PageStatusBar.ShowAsync();
                await PageStatusBar.ProgressIndicator.ShowAsync();
            });
        }

        public void HideStatusBarProgressIndicator()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                PageStatusBar.ForegroundColor = Colors.Gray;
                PageStatusBar.ProgressIndicator.Text = "MercadoPago";
                PageStatusBar.ProgressIndicator.ProgressValue = 0;
                _statusBarMessage = "MercadoPago";

                await PageStatusBar.ShowAsync();
                await PageStatusBar.ProgressIndicator.ShowAsync();
            });
        }

        public abstract void CleanViewModel();
    }
}
