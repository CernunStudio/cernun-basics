using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigHandler : EditorWindow
{
    private VisualElement container;

    private VisualElement configList;
    private VisualElement configView;

    private VisualElement pathContent;
    private VisualElement audioCategContent;
    private VisualElement audioSubCategContent;

    private ScrollView configListScrollView;

    private Label selectedLabel;

    // Definir les variables pour l'affichage
    private const string tabClassName = "tab";
    private const string currentlySelectedTabClassName = "selectedTab";
    private const string unselectedContentClassName = "unselectedContent";
    private const string tabNameSuffix = "Tab";
    private const string contentNameSuffix = "Content";

    // Add menu named "Cernun Basics" to the Window menu

    [MenuItem("Cernun Basics/Configuration")]
    private static void ShowWindow()
    {
        ConfigHandler configHandler = GetWindow<ConfigHandler>();
        configHandler.titleContent = new GUIContent("Configuration");

        configHandler.minSize = new Vector2(900, 600);
        configHandler.maxSize = new Vector2(900, 600);
    }

    private void CreateGUI()
    {
        container = rootVisualElement;

        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.ChangeExtension(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)), "uxml"));
        container.Add(visualTreeAsset.Instantiate());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.ChangeExtension(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)), "uss"));
        container.styleSheets.Add(styleSheet);

        configList = container.Q<VisualElement>("ConfigList");
        configView = container.Q<VisualElement>("ConfigView");

        configListScrollView = new ScrollView();
        configList.Add(configListScrollView);

        // Bloc Path
        Label pathTabLabel = new()
        {
            name = "Path" + tabNameSuffix,
            text = "Path",
        };
        configListScrollView.Add(pathTabLabel);
        pathTabLabel.AddToClassList(tabClassName);
        pathTabLabel.RegisterCallback<ClickEvent>(OnClick);
        pathContent = new()
        {
            name = "Path" + contentNameSuffix,
            style = {
                width = configView.style.width,
                height = configView.style.height,
                alignContent = Align.FlexStart,
            }
        };
        GeneratePathContent();
        configView.Add(pathContent);
        SelectTab(pathTabLabel);

        // Bloc Audio
        Foldout audioFoldout = new()
        {
            text = "Audio"
        };

        // Bloc Categorie
        Label audioCategTabLabel = new()
        {
            name = "AudioCategory" + tabNameSuffix,
            text = "Category",
        };
        audioFoldout.Add(audioCategTabLabel);
        audioCategTabLabel.AddToClassList(tabClassName);
        audioCategTabLabel.RegisterCallback<ClickEvent>(OnClick);
        audioCategContent = new()
        {
            name = "AudioCategory" + contentNameSuffix,
            style = {
                width = configView.style.width,
                height = configView.style.height
            }
        };
        GenerateAudioCategContent();
        configView.Add(audioCategContent);
        UnselectTab(audioCategTabLabel);

        // Bloc Sous categorie
        Label audioSubCategTabLabel = new()
        {
            name = "AudioSubCategory" + tabNameSuffix,
            text = "SubCategory",
        };
        audioFoldout.Add(audioSubCategTabLabel);
        audioSubCategTabLabel.AddToClassList(tabClassName);
        audioSubCategTabLabel.RegisterCallback<ClickEvent>(OnClick);
        audioSubCategContent = new()
        {
            name = "AudioSubCategory" + contentNameSuffix,
            style = {
                width = configView.style.width,
                height = configView.style.height
            }
        };
        GenerateAudioSubCategContent();
        configView.Add(audioSubCategContent);
        UnselectTab(audioSubCategTabLabel);

        configListScrollView.Add(audioFoldout);
    }

    private void GeneratePathContent()
    {
        Label pathContentLabel = new()
        {
            text = "Path Configuration",
            style =
            {
                marginBottom = 10,
            }
        };
        pathContent.Add(pathContentLabel);

        ScrollView pathContentScrollView = new()
        {
            style = {
                flexDirection = FlexDirection.Column,
            }
        };
        pathContent.Add(pathContentScrollView);

        GeneratePathContentScrollView(pathContentScrollView);

        VisualElement buttons = new()
        {
            style =
            {
                flexDirection = FlexDirection.RowReverse,
                marginTop = 10,
            }
        };

        Button resetButton = new() { 
            text = "Reset",
            style = {
                paddingBottom = 10,
                paddingLeft = 10,
                paddingRight = 10,
                paddingTop = 10,
            }
        };
        resetButton.clicked += () =>
        {
            CernunBasicConfig.ResetConfig();
            GeneratePathContentScrollView(pathContentScrollView);
        };

        buttons.Add(resetButton);
        pathContent.Add(buttons);
    }

    private void GeneratePathContentScrollView(ScrollView pathContentScrollView)
    {
        pathContentScrollView.Clear();

        List<PropertyInfo> properties = typeof(CernunBasicConfig).GetProperties(BindingFlags.NonPublic | BindingFlags.Static).ToList()
            .OrderBy((properties) => properties.GetValue(null)).ToList();
        properties.ForEach((propertyInfo) => {
            VisualElement element = new()
            {
                style = {
                    flexDirection = FlexDirection.Row,
                    marginBottom = 5,
                }
            };
            Label label = new()
            {
                name = propertyInfo.Name,
                text = GenerateLabelText(propertyInfo.Name),
                style = {
                    flexGrow = 1,
                }
            };
            TextField textField = new()
            {
                name = propertyInfo.Name,
                value = propertyInfo.GetValue(null).ToString(),
                style = {
                    flexGrow = 3,
                }
            };
            textField.RegisterValueChangedCallback((evt) => {
                propertyInfo.SetValue(null, evt.newValue);
            });
            element.Add(label);
            element.Add(textField);
            pathContentScrollView.Add(element);
        });
    }

    private void GenerateAudioCategContent()
    {
        Label audioCategContentLabel = new()
        {
            text = "Audio Category Configuration",
            style =
            {
                marginBottom = 10,
            }
        };
        audioCategContent.Add(audioCategContentLabel);

        TextField audioCategTextField = new()
        {
            name = "AudioCategory",
            value = AudioEnumHandler.GetListAudioCategInOneString(),
            multiline = true
        };
        audioCategTextField.RegisterCallback<FocusOutEvent>((evt) =>
        {
            string value = audioCategTextField.text;
            List<string> list = value.Split('\n').ToList();
            AudioEnumHandler.UpdateAudioCateg(list);
            audioCategTextField.value = AudioEnumHandler.GetListAudioCategInOneString();
        });
        audioCategContent.Add(audioCategTextField);

        VisualElement buttons = new()
        {
            style =
            {
                flexDirection = FlexDirection.RowReverse,
                marginTop = 10,
            }
        };

        Button resetButton = new()
        {
            text = "Reset",
            style = {
                paddingBottom = 10,
                paddingLeft = 10,
                paddingRight = 10,
                paddingTop = 10,
            }
        };
        resetButton.clicked += () =>
        {
            AudioEnumHandler.ResetAudioCateg();
        };

        buttons.Add(resetButton);
        audioCategContent.Add(buttons);
    }

    private void GenerateAudioSubCategContent()
    {
        Label audioCategContentLabel = new()
        {
            text = "Audio SubCategory Configuration",
            style =
            {
                marginBottom = 10,
            }
        };
        audioSubCategContent.Add(audioCategContentLabel);

        TextField audioSubCategTextField = new()
        {
            name = "AudioSubCategory",
            value = AudioEnumHandler.GetListAudioSubCategInOneString(),
            multiline = true
        };
        audioSubCategTextField.RegisterCallback<FocusOutEvent>((evt) =>
        {
            string value = audioSubCategTextField.text;
            List<string> list = value.Split('\n').ToList();
            AudioEnumHandler.UpdateAudioSubCateg(list);
            audioSubCategTextField.value = AudioEnumHandler.GetListAudioSubCategInOneString();
        });
        audioSubCategContent.Add(audioSubCategTextField);

        VisualElement buttons = new()
        {
            style =
            {
                flexDirection = FlexDirection.RowReverse,
                marginTop = 10,
            }
        };

        Button resetButton = new()
        {
            text = "Reset",
            style = {
                paddingBottom = 10,
                paddingLeft = 10,
                paddingRight = 10,
                paddingTop = 10,
            }
        };
        resetButton.clicked += () =>
        {
            AudioEnumHandler.ResetAudioSubCateg();
        };

        buttons.Add(resetButton);
        audioSubCategContent.Add(buttons);
    }

    private string GenerateLabelText(string text)
    {
        string returnText = "";
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (char.IsUpper(c))
            {
                if (i == 0)
                {
                    returnText += c;
                    continue;
                }
                if (string.Compare(text.Substring(i - 1, 2), "SO") == 0 ||
                    string.Compare(text.Substring(i - 1, 2), "GO") == 0 ||
                    string.Compare(text.Substring(i - 1, 2), "UI") == 0)
                {
                    returnText += c;
                    continue;
                }
                returnText += " ";
            }
            returnText += c;
        }
        return returnText;
    }

    private void OnClick(ClickEvent evt)
    {
        Label clickedTab = evt.currentTarget as Label;
        if (!TabIsCurrentlySelected(clickedTab))
        {
            //currentPath = null;
            if (selectedLabel != null)
                selectedLabel.style.color = new Color(210, 210, 210);
            selectedLabel = null;

            GetAllTabs().Where(
                (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)
            ).ForEach(UnselectTab);
            SelectTab(clickedTab);
        }
    }

    private bool TabIsCurrentlySelected(Label clickedTab)
    {
        return clickedTab.ClassListContains(currentlySelectedTabClassName);
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
