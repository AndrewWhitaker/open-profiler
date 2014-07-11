namespace OpenProfiler.WPF.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using OpenProfiler.WPF.DataAccess;

    public class NHibernateLogDataProvider
    {
        private readonly XmlSerializer serializer;
        private readonly UdpClient udpClient;
        private readonly Dictionary<Guid, List<Event>> sessions;

        public NHibernateLogDataProvider()
        {
            this.serializer = new XmlSerializer(typeof(Event));
            this.udpClient = new UdpClient();
            this.sessions = new Dictionary<Guid, List<Event>>();

            this.udpClient.Client.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReuseAddress,
                true);

            this.udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 329));
        }

        public EventHandler<SessionEventAddedEventArgs> SessionEventAdded { get; set; }
        
        public EventHandler<SessionEventAddedEventArgs> NewSessionAdded { get; set; }

        public void Listen()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, 0);

            UdpState state = new UdpState
            {
                Client = this.udpClient,
                EndPoint = endpoint
            };

            this.udpClient.BeginReceive(this.Accepted, state);
        }

        public void StopListening()
        {
            this.udpClient.Close();
        }

        public void Accepted(IAsyncResult result)
        {
            UdpState state = result.AsyncState as UdpState;

            IPEndPoint endpoint = state.EndPoint;
            UdpClient client = state.Client;

            if (client.Client != null)
            {
                byte[] received = client.EndReceive(result, ref endpoint);

                Event evt;

                using (var strm = new MemoryStream(received))
                {
                    evt = (Event)this.serializer.Deserialize(strm);
                }

                this.HandleSessionEvent(evt);
            }

            this.udpClient.BeginReceive(this.Accepted, state);
        }

        private void HandleSessionEvent(Event evt)
        {
            if (evt != null)
            {
                if (this.sessions.ContainsKey(evt.SessionId))
                {
                    this.sessions[evt.SessionId].Add(evt);
                    this.SessionEventAdded(this, new SessionEventAddedEventArgs(evt));
                }
                else
                {
                    this.sessions.Add(evt.SessionId, new List<Event> { evt });
                    this.NewSessionAdded(this, new SessionEventAddedEventArgs(evt));
                }
            }
        }
    }
}
