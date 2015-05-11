using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MiProximoColectivo.Classes.Interfaces;

namespace MiProximoColectivo.Classes.RequestTasks
{
    public class RequestTaskFailedEventArgs<T> : EventArgs, IResultDebugWriter
    {
        private string statusCode;
        private string reason;
        private string responseAsString;

        public string StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
        public string ResponseAsString
        {
            get { return responseAsString; }
            set { responseAsString = value; }
        }

        public RequestTaskFailedEventArgs(string statusCode, string reason, string responseAsString)
        {
            StatusCode = statusCode;
            Reason = reason;
            ResponseAsString = responseAsString;
        }

        public void WriteResultOnDebugLine()
        {
            Debug.WriteLine("RequestTaskFailedEventArgs: Awaited Type of Result: {0} ResponseAsString: {1}", typeof(T).ToString(), this.ResponseAsString);
        }

    }
}
