namespace OpenProfiler.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using log4net;
    using log4net.Core;

    public class DebugErrorHandler : IErrorHandler
    {
        public void Error(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Error(string msg, Exception e)
        {
            Console.WriteLine(msg);
            Console.WriteLine(e);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine(e.InnerException);
        }

        public void Error(string msg, Exception e, ErrorCode c)
        {
            Error(msg, e);
            //Console.WriteLine(msg);
            ////Console.WriteLine(e);
            //Console.WriteLine(e.StackTrace);
        }
    }

}
