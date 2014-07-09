namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Logger : Log4NetProxy
    {
        public Logger(object underlyingObject) : base(underlyingObject)
        {
        }

        public void AddAppender(UdpAppender appender)
        {
            this.InvokePublicInstanceMethod("AddAppender", appender.UnderlyingObject);
        }
    }
}
