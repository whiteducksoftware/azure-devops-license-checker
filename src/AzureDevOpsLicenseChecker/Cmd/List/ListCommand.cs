using AzureDevOpsLicenseChecker.AzureDevOpsClient.Models;
using Spectre.Console;
using AzureDevOpsLicenseChecker.Services;

namespace AzureDevOpsLicenseChecker.Cmd.List;

public class ListCommand
{
    public static async Task ListAsync(string org, string pat)
    {
        var table = new Table();
        table.BorderColor(Color.Cyan1);
        table.Border(TableBorder.Rounded);
        string[] columns = ["Id", "Account License Type", "MSDN License Type", "Status", "User", "Last Accessed", "Updateable"];
        table.AddColumns(columns);

        var licenseCheckerService = new LicenseCheckerService(org, pat);
        var users = await licenseCheckerService.GetUsersAsync();

        foreach (var entry in users)
        {
            table.AddRow(entry.Key,
                         entry.Value.AccessLevel.AccountLicenseType,
                         entry.Value.AccessLevel.MsdnLicenseType,
                         entry.Value.AccessLevel.Status,
                         entry.Value.User.DisplayName,
                         entry.Value.LastAccessedDate.ToShortDateString(),
                         IsLicenseValidToUpdate(entry.Value));
        }

        AnsiConsole.Write(table);
    }

    private static string IsLicenseValidToUpdate(UserEntitlement user)
    {
        string accLicenseType = user.AccessLevel.AccountLicenseType;
        string partly = "[bold yellow]";
        string positive = "[bold green]";
        string negative = "[bold red]";
        string yesMsg = "YES[/]";
        string noMsg = "NO[/]";

        switch (accLicenseType)
        {
            case "stakeholder": return partly+yesMsg;
            case "express": return positive+yesMsg;
            case "advanced": return positive+yesMsg;
            case "none": return negative+noMsg;
            default: return string.Empty;
        }
    }

}