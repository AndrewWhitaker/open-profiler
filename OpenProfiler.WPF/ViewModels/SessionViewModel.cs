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
        private readonly NHibernateLogDataProvider _provider;
        private SessionEventViewModel _selectedSessionEvent;

        public SessionViewModel(Guid id, NHibernateLogDataProvider provider)
        {
            this.Id = id;
            this._provider = provider;

            this._provider.SessionEventAdded += _newSessionEventAdded;

            this.SessionEvents = new ObservableCollection<SessionEventViewModel>();
        }

        public SessionEventViewModel SelectedSessionEvent 
        { 
            get
            {
                return this._selectedSessionEvent;
            }
            set
            {
                this._selectedSessionEvent = value;
                this.NotifyPropertyChanged("SelectedSessionEvent");
            }
        }

        public Guid Id { get; private set; }

        public void _newSessionEventAdded(object sender, SessionEventAddedEventArgs args)
        {
            if (this.Id == args.SessionEvent.SessionId)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.SessionEvents.Add(
                            new SessionEventViewModel(args.SessionEvent.TimeStamp, args.SessionEvent.Message));
                    }), DispatcherPriority.Background);
            }
        }

        public ObservableCollection<SessionEventViewModel> SessionEvents { get; private set; }
    }
}
