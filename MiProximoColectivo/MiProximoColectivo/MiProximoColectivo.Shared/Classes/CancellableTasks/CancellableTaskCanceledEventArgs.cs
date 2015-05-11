using System;
using System.Collections.Generic;
using System.Text;

namespace MiProximoColectivo.Classes.CancellableTasks
{
    public class CancellableTaskCanceledEventArgs : EventArgs
    {
        private string reason;
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
        public CancellableTaskCanceledEventArgs(string reason)
        {
            Reason = reason;
        }
    }
}