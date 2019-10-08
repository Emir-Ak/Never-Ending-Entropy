using UnityEngine;

[System.Serializable]
public class Sound //Class used for storing data about sounds that will later be assigned to the relative AudioSource
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3.0f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
