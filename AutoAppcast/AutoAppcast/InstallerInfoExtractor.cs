using System;
using System.Diagnostics;
using System.IO;

public class InstallerInfoExtractor
{
    public string AppName { get; private set; } = "UnknownApp";
    public string Version { get; private set; } = "1.0.0";
    public string FileSize { get; private set; } = "0";

    public void ExtractInfo(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: Installer file not found: {filePath}");
            return;
        }

        FileSize = new FileInfo(filePath).Length.ToString();

        if (filePath.EndsWith(".msi", StringComparison.OrdinalIgnoreCase))
        {
            ExtractMsiInfo(filePath);
        }
        else if (filePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            ExtractExeInfo(filePath);
        }
    }

    private void ExtractMsiInfo(string filePath)
    {
        string logFilePath = Path.Combine(Path.GetTempPath(), "msi_log.txt");

        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "msiexec";
            process.StartInfo.Arguments = $"/i \"{filePath}\" /quiet /log \"{logFilePath}\"";
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();

            if (File.Exists(logFilePath))
            {
                string[] logLines = File.ReadAllLines(logFilePath);
                foreach (string line in logLines)
                {
                    if (line.Contains("ProductVersion"))
                    {
                        Version = NormalizeVersion(line.Split('=')[1].Trim());
                    }
                    if (line.Contains("ProductName"))
                    {
                        AppName = line.Split('=')[1].Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting MSI info: {ex.Message}");
        }
        finally
        {
            // Ensure log file is deleted
            if (File.Exists(logFilePath))
            {
                try
                {
                    File.Delete(logFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to delete log file: {ex.Message}");
                }
            }
        }
    }

    private void ExtractExeInfo(string filePath)
    {
        try
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            Version = NormalizeVersion(versionInfo.ProductVersion ?? "1.0.0");
            AppName = versionInfo.ProductName ?? "UnknownApp";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting EXE info: {ex.Message}");
        }
    }

    private string NormalizeVersion(string version)
    {
        string[] parts = version.Split('.');
        if (parts.Length == 1) return $"{parts[0]}.0.0";
        if (parts.Length == 2) return $"{parts[0]}.{parts[1]}.0";
        return $"{parts[0]}.{parts[1]}.{parts[2]}";
    }
}
