using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PrefabCreator : CernunWindow
{
    string prefabName = "new Prefab";

    string[] paths;

    Dictionary<PathInfo, bool> editorToogle;
    Dictionary<PathInfo, bool> otherToogle;

    //int selectedTab;

    [MenuItem("Cernun Basics/Prefab Maker")]
    static void Init()
    {
        PrefabCreator window = GetWindowWithRect<PrefabCreator>(new Rect(0, 0, 600, 400));
        window.minSize = new Vector2(50, 50);
        window.maxSize = new Vector2(1200, 600);
        window.Show();
    }

    private void OnEnable()
    {
        CernunBasicUtils.ControlPath(CernunBasicConfig.ScriptPath);
        paths = AssetDatabase.FindAssets("t:TextAsset", new string[] { CernunBasicConfig.ScriptPath });

        editorToogle = new Dictionary<PathInfo, bool>();
        otherToogle = new Dictionary<PathInfo, bool>();

        foreach (string path in paths)
        {
            PathInfo info = new (path);
            if (info.Folder == "CernunEditor")
            {
                editorToogle.Add(info, false);
            }
            else
            {
                otherToogle.Add(info, false);
            }
        }
    }

    private void OnGUI()
    {
        // Initialisation des paramètres
        GUIInitialize(position.width, position.height, 5f, 10f);

        ChangeLineColor(Color.gray);

        DrawTextBox("Nom du prefab", ref prefabName);

        DrawTab(new Dictionary<string, Dictionary<PathInfo, bool>>() { { "Editor" , editorToogle  }, { "Other" , otherToogle } });

        DrawHorizontalSeparator();

        ButtonDrawer(400, "Generate Prefab", GeneratePrefab);

        DrawVerticalScrollBar();
    }

    void GeneratePrefab()
    {
        GameObject go = new (prefabName);

        foreach (KeyValuePair<PathInfo, bool> item in otherToogle)
        {
            if (item.Value)
            {
                Type t = CernunBasicUtils.GetDynamicType(item.Key.PathName);
                if (t != null)
                {
                    go.AddComponent(t);
                }
            }
        }

        foreach (KeyValuePair<PathInfo, bool> item in editorToogle)
        {
            if (item.Value)
            {
                Type t = CernunBasicUtils.GetDynamicType(item.Key.PathName);
                if (t != null)
                {
                go.AddComponent(t);
                }
            }
        }

        CernunBasicUtils.ControlPath(CernunBasicConfig.PrefabPath);

        PrefabUtility.SaveAsPrefabAsset(go, CernunBasicConfig.PrefabPath  + "/ " + prefabName + ".prefab");
        DestroyImmediate(go);
    }
}
