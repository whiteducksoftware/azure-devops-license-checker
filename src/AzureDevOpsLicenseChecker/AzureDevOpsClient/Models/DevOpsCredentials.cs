
namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Models
{
    public class DevOpsCredentials
    {
        public string OrganisationName { get; }
        public string Pat { get; }

        public DevOpsCredentials(string OrganisationName, string Pat)
        {
            if ((OrganisationName == null) || (Pat == null))
            {
                throw new ArgumentNullException();
            }
            else
            {
                this.OrganisationName  = OrganisationName;
                this.Pat = Pat;
            }
        }
    }
}
