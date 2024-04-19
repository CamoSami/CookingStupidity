using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    //  Check how to handle loading scene later

    private void Awake() {
        playButton.onClick.AddListener(() => {
            //  Lambda expression - also a delegate

            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => {
            //  Lambda expression - also a delegate

            //  Quit the game, but when it is running in the Editor, it will not do anything (bruh)
            Application.Quit();
        });

        Time.timeScale = 1f;
    }

    

}
