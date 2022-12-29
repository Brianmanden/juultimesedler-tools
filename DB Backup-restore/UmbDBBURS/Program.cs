using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace UmbDBBURS
{
    class Program
    {
        public static int Main(string[] args)
        {
            #region CONFIG
            var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

            var sourceDir = configuration.GetSection("Source");
            var destRootDir = configuration.GetSection("Destination");
            var usage = configuration
                        .GetSection("Usage")
                        .GetChildren()
                        .Select(line => line.Value)
                        .ToArray();
            string parameters = args.Length > 0 ? args[0].ToString().ToLower() : "";
            #endregion

            #region NO PARAMETERS GIVEN
            if (String.IsNullOrEmpty(parameters))
            {
                AnsiConsole.MarkupLine("[red]No parameter given[/]");
                foreach (var line in usage)
                {
                    AnsiConsole.MarkupLine(line);
                }
                return 1;
            }
            #endregion

            switch (parameters)
            {
                #region RESTORE
                case "rs":
                    IEnumerable<string> directories = Directory.EnumerateDirectories(destRootDir.Value);
                    string chosenBackup = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Chose backup to restore")
                        .PageSize(10)
                        .MoreChoicesText("[grey] Move up/down to chose backup [/]")
                        .AddChoices(directories.ToArray()));

                    string[] dbRSFiles = Directory.GetFiles(chosenBackup);

                    foreach (string file in dbRSFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        string destFile = Path.Combine(sourceDir.Value, fileName);
                        File.Copy(file, destFile, true);
                    }

                    AnsiConsole.MarkupLine($"[green]{chosenBackup}[/] is restored");
                    break; 
                #endregion

                #region BACKUP
                case "bu":
                    if (Directory.Exists(@sourceDir.Value))
                    {
                        HelperMethods helpers = new();
                        string formattedTimestamp = helpers.GetFormattedTimestamp();
                        string backupDir = Directory.CreateDirectory(@destRootDir.Value + "\\" + formattedTimestamp).ToString();

                        string[] dbBUFiles = Directory.GetFiles(@sourceDir.Value);

                        foreach (string file in dbBUFiles)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string fileName = Path.GetFileName(file);
                            string destFile = Path.Combine(backupDir, fileName);
                            File.Copy(file, destFile, true);
                        }

                        AnsiConsole.MarkupLine($"Database files backed up.");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"The Directory '[red]{sourceDir.Value}[/]' does not exist.");
                        AnsiConsole.MarkupLine("Please verify [red]Source[/] in [red]appsettings.json[/]");
                    }

                    break; 
                #endregion

                default:
                    AnsiConsole.MarkupLine("[red]No valid parameter given[/]");
                    foreach (var line in usage)
                    {
                        AnsiConsole.MarkupLine(line);
                    }
                    return 1;
                    break;
            }

            return 0;
        }

        public class HelperMethods
        {
            public string GetFormattedTimestamp()
            {
                return DateTime.Now.ToString("yyyy_MM_dd-HH_mm");
            }
        }
    }
}