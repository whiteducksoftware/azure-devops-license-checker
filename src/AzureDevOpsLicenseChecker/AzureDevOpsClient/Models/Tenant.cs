
namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Models
{
    public class Tenant
    {
        public string Id { get; }
        public string Name { get; }

        public Tenant(string Id, string Name)
        {
            if( (Id == null) || (Name == null) )
            {
                throw new ArgumentNullException();
            }
            else
            {
                this.Id = Id;
                this.Name = Name;
            }
        }
    }
}
