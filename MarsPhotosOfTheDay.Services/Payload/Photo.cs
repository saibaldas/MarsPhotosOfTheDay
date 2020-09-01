using System;
using System.Text.Json.Serialization;

namespace MarsPhotosOfTheDay.Services.Payload
{
    public class Photo : IEquatable<Photo>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sol")]
        public string Sol { get; set; }

        [JsonPropertyName("img_src")]
        public string ImageSrc { get; set; }

        [JsonPropertyName("earth_date")]
        public DateTime EarthDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Photo content)
            {
                return Equals(content);
            }

            return false;
        }

        public static bool operator ==(Photo a, Photo b)
        {
            if (ReferenceEquals(a, b)) { return true; }
            if (a is null) { return false; }
            return a.Equals(b);
        }

        public static bool operator !=(Photo a, Photo b)
            => !(a == b);

        public bool Equals(Photo other)
            => other is object
            && (Id, Sol, EarthDate, ImageSrc)
            .Equals((other.Id, other.Sol, other.EarthDate, other.ImageSrc));

        public override int GetHashCode()
            => (Id, Sol, EarthDate, ImageSrc).GetHashCode();
    }
}
