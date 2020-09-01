using MarsPhotosOfTheDay.Services.Entities;
using System;

namespace MarsPhotosOfTheDay.Services.Errors
{
    public class ErrorBuilder : IErrorBuilder
    {
        private readonly string _dateFormat;
        private readonly string _apiUrl = "https://api.nasa.gov/";

        public ErrorBuilder(string dateFormat = "MMMM dd yyyy")
        {
            _dateFormat = dateFormat;
        }

        public Error GetDateOutOfRangeError(DateTime date)
        {
            var errorMessage = $"Dates is not valid {date.ToString(_dateFormat)}.";
            var apodError = new Error(ErrorCode.DateOutOfRange, errorMessage);
            return apodError;
        }

        public Error GetTimeoutError()
        {
            var errorMessage = "The API timed out.";
            var apodError = new Error(ErrorCode.Timeout, errorMessage);
            return apodError;
        }

        public Error GetBadRequestError(string errorMessage = "")
        {
            var apodError = new Error(ErrorCode.BadRequest, errorMessage);
            return apodError;
        }

        public Error GetInternalServiceError(string errorMessage = "")
        {
            var apodError = new Error(ErrorCode.InternalServiceError, errorMessage);
            return apodError;
        }

        public Error GetUnknownError(string errorMessage = "")
        {
            var fullErrorMessage = $"{errorMessage}. issue Unknown.";
            var apodError = new Error(ErrorCode.Unknown, fullErrorMessage);
            return apodError;
        }

        public Error GetApiKeyMissingError()
        {
            var errorMessage = $"You must provide an API key. Get one at {_apiUrl}.";
            var apodError = new Error(ErrorCode.ApiKeyMissing, errorMessage);
            return apodError;
        }

        public Error GetApiKeyInvalidError()
        {
            var errorMessage = $"The API key you provided was invalid. Get one at {_apiUrl}.";
            var apodError = new Error(ErrorCode.ApiKeyInvalid, errorMessage);
            return apodError;
        }

        public Error GetOverRateLimitError()
        {
            // If/when this library provides caching in the future, information about that should be added here.
            var errorMessage = $"You have exceeded your rate limit. Try again later or go to {_apiUrl}/contact/ for assistance.";
            var apodError = new Error(ErrorCode.OverRateLimit, errorMessage);
            return apodError;
        }
    }
}
