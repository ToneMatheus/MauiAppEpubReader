using MauiAppEpubReader.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppEpubReader.Models
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private readonly HtmlContentService _htmlContentService;

        public EditViewModel(HtmlContentService htmlContentService)
        {
            _htmlContentService = htmlContentService;
            _htmlContentService.PropertyChanged += HtmlContentService_PropertyChanged;
        }

        private void HtmlContentService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HtmlContentService.HtmlContent))
            {
                OnPropertyChanged(nameof(HtmlContent));
            }
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
