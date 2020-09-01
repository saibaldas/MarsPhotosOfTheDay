using MarsPhotosOfTheDay.Services.Entities;
using MarsPhotosOfTheDay.Services.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsPhotosOfTheDay.Services.Interfaces
{
    public interface IMarsRoverPhotosClient
    {
        Task<Response> FetchMarsRoverPhotosOfTheDayAsync(DateTime dateTime);
    }
}
