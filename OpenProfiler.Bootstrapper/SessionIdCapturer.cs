namespace OpenProfiler.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NHibernate.Impl;

    public class SessionIdCapturer
    {
        public override string ToString()
        {
            return SessionIdLoggingContext.SessionId.ToString();
        }
    }
}
