namespace OpenProfiler.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using NHibernate.Impl;

    public class OpenProfilerAppender : UdpAppender
    {
        public OpenProfilerAppender()
        {
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            loggingEvent.Properties["sessionId"] = SessionIdLoggingContext.SessionId.ToString();
            base.Append(loggingEvent);
        }
    }
}
