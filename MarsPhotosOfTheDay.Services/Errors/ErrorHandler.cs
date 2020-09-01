using MarsPhotosOfTheDay.Services.Entities;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarsPhotosOfTheDay.Services.Errors
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IErrorBuilder _errorBuilder;
        private readonly DateTime _firstValidDate;
        private readonly DateTime _lastValidDate;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ErrorHandler(IErrorBuilder errorBuilder, DateTime firstValidDate = default, DateTime lastValidDate = default)
        {
            _errorBuilder = errorBuilder;
            _firstValidDate = firstValidDate == default ? GetDefaultFirstValidDate() : firstValidDate;
            _lastValidDate = lastValidDate == default ? GetDefaultLastValidDate() : lastValidDate;
            _jsonSerializerOptions = GetDefaultJsonSerializerOptions();
        }

        private DateTime GetDefaultFirstValidDate() 
            => new DateTime(1995, 06, 16);

        private DateTime GetDefaultLastValidDate() 
            => DateTime.Today;

        private JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }

        public Error ValidateDate(DateTime dateTime)
        {
            if (!DateIsInRange(dateTime)) { return _errorBuilder.GetDateOutOfRangeError(dateTime); }
            return new Error(ErrorCode.None);
        }

        public async Task<Error> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode) { return new Error(ErrorCode.None); }

            if (IsTimeoutError(httpResponse)) { return _errorBuilder.GetTimeoutError(); }

            JsonElement errorObject = default;
            using (var responseStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                errorObject = await JsonSerializer.DeserializeAsync<JsonElement>(responseStream, _jsonSerializerOptions).ConfigureAwait(false);
            }

            if (errorObject.ValueKind != JsonValueKind.Object) { return _errorBuilder.GetUnknownError(); }

            if (ErrorHasServiceVersionProperty(errorObject))
            {
                var code = int.Parse(errorObject.GetProperty("code").ToString());
                var message = errorObject.GetProperty("msg").ToString();

                switch (code)
                {
                    case 400: return _errorBuilder.GetBadRequestError(message);
                    case 500: return _errorBuilder.GetInternalServiceError(message);
                    default:  return _errorBuilder.GetUnknownError(message);
                }
            }
            
            var hasError = errorObject.TryGetProperty("error", out var error);
            if (!hasError) { return _errorBuilder.GetUnknownError(); }

            var errorCode = error.GetProperty("code").ToString();
            var apodErrorCode = GetErrorCode(errorCode);

            var errorMessage = error.GetProperty("message").ToString();

            switch (apodErrorCode)
            {
                case ErrorCode.ApiKeyMissing: return _errorBuilder.GetApiKeyMissingError();
                case ErrorCode.ApiKeyInvalid: return _errorBuilder.GetApiKeyInvalidError();
                case ErrorCode.OverRateLimit: return _errorBuilder.GetOverRateLimitError();
                default:                      return _errorBuilder.GetUnknownError(errorMessage);
            }
        }

        private ErrorCode GetErrorCode(string errorCode)
        {
            switch (errorCode)
            {
                case "API_KEY_MISSING": return ErrorCode.ApiKeyMissing;
                case "API_KEY_INVALID": return ErrorCode.ApiKeyInvalid;
                case "OVER_RATE_LIMIT": return ErrorCode.OverRateLimit;
                default:                return ErrorCode.Unknown;
            }
        }

        private bool ErrorHasServiceVersionProperty(JsonElement errorObject)
            => errorObject.TryGetProperty("service_version", out var _);

        // If the application times out, it returns html content instead of json.
        private bool IsTimeoutError(HttpResponseMessage httpResponse)
            => httpResponse.Content.Headers.ContentType.ToString().Contains("text/html");

        /// <summary>
        /// Checks if the <paramref name="dateTime"/> is between the first valid date and the last valid date (inclusive).
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to check.</param>
        /// <returns>
        /// Whether or not the <paramref name="dateTime"/> is within the allowed range.
        /// </returns>
        private bool DateIsInRange(DateTime dateTime)
            => (DateTime.Compare(dateTime, _lastValidDate.AddDays(1)) < 0) // The date is before or equal to the last valid date
            && (DateTime.Compare(dateTime, _firstValidDate.AddDays(-1)) > 0); // The date is after or equal to the first valid date

        private bool CountIsInRange(int count)
            => count > 0 && count <= 100;
    }
}
