namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class Level
    {
        public static object DebugLevel()
        {
            Type t = Loader.GetType("log4net.Core.Level");

            return t.GetField("Debug").GetValue(null);
        }
    }
}
