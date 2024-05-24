namespace AzureDevOpsLicenseChecker.Models;

public class ExcludeUsersModel
{
    public List<string> Users {get; set;}

    public ExcludeUsersModel()
    {
        Users = new List<string>();
    }
}