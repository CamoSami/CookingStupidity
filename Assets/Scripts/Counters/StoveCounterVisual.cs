using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {

    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        switch (e.state) {
            case StoveCounter.State.Frying:
            case StoveCounter.State.Fried:
                Show();

                break;
            default:
                Hide();

                break;
        }
    }

    private void Show() {
        this.SetActive(true);
    }

    private void Hide() {
        this.SetActive(false);
    }

    private void SetActive(bool value) {
        stoveOnGameObject.SetActive(value);
        particlesGameObject.SetActive(value);
    }
}
