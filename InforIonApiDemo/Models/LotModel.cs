namespace InforIonApiDemo.Models;

public class Lots
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public string Lot { get; set; }
    public string Item { get; set; }
    public string Description { get; set; }
    public string _ItemId { get; set; }
}

public class LotsRoot
{
    public List<Lots> Lots { get; set; }
}