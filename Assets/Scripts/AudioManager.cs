using UnityEngine;
using System.Collections;

public static class AudioManager {

    private static AudioSource _audioSource;

    private static AudioClip _winSound,
                             _loseSound,
                             _tabSound;

    public static void Init() {
        _audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        _winSound = Resources.Load<AudioClip>("Sounds/Xonix_Win");
        _loseSound = Resources.Load<AudioClip>("Sounds/Xonix_Lose");
        _tabSound = Resources.Load<AudioClip>("Sounds/Xonix_Scoretab");
    }

    public static void PlaySoundType(Enumerators.SoundType soundType) {
        switch (soundType) {
            case Enumerators.SoundType.WIN:
                _audioSource.PlayOneShot(_winSound);
                break;
            case Enumerators.SoundType.LOSE:
                _audioSource.PlayOneShot(_loseSound);
                break;
            case Enumerators.SoundType.TAB:
                _audioSource.PlayOneShot(_tabSound);
                break;
        }
    }
}
