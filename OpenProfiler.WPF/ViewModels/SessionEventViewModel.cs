namespace OpenProfiler.WPF.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class SessionEventViewModel : ViewModelBase
    {
        public SessionEventViewModel(DateTime timeStamp, string message)
        {
            this.TimeStamp = timeStamp;
            this.Message = message;
        }

        public DateTime TimeStamp { get; private set; }

        public string Message { get; private set; }

        public string MessagePreview
        {
            get
            {
                return Regex.Replace(this.Message, @"\s+", " ").Trim();
            }
        }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
