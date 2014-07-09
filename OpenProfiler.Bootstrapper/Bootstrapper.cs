namespace OpenProfiler.Bootstrapper
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using OpenProfiler.Bootstrapper.log4netProxies;

    public class Bootstrapper
    {
        private const int RemotePort = 331;
        
        private static readonly IPAddress RemoteAddress = IPAddress.Parse("127.0.0.1");

        public static void Initialize()
        {
            Loader.Initialize();

            ThreadContext.Properties["sessionId"] = BuildSessionLogger();

            Hierarchy hierarchy = LogManager.GetRepository();
            Logger logger = hierarchy.GetLogger("NHibernate.SQL");

            UdpAppender appender = new UdpAppender();
            appender.Encoding = Encoding.UTF8;
            appender.RemoteAddress = RemoteAddress;
            appender.RemotePort = RemotePort;

            XmlLayout layout = new XmlLayout();

            layout.LocationInfo = true;

            appender.Layout = layout;
            appender.ActivateOptions();

            logger.Level = Level.DebugLevel(); 
            logger.AddAppender(appender);

            hierarchy.Configured = true;
            hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
        }

        private static object BuildSessionLogger()
        {
            var parameters = new CompilerParameters(
                new[] { "NHibernate.dll" });

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            string resourceText = getResourceText("SessionIdCapturer.cs");

            CompilerResults results = CodeDomProvider.CreateProvider("CSharp")
                .CompileAssemblyFromSource(parameters, resourceText);

            object result = null;

            if (results.Errors.Count == 0)
            {
                Assembly compiledAssembly = results.CompiledAssembly;
                Type sessionIdCapturerType = compiledAssembly.GetType("OpenProfiler.Bootstrapper.SessionIdCapturer");

                result = Activator.CreateInstance(sessionIdCapturerType);
            }

            return result;
        }

        private static string getResourceText(string fileName)
        {
            Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("OpenProfiler.Bootstrapper.SessionIdCapturer.cs");

            string result = null;

            using (stream)
            {
                byte[] contents = new byte[stream.Length];
                stream.Read(contents, 0, (int)stream.Length);

                result = Encoding.UTF8.GetString(contents);
            }

            return result;
        }
    }
}
