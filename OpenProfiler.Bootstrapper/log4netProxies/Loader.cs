namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public static class Loader
    {
        private const string Log4NetDllName = "log4net.dll";
        private static Assembly Assembly;

        public static void Initialize()
        {
            string log4NetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Log4NetDllName);

            if (File.Exists(log4NetPath))
            {
                Assembly = Assembly.LoadFile(log4NetPath);
            }
        }

        public static Type GetType(string typeName)
        {
            Type type = Assembly.GetType(typeName);

            if (type == null)
            {
                throw new ArgumentException("Unable to find type with name {0} in log4net assembly", typeName);
            }

            return type;
        }
    }
}
