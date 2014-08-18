namespace OpenProfiler.WPF
{
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Xml;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ConfigureSyntaxHighlighting();
        }

        private void ConfigureSyntaxHighlighting()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream s = assembly.GetManifestResourceStream("OpenProfiler.WPF.AvalonEdit.SQL.vshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    this.textEditor.SyntaxHighlighting =
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }
    }
}
