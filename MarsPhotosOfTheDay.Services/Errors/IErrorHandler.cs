using MarsPhotosOfTheDay.Services.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsPhotosOfTheDay.Services.Errors
{
    public interface IErrorHandler
    {
        Error ValidateDate(DateTime dateTime);
        Task<Error> ValidateHttpResponseAsync(HttpResponseMessage httpResponse);
    }
}
