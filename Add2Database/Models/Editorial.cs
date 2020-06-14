using System.Collections.Generic;

namespace Add2Database.Models
{
    internal sealed class Editorial
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Collection> Collections { get; set; }
        public Editorial( string name)
        {
            Name = name;
            Collections = new List<Collection>();
        }
    }

}
