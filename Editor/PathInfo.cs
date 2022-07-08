
using UnityEditor;

public class PathInfo
{
    public string Guid { get; set; }
    public string AssetPath { get; set; }
    public string Folder { get; set; }
    public string PathName { get; set; }
    public string Extension { get; set; }

    public PathInfo(string gID)
    {

        AssetPath = AssetDatabase.GUIDToAssetPath(gID);
        string[] tabPath = AssetPath.Split("/");

        Folder = tabPath[tabPath.Length -2];

        string[] tabName = tabPath[tabPath.Length - 1].Split(".");
        PathName = tabName[0];
        Extension = tabName[1];
    }
}
