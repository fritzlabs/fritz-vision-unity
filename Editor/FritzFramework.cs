using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class FritzFramework
{
    public string name;

    public FritzFramework(string name)
    {
        this.name = name;
    }

    public string Version
    {
        get
        {
            var frameworkPath = "Assets/Plugins/iOS/Frameworks";
            var path = Path.Combine(frameworkPath, name + ".framework");

            if (!Directory.Exists(path))
            {
                return null;
            }
            var infoPath = Path.Combine(path, "Info.plist");

            Dictionary<string, object> plist = (Dictionary<string, object>)PlistCS.Plist.readPlist(infoPath);

            return (string)plist["FritzSDKVersion"];
        }

    }


}
