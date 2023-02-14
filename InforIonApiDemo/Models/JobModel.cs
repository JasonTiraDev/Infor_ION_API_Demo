
namespace InforIonApiDemo.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    
    public class Jobs
    {
        public string Job { get; set; }
        public string Suffix { get; set; }
        public string ReleaseDate { get; set; }
        public string _ItemId { get; set; }
        public string Item { get; set; }
    }

    public class JobRoot
    {
        public List<Jobs> Items { get; set; }
        public string Bookmark { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
