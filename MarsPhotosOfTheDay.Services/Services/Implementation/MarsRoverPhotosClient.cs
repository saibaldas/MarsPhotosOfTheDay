using MarsPhotosOfTheDay.Services.Entities;
using MarsPhotosOfTheDay.Services.Errors;
using MarsPhotosOfTheDay.Services.Interfaces;
using MarsPhotosOfTheDay.Services.Net;
using MarsPhotosOfTheDay.Services.Payload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarsPhotosOfTheDay.Services.Services.Implementation
{
    public class MarsRoverPhotosClient : IMarsRoverPhotosClient, IDisposable
    {
        private bool _disposed;
        private readonly IHttpRequester _httpRequester;
        private readonly IHttpResponseParser _httpResponseParser;
        private readonly IErrorHandler _errorHandler;
        private string apiKey = "DEMO_KEY";

        public MarsRoverPhotosClient(IHttpRequester httpRequester, IHttpResponseParser httpResponseParser, IErrorHandler errorHandler)
        {
            _httpRequester = httpRequester;
            _httpResponseParser = httpResponseParser;
            _errorHandler = errorHandler;
        }

        public async Task<Response> FetchMarsRoverPhotosOfTheDayAsync(DateTime dateTime)
        {
            ThrowExceptionIfDisposed();

            var dateError = _errorHandler.ValidateDate(dateTime);
            if (dateError.ErrorCode != ErrorCode.None) { return dateError.ToResponse(); }

            var httpResponse = await _httpRequester.SendHttpRequestAsync(dateTime).ConfigureAwait(false);

            //var responseError = await _errorHandler.ValidateHttpResponseAsync(httpResponse).ConfigureAwait(false);
            //if (responseError.ErrorCode != ErrorCode.None) { return responseError.ToResponse(); }

            return await _httpResponseParser.ParseMarsRoverPhotosOfTheDayAsync(httpResponse).ConfigureAwait(false);

        }

        private void ThrowExceptionIfDisposed()
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }
        }

        public void Dispose()
        {
            if (_disposed) { return; }
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
