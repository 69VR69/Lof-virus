using UnityEngine;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem> {
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundsSource;

    [SerializeField] private AudioSource _chillMusic;
    [SerializeField] private AudioSource _drunkMusic;


    public void PlayMusic(AudioClip clip) {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1) {
        _soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1) {
        _soundsSource.PlayOneShot(clip, vol);
    }

    public void Awake()
    {
        _drunkMusic.Stop();
        _chillMusic.Play();
    }

    public void OnStateChange(GameManager.GameState state)
    {
        if(state == GameManager.GameState.StartOfTurn)
        {
            if (!_drunkMusic.isPlaying)
            {
                _chillMusic.Stop();
                _drunkMusic.Play();
            }
        }
        else if(state == GameManager.GameState.Win || state == GameManager.GameState.GameOver)
        {
            _drunkMusic.Stop();
            _chillMusic.Play();
        }
        else if (state == GameManager.GameState.WaitingForMakeLaugh)
        {
            if (!_chillMusic.isPlaying)
            {
                _drunkMusic.Stop();
                _chillMusic.Play();
            }
        }
    }
}