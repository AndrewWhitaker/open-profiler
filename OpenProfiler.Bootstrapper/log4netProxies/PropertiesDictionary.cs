namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PropertiesDictionary : Log4NetProxy
    {
        public PropertiesDictionary(object underlyingObject) 
            : base(underlyingObject)
        {
        }

        public object this[string key]
        {
            get
            {
                return this.InvokeIndexerGetter(key);
            }
            set
            {
                this.InvokeIndexerSetter(key, value);
            }
        }
    }
}
