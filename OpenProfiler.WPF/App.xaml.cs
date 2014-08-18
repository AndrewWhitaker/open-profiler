namespace OpenProfiler.WPF
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using OpenProfiler.WPF.ViewModels;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new MainWindow();

            var viewModel = new MainWindowViewModel();

            window.DataContext = viewModel;
            window.Show();
        }
    }
}
