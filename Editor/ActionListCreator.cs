using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.IO;

internal class ActionListCreator
{
    [MenuItem("Assets/Cernun Basics/Create Action List")]
    internal static void CreateActionList()
    {
        if(Selection.assetGUIDs.Length == 0)
            return;

        string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        string[] folderContent = Directory.GetFiles(path);

        string actionListName = path.Split('/').Last() + "ActionList";
        string interfaceName = "I" + path.Split('/').Last();

        List<string> elements = new();
        foreach (string itemContent in folderContent)
        {
            string obj = itemContent.Split("\\").Last();
            if (obj.Split(".").Last() == "meta")
                continue;

            elements.Add(obj.Split(".").First());
        }

        CernunBasicUtils.ControlPath(CernunBasicConfig.ActionListPath);

        using (StreamWriter st = new(CernunBasicConfig.ActionListPath + "/" + actionListName + ".cs"))
        {
            st.WriteLine("public class " + actionListName);
            st.WriteLine("{");
            st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + "private " + actionListName + "( " + interfaceName + " action) { Action = action; }");
            st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + "public " + interfaceName + " Action { get; private set; }");

            foreach (var item in elements)
            {
                st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + "public static " + actionListName + " " + item + " { get { return new " + actionListName + "(new " + item + "()); } }");
            }

            st.WriteLine();
            st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + "public static " + interfaceName + "[] ActionList = {");
            foreach (var item in elements)
            {
                st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + CernunBasicConfig._DEFAULT_INDENTATION + item + ".Action,");
            }
            st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + "};");

            st.WriteLine();
            st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + "public enum EActionList{");
            foreach (var item in elements)
            {
                st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + CernunBasicConfig._DEFAULT_INDENTATION + item + ",");
            }
            st.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + "}");

            st.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }
}

