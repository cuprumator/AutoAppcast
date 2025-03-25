using System;
using System.IO;

namespace AutoAppcast
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || Array.Exists(args, arg => arg == "-h" || arg == "--help"))
            {
                ShowUsage();
                return;
            }

            string inputFilePath = null;
            string outputFilePath = "appcast.xml";
            string version = "1.0.0";
            string pubDate = DateTime.UtcNow.ToString("r");
            string appName = "UnknownApp";
            string fileUrl = "";
            string filePath = "";
            string fileSize = "0";

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-i": if (i + 1 < args.Length) inputFilePath = args[++i]; break;
                    case "-o": if (i + 1 < args.Length) outputFilePath = args[++i]; break;
                    case "-v": if (i + 1 < args.Length) version = args[++i]; break;
                    case "-d": if (i + 1 < args.Length) pubDate = args[++i]; break;
                    case "-a": if (i + 1 < args.Length) appName = args[++i]; break;
                    case "-f": if (i + 1 < args.Length) filePath = args[++i]; break;
                    case "-url": if (i + 1 < args.Length) fileUrl = args[++i]; break;
                }
            }

            if (string.IsNullOrEmpty(inputFilePath) || !File.Exists(inputFilePath))
            {
                Console.WriteLine("Error: Markdown input file is required.");
                ShowUsage();
                return;
            }

            var installerInfo = new InstallerInfoExtractor();
            if (!string.IsNullOrEmpty(filePath)) installerInfo.ExtractInfo(filePath);

            string markdown = File.ReadAllText(inputFilePath);
            string releaseNotesHtml = MarkdownConverter.ConvertMarkdownToHtml(markdown);

            string xmlOutput = AppcastGenerator.GenerateAppcastXml(
                installerInfo.AppName, installerInfo.Version, pubDate, fileUrl, installerInfo.FileSize, releaseNotesHtml);

            // Save and print the Appcast XML
            AppcastGenerator.SaveAndPrintAppcast(outputFilePath, xmlOutput);
        }

        static void ShowUsage()
        {
            Console.WriteLine("\nAutoAppcast - Generate Appcast XML from Markdown.");
            Console.WriteLine("\nUsage:");
            Console.WriteLine("  AutoAppcast -i input.md [-o output.xml] [-v version] [-d date] [-a app_name] [-f installer_file] [-url file_url]\n");

            Console.WriteLine("Options:");
            Console.WriteLine("  -i <file>    Input Markdown file (**Required**)");
            Console.WriteLine("  -o <file>    Output Appcast XML file (**Default: appcast.xml**)");
            Console.WriteLine("  -v <version> Application version (**Default: 1.0.0 or extracted from installer**)");
            Console.WriteLine("  -d <date>    Release date (**Default: Now, in RFC 822 format**)");
            Console.WriteLine("  -a <name>    Application name (**Default: Extracted from installer or 'UnknownApp'**)");
            Console.WriteLine("  -f <file>    Path to installer file (EXE/MSI) for metadata extraction");
            Console.WriteLine("  -url <url>   Public URL to download the installer");
            Console.WriteLine("  -h, --help   Show this help message and exit\n");

            Console.WriteLine("Examples:");
            Console.WriteLine("  AutoAppcast -i release-notes.md -o myappcast.xml -v 3.2.0 -d \"Tue, 11 Mar 2025 10:40:31 GMT\"");
            Console.WriteLine("  AutoAppcast -i release-notes.md -f MyAppInstaller.msi -url \"https://myapp.com/download/MyAppInstaller.msi\"");
            Console.WriteLine("  AutoAppcast -i release-notes.md -f MyAppInstaller.exe -a \"MyApp\" -d \"Mon, 24 Mar 2025 14:54:09 GMT\"\n");
        }
    }
}
