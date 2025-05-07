using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AzureDevOpsLicenseChecker.Cmd.List;
using AzureDevOpsLicenseChecker.Cmd.Update;
using Spectre.Console.Cli;

namespace AzureDevOpsLicenseChecker;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var app = new CommandApp();

        app.Configure(config =>
        {

            string fullVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
            string version = fullVersion.EndsWith(".0") ? fullVersion.Substring(0, fullVersion.Length - 2) : fullVersion;
            config.SetApplicationVersion(version);
            config.AddCommand<ListCommandCli>("list")
                .WithDescription("List Azure DevOps licenses")
                .WithExample(new[] { "list", "--org", "<ORG_NAME>", "--pat", "<PAT>" });

            config.AddCommand<UpdateCommandCli>("update")
                .WithDescription("Update Azure DevOps licenses")
                .WithExample(new[] { "update", "--org", "<ORG_NAME>", "--pat", "<PAT>", "--license", "<LICENSE_TYPE>", "--target", "<LICENSE_TYPE>" });
        });

        return await app.RunAsync(args);
    }
}

// Common settings used by both commands
public class AzureDevOpsCommandSettings : CommandSettings
{
    [CommandOption("--org")]
    [Description("The organization name")]
    [Required]
    public string Organization { get; set; } = string.Empty;

    [CommandOption("--pat")]
    [Description("The value of the personal access token which will be used to authenticate against Azure DevOps")]
    [Required]
    public string PersonalAccessToken { get; set; } = string.Empty;
}

// List command implementation
public class ListCommandSettings : AzureDevOpsCommandSettings
{
}

public class ListCommandCli : AsyncCommand<ListCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ListCommandSettings settings)
    {
        await ListCommand.ListAsync(settings.Organization, settings.PersonalAccessToken);
        return 0;
    }
}

// Update command implementation
public class UpdateCommandSettings : AzureDevOpsCommandSettings
{
    [CommandOption("--license")]
    [Description("The license to update")]
    [Required]
    public string License { get; set; } = string.Empty;

    [CommandOption("--target")]
    [Description("The target license")]
    [Required]
    public string TargetLicense { get; set; } = string.Empty;

    [CommandOption("--since")]
    [Description("The minimum number of days since the last login [[Default: 0]]")]
    public int SinceLastLogin { get; set; } = 0;

    [CommandOption("--file")]
    [Description("Exclude users from update")]
    public string? ExcludeFile { get; set; }
}

public class UpdateCommandCli : AsyncCommand<UpdateCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, UpdateCommandSettings settings)
    {
        await UpdateCommand.UpdateAsync(
            settings.Organization,
            settings.PersonalAccessToken,
            settings.License,
            settings.TargetLicense,
            settings.SinceLastLogin,
            settings.ExcludeFile);
        return 0;
    }
}