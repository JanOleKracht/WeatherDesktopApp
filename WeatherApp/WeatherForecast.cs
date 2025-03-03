using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class WeatherForecast
    {
        public ForecastData main;
        public string dt_txt;

        public List<Weather> weather;

        public class ForecastData
        {
            public double temp;
        }
    }
}