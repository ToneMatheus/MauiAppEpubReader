using MauiAppEpubReader.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiAppEpubReader.Models
{
    public class TextSproutDetailViewModel : INotifyPropertyChanged
    {
        private bool _isEditVisible;
        private TextSprout _textSprout;
        private readonly MysqlDataStore _mysqlDataStore_ = new MysqlDataStore();
        private readonly INavigation _navigation;

        public TextSproutDetailViewModel(TextSprout textSprout, INavigation navigation)
        {
            _textSprout = textSprout;
            _navigation = navigation;
            DelCommand = new Command(async () => await DeleteTextSprout());
            EditCommand = new Command(OnEdit);
            SaveCommand = new Command(async () => await EditTextSprout());
            IsEditVisible = false;
        }

        public string Text
        {
            get => _textSprout.Text;
            set
            {
                _textSprout.Text = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _textSprout.Title;
            set
            {
                _textSprout.Title = value;
                OnPropertyChanged();
            }
        }

        public ICommand DelCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }

        public bool IsEditVisible
        {
            get => _isEditVisible;
            set
            {
                _isEditVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLabelVisible));
            }
        }

        public bool IsLabelVisible => !IsEditVisible;

        private async Task DeleteTextSprout()
        {
            await _mysqlDataStore_.DeleteTextSprout(_textSprout.Id);
            MessagingCenter.Send(this, "DeleteTextSprout", _textSprout);
            await _navigation.PopAsync();
        }
        private void OnEdit()
        {
            IsEditVisible = !IsEditVisible;
        }

        private async Task EditTextSprout()
        {
            var updatedTextSprout = new TextSprout
            {
                Id = _textSprout.Id,
                Title = Title,
                Text = Text
            };
            await _mysqlDataStore_.EditTextSprout(_textSprout.Id, updatedTextSprout);
            MessagingCenter.Send(this, "EditTextSprout", updatedTextSprout);
            await _navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
