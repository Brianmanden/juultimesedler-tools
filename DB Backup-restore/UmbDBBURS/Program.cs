﻿using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace UmbDBBURS
{
    class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            var sourceDir = configuration.GetSection("Source");
            var destDir = configuration.GetSection("Destination");
            var parameters = args.Length > 0 ? args[0].ToString() : "";

            AnsiConsole.MarkupLine($"[red] Source: [/]" + sourceDir.Value);
            AnsiConsole.MarkupLine($"[yellow] Destination: [/]" + destDir.Value);

            IEnumerable<string> directories = Directory.EnumerateDirectories(destDir.Value);
            string chosenBackup = "";

            if (String.IsNullOrEmpty(parameters))
            {
                // TODO Update usage description in appsettings.json and use to print manual before exiting
                return 0;
            }

            switch (parameters)
            {
                case "rs":
                    chosenBackup = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Chose backup to restore")
                        .PageSize(10)
                        .MoreChoicesText("[grey] Move up/down to chose backup [/]")
                        .AddChoices(directories.ToArray()));
                    // TODO Restore DB with files
                    AnsiConsole.MarkupLine("[green] Chosen backup:[/] " + chosenBackup);
                break;

                case "bu":
                    // TODO Backup DB files to folder
                    HelperMethods helpers = new HelperMethods();
                    string theTime = helpers.GetFormattedTimestamp();
                    AnsiConsole.MarkupLine("Backup DB files using: " + theTime);
                break;

                default:
                    break;
            }

            return 1;

        }

        public class HelperMethods
        {
            public string GetFormattedTimestamp() {
                return DateTime.Now.ToString("yyyy_MM_dd-HH_mm");
            }
        }
    }
}