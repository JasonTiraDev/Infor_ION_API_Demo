using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InforIonApiDemo.Models; // Ensure your models (Lots, LotsRoot, etc.) are in this namespace
using Microsoft.AspNetCore.Components;

namespace InforIonApiDemo.Pages
{
    public partial class Lots
    {
        // A HashSet to keep track of selected lots.
        private HashSet<Models.Lots> selectedLots = new HashSet<Models.Lots>();
        private bool _selectOnRowClick = true;

        // Fields used for the fetched data.
        private Models.Lots lots;
        private Models.LotsRoot lotsRoot;
        private IEnumerable<Models.Lots> lotEnum = new List<Models.Lots>();

        private InforConnection? inforConnection = new InforConnection();
        private static ProgramSettingsModel? settings = new ProgramSettingsModel();

        TokenService tokenService = new TokenService();
        LotGet lotGet = new LotGet();

        // Sample data to display if no data is returned.
        private IEnumerable<Models.Lots> sampleData = new List<Models.Lots>
        {
            new Models.Lots { Lot = "Sample Lot 1", Item = "Sample Item 1", Description = "This is sample lot 1", _ItemId = "L001" },
            new Models.Lots { Lot = "Sample Lot 2", Item = "Sample Item 2", Description = "This is sample lot 2", _ItemId = "L002" },
            new Models.Lots { Lot = "Sample Lot 3", Item = "Sample Item 3", Description = "This is sample lot 3", _ItemId = "L003" }
        };

        // Search string bound to the search field.
        private string searchString = "";

        // Filtered lots based on search input.
        private IEnumerable<Models.Lots> FilteredLots => string.IsNullOrWhiteSpace(searchString)
            ? lotEnum
            : lotEnum.Where(x => x.Lot.Contains(searchString, System.StringComparison.OrdinalIgnoreCase)
                               || x.Item.Contains(searchString, System.StringComparison.OrdinalIgnoreCase)
                               || x.Description.Contains(searchString, System.StringComparison.OrdinalIgnoreCase));

        // On page load
        protected override async Task OnInitializedAsync()
        {
            // Get infor connection information from JSON file.
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

            // Get settings information from JSON file.
            string settingsText = File.ReadAllText(@"./Settings.json");
            settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);

            // If connection info is available (ci is not null), then attempt to get token and lots.
            if (inforConnection?.ci != null)
            {
                TokenResponse token = await tokenService.GetToken();
                if (!token.IsError)
                {
                    // Use the already declared 'lotGet'
                    lotsRoot = await lotGet.GetLots(token);
                    // If lots are returned, convert them to a list; otherwise, use sample data.
                    lotEnum = (lotsRoot.Lots != null && lotsRoot.Lots.Any())
                                ? lotsRoot.Lots.ToList()
                                : sampleData;
                }
                else
                {
                    lotEnum = sampleData;
                }
            }
            else
            {
                lotEnum = sampleData;
            }
        }
    }
}
