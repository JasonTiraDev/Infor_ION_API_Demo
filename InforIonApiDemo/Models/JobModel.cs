
namespace InforIonApiDemo.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    
    public class Jobs
    {
        public string Job { get; set; }
        public string Suffix { get; set; }
        public string MOJobDescription { get; set; }
        public string _ItemId { get; set; }
        public string Item { get; set; }
    }

    public class JobsRoot
    {
        public List<Jobs> Items { get; set; }
        public string Bookmark { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
