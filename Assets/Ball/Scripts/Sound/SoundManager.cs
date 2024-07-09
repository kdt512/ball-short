using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _audioSourceSound;

    [SerializeField] private List<AudioClip> listAudioSoundClips = new List<AudioClip>();


    public void Play(SoundType soundType)
    {
        if (DataManager.IsSoundOn)
        {
            _audioSourceSound.PlayOneShot(
                listAudioSoundClips[Mathf.Clamp((int)soundType, 0, listAudioSoundClips.Count)]);
        }
    }
}

public enum SoundType
{
    CLICK,
    BALL_TOP,
    BALL_DOWN,
    FINISH_TUBE,
    FINISH_GAME
}