using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioHandler : EditorWindow
{
    private VisualElement container;

    private Button addAudioButton;
    private Button refreshButton;
    private Button updateListButton;

    private PathInfo currentPath;

    private Label selectedLabel;

    private Dictionary<string, VisualElement> contentDictionnary = new ();

    /* Define member variables*/
    private const string tabClassName = "tab";
    private const string currentlySelectedTabClassName = "selectedTab";
    private const string unselectedContentClassName = "unselectedContent";
    // Tab and tab content have the same prefix but different suffix
    // Define the suffix of the tab name
    private const string tabNameSuffix = "Tab";
    // Define the suffix of the tab content name
    private const string contentNameSuffix = "Content";

    [MenuItem("Cernun Basics/Audio Handler")]
    private static void ShowWindow()
    {
        InitPaths();

        AudioHandler audioHandler = GetWindow<AudioHandler>();
        audioHandler.titleContent = new GUIContent("Audio Handler");

        audioHandler.minSize = new Vector2(900, 600);
        audioHandler.maxSize = new Vector2(900, 600);
    }

    private static void InitPaths()
    {
        if (!AssetDatabase.IsValidFolder(CernunBasicConfig.AudioSOPath))
        {
            CernunBasicUtils.InitializePath(CernunBasicConfig.AudioSOPath);
        }
        string[] listCateg = Enum.GetNames(typeof(ESoundCategory));
        string[] listSubCateg = Enum.GetNames(typeof(ESoundSubCategory));
        foreach (var categItem in listCateg)
        {
            foreach (var subCategItem in listSubCateg)
            {
                if (!subCategItem.Equals(CernunBasicConfig._DEFAULT_RESERVED_TERM))
                    CernunBasicUtils.InitializePath(CernunBasicConfig.AudioSOPath + "/" + categItem + "/" + subCategItem);
            }
        }
    }
    
    private void CreateGUI()
    {
        container = rootVisualElement;

        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.ChangeExtension(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)), "uxml"));
        container.Add(visualTreeAsset.Instantiate());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.ChangeExtension(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)), "uss"));
        container.styleSheets.Add(styleSheet);

        VisualElement tabsContainer = container.Q<VisualElement>("Tabs");
        VisualElement contentsContainer = container.Q<VisualElement>("Contents");

        // Récupération du AudioInformationDisplay
        string[] pathElem = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).Split('/');
        pathElem[^1] = "AudioInformationDisplay.uxml";
        string pathAudioInfo = string.Join("/", pathElem);
        VisualTreeAsset audioInfoAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(pathAudioInfo);

        Label firstLabel = new();

        // Création des tabs et contents
        List<string> listAudioCateg = AudioEnumHandler.GetListAudioCategInListString();
        foreach (var item in listAudioCateg)
        {
            Label label = new ()
            {
                text = item,
                name = item + tabNameSuffix,
            };
            label.AddToClassList(tabClassName);
            tabsContainer.Add(label);

            if (item == listAudioCateg.First())
                firstLabel = label;

            VisualElement content = new()
            {
                name = item + contentNameSuffix,
            };
            content.AddToClassList(unselectedContentClassName);
            content.Add(audioInfoAsset.Instantiate());

            VisualElement infoContainer = content.Q<VisualElement>("AudioInformationDetail");
            Button updateButton = infoContainer.Q<Button>("UpdateButton");
            updateButton.clicked += delegate ()
            {
                MajAudio(infoContainer);
            };
            Button deleteButton = infoContainer.Q<Button>("DeleteButton");
            deleteButton.clicked += delegate ()
            {
                DeleteAudio();
            };
            contentDictionnary.Add(item, content);

            contentsContainer.Add(content);
        }

        SelectTab(firstLabel);

        UQueryBuilder<Label> tabs = GetAllTabs();
        tabs.ForEach((Label tab) => {
            tab.RegisterCallback<ClickEvent>(TabOnClick);
        });

        RefreshFoldout();
        
        container.Query<DropdownField>("CategoryDropdown").ForEach(field => {
            field.choices = new();
            foreach (var item in Enum.GetNames(typeof(ESoundCategory)))
            {
                field.choices.Add(item);
            }
        });
        container.Query<DropdownField>("SubCategoryDropdown").ForEach(field => {
            field.value = null;
            field.choices = new();
            foreach (var item in Enum.GetNames(typeof(ESoundSubCategory)))
            {
                if (!item.Equals(CernunBasicConfig._DEFAULT_RESERVED_TERM))
                {
                    field.choices.Add(item);
                }
                else
                {
                    field.choices.Add(" --- ");
                }
            }
        });
        
        addAudioButton = container.Q<Button>("NewAudioButton");
        addAudioButton.clicked += delegate ()
        {
            NewAudio window = ScriptableObject.CreateInstance<NewAudio>();
            window.Show();
        };

        NewAudio.OnCloseTrigger += RefreshFoldout;

        refreshButton = container.Q<Button>("RefreshButton");
        refreshButton.clicked += RefreshFoldout;

        updateListButton = container.Q<Button>("UpdateListButton");
        updateListButton.clicked += CernunBasicUtils.UpdateListAudio;
        
    }

    private void RefreshFoldout()
    {
        foreach (KeyValuePair<string, VisualElement> item in contentDictionnary)
        {
            DisplayFoldoutContent(item.Key, item.Value);
            RefreshAudioInformation(item.Value);
        }
    }

    private void DisplayFoldoutContent(string categ, VisualElement display)
    {
        VisualElement audioListVisualElement = display.Q<VisualElement>("AudioInformationList");
        audioListVisualElement.Clear();

        CernunBasicUtils.ControlPath(CernunBasicConfig.AudioSOPath + "/" + categ);
        string[] paths = AssetDatabase.FindAssets("t:AudioSO", new string[] { CernunBasicConfig.AudioSOPath + "/" + categ + "/" });
        foreach (var path in paths)
        {
            PathInfo current = new(path);

            Label audioLabel = new(current.PathName);
            audioLabel.RegisterCallback<ClickEvent>(delegate (ClickEvent evt)
            {
                currentPath = current;
                DisplayAudioInformation(current, display);
                if (selectedLabel != null)
                    selectedLabel.style.color = new Color(210, 210, 210);
                selectedLabel = audioLabel;
                selectedLabel.style.color = Color.cyan;
            });

            if (current.Folder == categ)
            {
                Foldout subFold = audioListVisualElement.Q<Foldout>("No Category");
                subFold ??= new()
                {
                    name = "No Category",
                    text = "No Category"
                };
                audioListVisualElement.Add(subFold);
                subFold.Add(audioLabel);
            }
            else
            {
                Foldout subFold = audioListVisualElement.Q<Foldout>(current.Folder);
                subFold ??= new()
                {
                    name = current.Folder,
                    text = current.Folder
                };
                audioListVisualElement.Add(subFold);
                subFold.Add(audioLabel);
            }
        }
    }

    private void DisplayAudioInformation(PathInfo pathInfo, VisualElement container)
    {
        AudioSO audio = AssetDatabase.LoadAssetAtPath<AudioSO>(pathInfo.AssetPath);

        TextField nameField = container.Q<TextField>("AudioNameField");
        nameField.value = audio.audioName;

        ObjectField clipField = container.Q<ObjectField>("AudioClipField");
        clipField.value = audio.audioClip;

        DropdownField categoryDropdown = container.Q<DropdownField>("CategoryDropdown");
        categoryDropdown.value = audio.category.ToString();

        DropdownField subCategoryDropdown = container.Q<DropdownField>("SubCategoryDropdown");
        if (audio.subCategory != ESoundSubCategory.NONE)
            subCategoryDropdown.value = audio.subCategory.ToString();
        else
            subCategoryDropdown.value = " --- ";

    }

    private void RefreshAudioInformation(VisualElement container)
    {
        container.Q<TextField>("AudioNameField").value = "";
        container.Q<ObjectField>("AudioClipField").value = null;
        container.Q<DropdownField>("CategoryDropdown").value = "";
        container.Q<DropdownField>("SubCategoryDropdown").value = "";
    }

    private void MajAudio(VisualElement container)
    {
        if (currentPath == null)
            return;

        AudioSO audio = CreateInstance<AudioSO>();
        audio.audioName = container.Q<TextField>("AudioNameField").value;
        audio.audioClip = container.Q<ObjectField>("AudioClipField").value as AudioClip;
        audio.category = Enum.Parse<ESoundCategory>(container.Q<DropdownField>("CategoryDropdown").value);
        if (container.Q<DropdownField>("SubCategoryDropdown").value.Equals(" --- "))
        {
            audio.subCategory = Enum.Parse<ESoundSubCategory>(CernunBasicConfig._DEFAULT_RESERVED_TERM);
        }
        else
        {
            audio.subCategory = Enum.Parse<ESoundSubCategory>(container.Q<DropdownField>("SubCategoryDropdown").value);
        }
        audio.isLooping = audio.category == ESoundCategory.Music;

        string newPath = CernunBasicConfig.AudioSOPath + "/" + audio.category + "/";

        if (audio.subCategory != Enum.Parse<ESoundSubCategory>(CernunBasicConfig._DEFAULT_RESERVED_TERM))
            newPath += audio.subCategory + "/";

        newPath += audio.audioName + ".asset";

        DeleteFile(currentPath.AssetPath);

        AssetDatabase.CreateAsset(audio, newPath);

        AssetDatabase.Refresh();
        RefreshFoldout();
        RefreshAudioInformation(container);
    }

    private void DisplayWarningMessage(string message)
    {
        /*WarningMessage window = ScriptableObject.CreateInstance<WarningMessage>();
        window.Show();
        window.SetMessage(message);*/
    }

    private void DeleteAudio()
    {
        DeleteFile(currentPath.AssetPath);
        RefreshFoldout();
    }

    private void DeleteFile(string path)
    {
        File.Delete(path + ".meta");
        File.Delete(path);
        AssetDatabase.Refresh();
    }

    /* Method for the tab on-click event: 
       - If it is not selected, find other tabs that are selected, unselect them 
       - Then select the tab that was clicked on */
    private void TabOnClick(ClickEvent evt)
    {
        Label clickedTab = evt.currentTarget as Label;
        if (!TabIsCurrentlySelected(clickedTab))
        {
            currentPath = null;
            if (selectedLabel != null)
                selectedLabel.style.color = new Color(210, 210, 210);
            selectedLabel = null;
            //RefreshAudioInformation(musicInfoContainer);
            //RefreshAudioInformation(sfxInfoContainer);
            //RefreshAudioInformation(voiceInfoContainer);

            GetAllTabs().Where(
                (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)
            ).ForEach(UnselectTab);
            SelectTab(clickedTab);
        }
    }

    //Method that returns a Boolean indicating whether a tab is currently selected
    private static bool TabIsCurrentlySelected(Label tab)
    {
        return tab.ClassListContains(currentlySelectedTabClassName);
    }

    private UQueryBuilder<Label> GetAllTabs()
    {
        return container.Query<Label>(className: tabClassName);
    }

    /* Method for the selected tab: 
       -  Takes a tab as a parameter and adds the currentlySelectedTab class
       -  Then finds the tab content and removes the unselectedContent class */
    private void SelectTab(Label tab)
    {
        tab.AddToClassList(currentlySelectedTabClassName);
        VisualElement content = FindContent(tab);
        content.RemoveFromClassList(unselectedContentClassName);
    }

    /* Method for the unselected tab: 
       -  Takes a tab as a parameter and removes the currentlySelectedTab class
       -  Then finds the tab content and adds the unselectedContent class */
    private void UnselectTab(Label tab)
    {
        tab.RemoveFromClassList(currentlySelectedTabClassName);
        VisualElement content = FindContent(tab);
        content.AddToClassList(unselectedContentClassName);
    }

    // Method to generate the associated tab content name by for the given tab name
    private static string GenerateContentName(Label tab)
    {
        return tab.name.Replace(tabNameSuffix, contentNameSuffix);
    }

    // Method that takes a tab as a parameter and returns the associated content element
    private VisualElement FindContent(Label tab)
    {
        return container.Q(GenerateContentName(tab));
    }

}
