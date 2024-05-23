
namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Models
{
    public class DevOpsCredentials
    {
        public Tenant Tenant { get; }
        public string Pat { get; }

        public DevOpsCredentials(Tenant Tenant, string Pat)
        {
            if ((Tenant == null) || (Pat == null))
            {
                throw new ArgumentNullException();
            }
            else
            {
                this.Tenant = Tenant;
                this.Pat = Pat;
            }
        }
    }
}
