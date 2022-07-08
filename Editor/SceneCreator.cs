using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneCreator : CernunWindow
{
    string sceneName;

    string[] paths;

    Dictionary<PathInfo, bool> controllersToggle;
    Dictionary<PathInfo, int> othersCount;

    [MenuItem("Cernun Basics/Scene Maker")]
    static void Init()
    {
        GetWindowWithRect<SceneCreator>(new Rect(0, 0, 600, 400)).Show();
    }

    private void OnEnable()
    {
        sceneName = "new Scene";

        paths = AssetDatabase.FindAssets("t:GameObject", new string[] { "Assets/Prefabs" });

        controllersToggle = new Dictionary<PathInfo, bool>();
        othersCount = new Dictionary<PathInfo, int>();

        foreach (string path in paths)
        {
            PathInfo info = new PathInfo(path);
            if (info.PathName.Contains("Controller"))
            {
                controllersToggle.Add(info, false);
            }
            else
            {
                othersCount.Add(info, 0);
            }
        }
    }

    void OnGUI()
    {
        // Initialisation des paramètres pour l'éditeur
        posY = 5;
        ChangeLineColor(Color.gray);

        DrawTextBox(new Rect(10, posY, position.width - 20, 20), "Nom de la scene", ref sceneName);

        DrawHorizontalSeparator(position.width - 20);

        // Liste des prefab en deux colones
        DrawColumnSelector<bool, int>(ref controllersToggle, ref othersCount);

        DrawHorizontalSeparator(position.width - 20);

        // Récapitulatif
        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUI.LabelField(new Rect(0, posY, position.width, 20), "Recapitulatif", centeredStyle);
        posY += 25;

        DrawHorizontalSeparator(position.width - 20);

        GUIStyle boldStyle = GUI.skin.GetStyle("Label");
        boldStyle.alignment = TextAnchor.MiddleLeft;
        boldStyle.fontStyle = FontStyle.Bold;

        foreach (KeyValuePair<PathInfo, bool> item in controllersToggle)
        {
            if (item.Value)
            {
                EditorGUI.LabelField(new Rect(10, posY, position.width / 2 - 20, 20), item.Key.PathName + " x1", boldStyle);
                posY += 25;
            }
        }

        foreach (KeyValuePair<PathInfo, int> item in othersCount)
        {
            if (item.Value > 0)
            {
                EditorGUI.LabelField(new Rect(10, posY, position.width / 2 - 20, 20), item.Key.PathName + " x" + item.Value);
                posY += 25;
            }
        }

        DrawHorizontalSeparator(position.width - 20);

        ButtonDrawer(position.width / 2 - 200, 400, "Generate Scene", GenerateScene);

        DrawHorizontalSeparator(position.width - 20);
    }

    void GenerateScene()
    {
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

        foreach (KeyValuePair<PathInfo, bool> item in controllersToggle)
        {
            if (item.Value)
            {
                PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(item.Key.AssetPath, typeof(GameObject)), newScene);
            }
        }

        foreach (KeyValuePair<PathInfo, int> item in othersCount)
        {
            if (item.Value > 0)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(item.Key.AssetPath, typeof(GameObject)), newScene);
                }
            }
        }

        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/" + sceneName + ".unity");
    }
}
