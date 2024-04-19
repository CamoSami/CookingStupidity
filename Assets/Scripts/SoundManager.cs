using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //  For saving settings
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    private const float VOLUME_INCREASE_AMOUNT = .1f;
    private const float VOLUME_MAX_AMOUNT = 1f;
    private const float VOLUME_MIN_AMOUNT = 0f;

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 0.7f;

    private void Awake() {
        Instance = this;

        //  Load player's saved settings
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

        Player.Instance.OnPickedUpSomething += Player_OnPickedUpSomething;

        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;

        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;

        this.PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;

        this.PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedUpSomething(object sender, System.EventArgs e) {
        Player player = Player.Instance;

        this.PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;

        this.PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        DeliveryCounter deliverCounter = DeliveryCounter.Instance;

        this.PlaySound(audioClipRefsSO.deliverySuccess, deliverCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        DeliveryCounter deliverCounter = DeliveryCounter.Instance;

        this.PlaySound(audioClipRefsSO.deliveryFail, deliverCounter.transform.position);
    }

    public void PlayFootstep(Vector3 position, float volume) {
        this.PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void PlayCountdownSound() {
        this.PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position) {
        this.PlaySound(audioClipRefsSO.warning, position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        this.PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void ChangeVolume() {
        volume += VOLUME_INCREASE_AMOUNT;

        if (volume > VOLUME_MAX_AMOUNT) {
            volume = VOLUME_MIN_AMOUNT;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);

        //  This will force Unity to save the Player Prefs, in case the game crashes
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }

}
