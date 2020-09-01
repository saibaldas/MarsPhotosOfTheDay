using MarsPhotosOfTheDay.Services.Entities;

namespace MarsPhotosOfTheDay.Services.Payload
{
    public class Response
    {
        public readonly StatusCode StatusCode;

        public readonly Dto.Root Photos;

        public readonly Error Error;

        public Response(StatusCode statusCode, Dto.Root photos = null, Error error = null)
        {
            StatusCode = statusCode;
            Photos = photos;
            Error = error;
        }
    }
}
