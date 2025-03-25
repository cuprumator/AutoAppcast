using System;
using System.IO;
using System.Xml.Linq;

public class AppcastGenerator
{
    public static string GenerateAppcastXml(string appName, string version, string pubDate, string fileUrl, string fileSize, string releaseNotesHtml)
    {
        XNamespace sparkle = "http://www.andymatuschak.org/xml-namespaces/sparkle";
        XNamespace dc = "http://purl.org/dc/elements/1.1/";

        XElement appcast = new XElement("rss",
            new XAttribute("version", "2.0"),
            new XAttribute(XNamespace.Xmlns + "dc", dc),
            new XAttribute(XNamespace.Xmlns + "sparkle", sparkle),
            new XElement("channel",
                new XElement("title", appName),
                new XElement("link"),
                new XElement("item",
                    new XElement("title", $"Version ({version})"),
                    new XElement("description", releaseNotesHtml), // ✅ Already encoded manually
                    new XElement("pubDate", pubDate),
                    new XElement("enclosure",
                        new XAttribute(sparkle + "version", version),
                        new XAttribute(sparkle + "shortVersionString", ""),
                        new XAttribute("url", fileUrl),
                        new XAttribute("length", fileSize),
                        new XAttribute("type", "application/octet-stream")
                    ),
                    new XElement(sparkle + "minimumSystemVersion")
                )
            )
        );

        return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), appcast).ToString();
    }

    public static void SaveAndPrintAppcast(string outputFilePath, string xmlContent)
    {
        try
        {
            // Save to file
            File.WriteAllText(outputFilePath, xmlContent);
            Console.WriteLine($"Appcast XML saved successfully to: {outputFilePath}\n");

            // Print to console
            Console.WriteLine("Generated Appcast XML:\n");
            Console.WriteLine(xmlContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving Appcast XML: {ex.Message}");
        }
    }
}
