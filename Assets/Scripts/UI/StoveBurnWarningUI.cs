using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;

    float burnShowProgressPercentage = .5f;

    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        this.Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        bool showWarningIcon = stoveCounter.GetState() == StoveCounter.State.Fried && e.progressNormalised >= burnShowProgressPercentage;

        if (showWarningIcon) {
            this.Show();
        }
        else {
            this.Hide();
        }
    }

    private void Show() {
        this.gameObject.SetActive(true);
    }

    private void Hide() {
        this.gameObject.SetActive(false);
    }
}
