using MarsPhotosOfTheDay.Services.Payload;

namespace MarsPhotosOfTheDay.Services.Entities
{
    public class Error
    {
        public readonly ErrorCode ErrorCode;

        public readonly string ErrorMessage;

        public Error(ErrorCode errorCode, string errorMessage = "")
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public Response ToResponse()
            => new Response(StatusCode.Error, error: this);
    }
}
