using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace MiProximoColectivo.Classes
{
    public class FramingHttpResponse<T> : INotifyPropertyChanged
    {
        private T result;
        private string reasonPhrase;
        private bool isSuccess;
        private HttpStatusCode statusCode;
        private string responseAsString;

        public T Result
        {
            get { return result; }
            set
            {
                result = value;
                RaisePropertyChanged();
            }
        }
        public string ReasonPhrase
        {
            get { return reasonPhrase; }
            set
            {
                reasonPhrase = value;
                RaisePropertyChanged();
            }
        }
        public bool IsSuccess
        {
            get { return isSuccess; }
            set
            {
                isSuccess = value;
                RaisePropertyChanged();
            }
        }
        public HttpStatusCode StatusCode
        {
            get { return statusCode; }
            set
            {
                statusCode = value;
                RaisePropertyChanged();
            }
        }
        public string ResponseAsString
        {
            get { return responseAsString; }
            set
            {
                responseAsString = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var Handler = PropertyChanged;
            if (Handler != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                Handler(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}