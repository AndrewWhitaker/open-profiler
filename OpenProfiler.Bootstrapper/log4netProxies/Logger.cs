namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class Logger : Log4NetProxy
    {
        public Logger(object underlyingObject) : base(underlyingObject)
        {
        }

        public void AddAppender(UdpAppender appender)
        {
            this.InvokePublicInstanceMethod("AddAppender", appender.UnderlyingObject);
        }

        public object Level
        {
            get
            {
                return this.InvokePublicGetter("Level");
            }
            set
            {
                this.InvokePublicSetter("Level", value);
            }
        }
    }
}
