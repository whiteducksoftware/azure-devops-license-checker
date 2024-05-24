using Spectre.Console;
using AzureDevOpsLicenseChecker.Services;

namespace AzureDevOpsLicenseChecker.Cmd.Update;

public class UpdateCommand
{
    public static async Task updateAsync(string tenant, string org, string pat, string license, string target, int since, string file)
    {
        var licenseCheckerService = new LicenseCheckerService(tenant, org, pat, license, target, since, file);
        var usersToPatch = await licenseCheckerService.GetUsersToPatch();

        if (usersToPatch.Count == 0)
            {
                Console.WriteLine("No licenses to update");
                Environment.Exit(0);
            }
            else
            {
                var table = new Table();
                table.BorderColor(Color.Cyan1);
                table.Border(TableBorder.Rounded);
                string[] columns = [ "Id", "Account License Type", "MSDN License Type", "Status", "User", "Last Accessed" ];
                table.AddColumns(columns);
            
                foreach(var entry in usersToPatch)
                {
                    table.AddRow(entry.Id, $"[red]{licenseCheckerService.licenseToReplace}[/] -> [green]{licenseCheckerService.targetLicense}[/]", entry.AccessLevel.MsdnLicenseType, entry.AccessLevel.Status, entry.User.DisplayName, entry.LastAccessedDate.ToShortDateString());
                }

                AnsiConsole.Write(table);

                var answer = "";

                while (answer != "Y")
                {
                    Console.WriteLine();
                    Console.Write("Do you really want to update these licenses? [Y/N]: ");
                    answer = Console.ReadLine();

                    if (answer == "N")
                        Environment.Exit(0);
                }

                await licenseCheckerService.UpdateUserLicenses();
            }

    }
}