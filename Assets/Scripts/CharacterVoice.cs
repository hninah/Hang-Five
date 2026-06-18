using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Character Voice", fileName="New Character Voice")]
public class CharacterVoice : ScriptableObject
{
    public string characterName;
    public AudioClip[] voiceClips;

    public AudioClip getVoiceClip()
    {
        return voiceClips[Random.Range(0, voiceClips.Length)];
    }
}
