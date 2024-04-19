using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {

    public static GamePauseUI Instance { get; private set; }

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;

    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            KitchenGameManager.Instance.TogglePauseGame();

            Loader.Load(Loader.Scene.MainMenuScene);
        });

        resumeButton.onClick.AddListener(() => {
            KitchenGameManager.Instance.TogglePauseGame();
        });

        optionsButton.onClick.AddListener(() => {
            OptionsUI.Instance.Show();

            this.Hide();
        });
    }

    private void Start() {
        Instance = this;

        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        this.Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        this.Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e) {
        this.Show();
    }

    public void Show() {
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

}

