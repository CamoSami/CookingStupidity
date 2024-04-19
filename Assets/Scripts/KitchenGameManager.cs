using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour {

    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public enum State {
        Tutorial,
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    [SerializeField] private float waitingToStartMax = 1f;
    [SerializeField] private float countdownToStartMax = 3f;
    [SerializeField] private float gamePlayingMax = 10f;

    private State state;
    private float timer = 0f;
    private bool isGamePaused = false;

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (state == State.Tutorial) {
            state = State.WaitingToStart;

            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        this.TogglePauseGame();
    }

    private void Awake() {
        state = State.Tutorial;

        Instance = this;
    }



    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                timer += Time.deltaTime;

                if (timer >= waitingToStartMax) {
                    state = State.CountdownToStart;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                    timer = 0f;
                }

                break;
            case State.CountdownToStart:
                timer += Time.deltaTime;

                if (timer >= countdownToStartMax) {
                    state = State.GamePlaying;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                    timer = 0f;
                }

                break;
            case State.GamePlaying:
                timer += Time.deltaTime;
        
                if (timer >= gamePlayingMax) {
                    state = State.GameOver;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                    timer = 0f;
                }

                break;
            case State.GameOver:

                break;
        }

        //  Debug.Log(state + " " + timer);
    }

    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;

        if (isGamePaused) {
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else {
            Time.timeScale = 1f;

            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

        public State GetState() {
        return state;
    }

    public float GetTimer() {
        return timer;
    }

    public float GetCountdownToStartMax() {
        return countdownToStartMax;
    }

    public float GetGamePlayingMax() {
        return gamePlayingMax;
    }

}