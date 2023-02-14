namespace InforIonApiDemo.Models
{
    public class JobRouteResultsModel
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public List<string> Parameters { get; set; }
        public string ReturnValue { get; set; }

    }
}
