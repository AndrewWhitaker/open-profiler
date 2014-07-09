namespace OpenProfiler.WPF.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Threading;
    using OpenProfiler.WPF.DataAccess;
    using OpenProfiler.WPF.Models;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NHibernateLogDataProvider _provider;
        private SessionViewModel _selectedSession;

        public MainWindowViewModel()
        {
            this.Sessions = new ObservableCollection<SessionViewModel>();

            this._provider = new NHibernateLogDataProvider();
            this._provider.NewSessionAdded += _newSessionAddedHandler;

            this._provider.Listen();
        }

        private void _newSessionAddedHandler(object sender, SessionEventAddedEventArgs args)
        {
            var sessionViewModel = new SessionViewModel(
                args.SessionEvent.SessionId,
                this._provider);

            sessionViewModel.SessionEvents.Add(
                new SessionEventViewModel(
                    args.SessionEvent.TimeStamp,
                    args.SessionEvent.Message));

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Sessions.Add(sessionViewModel);
                }), DispatcherPriority.Background);
        }

        public ObservableCollection<SessionViewModel> Sessions { get; private set; }

        public SessionViewModel SelectedSession
        {
            get
            {
                return this._selectedSession;
            }
            set
            {
                this._selectedSession = value;
                this.NotifyPropertyChanged("SelectedSession");
            }
        }
    }
}