using MarsPhotosOfTheDay.Services.Payload;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsPhotosOfTheDay.Services.Net
{
    public interface IHttpResponseParser
    {
        Task<Response> ParseMarsRoverPhotosOfTheDayAsync(HttpResponseMessage httpResponse);
    }
}
