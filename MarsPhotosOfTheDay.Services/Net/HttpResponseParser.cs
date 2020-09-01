using MarsPhotosOfTheDay.Services.Net;
using MarsPhotosOfTheDay.Services.Entities;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MarsPhotosOfTheDay.Services.Payload;
using System.Text;
using System.IO;

namespace MarsPhotosOfTheDay.Services.Net
{
    public class HttpResponseParser : IHttpResponseParser
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpResponseParser()
        {
            _jsonSerializerOptions = GetDefaultJsonSerializerOptions();
        }

        private JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }

        public async Task<Response> ParseMarsRoverPhotosOfTheDayAsync(HttpResponseMessage httpResponse)
        {
            Dto.Root photos = null;
            /*
            var responseContent = string.Empty;

            var responseStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var encoding = Encoding.UTF8;

            using (var sr = new StreamReader(responseStream, encoding))
            {
                responseContent = await sr.ReadToEndAsync().ConfigureAwait(false);
            }
            */
            using (var responseStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                photos = await JsonSerializer.DeserializeAsync<Dto.Root>(responseStream, _jsonSerializerOptions).ConfigureAwait(false);
            }

            httpResponse.Dispose();

            return new Response(StatusCode.OK, photos);
        }
    }
}
