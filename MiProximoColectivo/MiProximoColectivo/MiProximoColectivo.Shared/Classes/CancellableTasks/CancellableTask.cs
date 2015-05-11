using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MiProximoColectivo.Classes.CancellableTasks
{
    public class CancellableTask
    {
        private Task task;
        private static CancellationTokenSource MyCancellationTokenSource;
        private Action<Task> taskContinueWith;
        private static CancellationToken MyCancellationToken
        {
            get
            {
                if (MyCancellationTokenSource == null || MyCancellationTokenSource.Token == null || !MyCancellationTokenSource.Token.CanBeCanceled)
                    MyCancellationTokenSource = new CancellationTokenSource();
                return MyCancellationTokenSource.Token;
            }
        }
        public bool IsRunning
        {
            get { return (task != null && (!task.IsCompleted && !task.IsCanceled && !task.IsFaulted) || (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation || task.Status == TaskStatus.WaitingToRun)); }
        }
        public CancellableTask(Action action, bool cancelIfNetworkFalls, Action<Task> continueWith = null)
        {
            MyCancellationTokenSource = new CancellationTokenSource();
            task = new Task(action, MyCancellationToken);

            if (continueWith != null)
                taskContinueWith = continueWith;

            if (cancelIfNetworkFalls)
               Messenger.Default.Register<NotificationMessage<NetworkStatusArgs>>(this, false, new Action<NotificationMessage<NetworkStatusArgs>>(Network_Status_Changed));
        }
        public bool TryStart()
        {
            bool resultado = false;
            try
            {
                task.Start();
                if (taskContinueWith != null)
                    task.ContinueWith(taskContinueWith, MyCancellationToken);
                resultado = true;
            }
            catch (Exception ex)
            { }
            return resultado;
        }
        public void Cancel(string reason, bool forceRaiseOnCanceledEvent = false)
        {
            bool operationCanceled = false;
            try
            {
                if (task != null && !task.IsCanceled && !task.IsFaulted && !task.IsCompleted && MyCancellationToken.CanBeCanceled)
                {
                    MyCancellationTokenSource.Cancel();
                    task.Wait(MyCancellationToken);
                }
            }
            catch (OperationCanceledException tEx)
            {
                operationCanceled = true;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (operationCanceled || forceRaiseOnCanceledEvent)
                    OnCanceled(new CancellableTaskCanceledEventArgs(reason));
            }
        }
        private void Network_Status_Changed(NotificationMessage<NetworkStatusArgs> currentNetworkStatus)
        {
            //Si se perdio la conexion a internet, cancelo el Task
            if (currentNetworkStatus.Content.NetworkType == NetworkType.Null)
                Cancel("Network Falls", true);
        }

        public event EventHandler Canceled;
        protected virtual void OnCanceled(CancellableTaskCanceledEventArgs e)
        {
            EventHandler handler = Canceled;
            if (handler != null)
                handler(this, e);
        }
    }
    public class CancellableTask<T>
    {
        private Task<T> task;
        private static CancellationTokenSource MyCancellationTokenSource;
        private Action<Task<T>> taskContinueWith;

        private static CancellationToken MyCancellationToken
        {
            get
            {
                if (MyCancellationTokenSource == null)
                    MyCancellationTokenSource = new CancellationTokenSource();
                return MyCancellationTokenSource.Token;
            }
        }
        public bool IsRunning
        {
            get { return (task != null && (!task.IsCompleted && !task.IsCanceled && !task.IsFaulted) || (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation || task.Status == TaskStatus.WaitingToRun)); }
        }
        protected Action<Task<T>> ContinueWith
        {
            private get { return taskContinueWith; }
            set { taskContinueWith = value; }
        }
        public CancellableTask(Func<T> action, bool cancelIfNetworkFalls, Action<Task<T>> continueWith = null, double secondsToWait = 0)
        {
            MyCancellationTokenSource = new CancellationTokenSource();
            task = new Task<T>(action, MyCancellationToken);

            if (continueWith != null)
                taskContinueWith = continueWith;

            if (cancelIfNetworkFalls)
                Messenger.Default.Register<NotificationMessage<NetworkStatusArgs>>(this, false, new Action<NotificationMessage<NetworkStatusArgs>>(Network_Status_Changed));
        }
        //Intenta iniciar la Tarea en segundo plano
        public virtual bool TryStart()
        {
            bool resultado = false;

            try
            {
                task.Start();
                if (taskContinueWith != null)
                    task.ContinueWith(taskContinueWith, MyCancellationToken);

                resultado = true;
            }
            catch (Exception ex)
            {
            }
            return resultado;
        }
        public void Cancel(string reason, bool forceRaiseOnCanceledEvent = false)
        {
            bool operationCanceled = false;
            try
            {
                if (task != null && !task.IsCanceled && !task.IsFaulted && !task.IsCompleted && MyCancellationToken.CanBeCanceled)
                {
                    MyCancellationTokenSource.Cancel();
                    task.Wait(MyCancellationToken);
                }
            }
            catch (OperationCanceledException tEx)
            {
                operationCanceled = true;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (operationCanceled || forceRaiseOnCanceledEvent)
                    OnCanceled(new CancellableTaskCanceledEventArgs(reason));
            }
        }
        private void Network_Status_Changed(NotificationMessage<NetworkStatusArgs> currentNetworkStatus)
        {
            //Si se perdio la conexion a internet, cancelo el Task
            if (currentNetworkStatus.Content.NetworkType == NetworkType.Null)
                Cancel("Network Falls", true);
        }

        public event EventHandler Canceled;

        protected virtual void OnCanceled(CancellableTaskCanceledEventArgs e)
        {
            EventHandler handler = Canceled;
            if (handler != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    handler(this, e));
        }
    }

    public class CancellableTask2<T>
    {
        private Task<T> task;
        private static CancellationTokenSource MyCancellationTokenSource;
        private Action<T> taskContinueWith;

        private Func<T> _action;
        private static CancellationToken MyCancellationToken
        {
            get
            {
                if (MyCancellationTokenSource == null)
                    MyCancellationTokenSource = new CancellationTokenSource();
                return MyCancellationTokenSource.Token;
            }
        }
        public bool IsRunning
        {
            get { return (task != null && (!task.IsCompleted && !task.IsCanceled && !task.IsFaulted) || (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation || task.Status == TaskStatus.WaitingToRun)); }
        }
        protected Action<T> ContinueWith
        {
            private get { return taskContinueWith; }
            set { taskContinueWith = value; }
        }
        public CancellableTask2(Func<T> action, bool cancelIfNetworkFalls, Action<T> continueWith = null, double secondsToWait = 0)
        {
            MyCancellationTokenSource = new CancellationTokenSource();
            task = new Task<T>(action, MyCancellationToken);

            _action = action;

            if (continueWith != null)
                taskContinueWith = continueWith;

            if (cancelIfNetworkFalls)
                Messenger.Default.Register<NotificationMessage<NetworkStatusArgs>>(this, false, new Action<NotificationMessage<NetworkStatusArgs>>(Network_Status_Changed));
        }
        //Intenta iniciar la Tarea en segundo plano
        public virtual async Task<bool> TryStart()
        {
            bool resultado = false;

            try
            {
                var result = await Task.Run<T>(_action, MyCancellationToken);

                if (taskContinueWith != null)
                    await Task.Run(() => taskContinueWith(result), MyCancellationToken);

                /*task.Start();
                if (taskContinueWith != null)
                    task.ContinueWith(taskContinueWith, MyCancellationToken);*/

                resultado = true;
            }
            catch (Exception ex)
            {
            }
            return resultado;
        }
        public void Cancel(string reason, bool forceRaiseOnCanceledEvent = false)
        {
            bool operationCanceled = false;
            try
            {
                if (task != null && !task.IsCanceled && !task.IsFaulted && !task.IsCompleted && MyCancellationToken.CanBeCanceled)
                {
                    MyCancellationTokenSource.Cancel();
                    task.Wait(MyCancellationToken);
                }
            }
            catch (OperationCanceledException tEx)
            {
                operationCanceled = true;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (operationCanceled || forceRaiseOnCanceledEvent)
                    OnCanceled(new CancellableTaskCanceledEventArgs(reason));
            }
        }
        private void Network_Status_Changed(NotificationMessage<NetworkStatusArgs> currentNetworkStatus)
        {
            //Si se perdio la conexion a internet, cancelo el Task
            if (currentNetworkStatus.Content.NetworkType == NetworkType.Null)
                Cancel("Network Falls", true);
        }

        public event EventHandler Canceled;

        protected virtual void OnCanceled(CancellableTaskCanceledEventArgs e)
        {
            EventHandler handler = Canceled;
            if (handler != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    handler(this, e));
        }
    }

    public class NetworkStatusArgs
    {
        public NetworkType NetworkType { get; set; }
        public string ConnectionType { get; set; }
        public string ConnectionError { get; set; }
    }

    public enum NetworkType
    {
        Null, Internet
    }

}

