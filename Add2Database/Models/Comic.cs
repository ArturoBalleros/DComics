using Newtonsoft.Json;

namespace Add2Database.Models
{
    internal sealed class Comic
    {
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        public Comic()
        {

        }
        public Comic(string name, string link)
        {
            Name = name;
            Link = link;
        }
        public override string ToString()
        {
            return string.Format("Name: {0}, Link: {1}", Name, Link);
        }

    }
}
