using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class NewAudio : EditorWindow
{
    private VisualElement container;

    private TextField audioNameField;
    private ObjectField audioClipField;
    private DropdownField audioCategoryDropdown;
    private DropdownField audioSubCategoryDropdown;
    private Button addAudioButton;

    public delegate void OnClose();
    public static event OnClose OnCloseTrigger;

    private void CreateGUI()
    {
        NewAudio newAudio = GetWindow<NewAudio>();
        newAudio.titleContent = new GUIContent("New Audio");

        newAudio.minSize = new Vector2(350, 450);
        newAudio.maxSize = new Vector2(350, 450);

        container = rootVisualElement;

        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.ChangeExtension(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)), "uxml"));
        container.Add(visualTreeAsset.Instantiate());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.ChangeExtension(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)), "uss"));
        container.styleSheets.Add(styleSheet);

        audioNameField = container.Q<TextField>("AudioNameField");
        audioClipField = container.Q<ObjectField>("AudioClipSelector");

        VisualElement DropDownContainer = container.Q<VisualElement>("DropDownContainer");

        audioCategoryDropdown = new DropdownField("CategoryDropdown");
        audioSubCategoryDropdown = new DropdownField("SubCategoryDropdown");

        DropDownContainer.Add(audioCategoryDropdown);
        DropDownContainer.Add(audioSubCategoryDropdown);

        audioCategoryDropdown.choices = new();
        foreach (var item in Enum.GetNames(typeof(ESoundCategory)))
        {
            audioCategoryDropdown.choices.Add(item);
        }

        audioSubCategoryDropdown.choices = new();
        foreach (var item in Enum.GetNames(typeof(ESoundSubCategory)))
        {
            if (!item.Equals("NONE"))
                audioSubCategoryDropdown.choices.Add(item);
        }

        addAudioButton = container.Q<Button>("AddAudioButton");
        addAudioButton.clicked += AddAudio;

        if (audioSubCategoryDropdown.choices.Count == 0)
            audioSubCategoryDropdown.style.display = DisplayStyle.None;

    }

    private void AddAudio()
    {
        bool isSoCreated = true;

        if (String.IsNullOrEmpty(audioNameField.value))
        {
            audioNameField.style.borderBottomWidth = 1;
            isSoCreated = false;
        }
        else
        {
            audioNameField.style.borderBottomWidth = 0;
        }

        if (audioClipField.value == null)
        {
            audioClipField.style.borderBottomWidth = 1;
            isSoCreated = false;
        }
        else
        {
            audioClipField.style.borderBottomWidth = 0;
        }

        if (audioCategoryDropdown.value == null)
        {
            audioCategoryDropdown.style.borderBottomWidth = 1;
            isSoCreated = false;
        }
        else
        {
            audioCategoryDropdown.style.borderBottomWidth = 0;
        }

        if (isSoCreated)
        {
            AudioSO audioSO = ScriptableObject.CreateInstance<AudioSO>();

            audioSO.audioName = audioNameField.value;
            audioSO.audioClip = audioClipField.value as AudioClip;
            audioSO.category = Enum.Parse<ESoundCategory>(audioCategoryDropdown.value);
            if (audioSubCategoryDropdown.choices.Count != 0 && audioSubCategoryDropdown.value != null)
            {
                audioSO.subCategory = Enum.Parse<ESoundSubCategory>(audioSubCategoryDropdown.value);
            }
            else
            {
                audioSO.subCategory = Enum.Parse<ESoundSubCategory>(CernunBasicConfig._DEFAULT_RESERVED_TERM);
            }

            //if (audioSO.category == ESoundCategory.Music)
            //    audioSO.isLooping = true;

            string path = "Assets/SOData/AudioSO/" + audioSO.category + "/";

            if (audioSubCategoryDropdown.choices.Count != 0 && audioSubCategoryDropdown.value != null)
                path += audioSO.subCategory + "/";

            path += audioSO.audioName + ".asset";

            AssetDatabase.CreateAsset(audioSO, path);

            OnCloseTrigger?.Invoke();
            this.Close();
        }
    }
}
