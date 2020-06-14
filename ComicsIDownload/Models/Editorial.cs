using Newtonsoft.Json;

namespace ComicsIDownload.Models
{
    class Editorial
    {
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        public Editorial()
        {
        }
    }
}
