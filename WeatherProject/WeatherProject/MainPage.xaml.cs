using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WeatherProject.Models;
using Newtonsoft.Json;
using WeatherProject.Views;
using WeatherProject.Helpers;
namespace WeatherProject
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage()
        {
            InitializeComponent();
            GetWeatherInfo();
            
        }
        private string _location;
        public string location {

            get
            {
                if (string.IsNullOrEmpty(_location))
                {
                    _location = Helpers.Settings.txtname;
                }

                return _location;
            }
            set { _location = value; }

        } 



        private async void GetWeatherInfo()
        {

            //string url = "https://api.openweathermap.org/data/2.5/weather?q={location},&appid=4ccc668d10ab90efba3683f6d367d5cd";
            string url = "https://api.openweathermap.org/data/2.5/weather?q=" + location + "&appid=4ccc668d10ab90efba3683f6d367d5cd";

            WeatherInfo oWeatherInfo = new WeatherInfo();
            string json = await Helpers.Utility.CallWebApi(url);

            oWeatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(json);
            if (oWeatherInfo != null)
            {
                try
                {

                    descriptionTxt.Text = oWeatherInfo.weather[0].description.ToUpper();
                    cityTxt.Text = oWeatherInfo.name.ToUpper();
                    float c = 273;
                    float kelvin = float.Parse(oWeatherInfo.main.temp.ToString("0"));// this format to decimal round
                    float temperatureKelvin = kelvin - c;
                    temperatureTxt.Text = temperatureKelvin.ToString();
                    humidityTxt.Text = oWeatherInfo.main.humidity.ToString();
                    pressureTxt.Text = $"{oWeatherInfo.main.pressure} hpa";
                    windTxt.Text = $"{oWeatherInfo.wind.speed} m/s";
                    cloudinessTxt.Text = $"{oWeatherInfo.clouds.all}%";

                    var dt = DateTimeOffset.FromUnixTimeSeconds(oWeatherInfo.dt).ToLocalTime().Date;
                    dateTxt.Text = dt.ToString("dddd, MMM dd", CultureInfo.InvariantCulture).ToUpper();




                    GetForecast();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No weather information found", "OK");
            }
        }

        private async void GetForecast()
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast?q=sohag&appid=4ccc668d10ab90efba3683f6d367d5cd";
            ForecastInfo oForecastInfo = new ForecastInfo();
            string json = await Helpers.Utility.CallWebApi(url);

            oForecastInfo = JsonConvert.DeserializeObject<ForecastInfo>(json);

            if (oForecastInfo != null)
            {
                try
                {


                    List<List> allList = new List<List>();

                    foreach (var list in oForecastInfo.list)
                    {

                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }
                    float c = 273;

                    dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd");
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM");
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}.png";
                    tempOneTxt.Text = allList[0].main.temp.ToString("0");
                    float kelvin = float.Parse(allList[0].main.temp.ToString("0"));// this format to decimal round
                    float temperatureKelvin = kelvin - c;
                    tempOneTxt.Text = temperatureKelvin.ToString();

                    dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd");
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM");
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}.png";
                    kelvin = float.Parse(allList[1].main.temp.ToString("0"));// this format to decimal round
                    temperatureKelvin = kelvin - c;
                    tempTwoTxt.Text = temperatureKelvin.ToString();


                    dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd");
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM");
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}.png ";
                    kelvin = float.Parse(allList[2].main.temp.ToString("0"));
                    temperatureKelvin = kelvin - c;
                    tempThreeTxt.Text = temperatureKelvin.ToString();


                    dayFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd");
                    dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM");
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}.png";

                    tempFourTxt.Text = allList[3].main.temp.ToString("0");
                    temperatureKelvin = kelvin - c;
                    tempFourTxt.Text = temperatureKelvin.ToString();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
          await  Navigation.PopAsync();
        }
    }
}
