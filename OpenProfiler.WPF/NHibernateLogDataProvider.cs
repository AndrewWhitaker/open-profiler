namespace OpenProfiler.WPF
{
    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;

    public class NHibernateLogDataProvider : INotifyPropertyChanged
    {
        private string _stuff;
        private XmlSerializer _serializer;
        private readonly UdpClient udpClient;
        private readonly Dictionary<string, string> _sessionInformation;

        public NHibernateLogDataProvider()
        {
            this.udpClient = new UdpClient();

            udpClient.Client.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReuseAddress,
                true);

            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 317));

            Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action(() => Listen()), DispatcherPriority.Background);

            this._serializer = new XmlSerializer(typeof(Event));
            this.Sessions = new ObservableCollection<string>();
            this._sessionInformation = new Dictionary<string, string>();
        }

        public void Close()
        {
            this.udpClient.Close();
        }

        public void Listen()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, 0);
            udpClient.BeginReceive(Accepted, new UdpState { Client = udpClient, EndPoint = endpoint });
        }
        public void Accepted(IAsyncResult r)
        {
            UdpState state = r.AsyncState as UdpState;
            IPEndPoint endpoint = state.EndPoint;
            UdpClient client = state.Client;

            if (client.Client != null)
            {
                byte[] received = client.EndReceive(r, ref endpoint);
                Event evt;
                using (var strm = new MemoryStream(received))
                {
                    evt = (Event)this._serializer.Deserialize(strm);
                }

                HandleEvent(evt);

                client.BeginReceive(Accepted, state);
            }
        }

        private void HandleEvent(Event evt)
        {
            string sessionId = 
                evt.Properties
                    .Where(pr => pr.Name == "sessionId")
                    .Select(pr => pr.Value)
                    .SingleOrDefault();

            if (!string.IsNullOrEmpty(sessionId))
            {
                var d = App.Current.Dispatcher.BeginInvoke(new Action(
                    () =>
                    {
                        string value = string.Empty;
                        if (this._sessionInformation.ContainsKey(sessionId))
                        {
                            this._sessionInformation[sessionId] +=
                                evt.Message;
                        }
                        else
                        {
                            this._sessionInformation.Add(sessionId, evt.Message);
                            this.Sessions.Add(sessionId);
                        }

                    }), DispatcherPriority.Background);
            }
        }

        public ObservableCollection<string> Sessions
        {
            get;
            set;
        }

        public string SelectedSession
        {
            get
            {
                return this._selectedSession;
            }
            set
            {
                this._selectedSession = value;
                NotifyPropertyChanged("SelectedSessionInformation");
            }
        }

        public string SelectedSessionInformation
        {
            get 
            { 
                if (this._selectedSession != null &&
                    this._sessionInformation.ContainsKey(this._selectedSession))
                {
                    return this._sessionInformation[this._selectedSession];
                }
                else 
                {
                    return string.Empty;
                }
            }
        }

        //public string SelectedSessionInformation
        //{
        //    get { return this._stuff; }
        //    set
        //    {
        //        this._stuff = value;
        //        NotifyPropertyChanged("Stuff");
        //    }
        //}

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
private  string _selectedSession;
    }

    public class UdpState
    {
        public UdpClient Client { get; set; }

        public IPEndPoint EndPoint { get; set; }
    }
}
