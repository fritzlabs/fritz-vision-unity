using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Net;
using System.IO;
using System.IO.Compression;
using System;

using System.Diagnostics;

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
			var tempPath = Path.GetTempFileName();
			var path = String.Format(FRAMEWORK_FMT, version, name);

			client.DownloadFile(new Uri(path), tempPath);

			ExecuteBashCommand(String.Format("unzip -o {0} -d {1}", tempPath, "tmp/"));
			var result = ExecuteBashCommand(
				String.Format("cp -R {0}/Frameworks/* {1}", "tmp/", "Assets/Plugins/iOS/FritzVisionUnity/Frameworks/")
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