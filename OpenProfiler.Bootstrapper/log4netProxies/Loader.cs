﻿namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    internal static class Loader
    {
        private static Assembly Assembly;

        public static void Initialize(Assembly log4netAssembly)
        {
            Assembly = log4netAssembly;
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
