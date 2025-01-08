using Newtonsoft.Json;
using System.Net.Http.Headers;
using AzureDevOpsLicenseChecker.AzureDevOpsClient.Models;

namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Services
{
    public class AzureDevOpsService : DevOpsHttpClient
    {
        private readonly string _apiVersion = "7.1-preview.4";


        private readonly string _baseURL = "https://vsaex.dev.azure.com";
        private readonly string _versionQuery = "?api-version=";
        private readonly string _userentitlementApiURL = "_apis/userentitlements";


        private readonly DevOpsCredentials _credentials;

        private HttpClient _client;



        public AzureDevOpsService(DevOpsCredentials credentials)
        {
            _credentials = credentials;
            _client = GetHttpClient();
        }



        public async Task<UserEntitlements?> GetAllUsersAsync()
        {
            UserEntitlements? entitlements = null;
            try 
            {
            HttpResponseMessage response = await GetAsync(_client, $"{_userentitlementApiURL}{_versionQuery}{_apiVersion}");
            response.EnsureSuccessStatusCode();
            entitlements = JsonConvert.DeserializeObject<UserEntitlements>(await response.Content.ReadAsStringAsync());
            } catch (HttpRequestException) 
            {
                Console.WriteLine("\n ACCESS DENIED! Check your personal access token or organisation name! \n");
                Environment.Exit(0);
            }
            return entitlements;
        }


        public async Task UpdateUsersAsync(IList<UpdateUserAccessLevel> usersToPatch)
        {
            try
            {
                var resp = await PatchAsync(_client, $"{_userentitlementApiURL}{_versionQuery}{_apiVersion}", usersToPatch);
                resp.EnsureSuccessStatusCode();
            }
            catch (Exception ex) when (ex is HttpRequestException)
            {
                Console.WriteLine("An Error occured. Please try now or later!");
            }
        }


        public override HttpClient GetHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri($"{_baseURL}/{_credentials.Tenant.Name}/") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "ManagedClientConsoleAppSample");
            client.DefaultRequestHeaders.Add("X-TFS-FedAuthRedirect", "Suppress");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _credentials.Pat))));

            return client;
        }
    }
}
