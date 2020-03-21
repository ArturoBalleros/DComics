using System;

namespace ReadDCInfo.Models
{
    class Issue
    {
        public int Id { get; set; }//PK por defecto
        public string NameDC { get; set; }
        public string NameLibrary { get; set; }
        public int Number { get; set; }
        public int PageCount { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsOneShot { get; set; }
        public string LinkImage { get; set; }
    }
}
