namespace InforIonApiDemo.Models;

public class LotGet
{
    public async Task<Models.LotsRoot> GetLots(TokenResponse token)
    {
        try
        {
            string json = string.Empty;
            // get settings information
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            InforConnection? inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

            string settingsText = File.ReadAllText(@"./Settings.json");
            ProgramSettingsModel? settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);

            string environment = inforConnection.ti;
            string url = inforConnection.iu;

            using (HttpClient client = new HttpClient())
            {
                // string for properties, list whatever wanted to retreive  
                string properties = "properties=Lot,Item,Description,_ItemId";
                // the ido pulling from
                string ido = "SLLots";
                // record cap, how many records to pull
                string recordCap = "recordCap=500";
                // build the url using the variables
                string requestUrl = $"{url}/{environment}/CSI/IDORequestService/ido/load/{ido}?{properties}&{recordCap}";
                // set the bearer token and mongoose config - this config should be pulled from available configs

                client.SetBearerToken(token.AccessToken);
                client.DefaultRequestHeaders.Add(
                    "X-Infor-MongooseConfig", settings.MongooseConfig);

                HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                using (HttpContent content = response.Content)
                {
                    Task<string> result = content.ReadAsStringAsync();
                    json = result.Result;
                }

                Models.LotsRoot? lotsRoot = JsonConvert.DeserializeObject<Models.LotsRoot>(json);

                TokenService tokenService = new TokenService();
                tokenService.RevokeToken(token.AccessToken, token.TokenType);
                return lotsRoot;
            }
        }
        catch (Exception e)
        {
            string message = e.Message;
        }
        return new();
    }

}