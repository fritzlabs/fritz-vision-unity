using UnityEditor;
using System.Net;
using System.IO;
using System;
using System.Diagnostics;

// Downloads framework for a specific SDK version.
public class DownloadFramework
{

    private readonly string FRAMEWORK_FMT =
        "https://github.com/fritzlabs/swift-framework/releases/download/{0}/{1}.zip";
    public string version;
    public string name;


    public DownloadFramework(string version, string name)
    {
        this.version = version;
        this.name = name;
    }

    public void Download()
    {
        using (var client = new WebClient())
        {
            var tempFile = Path.GetTempFileName();
            var tempDir = Path.GetTempPath();

            var path = String.Format(FRAMEWORK_FMT, version, name);

            client.DownloadFile(new Uri(path), tempFile);

            ExecuteBashCommand(String.Format("unzip -o {0} -d {1}", tempFile, tempDir));
            var result = ExecuteBashCommand(
                String.Format("cp -R {0}/Frameworks/* {1}", tempDir, "Assets/Plugins/iOS/FritzVisionUnity/Frameworks/")
            );
            AssetDatabase.Refresh();
        }
    }

    static string ExecuteBashCommand(string command)
    {
        // According to: https://stackoverflow.com/a/15262019/637142
        // this will properly escape double quotes
        command = command.Replace("\"", "\"\"");

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"" + command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        proc.Start();
        proc.WaitForExit();

        return proc.StandardOutput.ReadToEnd();
    }
}