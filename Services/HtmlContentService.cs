using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppEpubReader.Services
{
    public class HtmlContentService : INotifyPropertyChanged
    {
        private string htmlContent;
        public string HtmlContent
        {
            get => htmlContent;
            set
            {
                if (htmlContent != value)
                {
                    htmlContent = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
