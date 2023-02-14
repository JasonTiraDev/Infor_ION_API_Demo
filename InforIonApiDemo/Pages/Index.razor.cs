namespace InforIonApiDemo.Pages;

public partial class Index
{

    private static string ResourceOwnerClientId = "";
    private static string ResourceOwnerClientSecret = "";
    private static string OAuth2TokenEndpoint = "";
    private static string OAuth2TokenRevocationEndpoint = "";
    private static string IONAPIBaseUrl = "";
    private static string ServiceAccountAccessKey = "";
    private static string ServiceAccountSecretKey = "";
    private static OAuth2Client _oauth2;

    
    private InforConnection? inforConnection = new InforConnection();
    private static ProgramSettingsModel? settings = new ProgramSettingsModel();
    private string _NoConnectionSettings;


    protected override async void OnInitialized()
        {
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

            string settingsText = File.ReadAllText(@"./Settings.json");
            settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);
            string config = settings.MongooseConfig;

            // set variables to use to get tokens pulled from json file
            ResourceOwnerClientId = inforConnection.ci;
            ResourceOwnerClientSecret = inforConnection.cs;
            OAuth2TokenEndpoint = inforConnection.pu + inforConnection.ot;
            OAuth2TokenRevocationEndpoint = inforConnection.pu + inforConnection.ot;
            IONAPIBaseUrl = inforConnection.iu;
            ServiceAccountAccessKey = inforConnection.saak;
            ServiceAccountSecretKey = inforConnection.sask;

            // check to see if the connection settings are valid
            if (ResourceOwnerClientId == "" || ResourceOwnerClientSecret == "" || OAuth2TokenEndpoint == "" || OAuth2TokenRevocationEndpoint == "" || IONAPIBaseUrl == "" || ServiceAccountAccessKey == "" || ServiceAccountSecretKey == "")
            {
                _NoConnectionSettings = "No connection settings found. Please check your inforConnection.json file.";
            }
            else
            {
                _NoConnectionSettings = "";
            }
        }

    public async Task GetTokenAndRun()
    {
        
        _oauth2 = new OAuth2Client(new Uri(OAuth2TokenEndpoint), ResourceOwnerClientId, ResourceOwnerClientSecret);
        //Request a token with the provided ServiceAccountAccessKey and ServiceAccountSecretKey
        TokenResponse token = RequestToken();
            ShowResponse(token);
            if (!token.IsError)
            {
                //Use the access_token to make a call to ION API
                //CallService(token.AccessToken); // Run a method feeding it the token
                //If a refresh token is available the application can obtain new access_token after those have expired.
                if (token.RefreshToken != null)
                {
                    token = RefreshToken(token.RefreshToken);
                    //It should be possible to continue calling the service with the new token.
                    if (!token.IsError)
                    {
                        //CallService(token.AccessToken);
                    }
                }

                //When there is no need for the token it should be revoked so no further access is allowed.
                RevokeToken(token.AccessToken, OAuth2Constants.AccessToken);
                //If the refresh token is provided is recommended to revoke the refresh token.
                if (token.RefreshToken != null)
                {
                    RevokeToken(token.RefreshToken, OAuth2Constants.RefreshToken);
                }

                //It is not possible to use the access_token anymore...
                //CallService(token.AccessToken);
                //It should not be possible to refresh the token again...
                token = RefreshToken(token.RefreshToken);
                ShowResponse(token);

            }
            var client = new RestClient(inforConnection.iu);
    }
    private static TokenResponse RequestToken()
        {
            return _oauth2.RequestResourceOwnerPasswordAsync(ServiceAccountAccessKey, ServiceAccountSecretKey).Result;
        }

        private static TokenResponse RefreshToken(string refreshToken)
        {
            return _oauth2.RequestRefreshTokenAsync(refreshToken).Result;
        }

        private static void RevokeToken(string token, string tokenType)
        {
            var client = new HttpClient();
            client.SetBasicAuthentication(ResourceOwnerClientId, ResourceOwnerClientSecret);
            var postBody = new Dictionary<string, string>{{"token", token}, {"token_type_hint", tokenType}, {"X-Infor-MongooseConfig", settings.MongooseConfig } };
            var result = client.PostAsync(OAuth2TokenRevocationEndpoint, new FormUrlEncodedContent(postBody)).Result;
            if (result.IsSuccessStatusCode)
            {
                "Succesfully revoked token.".ConsoleGreen();
            }
            else
            {
                "Error revoking token.".ConsoleRed();
            }

            Console.WriteLine("{1}, {0}", token, tokenType);
        }
        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                "\nToken response:".ConsoleGreen();
                Console.WriteLine(response.Json);
                "\nAccess Token:".ConsoleGreen();
                Console.WriteLine(response.AccessToken);
            }
            else
            {
                if (response.IsHttpError)
                {
                    "HTTP error: ".ConsoleRed();
                    Console.WriteLine(response.HttpErrorStatusCode);
                    "HTTP error reason: ".ConsoleRed();
                    Console.WriteLine(response.HttpErrorReason);
                }
                else
                {
                    "Protocol error response:".ConsoleRed();
                    Console.WriteLine(response.Json);
                }
            }
        }

}