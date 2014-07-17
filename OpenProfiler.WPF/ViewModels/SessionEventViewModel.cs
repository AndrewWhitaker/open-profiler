namespace OpenProfiler.WPF.ViewModels
{
    using System;
    using System.Text.RegularExpressions;
    using OpenProfiler.WPF.Utils;
    using SqlFormatter;

    public class SessionEventViewModel : ViewModelBase
    {
        private string message;
        private string formatted;

        public SessionEventViewModel(DateTime timeStamp, string message)
        {
            this.TimeStamp = timeStamp;
            this.message = message.Trim();
        }

        public DateTime TimeStamp { get; private set; }

        public string FormattedSql
        {
            get
            {
                if (this.formatted == null)
                {
                    this.formatted = SqlStringUtils.Format(this.message);
                }

                return this.formatted;
            }
        }

        public string MessagePreview
        {
            get
            {
                return Regex.Replace(this.message, @"\s+", " ").Trim();
            }
        }

        public override string ToString()
        {
            return this.message;
        }
    }
}
