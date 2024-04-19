using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour {

    //  For saving settings
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    private const float VOLUME_INCREASE_AMOUNT = .1f;
    private const float VOLUME_MAX_AMOUNT = 1f;
    private const float VOLUME_MIN_AMOUNT = 0f;

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float volume = 0.7f;

    private void Awake() {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        audioSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += VOLUME_INCREASE_AMOUNT;

        if (volume > VOLUME_MAX_AMOUNT) {
            volume = VOLUME_MIN_AMOUNT;
        }

        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);

        //  This will force Unity to save the Player Prefs in case the game crashes
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }

}
