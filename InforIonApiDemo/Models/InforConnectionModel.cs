namespace InforIonApiDemo.Models
{
    // InforConnectionRoot myDeserializedClass = JsonConvert.DeserializeObject<InforConnectionRoot>(myJsonResponse);
    public class InforConnection
    {
        public string ti { get; set; }
        public string cn { get; set; }
        public string dt { get; set; }
        public string ci { get; set; }
        public string cs { get; set; }
        public string iu { get; set; }
        public string pu { get; set; }
        public string oa { get; set; }
        public string ot { get; set; }
        public string or { get; set; }
        public string ev { get; set; }
        public string v { get; set; }
        public string saak { get; set; }
        public string sask { get; set; }
    }

    public class InforConnectionRoot
    {
        public InforConnection InforConnection { get; set; }
        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        public string MongooseEnvironment { get; set; }
    }

}