namespace OpenProfiler.WPF.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using OpenProfiler.WPF.DataAccess;
    using OpenProfiler.WPF.Models;

    public class SessionViewModel : ViewModelBase
    {
        private static int nextNumber = 1;
        private readonly NHibernateLogDataProvider provider;
        private SessionEventViewModel selectedSessionEvent;

        public SessionViewModel(Guid id, NHibernateLogDataProvider provider)
        {
            this.Id = id;
            this.provider = provider;

            this.provider.SessionEventAdded += this.NewSessionEventAdded;
            this.Number = nextNumber++;

            this.SessionEvents = new ObservableCollection<SessionEventViewModel>();
        }

        public SessionEventViewModel SelectedSessionEvent 
        { 
            get
            {
                return this.selectedSessionEvent;
            }

            set
            {
                this.selectedSessionEvent = value;
                this.NotifyPropertyChanged("SelectedSessionEvent");
            }
        }

        public Guid Id { get; private set; }

        public int Number { get; private set; }

        public ObservableCollection<SessionEventViewModel> SessionEvents { get; private set; }

        private void NewSessionEventAdded(object sender, SessionEventAddedEventArgs args)
        {
            if (this.Id == args.SessionEvent.SessionId)
            {
                App.Current.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        this.SessionEvents.Add(
                            new SessionEventViewModel(args.SessionEvent.TimeStamp, args.SessionEvent.Message));
                    }), 
                    DispatcherPriority.Background);
            }
        }
    }
}
