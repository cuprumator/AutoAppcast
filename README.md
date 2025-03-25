# AutoAppcast

A **CLI tool** that **automatically generates Appcast XML** from **Markdown release notes** and **extracts metadata** from application installers (MSI/EXE). Ideal for **Sparkle Updater** or any app distribution system.

## 🚀 Features
✅ **Convert Markdown to Appcast XML** with multi-language support.  
✅ **Extract app metadata** (name, version, file size) from MSI & EXE.  
✅ **Ensures installer version format is always `x.y.z` (e.g., `1.2.3`).**  
✅ **Deletes MSI log files after extraction to prevent clutter.**  
✅ **Generate valid Sparkle-compatible Appcast feeds.**  
✅ **Auto-encode HTML properly for XML compatibility.**  
✅ **Flexible CLI with multiple customization options.**  

---

## 📌 **Installation**
### **1️⃣ Clone the repository**
```sh
git clone https://github.com/yourusername/AutoAppcast.git
cd AutoAppcast
```

### **2️⃣ Build the project**
```sh
dotnet build -c Release
```

### **3️⃣ Run the CLI**
```sh
dotnet run -- -i release-notes.md -o appcast.xml -f MyAppInstaller.msi
```

---

## 📌 **Usage**
Run the tool with the following parameters:

```sh
AutoAppcast -i input.md [-o output.xml] [-v version] [-d date] [-a app_name] [-u app_url] [-f installer_file] [-url file_url]
```

### **Options**
| Flag | Description | Example |
|------|------------|---------|
| `-i <file>` | Input Markdown file (**Required**) | `-i release-notes.md` |
| `-o <file>` | Output Appcast XML file (**Default: appcast.xml**) | `-o myappcast.xml` |
| `-v <version>` | App version (**Default: Extracted from installer or `1.0.0`**) | `-v 3.2.0` |
| `-d <date>` | Release date (**Default: Now, in RFC 822 format**) | `-d "Tue, 11 Mar 2025 10:40:31 GMT"` |
| `-a <name>` | Application name (**Default: Extracted from installer or `UnknownApp`**) | `-a "MyApp"` |
| `-u <url>` | Application website URL (**Default: https://example.com**) | `-u "https://myapp.com"` |
| `-f <file>` | Path to the installer file (EXE/MSI) for metadata extraction | `-f MyAppInstaller.msi` |
| `-url <url>` | Public download URL for the installer | `-url "https://myapp.com/download/MyAppInstaller.msi"` |
| `-h, --help` | Show help message and exit | `-h` |

---

## 📌 **Examples**
### **1️⃣ Generate Appcast from Markdown with custom output file**
```sh
AutoAppcast -i release-notes.md -o myappcast.xml
```

### **2️⃣ Generate Appcast with extracted metadata from an MSI installer**
```sh
AutoAppcast -i release-notes.md -f MyAppInstaller.msi -url "https://myapp.com/download/MyAppInstaller.msi"
```

### **3️⃣ Generate Appcast with extracted metadata from an EXE installer**
```sh
AutoAppcast -i release-notes.md -f MyAppInstaller.exe -a "MyApp" -u "https://myapp.com"
```

### **4️⃣ Full command with all options**
```sh
AutoAppcast -i release-notes.md -o myappcast.xml -v 3.2.0 -d "Tue, 11 Mar 2025 10:40:31 GMT" -a "MyApp" -u "https://myapp.com" -f MyAppInstaller.exe -url "https://myapp.com/download/MyAppInstaller.exe"
```

---

## 📌 **How It Works**
1️⃣ **Reads Markdown release notes** and converts them to **HTML**.  
2️⃣ **Extracts metadata** (app name, version, size) from an **MSI/EXE installer** if provided.  
3️⃣ **Ensures version follows `x.y.z` format (e.g., `1.2.3`).**  
4️⃣ **Deletes temporary MSI log files to prevent clutter.**  
5️⃣ **Encodes HTML for **XML safety** and generates a valid Appcast XML file.  

---

## 📌 **Output Example (`appcast.xml`)**
```xml
<?xml version="1.0" encoding="UTF-8"?>
<rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:sparkle="http://www.andymatuschak.org/xml-namespaces/sparkle">
    <channel>
        <title>MyApp Releases</title>
        <link>https://myapp.com/appcast.xml</link>
        <description>Latest updates for MyApp</description>
        <language>en-us</language>
        <item>
            <title>Version 3.2.0</title>
            <description>
                <div lang="en">
                    <h2>What's New in MyApp?</h2>
                    <ul>
                        <li>Fixed bugs and improved performance.</li>
                        <li>Improved connection stability.</li>
                    </ul>
                </div>
            </description>
            <pubDate>Tue, 11 Mar 2025 10:40:31 GMT</pubDate>
            <enclosure sparkle:version="3.2.0" url="https://myapp.com/download/MyAppInstaller.exe" length="12345678" type="application/octet-stream"/>
        </item>
    </channel>
</rss>
```

---

## 📌 **License**
📜 MIT License - Free to use, modify, and distribute.

---

## 📌 **Contributing**
💡 Pull requests are welcome!  
📢 If you find a bug or have feature suggestions, open an **issue** on GitHub.  
