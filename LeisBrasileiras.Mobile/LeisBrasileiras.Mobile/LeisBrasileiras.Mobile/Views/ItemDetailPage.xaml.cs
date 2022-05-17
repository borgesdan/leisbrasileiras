using LeisBrasileiras.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LeisBrasileiras.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}