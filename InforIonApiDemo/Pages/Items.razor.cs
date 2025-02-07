using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InforIonApiDemo.Models; // Ensure your models are in this namespace
using Microsoft.AspNetCore.Components;

namespace InforIonApiDemo.Pages
{
    public partial class Items
    {
        // A HashSet to keep track of selected items.
        private HashSet<Models.Items> selectedItems = new HashSet<Models.Items>();
        private bool _selectOnRowClick = true;

        // These fields will be used for the fetched data.
        private Models.Items items;
        private Models.ItemsRoot itemsRoot;

        // Use fully qualified type names to avoid naming conflicts.
        private IEnumerable<Models.Items> itemEnum = new List<Models.Items>();

        private InforConnection? inforConnection = new InforConnection();
        private static ProgramSettingsModel? settings = new ProgramSettingsModel();

        TokenService tokenService = new TokenService();
        ItemGet itemGet = new ItemGet();

        // Sample data to display if no data is returned.
        private IEnumerable<Models.Items> sampleData = new List<Models.Items>
        {
            new Models.Items { Item = "Sample Item 1", Description = "This is sample item 1", _ItemId = "S001" },
            new Models.Items { Item = "Sample Item 2", Description = "This is sample item 2", _ItemId = "S002" },
            new Models.Items { Item = "Sample Item 3", Description = "This is sample item 3", _ItemId = "S003" }
        };

        // Search string bound to the search field.
        private string searchString = "";

        // Filtered items based on search input.
        private IEnumerable<Models.Items> FilteredItems => string.IsNullOrWhiteSpace(searchString)
            ? itemEnum
            : itemEnum.Where(x => x.Item.Contains(searchString, System.StringComparison.OrdinalIgnoreCase));

        // On page load
        protected override async Task OnInitializedAsync()
        {
            // Get infor connection information from json file.
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

            // Get settings information from json file.
            string settingsText = File.ReadAllText(@"./Settings.json");
            settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);

            // If connection info is available (ci is not null) then attempt to get token and items.
            if (inforConnection?.ci != null)
            {
                // Get token using the token service.
                TokenResponse token = await tokenService.GetToken();
                if (!token.IsError)
                {
                    Data.ItemGet itemGet = new Data.ItemGet();
                    // Get items using the token.
                    itemsRoot = await itemGet.GetItems(token);
                    // If items are returned, convert them to a list; otherwise use sample data.
                    itemEnum = (itemsRoot.Items != null && itemsRoot.Items.Any())
                                ? itemsRoot.Items.ToList()
                                : sampleData;
                }
                else
                {
                    // If token retrieval failed, fall back to sample data.
                    itemEnum = sampleData;
                }
            }
            else
            {
                // If connection info is missing, fall back to sample data.
                itemEnum = sampleData;
            }
        }
    }
}
