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
        private const int RemotePort = 329;
        private static readonly IPAddress RemoteAddress = IPAddress.Parse("127.0.0.1");

        private static Assembly nHibernateAssembly;
        private static Assembly log4netAssembly;

        public static void Initialize()
        {
            nHibernateAssembly = FindAssembly("NHibernate.dll");
            log4netAssembly = FindAssembly("log4net.dll");

            if (nHibernateAssembly != null && log4netAssembly != null)
            {
                Loader.Initialize(log4netAssembly);

                Hierarchy hierarchy = LogManager.GetRepository();
                Logger logger = hierarchy.GetLogger("NHibernate.SQL");

                var openProfilerAppender = BuildAppender();

                UdpAppender appender = new UdpAppender(openProfilerAppender);
                appender.Encoding = Encoding.UTF8;
                appender.RemoteAddress = RemoteAddress;
                appender.RemotePort = RemotePort;

                appender.Layout = new XmlLayout();
                appender.ActivateOptions();

                logger.Level = Level.DebugLevel();
                logger.AddAppender(appender);

                hierarchy.Configured = true;
                hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
            }
        }

        private static object BuildAppender()
        {
            string nHibernatePath = nHibernateAssembly.Location;
            string log4netPath = log4netAssembly.Location;

            var parameters = new CompilerParameters(
                new[] { nHibernatePath, log4netPath });

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            string resourceText = getResourceText("OpenProfiler.Bootstrapper.OpenProfilerAppender.cs");

            CompilerResults results = CodeDomProvider.CreateProvider("CSharp")
                .CompileAssemblyFromSource(parameters, resourceText);

            object result = null;

            if (results.Errors.Count == 0)
            {
                Assembly compiledAssembly = results.CompiledAssembly;
                Type appenderType = compiledAssembly.GetType("OpenProfiler.Bootstrapper.OpenProfilerAppender");

                result = Activator.CreateInstance(appenderType);
            }

            return result;
        }

        private static Assembly FindAssembly(string dllName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
            string binPath = relativeSearchPath == null ? baseDirectory : Path.Combine(baseDirectory, relativeSearchPath);
            string path = binPath == null ? dllName : Path.Combine(binPath, dllName);

            return File.Exists(path) ?
                Assembly.LoadFrom(path) :
                null;
        }

        private static string getResourceText(string fileName)
        {
            Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(fileName);

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
