using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using MiProximoColectivo.Classes.CancellableTasks;

namespace MiProximoColectivo.Classes.RequestTasks
{
    public class RequestTask<T> : CancellableTask<FramingHttpResponse<T>>
    {
        private DispatcherTimer outOfTimeCounter;
        private bool cancelIfOutOfTime;
        private bool autoRefreshToken;

        public RequestTask(Func<FramingHttpResponse<T>> request, bool cancelIfNetworkFalls, int secondsForOutOfTime = 0, bool cancelIfOutOfTime = false)
            : base(request, cancelIfNetworkFalls)
        {
            base.ContinueWith = new Action<Task<FramingHttpResponse<T>>>(taskFinished);

            //Si el usuario decide que debe registrarse un contador de tiempo que notifique un TimeOut
            if (secondsForOutOfTime > 0)
            {
                outOfTimeCounter = new DispatcherTimer();
                outOfTimeCounter.Interval = TimeSpan.FromSeconds(secondsForOutOfTime);
                outOfTimeCounter.Tick += timer_Tick;
                this.cancelIfOutOfTime = cancelIfOutOfTime;
            }
        }
        /// <summary>
        /// Antes de comenzar la tarea, verifica localmente que el Access Token no haya expirado.
        /// Si no expiró, comienza la tarea.
        /// Si expiró, 
        /// </summary>
        /// <returns></returns>
        public override bool TryStart()
        {
            return base.TryStart();
        }
        /// <summary>
        /// Este método se acciona al Finalizar éste Request Task.
        /// En él, se controlan los distintos tipos de resultado que podría tener el Task, y se lanzan
        /// los eventos OnCompleted (en caso  de que se haya completado exitosamente) y
        /// OnFailed (en caso de que no se haya completado, debido a algún error.
        /// </summary>
        /// <param name="resultado">
        /// Es el resultado crudo de éste Task, el cual será analizado para determinar si finalizó correctamente o con error.
        /// </param>
        private void taskFinished(Task<FramingHttpResponse<T>> resultado)
        {
            FramingHttpResponse<T> response = resultado.Result;
            //Si recibi un resultado distinto de null
            if (response != null)
            {
                //Si el resultado es OK
                if (response.IsSuccess)
                {
                    //Si el resultado no es null, lanzo un OnCompleted
                    if (response.Result != null)
                        OnCompleted(new RequestTaskCompletedEventArgs<T>(response.Result, response.StatusCode.ToString(), response.ReasonPhrase, response.ResponseAsString));
                    //Sino, lanzo un OnFailed
                    else
                        OnFailed(new RequestTaskFailedEventArgs<T>(response.StatusCode.ToString(), "Null Response Result", response.ResponseAsString));
                }
                //Si el resultado No es OK
                else
                {
                    OnFailed(new RequestTaskFailedEventArgs<T>(response.StatusCode.ToString(), response.ReasonPhrase, response.ResponseAsString));
                }
            }
            //Si recibi un resultado null, lanzo un OnFailed
            else
                OnFailed(new RequestTaskFailedEventArgs<T>(RequestTaskExceptionalsErrors.NullResponseError.ToString(), "Null Response", ""));
        }
        private void timer_Tick(object sender, object e)
        {
            outOfTimeCounter.Stop();

            if (IsRunning)
            {
                if (cancelIfOutOfTime)
                    Cancel("Out of Time");
                else
                    OnOutOfTimeReached(EventArgs.Empty);
            }
        }

        public event EventHandler<RequestTaskCompletedEventArgs<T>> Completed;
        public event EventHandler<RequestTaskFailedEventArgs<T>> Failed;
        public event EventHandler OutOfTimeReached;

        protected virtual void OnCompleted(RequestTaskCompletedEventArgs<T> e)
        {
            EventHandler<RequestTaskCompletedEventArgs<T>> handler = Completed;
            if (handler != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    handler(this, e));
        }
        protected virtual void OnFailed(RequestTaskFailedEventArgs<T> e)
        {
            EventHandler<RequestTaskFailedEventArgs<T>> handler = Failed;
            if (handler != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    handler(this, e));
        }

        protected virtual void OnOutOfTimeReached(EventArgs e)
        {
            EventHandler handler = OutOfTimeReached;
            if (handler != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    handler(this, e));
        }
    }
    public enum RequestTaskExceptionalsErrors
    {
        NullResponseError = -1,
        RefreshTokenError = -2
    }
}
