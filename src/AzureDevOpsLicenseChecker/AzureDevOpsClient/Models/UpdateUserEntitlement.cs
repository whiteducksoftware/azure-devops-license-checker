
namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Models
{
    public class UpdateUserAccessLevel
    {
        public class AccessLevelUpdateValue
        {
            public string accountLicenseType = "{0}"; 
            public string licensingSource = "account";
        }

        public string from;
        public string op = "replace";
        public string path = "/{0}/accessLevel";
        public AccessLevelUpdateValue value;
    }
}