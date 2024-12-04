
using Newtonsoft.Json;
using System.Net.Http.Headers;

using AzureDevOpsLicenseChecker.AzureDevOpsClient.Models;
using System.Net.Http.Json;



namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Services
{
    public class AzureDevOpsService : DevOpsHttpClient
    {
        private readonly string _apiVersion = "7.1-preview.4";
        //private readonly string _apiVersion = "6.0-preview.3";


        private readonly string _baseURL = "https://vsaex.dev.azure.com";
        private readonly string _versionQuery = "?api-version=";
        private readonly string _userentitlementApiURL = "_apis/userentitlements";


        private readonly DevOpsCredentials _credentials;

        private HttpClient _client;



        public AzureDevOpsService(DevOpsCredentials credentials)
        {
            this._credentials = credentials;
            this._client = GetHttpClient();
        }



        public async Task<UserEntitlements?> GetAllUsersAsync()
        {
            UserEntitlements? entitlements = null;

            HttpResponseMessage response = await this.GetAsync(_client, $"{this._userentitlementApiURL}{this._versionQuery}{this._apiVersion}");
            if (response.IsSuccessStatusCode)
            {
                entitlements = JsonConvert.DeserializeObject<UserEntitlements>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception("ACCESS DENIED! Check your personal access token!");
            }

            return entitlements;
        }


        public async Task UpdateUsersAsync(IList<UpdateUserAccessLevel> usersToPatch)
        {
            try
            {
                var resp = await this.PatchAsync<IList<UpdateUserAccessLevel>>(_client, $"{this._userentitlementApiURL}{this._versionQuery}{this._apiVersion}", usersToPatch);
                resp.EnsureSuccessStatusCode();
            }
            catch (Exception ex) when (ex is HttpRequestException)
            {
                Console.WriteLine("An Error occured. Please try now or later!");
            }
        }


        public override HttpClient GetHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri($"{this._baseURL}/{this._credentials.Tenant.Name}/") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "ManagedClientConsoleAppSample");
            client.DefaultRequestHeaders.Add("X-TFS-FedAuthRedirect", "Suppress");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _credentials.Pat))));

            return client;
        }
    }
}
