using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsPhotosOfTheDay.Services.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MarsPhotosOfTheDay.Services.Interfaces;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;

namespace MarsPhotosOfTheDay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarsPhotosOfTheDayController : ControllerBase
    {
        private readonly ILogger<MarsPhotosOfTheDayController> _logger;
        private readonly IMarsRoverPhotosClient _marsRoverPhotosClient;

        public MarsPhotosOfTheDayController(ILogger<MarsPhotosOfTheDayController> logger,
                                            IMarsRoverPhotosClient marsRoverPhotosClient)
        {
            _logger = logger;
            _marsRoverPhotosClient = marsRoverPhotosClient;
        }

        [HttpGet]
        [Route("urls")]
        public async Task<IActionResult> Urls(string date)
        {
            DateTime dateRequested;
            if (DateTime.TryParse(date, out dateRequested))
            {
                var response = await _marsRoverPhotosClient.FetchMarsRoverPhotosOfTheDayAsync(dateRequested);

                if (response.Photos != null)
                {
                    return Ok(response.Photos.photos.Select(photo => new Photo
                    {
                        id = photo.id,
                        img_src = photo.img_src
                    })
                    .ToArray());
                }
                else
                {
                    return Ok("No Photo found");
                }
            }
            else
            {
                return NotFound("Invalid date");
            }
        }

        [HttpGet]
        [Route("downloadaszip")]
        public async Task<byte[]> DownloadAsZip(string date)
        {
            DateTime dateRequested;
            if (DateTime.TryParse(date, out dateRequested))
            {
                var response = await _marsRoverPhotosClient.FetchMarsRoverPhotosOfTheDayAsync(dateRequested);
                var client = new HttpClient();
                var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        response.Photos.photos.ForEach(async photo =>
                        {
                            var responseImage = await client.GetAsync(photo.img_src);
                            using (var streamReader = await responseImage.Content.ReadAsStreamAsync())
                            {
                                var theFile = archive.CreateEntry(photo.img_src);
                                using (var streamWriter = new StreamWriter(theFile.Open()))
                                {
                                    streamWriter.Write(streamReader);
                                }
                            }
                        });
                    }
                    return memoryStream.ToArray();
                }
            }
            else
            {
                return new byte[1];
            }
        }

        [HttpGet]
        [Route("download")]
        public FileContentResult DownloadOneImage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                return new FileContentResult(data, "image/jpg");
                /*
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    using (var imageStream = Image.FromStream(memoryStream))
                    {
                        imageStream.Save(memoryStream, ImageFormat.Jpeg);
                    }
                    var fileContents = memoryStream.ToArray();
                    return new FileContentResult(fileContents, "image/jpg");
                }
                */
            }           
        }
    }
}
