using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        
        }
       

        private async void Button_Clicked(object sender, EventArgs e)
        {
      
            // ...

            if (string.IsNullOrEmpty(txtCountry.Text) || !Regex.IsMatch(txtCountry.Text, @"^[a-zA-Z]+$"))
            {
                await DisplayAlert("Information", "Enter a valid City", "Ok", "Cancel");
                return;
            }

            Helpers.Settings.txtname = txtCountry.Text;
            
            await   Navigation.PushAsync(new MainPage());
        }
    }
}