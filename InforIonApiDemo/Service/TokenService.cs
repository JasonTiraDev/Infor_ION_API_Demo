
namespace InforIonApiDemo.Service
{
    public class TokenService
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

        
        public async Task<TokenResponse> GetToken()
        {
            // get settings information
            string inforConnectionText = File.ReadAllText(@"./inforConnection.json");
            inforConnection = System.Text.Json.JsonSerializer.Deserialize<InforConnection>(inforConnectionText);

            string settingsText = File.ReadAllText(@"./Settings.json");
            settings = System.Text.Json.JsonSerializer.Deserialize<ProgramSettingsModel>(settingsText);

            ResourceOwnerClientId = inforConnection.ci;
            ResourceOwnerClientSecret = inforConnection.cs;
            OAuth2TokenEndpoint = inforConnection.pu + inforConnection.ot;
            OAuth2TokenRevocationEndpoint = inforConnection.pu + inforConnection.ot;
            IONAPIBaseUrl = inforConnection.iu;
            ServiceAccountAccessKey = inforConnection.saak;
            ServiceAccountSecretKey = inforConnection.sask;


            _oauth2 = new OAuth2Client(new Uri(OAuth2TokenEndpoint), ResourceOwnerClientId, ResourceOwnerClientSecret);
            //Request a token with the provided ServiceAccountAccessKey and ServiceAccountSecretKey
            TokenResponse token = RequestToken();
            if (!token.IsError)
            {
                //Use the access_token to make a call to ION API

                //If a refresh token is available the application can obtain new access_token after those have expired.
                if (token.RefreshToken != null)
                {
                    token = RefreshToken(token.RefreshToken);
                    //It should be possible to continue calling the service with the new token.                    
                }

                //When there is no need for the token it should be revoked so no further access is allowed.
                //RevokeToken(token.AccessToken, OAuth2Constants.AccessToken);
                //If the refresh token is provided is recommended to revoke the refresh token.
                if (token.RefreshToken != null)
                {
                    RevokeToken(token.RefreshToken, OAuth2Constants.RefreshToken);
                }                
            }
            return token;
        }
        private static TokenResponse RequestToken()
        {
            return _oauth2.RequestResourceOwnerPasswordAsync(ServiceAccountAccessKey, ServiceAccountSecretKey).Result;
        }

        private static TokenResponse RefreshToken(string refreshToken)
        {
            return _oauth2.RequestRefreshTokenAsync(refreshToken).Result;
        }

        public void RevokeToken(string token, string tokenType)
        {
            var client = new HttpClient();
            client.SetBasicAuthentication(ResourceOwnerClientId, ResourceOwnerClientSecret);
            var postBody = new Dictionary<string, string> { { "token", token }, { "token_type_hint", tokenType }, { "X-Infor-MongooseConfig", settings.MongooseConfig } };
            var result = client.PostAsync(OAuth2TokenRevocationEndpoint, new FormUrlEncodedContent(postBody)).Result;
            
        }
    }
}