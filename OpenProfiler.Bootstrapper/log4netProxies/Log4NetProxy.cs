namespace OpenProfiler.Bootstrapper.log4netProxies
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    internal abstract class Log4NetProxy
    {
        private static BindingFlags PublicInstanceMethodFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;

        private static BindingFlags PublicPropertySetterFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty;

        private static BindingFlags PublicPropertyGetterFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;

        public Log4NetProxy()
        {
        }

        public Log4NetProxy(object underlyingObject)
        {
            this.UnderlyingObject = underlyingObject;
            this.UnderlyingType = underlyingObject.GetType();
        }

        public object UnderlyingObject { get; protected set; }
        public Type UnderlyingType { get; protected set; }

        public object InvokePublicInstanceMethod(string name, params object[] args)
        {
            return this.UnderlyingType.InvokeMember(
                name,
                PublicInstanceMethodFlags,
                null,
                this.UnderlyingObject,
                args);
        }

        public void InvokePublicSetter(string name, object value)
        {
            this.UnderlyingType.InvokeMember(
                name,
                PublicPropertySetterFlags,
                null,
                this.UnderlyingObject,
                new[] { value });
        }

        public object InvokePublicGetter(string name)
        {
            return this.UnderlyingType.InvokeMember(
                name,
                PublicPropertyGetterFlags,
                null,
                this.UnderlyingObject,
                null);
        }

        public void InvokeIndexerSetter(string name, object value)
        {
            this.UnderlyingType.InvokeMember(
                "Item",
                PublicPropertySetterFlags,
                null,
                this.UnderlyingObject,
                new[] { name, value });
        }

        public object InvokeIndexerGetter(string name)
        {
            return this.UnderlyingType.InvokeMember(
                "Item",
                PublicPropertyGetterFlags,
                null,
                this.UnderlyingObject,
                new[] { "name" });
        }
    }
}
