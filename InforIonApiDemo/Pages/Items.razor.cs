
namespace InforIonApiDemo.Pages
{
    public partial class Items
    {
        private HashSet<Models.Items> selectedItems = new HashSet<Models.Items>();
        private bool _selectOnRowClick = true;
        private Models.Items items;
        private Models.ItemsRoot itemsRoot;
        private IEnumerable<Models.Items> itemEnum = new List<Models.Items>();
        private InforConnection? inforConnection = new InforConnection();
        private static ProgramSettingsModel? settings = new ProgramSettingsModel();
        TokenService tokenService = new TokenService();
        ItemGet itemGet = new ItemGet();


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
                    
                    Data.ItemGet itemGet = new Data.ItemGet();
                    // get items from the data folder, ItemGet passing the token
                    itemsRoot = await itemGet.GetItems(token);
                    // convert items to a list to display on the items page
                    itemEnum = itemsRoot.Items.ToList();
                }
            }            
        }        
    }
}

