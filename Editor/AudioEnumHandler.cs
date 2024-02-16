using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

internal class AudioEnumHandler
{
    private static readonly List<string> _DEFAULT_AUDIO_CATEG = new() {
        "SFX",
        "Music",
        "Voice",
        "UI",
    };

    private static readonly List<string> _DEFAULT_AUDIO_SUBCATEG = new() {
        "NONE",
    };

    internal static string GetListAudioCategInOneString()
    {
        string returnListCateg = "";
        string[] listCateg = Enum.GetNames(typeof(ESoundCategory)).OrderBy((elt) => elt.ToString()).ToArray();
        foreach (var item in listCateg)
        {
            returnListCateg += item + "\n";
        }
        return returnListCateg;
    }

    internal static string GetListAudioSubCategInOneString()
    {
        string returnListSubCateg = "";
        string[] listSubCateg = Enum.GetNames(typeof(ESoundSubCategory)).ToList().OrderBy((elt) => elt.ToString()).ToArray();
        foreach (var item in listSubCateg)
        {
            returnListSubCateg += item + "\n";
        }
        return returnListSubCateg;
    }

    internal static List<string> GetListAudioCategInListString()
    {
        return Enum.GetNames(typeof(ESoundCategory)).OrderBy((elt) => elt.ToString()).ToList();
    }

    internal static List<string> GetListAudioSubCategInListString()
    {
        return Enum.GetNames(typeof(ESoundSubCategory)).ToList().OrderBy((elt) => elt.ToString()).ToList();
    }

    internal static void ResetAudioCateg()
    {
        UpdateAudioCateg(_DEFAULT_AUDIO_CATEG);
    }

    internal static void ResetAudioSubCateg()
    {
        UpdateAudioSubCateg(_DEFAULT_AUDIO_SUBCATEG);
    }

    internal static void UpdateAudioCateg(List<string> listCateg)
    {
        using (StreamWriter file = new (CernunBasicConfig._DEFAULT_CERNUN_FOLDER + "/ESoundCategory.cs"))
        {
            file.WriteLine("public enum ESoundCategory");
            file.WriteLine("{");
            foreach (string category in listCateg)
            {
                if (category != "")
                    file.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + CernunBasicUtils.TextToCamelCase(category) + ",");
            }
            file.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }

    internal static void UpdateAudioSubCateg(List<string> listSubCateg)
    {
        using (StreamWriter file = new( CernunBasicConfig._DEFAULT_CERNUN_FOLDER + "/ESoundSubCategory.cs"))
        {
            file.WriteLine("public enum ESoundSubCategory");
            file.WriteLine("{");
            foreach (string subCategory in listSubCateg)
            {
                if (subCategory != "")
                    file.WriteLine(CernunBasicConfig._DEFAULT_INDENTATION + CernunBasicUtils.TextToCamelCase(subCategory) + ",");
            }
            file.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }
}
