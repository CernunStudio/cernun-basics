using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

internal static class CernunBasicUtils
{
    [MenuItem("Assets/Cernun Basics/Initialize Paths")]
    internal static void PathInitializer()
    {
        List<string> listOfPathToInitialize = CernunBasicConfig.GetPath();
        listOfPathToInitialize.AddRange(GetAudioPath());

        foreach (string path in listOfPathToInitialize)
        {
            InitializePath(path);
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Cernun Basics/Refresh List Audio")]
    internal static void UpdateListAudio()
    {
        InitializePath(CernunBasicConfig.EnumeratorPath);

        string[] listCateg = Enum.GetNames(typeof(ESoundCategory));

        using (StreamWriter st = new(CernunBasicConfig.EnumeratorPath + "/EAudio.cs"))
        {
            string[] paths;

            st.WriteLine("public enum EAudio");
            st.WriteLine("{");

            foreach (var item in listCateg)
            {
                paths = AssetDatabase.FindAssets("t:AudioSO", new string[] { CernunBasicConfig.AudioSOPath + "/" + item + "/" });
                foreach (var path in paths)
                {
                    PathInfo pathInfo = new(path);

                    string line = CernunBasicConfig._DEFAULT_INDENTATION + item + CernunBasicConfig._DEFAULT_PATH_RESERVED_CHAR;
                    line += pathInfo.Folder == item ? CernunBasicConfig._DEFAULT_RESERVED_TERM : pathInfo.Folder;
                    line += CernunBasicConfig._DEFAULT_PATH_RESERVED_CHAR + pathInfo.PathName + ",";

                    st.WriteLine(line);
                }
            }

            st.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }

    
    internal static void InitializePath(string path)
    {
        string[] paths = SplitPath(path);

        string pastPath = "Assets";

        for (int i = 0; i < paths.Length; i++)
        {
            if (!AssetDatabase.IsValidFolder(pastPath + "/" + paths[i]))
            {
                AssetDatabase.CreateFolder(pastPath, paths[i]);
            }

            pastPath += "/";
            pastPath += paths[i];
        }
    }

    private static string[] SplitPath(string path)
    {
        string[] tempPaths = path.Split("/");

        int startIndex = 0;
        if (tempPaths[0].Equals("Assets"))
        {
            startIndex = 1;
        }

        string[] returnPaths = new string[tempPaths.Length - startIndex];

        for (int i = startIndex; i < tempPaths.Length; i++)
        {
            returnPaths[i - startIndex] = tempPaths[i];
        }

        return returnPaths;

    }

    internal static Type GetDynamicType(string nameObject)
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

    internal static List<string> GetAudioPath()
    {
        List<string> listOfAudioPath = new();

        string[] listCateg = Enum.GetNames(typeof(ESoundCategory));
        string[] listSubCateg = Enum.GetNames(typeof(ESoundSubCategory));
        foreach (var categItem in listCateg)
        {
            foreach (var subCategItem in listSubCateg)
            {
                if (!subCategItem.Equals(CernunBasicConfig._DEFAULT_RESERVED_TERM))
                {
                    listOfAudioPath.Add(CernunBasicConfig.AudioSOPath + "/" + categItem + "/" + subCategItem);
                    listOfAudioPath.Add(CernunBasicConfig.SoundPath + "/" + categItem + "/" + subCategItem);
                }
            }
        }

        return listOfAudioPath;
    }

    internal static void ControlPath(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            InitializePath(path);
        }
    }

    internal static string TextToCamelCase(string originalText)
    {
        string[] words = originalText.Split(" ");
        for (int i = 0; i < words.Length; i++)
        {
            char[] chars = words[i].ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            words[i] = new string(chars);
        }
        string result = string.Join("", words);
        return result;
    }
}
