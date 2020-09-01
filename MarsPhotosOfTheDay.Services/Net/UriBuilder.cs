using System;
using System.Text;

namespace MarsPhotosOfTheDay.Services
{
    public class UriBuilder : IUriBuilder
    {
        private readonly string _apiKey;
        private readonly string _baseUri;
        private readonly string _dateFormat;

        public UriBuilder(string apiKey, string baseUri = "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos", string dateFormat = "yyyy-MM-dd")
        {
            _apiKey = apiKey;
            _baseUri = baseUri;
            _dateFormat = dateFormat;
        }

        public string GetUri() 
            => BuildUri();

        public string GetUri(DateTime dateTime) 
            => BuildUri($"earth_date={dateTime.ToString(_dateFormat)}");

        private string BuildUri(params string[] queryParameters)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(_baseUri).Append("?api_key=").Append(_apiKey);

            foreach (var parameter in queryParameters)
            {
                if (string.IsNullOrWhiteSpace(parameter)) { continue; }
                stringBuilder.Append("&");
                stringBuilder.Append(parameter);
            }

            return stringBuilder.ToString();
        }
    }
}
