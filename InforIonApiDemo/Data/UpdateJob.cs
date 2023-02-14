namespace InforIonApiDemo.Data
{
    public class UpdateJob
    {
        public async Task<StringContent> GetJobToUpdate(string updateid, TokenResponse token)
        {
            try
            {

                // get settings information
                string inforConnectionText = File.ReadAllText(@"./InforSettings.json");
                InforConnectionRoot? inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnectionRoot>(inforConnectionText);
                string environment = inforConnection.InforConnection.ti;
                string iu = inforConnection.InforConnection.iu;

                string config = inforConnection.AppSettings.MongooseEnvironment;
                string ido = "SLJobs";
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(token.AccessToken);
                    client.DefaultRequestHeaders.Add(
                 "X-Infor-MongooseConfig", config);
                    string postUri = $"{iu}/{environment}/CSI/IDORequestService/ido/update/{ido}?refresh=true";

                    // Create the JSON to update
                    List<UpdateModelProperty> updateProperty = new List<UpdateModelProperty>();
                    UpdateModelProperty p1 = new UpdateModelProperty()
                    {
                        IsNull = false,
                        Modified = true,
                        Name = "Stat",
                        Value = "R"
                    };
                    updateProperty.Add(p1);

                    List<UpdateModelChange> updateModelChanges = new List<UpdateModelChange>();
                    UpdateModelChange u1 = new UpdateModelChange()
                    {
                        Action = 2,
                        ItemId = updateid,
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

                    return content;
                }
            }
            catch (Exception e)
            {
                var exception = new StringContent(e.Message);
                return exception;
            }

        }
    }
}
