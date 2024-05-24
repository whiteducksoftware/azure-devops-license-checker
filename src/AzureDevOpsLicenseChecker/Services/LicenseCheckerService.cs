using AzureDevOpsLicenseChecker.AzureDevOpsClient.Services;
using AzureDevOpsLicenseChecker.AzureDevOpsClient.Models;
using Spectre.Console;
using Newtonsoft.Json;
using AzureDevOpsLicenseChecker.Models;

namespace AzureDevOpsLicenseChecker.Services;

public class LicenseCheckerService
{
    private readonly AzureDevOpsService _devOpsClient;
    private int _threshold;
    public string licenseToReplace { get; }
    public string targetLicense { get; }

    private ExcludeUsersModel _excludeUsers;

    private static IDictionary<string, UserEntitlement> _users = new Dictionary<string, UserEntitlement>();
    private static IList<UserEntitlement> _usersToPatch = new List<UserEntitlement>();

    public LicenseCheckerService(string tenantId,
                                    string organizationName,
                                    string personalAccessToken,
                                    string licenseToReplace = "",
                                    string targetLicense = "",
                                    int sinceLastLogin = 0,
                                    string file = "")
    {
        // https://docs.microsoft.com/en-us/rest/api/azure/devops/memberentitlementmanagement/user-entitlements?view=azure-devops-rest-6.0&viewFallbackFrom=azure-devops-rest-7.0

        DevOpsCredentials credentials = new DevOpsCredentials(new Tenant(tenantId, organizationName), personalAccessToken);
        _devOpsClient = new AzureDevOpsService(credentials);
        this.licenseToReplace = licenseToReplace;
        this.targetLicense = targetLicense;
        _threshold = sinceLastLogin;

        if (file != string.Empty && file is not null)
        {
            var fileContent = File.ReadAllText(file);
            if (fileContent is not null)
                _excludeUsers = JsonConvert.DeserializeObject<ExcludeUsersModel>(fileContent);
        }
        else
        {
            _excludeUsers = new ExcludeUsersModel();
        }
    }

    /// <summary>
    /// Gets all user from Azure DevOps and returns them as a IDictionary. 
    /// </summary>
    public async Task<IDictionary<string, UserEntitlement>> GetUsers()
    {
        await PrepareUsers();

        return _users;
    }

    public async Task<IList<UserEntitlement>> GetUsersToPatch()
    {
        await PrepareUsers();

        return _usersToPatch;
    }

    /**
    Gets all users from an organization and adds them to the "_users" dictionary.
    It meanwhile checks the licenses of all users and the last time they were logged in.
    If certain conditions based on their license type and the last day their were logged in 
    are met the user is added to the "_usersToPatch" list.
    */
    public async Task PrepareUsers()
    {
        UserEntitlements? entitlements = await _devOpsClient.GetAllUsers();

        foreach(UserEntitlement member in entitlements.Members)
        {
            // Check if the license of a users has to be updated.
            // This is the case if a user has a license type defined in "_licenseToReplace"
            // and the user has not logged in for "_threshold" days
            // and the user is not whitelisted.
            if( member.AccessLevel.AccountLicenseType.Equals(licenseToReplace) && 
                (DateTime.UtcNow - member.LastAccessedDate).Days > _threshold && !_excludeUsers.Users.Contains(member.User.MailAddress)
            )
            {
                member.UpdateLicense = true;
                _usersToPatch.Add(member);
            }
            
            if(_users.ContainsKey(member.Id))
            {
                _users[member.Id] = member;
            }
            else
            {
                _users.Add(member.Id, member);
            }
        }

        // Current local collection of users smaller then the newly fetched collection -> users have been removed!
        if(_users.Count() < entitlements.Members.Count())
        {
            IDictionary<String, UserEntitlement> newUser = entitlements.Members.ToDictionary(user => user.Id, user => user);

            // Iterate over the current collection of users and check if all users 
            // still exist in the newly fetched collection.
            foreach(KeyValuePair<String, UserEntitlement> userEntry in _users)
            {
                if(!newUser.ContainsKey(userEntry.Key))
                {
                    _users.Remove(userEntry.Key);
                }
            }
        }
    }

    public async Task UpdateUserLicenses()
    {
        IList<UpdateUserAccessLevel> updateList = _usersToPatch.Select(user => new UpdateUserAccessLevel
        {
            from = "", path = $"/{user.Id}/accessLevel",
            value = new UpdateUserAccessLevel.AccessLevelUpdateValue {accountLicenseType = targetLicense}
        }).ToList();

        if (!(updateList.Count <= 0))
        {   
            await _devOpsClient.UpdateUsers(updateList);

            //var resp = await _devOpsClient.PatchAsync(this._endpoint, updateList);
            //Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }
    }
}

