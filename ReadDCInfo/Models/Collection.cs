namespace ReadDCInfo.Models
{
    class Collection
    {
        public int Id { get; set; }//PK por defecto
        public string NameDC { get; set; }
        public string NameLibrary { get; set; }
        public bool InProgress{ get; set; }
        public bool IsDelete { get; set; }
    }
}
