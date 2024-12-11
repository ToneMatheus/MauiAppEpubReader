using MauiAppEpubReader.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiAppEpubReader.Models
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private readonly HtmlContentService _htmlContentService;
        private readonly MysqlDataStore _mysqlDataStore = new MysqlDataStore();
        private string _title;
        private readonly INavigation _navigation;

        public EditViewModel(HtmlContentService htmlContentService, INavigation navigation/*, MysqlDataStore mysqlDataStore*/)
        {
            _htmlContentService = htmlContentService;
            _navigation = navigation;
            //_mysqlDataStore = mysqlDataStore;
            _htmlContentService.PropertyChanged += HtmlContentService_PropertyChanged;

            AddCommand = new Command(async () => await AddTextSprout());
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

        public ICommand AddCommand { get; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private async Task AddTextSprout()
        {
            var newTextSprout = new TextSprout
            {
                Title = Title, 
                Text = HtmlContent
            };

            await _mysqlDataStore.AddTextSprout(newTextSprout);
            await _navigation.PushAsync(new TextSproutPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
