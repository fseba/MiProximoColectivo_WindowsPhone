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
        public MainMenuPageViewModel MainMenuPageViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainMenuPageViewModel>(); }
        }
        public MapPageViewModel MapPageViewModel 
        {
            get { return ServiceLocator.Current.GetInstance<MapPageViewModel>(); }
        }
        public NearFromViewModel NearFromViewModel
        {
            get { return ServiceLocator.Current.GetInstance<NearFromViewModel>(); }
        }
        public HowToArriveViewModel HowToArriveViewModel
        {
            get { return ServiceLocator.Current.GetInstance<HowToArriveViewModel>(); }
        }
        public ViewTrackViewModel ViewTrackViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ViewTrackViewModel>(); }
        }

        #endregion
        public static void Cleanup()
        {
            SimpleIoc.Default.Reset();
            RegistrarViewModels();
        }
        public static void RegistrarViewModels()
        {
            SimpleIoc.Default.Register<MainMenuPageViewModel>();
            SimpleIoc.Default.Register<MapPageViewModel>();
            SimpleIoc.Default.Register<NearFromViewModel>();
            SimpleIoc.Default.Register<HowToArriveViewModel>();
            SimpleIoc.Default.Register<ViewTrackViewModel>();
        }
    }
}