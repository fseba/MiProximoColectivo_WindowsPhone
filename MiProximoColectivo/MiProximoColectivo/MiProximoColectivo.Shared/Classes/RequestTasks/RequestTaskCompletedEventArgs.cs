using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MiProximoColectivo.Classes.Interfaces;

namespace MiProximoColectivo.Classes.RequestTasks
{
    public class RequestTaskCompletedEventArgs<T> : EventArgs, IResultDebugWriter
    {
        private T result;
        private string statusCode;
        private string reason;
        private string responseAsString;
        public T Result
        {
            get { return result; }
            set { result = value; }
        }
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
        public RequestTaskCompletedEventArgs()
        { }
        public RequestTaskCompletedEventArgs(T result, string statusCode, string reason, string responseAsString)
        {
            Result = result;
            StatusCode = statusCode;
            Reason = reason;
            ResponseAsString = responseAsString;
        }

        public void WriteResultOnDebugLine()
        {
            Debug.WriteLine("RequestTaskCompletedEventArgs: Awaited Type of Result: {0} ResponseAsString: {1}", typeof(T).ToString(), this.ResponseAsString);
        }
    }
}