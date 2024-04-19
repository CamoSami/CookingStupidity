using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {

    [SerializeField] private Image timerImage;

    private void Update() {
        if (KitchenGameManager.Instance.GetState() == KitchenGameManager.State.GamePlaying) {
            //  Turn time left into percentage
            timerImage.fillAmount = 1 - KitchenGameManager.Instance.GetTimer() / KitchenGameManager.Instance.GetGamePlayingMax();
        }
    }

}