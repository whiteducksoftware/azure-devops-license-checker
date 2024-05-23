
namespace AzureDevOpsLicenseChecker.AzureDevOpsClient.Models
{
    public class UserEntitlements
    {
        public UserEntitlement[] Members;
    }

    public class UserEntitlement
    {
        public string Id;
        public User User;
        public AccessLevel AccessLevel;
        public DateTime LastAccessedDate;
        public DateTime DateCreated;

        public Boolean UpdateLicense = false;
    }

    public class User
    {
        public string SubjectKind;
        public string MetaType;
        public string Domain;
        public string PrincipalName;
        public string MailAddress;
        public string Origin;
        public string OriginId;
        public string DisplayName;
        public string Descriptor;
    }

    public class AccessLevel
    {
        public string LicensingSource;
        public string AccountLicenseType;
        public string MsdnLicenseType;
        public string LicenseDisplayName;
        public string Status;
        public string StatusMessage;
        public string AssignmentSource;
    }
}