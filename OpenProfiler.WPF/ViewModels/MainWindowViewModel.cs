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
        private readonly NHibernateLogDataProvider provider;
        private SessionViewModel selectedSession;

        public MainWindowViewModel()
        {
            this.Sessions = new ObservableCollection<SessionViewModel>();

            this.provider = new NHibernateLogDataProvider();
            this.provider.NewSessionAdded += this.NewSessionAddedHandler;

            this.provider.Listen();
        }

        public ObservableCollection<SessionViewModel> Sessions { get; private set; }

        public SessionViewModel SelectedSession
        {
            get
            {
                return this.selectedSession;
            }

            set
            {
                this.selectedSession = value;
                this.NotifyPropertyChanged("SelectedSession");
            }
        }

        private void NewSessionAddedHandler(object sender, SessionEventAddedEventArgs args)
        {
            var sessionViewModel = new SessionViewModel(
                args.SessionEvent.SessionId,
                this.provider);

            sessionViewModel.SessionEvents.Add(
                new SessionEventViewModel(
                    args.SessionEvent.TimeStamp,
                    args.SessionEvent.Message));

            App.Current.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    Sessions.Add(sessionViewModel);
                }), 
                DispatcherPriority.Background);
        }
    }
}