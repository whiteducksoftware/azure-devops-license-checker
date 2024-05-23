using Newtonsoft.Json;

namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Services
{
    public abstract class DevOpsHttpClient
    {
        public abstract HttpClient GetHttpClient();
        
        public async Task<HttpResponseMessage> GetAsync(HttpClient client, string url)
        {
            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PatchAsync<T>(HttpClient client, string url, T content)
        {
            return await client.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json-patch+json"));
        }
    }
}