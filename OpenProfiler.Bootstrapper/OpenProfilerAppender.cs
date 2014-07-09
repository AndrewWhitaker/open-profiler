namespace OpenProfiler.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using log4net;
    using log4net.Appender;
    using log4net.Layout;
    using log4net.Core;

    public class OpenProfilerAppender : UdpAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                Byte[] buffer = this.Encoding.GetBytes(RenderLoggingEvent(loggingEvent).ToCharArray());
                this.Client.Send(buffer, buffer.Length, this.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                ErrorHandler.Error(
                    "Unable to send logging event to remote host " +
                    this.RemoteAddress.ToString() +
                    " on port " +
                    this.RemotePort + ".",
                    ex,
                    ErrorCode.WriteFailure);
            }
        }
    }
}
