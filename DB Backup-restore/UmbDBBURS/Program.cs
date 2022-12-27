using Microsoft.Extensions.Configuration;

using Spectre.Console;
using System.Configuration;

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
            var parameters = args.Length > 0 ? args[0] : "";

            AnsiConsole.MarkupLine($"[red] Source: [/]" + sourceDir.Value);
            AnsiConsole.MarkupLine($"[yellow] Destination: [/]" + destDir.Value);
            AnsiConsole.MarkupLine($"[green] Args:  [/]" + parameters);

            return 1;
        }
    }
}