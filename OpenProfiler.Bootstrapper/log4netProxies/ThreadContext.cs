using System;
using System.Reflection;
namespace OpenProfiler.Bootstrapper.log4netProxies
{
    public class ThreadContext
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
