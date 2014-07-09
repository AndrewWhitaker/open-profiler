namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class XmlLayout : Log4NetProxy
    {
        public XmlLayout(object underlyingObject) : base(underlyingObject)
        {
        }

        public XmlLayout()
        {
            this.UnderlyingType = Loader.GetType("log4net.Layout.XmlLayout");
            this.UnderlyingObject = Activator.CreateInstance(this.UnderlyingType);
        }

        public bool LocationInfo
        {
            get
            {
                return (bool)this.InvokePublicGetter("LocationInfo");
            }
            set
            {
                this.InvokePublicSetter("LocationInfo", true);
            }
        }
    }
}
