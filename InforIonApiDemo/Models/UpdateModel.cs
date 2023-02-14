namespace InforIonApiDemo.Models
{
    public class UpdateModelChange
    {
        public int Action { get; set; }
        public string ItemId { get; set; }
        public List<UpdateModelProperty> Properties { get; set; }
    }

    public class UpdateModelProperty
    {
        public bool IsNull { get; set; }
        public bool Modified { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class UpdateModelRoot
    {
        public List<UpdateModelChange> Changes { get; set; }
    }
}
