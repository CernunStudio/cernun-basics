using System;
using System.Collections.Generic;

internal static class CernunBasicConfig
{
    // Customization not possible
    internal const string _DEFAULT_RESERVED_TERM = "NONE";
    internal const string _DEFAULT_PATH_RESERVED_CHAR = "_";
    internal const string _DEFAULT_INDENTATION = "    ";
    internal const string _DEFAULT_CERNUN_FOLDER = "Assets/Cernun";

    // Customization possible
    private static readonly string _DEFAULT_ANIMATION_PATH = "Assets/Animations";
    private static readonly string _DEFAULT_ANIMATION_UI_PATH = "Assets/Animations/UI";
    private static readonly string _DEFAULT_ANIMATION_GO_PATH = "Assets/Animations/GameObject";
    private static readonly string _DEFAULT_MATERIAL_PATH = "Assets/Materials";
    private static readonly string _DEFAULT_MATERIAL_UI_PATH = "Assets/Materials/UI";
    private static readonly string _DEFAULT_MATERIAL_GO_PATH = "Assets/Materials/GameObject";
    private static readonly string _DEFAULT_MODEL_PATH = "Assets/Models";
    private static readonly string _DEFAULT_PHYSIC_PATH = "Assets/Physics";
    private static readonly string _DEFAULT_PREFAB_PATH = "Assets/Prefabs";
    private static readonly string _DEFAULT_SCENE_PATH = "Assets/Scenes";
    private static readonly string _DEFAULT_SCRIPT_PATH = "Assets/Scripts";
    private static readonly string _DEFAULT_SCRIPT_ENUMERATOR_PATH = "Assets/Scripts/Enumerator";
    private static readonly string _DEFAULT_SCRIPT_ACTIONLIST_PATH = "Assets/Scripts/ActionList";
    private static readonly string _DEFAULT_SCRIPT_SCRIPTABLE_PATH = "Assets/Scripts/ScriptableObjects";
    private static readonly string _DEFAULT_SCRIPT_INTERFACE_PATH = "Assets/Scripts/Interfaces";
    private static readonly string _DEFAULT_SCRIPT_UI_PATH = "Assets/Scripts/UI";
    private static readonly string _DEFAULT_SCRIPT_CONTROLLER_PATH = "Assets/Scripts/Controllers";
    private static readonly string _DEFAULT_SCRIPT_PRELOAD_PATH = "Assets/Scripts/Preload";
    private static readonly string _DEFAULT_SCRIPT_DEV_PATH = "Assets/Scripts/__DEV";
    private static readonly string _DEFAULT_SCRIPT_DEV_DEBUG_PATH = "Assets/Scripts/__DEV/DebugCommand";
    private static readonly string _DEFAULT_SHADER_PATH = "Assets/Shaders";
    private static readonly string _DEFAULT_SODATA_PATH = "Assets/SOData";
    private static readonly string _DEFAULT_SODATA_AUDIO_PATH = "Assets/SOData/AudioSO";
    private static readonly string _DEFAULT_SODATA_CHARACTER_PATH = "Assets/SOData/CharacterSO";
    private static readonly string _DEFAULT_SOUND_PATH = "Assets/Sounds";
    private static readonly string _DEFAULT_SPRITES_PATH = "Assets/Sprites";
    private static readonly string _DEFAULT_SPRITES_UI_PATH = "Assets/Sprites/UI";
    private static readonly string _DEFAULT_SPRITES_GO_PATH = "Assets/Sprites/GameObject";
    private static readonly string _DEFAULT_TEXTURES_PATH = "Assets/Textures";
    private static readonly string _DEFAULT_TEXTURES_UI_PATH = "Assets/Textures/UI";
    private static readonly string _DEFAULT_TEXTURES_GO_PATH = "Assets/Textures/GameObject";

    //Gettter Setter
    internal static string ActionListPath { get; set; } = _DEFAULT_SCRIPT_ACTIONLIST_PATH;
    internal static string AnimationPath { get; set; } = _DEFAULT_ANIMATION_PATH;
    internal static string AnimationGOPath { get; set; } = _DEFAULT_ANIMATION_GO_PATH;
    internal static string AnimationUIPath { get; set; } = _DEFAULT_ANIMATION_UI_PATH;
    internal static string AudioSOPath { get; set; } = _DEFAULT_SODATA_AUDIO_PATH;
    internal static string CharacterSOPath { get; set; } = _DEFAULT_SODATA_CHARACTER_PATH;
    internal static string ControllerPath { get; set; } = _DEFAULT_SCRIPT_CONTROLLER_PATH;
    internal static string DebugPath { get; set; } = _DEFAULT_SCRIPT_DEV_DEBUG_PATH;
    internal static string DevPath { get; set; } = _DEFAULT_SCRIPT_DEV_PATH;
    internal static string EnumeratorPath { get; set; } = _DEFAULT_SCRIPT_ENUMERATOR_PATH;
    internal static string InterfacePath { get; set; } = _DEFAULT_SCRIPT_INTERFACE_PATH;
    internal static string MaterialPath { get; set; } = _DEFAULT_MATERIAL_PATH;
    internal static string MaterialGOPath { get; set; } = _DEFAULT_MATERIAL_GO_PATH;
    internal static string MaterialUIPath { get; set; } = _DEFAULT_MATERIAL_UI_PATH;
    internal static string ModelPath { get; set; } = _DEFAULT_MODEL_PATH;
    internal static string PhysicPath { get; set; } = _DEFAULT_PHYSIC_PATH;
    internal static string PrefabPath { get; set; } = _DEFAULT_PREFAB_PATH;
    internal static string PreloadPath { get; set; } = _DEFAULT_SCRIPT_PRELOAD_PATH;
    internal static string ScenePath { get; set; } = _DEFAULT_SCENE_PATH;
    internal static string ScriptPath { get; set; } = _DEFAULT_SCRIPT_PATH;
    internal static string ScriptUIPath { get; set; } = _DEFAULT_SCRIPT_UI_PATH;
    internal static string ScriptablePath { get; set; } = _DEFAULT_SCRIPT_SCRIPTABLE_PATH;
    internal static string ShaderPath { get; set; } = _DEFAULT_SHADER_PATH;
    internal static string SODataPath { get; set; } = _DEFAULT_SODATA_PATH;
    internal static string SoundPath { get; set; } = _DEFAULT_SOUND_PATH;
    internal static string SpritePath { get; set; } = _DEFAULT_SPRITES_PATH;
    internal static string SpriteGOPath { get; set; } = _DEFAULT_SPRITES_GO_PATH;
    internal static string SpriteUIPath { get; set; } = _DEFAULT_SPRITES_UI_PATH;
    internal static string TexturePath { get; set; } = _DEFAULT_TEXTURES_PATH;
    internal static string TextureGOPath { get; set; } = _DEFAULT_TEXTURES_GO_PATH;
    internal static string TextureUIPath { get; set; } = _DEFAULT_TEXTURES_UI_PATH;

    //Reinitialisation des configs de base
    internal static void ResetConfig()
    {
        ActionListPath = _DEFAULT_SCRIPT_ACTIONLIST_PATH;
        AnimationPath = _DEFAULT_ANIMATION_PATH;
        AnimationGOPath = _DEFAULT_ANIMATION_GO_PATH;
        AnimationUIPath = _DEFAULT_ANIMATION_UI_PATH;
        AudioSOPath = _DEFAULT_SODATA_AUDIO_PATH;
        CharacterSOPath = _DEFAULT_SODATA_CHARACTER_PATH;
        ControllerPath = _DEFAULT_SCRIPT_CONTROLLER_PATH;
        DebugPath = _DEFAULT_SCRIPT_DEV_DEBUG_PATH;
        DevPath = _DEFAULT_SCRIPT_DEV_PATH;
        EnumeratorPath = _DEFAULT_SCRIPT_ENUMERATOR_PATH;
        InterfacePath = _DEFAULT_SCRIPT_INTERFACE_PATH;
        MaterialPath = _DEFAULT_MATERIAL_PATH;
        MaterialGOPath = _DEFAULT_MATERIAL_GO_PATH;
        MaterialUIPath = _DEFAULT_MATERIAL_UI_PATH;
        ModelPath = _DEFAULT_MODEL_PATH;
        PhysicPath = _DEFAULT_PHYSIC_PATH;
        PrefabPath = _DEFAULT_PREFAB_PATH;
        PreloadPath = _DEFAULT_SCRIPT_PRELOAD_PATH;
        ScenePath = _DEFAULT_SCENE_PATH;
        ScriptPath = _DEFAULT_SCRIPT_PATH;
        ScriptUIPath = _DEFAULT_SCRIPT_UI_PATH;
        ScriptablePath = _DEFAULT_SCRIPT_SCRIPTABLE_PATH;
        ShaderPath = _DEFAULT_SHADER_PATH;
        SODataPath = _DEFAULT_SODATA_PATH;
        SoundPath = _DEFAULT_SOUND_PATH;
        SpritePath = _DEFAULT_SPRITES_PATH;
        SpriteGOPath = _DEFAULT_SPRITES_GO_PATH;
        SpriteUIPath = _DEFAULT_SPRITES_UI_PATH;
        TexturePath = _DEFAULT_TEXTURES_PATH;
        TextureGOPath = _DEFAULT_TEXTURES_GO_PATH;
        TextureUIPath = _DEFAULT_TEXTURES_UI_PATH;
}

    internal static List<string> GetPath()
    {
        return new List<string>
        {
            _DEFAULT_CERNUN_FOLDER,
            ActionListPath,
            AnimationPath,
            AnimationGOPath,
            AnimationUIPath,
            AudioSOPath,
            CharacterSOPath,
            ControllerPath,
            DebugPath,
            DevPath,
            EnumeratorPath,
            InterfacePath,
            MaterialPath,
            MaterialGOPath,
            MaterialUIPath,
            ModelPath,
            PhysicPath,
            PrefabPath,
            PreloadPath,
            ScenePath,
            ScriptPath,
            ScriptUIPath,
            ScriptablePath,
            ShaderPath,
            SODataPath,
            SoundPath,
            SpritePath,
            SpriteGOPath,
            SpriteUIPath,
            TexturePath,
            TextureGOPath,
            TextureUIPath,
        };
    }
}
