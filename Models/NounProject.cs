using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExerciseTracker.Models
{
    public class NounProjectResponse
    {
        [JsonPropertyName("icons")]
        public List<NounProjectIcon> Icons { get; set; }
    }

    public class NounProjectIcon
    {
        [JsonPropertyName("term")]
        public string Term { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
    }

}
