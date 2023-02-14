using static MudBlazor.CategoryTypes;

namespace InforIonApiDemo.Models
{
    public class BGTaskHistory
    {
        public class BackgroundTask
        {
            public string TaskName { get; set; }
            public string TaskTypeCode { get; set; }
            public string SubmissionDate { get; set; }
            public string TaskParm { get; set; }
            public string RowPointer { get; set; }
            public string _ItemId { get; set; }
        }

        public class BackgroundTaskRoot
        {
            public List<BackgroundTask> Items { get; set; }
            public string Bookmark { get; set; }
            public bool Success { get; set; }
            public object Message { get; set; }
        }


    }
}
