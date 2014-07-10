namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class Hierarchy : Log4NetProxy
    {
        public Hierarchy(object underlyingObject) : base(underlyingObject)
        {
        }

        public Logger GetLogger(string name)
        {
            object logger = this.InvokePublicInstanceMethod("GetLogger", name);

            return new Logger(logger);
        }

        public bool Configured
        {
            get
            {
                return (bool)this.InvokePublicGetter("Configured");
            }
            set
            {
                this.InvokePublicSetter("Configured", true);
            }
        }

        public void RaiseConfigurationChanged(EventArgs args)
        {
            this.InvokePublicInstanceMethod("RaiseConfigurationChanged", args);
        }
    }
}
