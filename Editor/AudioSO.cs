using UnityEngine;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Cernun Basics/Audio (SO)")]
public class AudioSO : ScriptableObject
{
    public string audioName;

    public AudioClip audioClip;

    public ESoundCategory category;

    public ESoundSubCategory subCategory;

    public bool isLooping;

}
