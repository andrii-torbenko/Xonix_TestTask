using UnityEngine;

public static class AudioManager {

    private static AudioSource _audioSource;

    private static AudioClip _winSound,
                             _loseSound,
                             _tabSound,
                             _music;

    public static void Init() {
        if (!StateManager.isAppStarted) {
            _audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
            _winSound = Resources.Load<AudioClip>("Sounds/Xonix_Win");
            _loseSound = Resources.Load<AudioClip>("Sounds/Xonix_Lose");
            _tabSound = Resources.Load<AudioClip>("Sounds/Xonix_Scoretab");
            _music = Resources.Load<AudioClip>("Sounds/Xonix_Music");
        }
        else {
            throw new System.NotImplementedException("Can't initialize audio manager more than once");
        }
    }

    public static void PlaySoundType(Enumerators.SoundType soundType) {
        _audioSource.Stop();
        _audioSource.loop = false;
        switch (soundType) {
            case Enumerators.SoundType.WIN:
                _audioSource.PlayOneShot(_winSound);
                break;
            case Enumerators.SoundType.LOSE:
                _audioSource.PlayOneShot(_loseSound);
                break;
            case Enumerators.SoundType.SCORETAB:
                _audioSource.PlayOneShot(_tabSound);
                break;
        }
    }
    
    public static void PlayMusic() {
        _audioSource.Stop();
        _audioSource.loop = true;
        _audioSource.clip = _music;
        _audioSource.Play();
    }

    public static void Stop() {
        _audioSource.Stop();
    }
    
    public static bool Mute() {
        if (!_audioSource.mute) {
            _audioSource.mute = true;
        }
        else {
            _audioSource.mute = false;
        }
        return _audioSource.mute;
    }
}
