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

    int selectedTab;

    [MenuItem("Cernun Basics/Prefab Maker")]
    static void Init()
    {
        GetWindowWithRect<PrefabCreator>(new Rect(0, 0, 600, 400)).Show();
    }

    private void OnEnable()
    {
        selectedTab = 0;

        paths = AssetDatabase.FindAssets("t:TextAsset", new string[] { "Assets/Scripts" });

        editorToogle = new Dictionary<PathInfo, bool>();
        otherToogle = new Dictionary<PathInfo, bool>();

        foreach (string path in paths)
        {
            PathInfo info = new PathInfo(path);
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
        posY = 5;
        ChangeLineColor(Color.gray);

        DrawTextBox(new Rect(10, posY, position.width - 20, 20), "Nom du prefab", ref prefabName);

        // Tentatives de Tab
        selectedTab = GUI.Toolbar(new Rect(10, posY, position.width - 20, 20), selectedTab, new string[]{"Editor", "Other" });

        posY += 25;

        switch (selectedTab)
        {
            case 0:
                PathInfo[] editorPaths = new PathInfo[editorToogle.Count];
                editorToogle.Keys.CopyTo(editorPaths, 0);
                foreach (PathInfo item in editorPaths)
                {
                    editorToogle[item] = EditorGUI.ToggleLeft(new Rect(10, posY, 280, 20), item.PathName, editorToogle[item]);
                    posY += 25;
                }
                break;
            case 1:
                PathInfo[] otherPaths = new PathInfo[otherToogle.Count];
                otherToogle.Keys.CopyTo(otherPaths, 0);
                foreach (PathInfo item in otherPaths)
                {
                    otherToogle[item] = EditorGUI.ToggleLeft(new Rect(10, posY, 280, 20), item.PathName, otherToogle[item]);
                    posY += 25;
                }
                break;
        }

        DrawHorizontalSeparator(position.width - 20);

        ButtonDrawer(position.width / 2 - 200, 400, "Generate Prefab", GeneratePrefab);
    }

    void GeneratePrefab()
    {
        GameObject go = new GameObject(prefabName);


        foreach (KeyValuePair<PathInfo, bool> item in otherToogle)
        {
            if (item.Value)
            {
                Type t = GetDynamicType(item.Key.PathName);
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
                Type t = GetDynamicType(item.Key.PathName);
                if (t != null)
                {
                go.AddComponent(t);
                }
            }
        }

        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        PrefabUtility.SaveAsPrefabAsset(go, "Assets/Prefabs/" + prefabName + ".prefab");
        DestroyImmediate(go);
    }

    private Type GetDynamicType(string nameObject)
    {
        Type t = null;
        foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (ass.FullName.StartsWith("System."))
                continue;
            t = ass.GetType(nameObject);
            if (t != null)
                break;
        }
        return t;
    }
}
