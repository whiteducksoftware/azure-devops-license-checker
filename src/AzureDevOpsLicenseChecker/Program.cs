using System.CommandLine;
using AzureDevOpsLicenseChecker.Cmd.List;
using AzureDevOpsLicenseChecker.Cmd.Update;

namespace AzureDevOpsLicenseChecker;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var organizationOption = new Option<string>(
            name: "--org",
            description: "The organization name")
        {
            IsRequired = true
        };

        var personalAccessTokenOption = new Option<string>(
            name: "--pat",
            description: "The value of the personal access token which will be used to authenticate against Azure DevOps")
        {
            IsRequired = true
        };

        var licenseOption = new Option<string>(
            name: "--license",
            description: "The license to update")
        {
            IsRequired = true
        };

        var targetLicenseOption = new Option<string>(
            name: "--target",
            description: "The target license")
        {
            IsRequired = true
        };

        var sinceLastLoginOption = new Option<int>(
            name: "--since",
            description: "the minimum number of days since the last login [Default: 0]");

        var fileOption = new Option<string>(
            name: "--file",
            description: "exclude users from update");

        var rootCommand = new RootCommand("Azure DevOps License Checker");

        var listCommand = new Command("list", "list azure devops licenses")
        {
            organizationOption,
            personalAccessTokenOption
        };

        var updateCommand = new Command("update", "update azure devops licenses")
        {
            organizationOption,
            personalAccessTokenOption,
            licenseOption,
            targetLicenseOption,
            sinceLastLoginOption,
            fileOption
        };

        listCommand.SetHandler(ListCommand.ListAsync,
                               organizationOption,
                               personalAccessTokenOption);
        updateCommand.SetHandler(UpdateCommand.UpdateAsync,
                                  organizationOption,
                                  personalAccessTokenOption,
                                  licenseOption,
                                  targetLicenseOption,
                                  sinceLastLoginOption,
                                  fileOption);

        rootCommand.AddCommand(listCommand);
        rootCommand.AddCommand(updateCommand);

        return await rootCommand.InvokeAsync(args);
    }
}