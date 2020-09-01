namespace MarsPhotosOfTheDay.Services.Entities
{
    public enum ErrorCode
    {
        None,

        BadRequest,

        DateOutOfRange,

        InternalServiceError,

        ApiKeyMissing,

        ApiKeyInvalid,

        Timeout,

        OverRateLimit,

        Unknown
    }
}
