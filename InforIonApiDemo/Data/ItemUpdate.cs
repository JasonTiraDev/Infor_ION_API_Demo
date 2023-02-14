
namespace InforIonApiDemo.Data
{
    public class ItemUpdate
    {
        public async Task UpdatePlannerExist(bool exist, TokenResponse token, string itemId)
        {
            // get settings information
            string inforConnectionText = File.ReadAllText(@"./InforSettings.json");
            InforConnectionRoot? inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnectionRoot>(inforConnectionText);
            
                string iu = inforConnection.InforConnection.iu;
            try
			{
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(token.AccessToken);
                    client.DefaultRequestHeaders.Add(
                        "X-Infor-MongooseConfig",
                    inforConnection.AppSettings.MongooseEnvironment);
                    string IDO = "SLItems";

                    string postUri = $"{iu}/{inforConnection.AppSettings.MongooseEnvironment}/CSI/IDORequestService/ido/update/{IDO}?refresh=true";

                    // Create the JSON to update
                    List<UpdateModelProperty> updateProperty = new List<UpdateModelProperty>();
                    UpdateModelProperty p1 = new UpdateModelProperty()
                    {
                        IsNull = false,
                        Modified = true,
                        Name = "Description", // name of property here
                        Value = exist.ToString()
                    };
                    updateProperty.Add(p1);

                    List<UpdateModelChange> updateModelChanges = new List<UpdateModelChange>();
                    // action 2 is update - feed it the itemId 
                    UpdateModelChange u1 = new UpdateModelChange()
                    {
                        Action = 2,
                        ItemId = itemId,
                        Properties = updateProperty
                    };
                    updateModelChanges.Add(u1);


                    UpdateModelRoot updateRoot = new UpdateModelRoot()
                    {
                        Changes = updateModelChanges
                    };

                    string strJson = System.Text.Json.JsonSerializer.Serialize<UpdateModelRoot>(updateRoot);

                    var content = new StringContent(strJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(postUri, content);

                }
            }
			catch (Exception e )
			{

			}

        }
    }
}
