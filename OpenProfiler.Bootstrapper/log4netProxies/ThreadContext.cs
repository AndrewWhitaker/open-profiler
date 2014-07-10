namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Reflection;

    internal class ThreadContext
    {
        public static PropertiesDictionary Properties
        {
            get
            {
                Type t = Loader.GetType("log4net.ThreadContext");
                object dictionary =
                    t.InvokeMember(
                        "Properties", 
                        BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty,
                        null,
                        null,
                        null);

                return new PropertiesDictionary(dictionary);
            }
        }
    }
}
