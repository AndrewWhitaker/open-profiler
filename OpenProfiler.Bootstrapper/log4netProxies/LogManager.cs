namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    
    internal class LogManager
    {
        public static Hierarchy GetRepository()
        {
            Type type = Loader.GetType("log4net.LogManager");

            object hierarchy = type.InvokeMember(
                "GetRepository", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null);

            return new Hierarchy(hierarchy);
        }
    }
}
