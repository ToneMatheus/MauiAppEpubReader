using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiAppEpubReader.Models
{
    public class MainViewModel
    {
        public ICommand CloseCommand { get; }

        public MainViewModel()
        {
            CloseCommand = new Command(CloseApplication);
        }

        private void CloseApplication()
        {
            MauiProgram.CloseApplication();
        }
    }
}
