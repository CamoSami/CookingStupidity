using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour {

    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;

    float burnShowProgressPercentage = .5f;

    private Animator animator;

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        bool showFlashingBar = stoveCounter.GetState() == StoveCounter.State.Fried && e.progressNormalised >= burnShowProgressPercentage;

        animator.SetBool(IS_FLASHING, showFlashingBar);
    }

}
