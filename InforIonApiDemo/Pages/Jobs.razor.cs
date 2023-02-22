namespace InforIonApiDemo.Pages;

public partial class Jobs
{
    private HashSet<Models.Jobs> selectedJobs = new HashSet<Models.Jobs>();
    private bool _selectOnRowClick = true;
    private Models.Jobs jobs;
    private Models.JobsRoot jobsRoot;
    private IEnumerable<Models.Jobs> jobEnum = new List<Models.Jobs>();
    private InforConnection? inforConnection = new InforConnection();
    private static ProgramSettingsModel? settings = new ProgramSettingsModel();
    TokenService tokenService = new TokenService();
    JobGet jobGet = new JobGet();

    // On page load
    protected override async Task OnInitializedAsync()
    {
        // get infor connection information from json file
        string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
        inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

        // get settings information from json file
        string settingsText = File.ReadAllText(@"./Settings.json");
        settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);

        // if ci is not null then get token and items
        if (inforConnection.ci != null)
        {
            // get token from the services folder, token service
            TokenResponse token = await tokenService.GetToken();
            if (!token.IsError)
            {
                // get items from the data folder, ItemGet passing the token
                jobsRoot = await jobGet.GetJobs(token);
                // convert items to a list to display on the items page
                jobEnum = jobsRoot.Items.ToList();
            }
        }
    }
}