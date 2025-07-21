using System;
using System.IO;
using System.Reflection;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ConnectApp.Extensions
{
    [ContentProperty(nameof(Source))]
    public class WebResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null) { return null; }

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Source);
            var html = new StreamReader(stream).ReadToEnd();
            return new HtmlWebViewSource()
            {
                Html = html
            };
        }
    }
}
