using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiProximoColectivo.ViewModels.Base
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            RegistrarViewModels();
        }

        #region View_Models
        public MainPageViewModel MainPageViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainPageViewModel>(); }
        }

        #endregion
        public static void Cleanup()
        {
            SimpleIoc.Default.Reset();
            RegistrarViewModels();
        }
        public static void RegistrarViewModels()
        {
            SimpleIoc.Default.Register<MainPageViewModel>();
        }
    }
}