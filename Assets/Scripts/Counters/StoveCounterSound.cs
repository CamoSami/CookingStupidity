using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float burnShowProgressPercentage = .5f;
    private float warningSoundTimerMax = .2f;
    private float warningSoundTimer = 0;
    private bool showWarningIcon = false;

    private void Awake() {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        showWarningIcon = stoveCounter.GetState() == StoveCounter.State.Fried && e.progressNormalised >= burnShowProgressPercentage;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool playSound = (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried);

        if (playSound) {
            audioSource.Play();
        }
        else {
            audioSource.Pause();
        }
    }

    private void Update() {
        if (showWarningIcon) {
            warningSoundTimer += Time.deltaTime;

            if (warningSoundTimer >= warningSoundTimerMax) {
                warningSoundTimer = 0f;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
