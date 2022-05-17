using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LeisBrasileiras.Mobile.ViewModels
{
    public class InicioViewModel : BaseViewModel
    {
        public InicioViewModel()
        {
            Title = "Início";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}