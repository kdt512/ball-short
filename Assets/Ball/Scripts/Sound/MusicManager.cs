using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        if (DataManager.IsMusicOn)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
    }
}