namespace OpenProfiler.WPF.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenProfiler.WPF.Models;

    public class SessionEventAddedEventArgs : EventArgs
    {
        public SessionEventAddedEventArgs(Event evt)
        {
            this.SessionEvent = evt;
        }

        public Event SessionEvent { get; private set; }
    }
}
