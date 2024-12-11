using MauiAppEpubReader.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiAppEpubReader.Models
{
    public class TextSproutDetailViewModel : INotifyPropertyChanged
    {
        private readonly MysqlDataStore _mysqlDataStore;
        private TextSprout _textSprout;
        private readonly MysqlDataStore _mysqlDataStore_ = new MysqlDataStore();

        public TextSproutDetailViewModel(TextSprout textSprout)
        {
            _textSprout = textSprout;
            DelCommand = new Command(async () => await DeleteTextSprout());
            EditCommand = new Command(async () => await EditTextSprout());
        }

        public ICommand DelCommand { get; }
        public ICommand EditCommand { get; }

        private async Task DeleteTextSprout()
        {
            await _mysqlDataStore_.DeleteTextSprout(_textSprout.Id);
            // Navigate back or show a confirmation message
        }

        private async Task EditTextSprout()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
