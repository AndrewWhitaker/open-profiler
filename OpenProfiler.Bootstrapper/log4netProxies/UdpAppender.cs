namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    public class UdpAppender : Log4NetProxy
    {
        public UdpAppender(object underlyingObject)
            : base(underlyingObject)
        {
        }

        public UdpAppender() : base()
        {
            this.UnderlyingType = Loader.GetType("log4net.Appender.UdpAppender");
            this.UnderlyingObject = Activator.CreateInstance(this.UnderlyingType);
        }

        public Encoding Encoding
        {
            get
            {
                return (Encoding)this.InvokePublicGetter("Encoding");
            }
            set
            {
                this.InvokePublicSetter("Encoding", value);
            }
        }

        public IPAddress RemoteAddress
        {
            get
            {
                return (IPAddress)this.InvokePublicGetter("RemoteAddress");
            }
            set
            {
                this.InvokePublicSetter("RemoteAddress", value);
            }
        }

        public int RemotePort
        {
            get
            {
                return (int)this.InvokePublicGetter("RemotePort");
            }
            set
            {
                this.InvokePublicSetter("RemotePort", value);
            }
        }

        public XmlLayout Layout
        {
            get
            {
                return new XmlLayout(this.InvokePublicGetter("Layout"));
            }
            set
            {
                this.InvokePublicSetter("Layout", value.UnderlyingObject);
            }
        }

        public object ErrorHandler
        {
            set { this.InvokePublicSetter("ErrorHandler", value); }
        }

        public void ActivateOptions()
        {
            this.InvokePublicInstanceMethod("ActivateOptions");
        }
    }
}
