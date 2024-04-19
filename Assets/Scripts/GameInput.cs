using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    //  For saving settings
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    //  For knowing which place the bindings are
    private const int BINDING_LOCATION_MOVE_UP = 1;
    private const int BINDING_LOCATION_MOVE_DOWN = 2;
    private const int BINDING_LOCATION_MOVE_LEFT = 3;
    private const int BINDING_LOCATION_MOVE_RIGHT = 4;
    private const int BINDING_LOCATION_GENERAL_PC = 0;

    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alt,
        Pause,
    }

    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        //  Debug.Log(this.GetBindingText(Binding.Interact));
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //  ? here makes the righter part of the code only run if the lefter object is not null
        //  It is the same as:
        //  if (OnInteractAction != null) OnInteractAction(this, EventArgs.Empty)
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        /*Debug.Log(inputVector);*/

        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[BINDING_LOCATION_MOVE_UP].ToDisplayString();

            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[BINDING_LOCATION_MOVE_DOWN].ToDisplayString();

            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[BINDING_LOCATION_MOVE_LEFT].ToDisplayString();

            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[BINDING_LOCATION_MOVE_RIGHT].ToDisplayString();

            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[BINDING_LOCATION_GENERAL_PC].ToDisplayString();

            case Binding.Interact_Alt:
                return playerInputActions.Player.InteractAlternate.bindings[BINDING_LOCATION_GENERAL_PC].ToDisplayString();

            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[BINDING_LOCATION_GENERAL_PC].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = BINDING_LOCATION_MOVE_UP;

                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = BINDING_LOCATION_MOVE_DOWN;

                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = BINDING_LOCATION_MOVE_LEFT;

                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = BINDING_LOCATION_MOVE_RIGHT;

                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = BINDING_LOCATION_GENERAL_PC;

                break;
            case Binding.Interact_Alt:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = BINDING_LOCATION_GENERAL_PC;

                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = BINDING_LOCATION_GENERAL_PC;

                break;
        }

        //  Lambda expression((callback) => {}) + multiple actions on one object, seperated by lines for clarity + Delegate(onActionRebound)
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete((callback) => {
                //  Debug.Log(callback.action.bindings[1].path);
                //  Debug.Log(callback.action.bindings[1].overridePath);

                callback.Dispose();

                playerInputActions.Player.Enable();

                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

}
