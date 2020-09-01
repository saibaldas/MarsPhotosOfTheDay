using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsPhotosOfTheDay.Services.Net
{
    public interface IHttpRequester //: IDisposable
    {
        Task<HttpResponseMessage> SendHttpRequestAsync();
        Task<HttpResponseMessage> SendHttpRequestAsync(DateTime dateTime);
    }
}
