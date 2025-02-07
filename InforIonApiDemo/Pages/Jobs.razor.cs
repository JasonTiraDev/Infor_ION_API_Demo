using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InforIonApiDemo.Models; // Ensure your models (Jobs, JobsRoot, etc.) are in this namespace
using Microsoft.AspNetCore.Components;

namespace InforIonApiDemo.Pages
{
    public partial class Jobs
    {
        // A HashSet to keep track of selected jobs.
        private HashSet<Models.Jobs> selectedJobs = new HashSet<Models.Jobs>();
        private bool _selectOnRowClick = true;

        // These fields will be used for the fetched data.
        private Models.Jobs jobs;
        private Models.JobsRoot jobsRoot;
        private IEnumerable<Models.Jobs> jobEnum = new List<Models.Jobs>();

        private InforConnection? inforConnection = new InforConnection();
        private static ProgramSettingsModel? settings = new ProgramSettingsModel();

        TokenService tokenService = new TokenService();
        JobGet jobGet = new JobGet();

        // Sample data to display if no data is returned.
        private IEnumerable<Models.Jobs> sampleData = new List<Models.Jobs>
        {
            new Models.Jobs
            {
                Item = "Sample Job 1",
                Suffix = "A",
                MOJobDescription = "This is sample job 1",
                _ItemId = "J001"
            },
            new Models.Jobs
            {
                Item = "Sample Job 2",
                Suffix = "B",
                MOJobDescription = "This is sample job 2",
                _ItemId = "J002"
            },
            new Models.Jobs
            {
                Item = "Sample Job 3",
                Suffix = "C",
                MOJobDescription = "This is sample job 3",
                _ItemId = "J003"
            }
        };

        // Search string bound to the search field.
        private string searchString = "";

        // Filtered jobs based on search input.
        private IEnumerable<Models.Jobs> FilteredJobs => string.IsNullOrWhiteSpace(searchString)
            ? jobEnum
            : jobEnum.Where(x => x.Item.Contains(searchString, System.StringComparison.OrdinalIgnoreCase));

        // On page load
        protected override async Task OnInitializedAsync()
        {
            // Get infor connection information from JSON file.
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

            // Get settings information from JSON file.
            string settingsText = File.ReadAllText(@"./Settings.json");
            settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);

            // If connection info is available (ci is not null) then attempt to get token and jobs.
            if (inforConnection?.ci != null)
            {
                // Get token using the token service.
                TokenResponse token = await tokenService.GetToken();
                if (!token.IsError)
                {
                    Data.JobGet jobGet = new Data.JobGet();
                    // Get jobs using the token.
                    jobsRoot = await jobGet.GetJobs(token);
                    // If jobs are returned, convert them to a list; otherwise, use sample data.
                    jobEnum = (jobsRoot.Items != null && jobsRoot.Items.Any())
                                ? jobsRoot.Items.ToList()
                                : sampleData;
                }
                else
                {
                    // If token retrieval failed, fall back to sample data.
                    jobEnum = sampleData;
                }
            }
            else
            {
                // If connection info is missing, fall back to sample data.
                jobEnum = sampleData;
            }
        }
    }
}
