using System;

namespace MarsPhotosOfTheDay.Services
{
    public interface IUriBuilder
    {
        string GetUri();
        string GetUri(DateTime dateTime);
    }
}
