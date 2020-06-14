using System.Collections.Generic;

namespace Add2Database.Models
{
    internal sealed class Collection
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Comic> Comics { get; set; }
        public Collection(string name)
        {
            Name = name;
            Comics = new List<Comic>();
        }
    }
}
