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
        private readonly XmlSerializer _serializer;
        private readonly UdpClient _udpClient;
        private readonly Dictionary<Guid, List<Event>> _sessions;

        public NHibernateLogDataProvider()
        {
            this._serializer = new XmlSerializer(typeof(Event));
            this._udpClient = new UdpClient();
            this._sessions = new Dictionary<Guid, List<Event>>();

            this._udpClient.Client.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReuseAddress,
                true);

            this._udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 329));
        }

        public void Listen()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, 0);

            UdpState state = new UdpState
            {
                Client = this._udpClient,
                EndPoint = endpoint
            };

            this._udpClient.BeginReceive(Accepted, state);
        }

        public void StopListening()
        {
            this._udpClient.Close();
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
                    evt = (Event)this._serializer.Deserialize(strm);
                }

                this._handleSessionEvent(evt);
            }

            this._udpClient.BeginReceive(Accepted, state);
        }

        private void _handleSessionEvent(Event evt)
        {
            if (evt != null)
            {
                if (this._sessions.ContainsKey(evt.SessionId))
                {
                    this._sessions[evt.SessionId].Add(evt);
                    SessionEventAdded(this, new SessionEventAddedEventArgs(evt));
                }
                else
                {
                    this._sessions.Add(evt.SessionId, new List<Event> { evt });
                    NewSessionAdded(this, new SessionEventAddedEventArgs(evt));
                }
            }
        }

        public EventHandler<SessionEventAddedEventArgs> SessionEventAdded;
        public EventHandler<SessionEventAddedEventArgs> NewSessionAdded;
    }
}
