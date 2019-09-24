﻿using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;

public class AddToAndroidManifest : IPostGenerateGradleAndroidProject
{

	public void OnPostGenerateGradleAndroidProject(string basePath)
	{
		// If needed, add condition checks on whether you need to run the modification routine.
		// For example, specific configuration/app options enabled

		var androidManifest = new AndroidManifest(GetManifestPath(basePath));

		FritzConfiguration config = FritzConfiguration.GetOrCreateSettings();

		androidManifest.AddAPIKey(config.androidAPIKey);

		// Add your XML manipulation routines

		androidManifest.Save();
	}

	public int callbackOrder { get { return 1; } }

	private string _manifestFilePath;

	private string GetManifestPath(string basePath)
	{
		if (string.IsNullOrEmpty(_manifestFilePath))
		{
			var pathBuilder = new StringBuilder(basePath);
			pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
			pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
			pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
			_manifestFilePath = pathBuilder.ToString();
		}
		return _manifestFilePath;
	}
}


internal class AndroidXmlDocument : XmlDocument
{
	private string m_Path;
	protected XmlNamespaceManager nsMgr;
	public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
	public AndroidXmlDocument(string path)
	{
		m_Path = path;
		using (var reader = new XmlTextReader(m_Path))
		{
			reader.Read();
			Load(reader);
		}
		nsMgr = new XmlNamespaceManager(NameTable);
		nsMgr.AddNamespace("android", AndroidXmlNamespace);
	}

	public string Save()
	{
		return SaveAs(m_Path);
	}

	public string SaveAs(string path)
	{
		using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
		{
			writer.Formatting = Formatting.Indented;
			Save(writer);
		}
		return path;
	}
}


internal class AndroidManifest : AndroidXmlDocument
{
	private readonly XmlNode ApplicationElement;

	public AndroidManifest(string path) : base(path)
	{
		ApplicationElement = SelectSingleNode("/manifest/application");
	}

	private XmlAttribute CreateAndroidAttribute(string key, string value)
	{
		XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
		attr.Value = value;
		return attr;
	}

	internal void AddAPIKey(string apiKey)
	{
		XmlNode newMetadata = this.CreateNode(XmlNodeType.Element, "meta-data", null);

		var settings = FritzConfiguration.GetSerializedSettings();
		XmlAttribute nameAttr = CreateAndroidAttribute("name", "fritz_api_key");
		XmlAttribute valAttr = CreateAndroidAttribute("value", apiKey);

		newMetadata.Attributes.Append(nameAttr);
		newMetadata.Attributes.Append(valAttr);

		ApplicationElement.AppendChild(newMetadata);
	}
}