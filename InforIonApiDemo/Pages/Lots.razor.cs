namespace InforIonApiDemo.Pages;

public partial class Lots
{
    private HashSet<Models.Lots> selectedLots = new HashSet<Models.Lots>();
    private bool _selectOnRowClick = true;
    private Models.Lots lots;
    private Models.LotsRoot lotsRoot;
    private IEnumerable<Models.Lots> lotEnum = new List<Models.Lots>();
    private InforConnection? inforConnection = new InforConnection();
    private static ProgramSettingsModel? settings = new ProgramSettingsModel();
    TokenService tokenService = new TokenService();
    LotGet lotGet = new LotGet();

    // on page load
    protected override async Task OnInitializedAsync()
    {
        // get infor connection information from json file
        string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
        inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

        // get settings information from json file
        string settingsText = File.ReadAllText(@"./Settings.json");
        settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);

        // if ci is not null then get token and lots
        if (inforConnection.ci != null)
        {
            // get token from the services folder, token service
            TokenResponse token = await tokenService.GetToken();
            if (!token.IsError)
            {
                // get lots from the data folder, LotGet passing the token
                lotsRoot = await lotGet.GetLots(token);
                // convert lots to a list to display on the lots page
                lotEnum = lotsRoot.Lots.ToList();
            }
        }
    }
}