using Newtonsoft.Json;
using System.Collections.Generic;

namespace DComics.Models
{
    class Comic
    {
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        public string NameWeb { get; set; }
        public string SizeWeb { get; set; }
        public Comic()
        {

        }
        public Comic(int id, string name, string link)
        {
            Id = id;
            Name = name;
            Link = link;
        }
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Link: {2}, NameWeb: {3}", Id, Name, Link, NameWeb);
        }

        public static string Serializer(List<Comic> comics) {
            return JsonConvert.SerializeObject(comics, Formatting.Indented);  
        }
    }
}
