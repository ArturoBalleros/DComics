using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
