using MauiAppEpubReader.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppEpubReader.Models.ConvertViewModel
{
    public class ConvertViewModel : INotifyPropertyChanged
    {
        private readonly HtmlContentService _htmlContentService;

        public ConvertViewModel(HtmlContentService htmlContentService)
        {
            _htmlContentService = htmlContentService;
        }

        public string HtmlContent
        {
            get => _htmlContentService.HtmlContent;
            set
            {
                _htmlContentService.HtmlContent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
