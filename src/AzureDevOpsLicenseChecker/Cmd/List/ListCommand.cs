using Spectre.Console;
using AzureDevOpsLicenseChecker.Services;

namespace AzureDevOpsLicenseChecker.Cmd.List;

public class ListCommand
{
    public static async Task ListAsync(string tenant, string org, string pat)
    {
        var table = new Table();
        table.BorderColor(Color.Cyan1);
        table.Border(TableBorder.Rounded);
        string[] columns = ["Id", "Account License Type", "MSDN License Type", "Status", "User", "Last Accessed"];
        table.AddColumns(columns);

        var licenseCheckerService = new LicenseCheckerService(tenant, org, pat);
        var users = await licenseCheckerService.GetUsersAsync();

        foreach (var entry in users)
        {
            table.AddRow(entry.Key,
                         entry.Value.AccessLevel.AccountLicenseType,
                         entry.Value.AccessLevel.MsdnLicenseType,
                         entry.Value.AccessLevel.Status,
                         entry.Value.User.DisplayName,
                         entry.Value.LastAccessedDate.ToShortDateString());
        }

        AnsiConsole.Write(table);
    }
}