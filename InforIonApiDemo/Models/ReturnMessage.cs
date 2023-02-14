namespace InforIonApiDemo.Models
{
    public class ReturnMessage
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public List<string> Parameters { get; set; }
        public string ReturnValue { get; set; }
    }
}
