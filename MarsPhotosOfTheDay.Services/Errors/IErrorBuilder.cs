using MarsPhotosOfTheDay.Services.Entities;
using System;

namespace MarsPhotosOfTheDay.Services.Errors
{
    public interface IErrorBuilder
    {
        Error GetDateOutOfRangeError(DateTime date);
        Error GetTimeoutError();
        Error GetBadRequestError(string errorMessage = "");
        Error GetInternalServiceError(string errorMessage = "");
        Error GetUnknownError(string errorMessage = "");
        Error GetApiKeyMissingError();
        Error GetApiKeyInvalidError();
        Error GetOverRateLimitError();
    }
}
