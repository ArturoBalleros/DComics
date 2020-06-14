namespace CheckHealthLinks.Models
{
    internal sealed class Comic
    {
        public string Id { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }
        public Comic(string id, string link)
        {
            Id = id;
            Link = link;
        }
    }
}
