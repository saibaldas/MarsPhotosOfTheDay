using MarsPhotosOfTheDay.Services.Net;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MarsPhotosOfTheDay.UnitTests
{
    public class HttpResponseParserTest
    {
        private string _exampleContent => @"{""photos"": [{""id"": 102685, ""sol"": 1004, ""camera"": { ""id"": 20, ""name"": ""FHAZ"", ""rover_id"": 5, ""full_name"": ""Front Hazard Avoidance Camera"" }, ""img_src"": ""http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01004/opgs/edr/fcam/FLB_486615455EDR_F0481570FHAZ00323M_.JPG"", ""earth_date"": ""2015-06-03"", ""rover"": { ""id"": 5,""name"": ""Curiosity"", ""landing_date"": ""2012-08-06"", ""launch_date"": ""2011-11-26"", ""status"": ""active"" }}]}";

        [Fact]
        public async Task ParseAsync_HttpResponseIsDisposed()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_exampleContent, Encoding.UTF8, "application/json")
            };

            await httpResponseParser.ParseMarsRoverPhotosOfTheDayAsync(input);

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await input.Content.ReadAsStringAsync());
        }
    }
}
