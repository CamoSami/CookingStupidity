using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State { 
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:

                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalised = fryingTimer / fryingRecipeSO.fryingTimerMax > 1f ? 1f : fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax) {
                        //  Done!

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;
                        burningTimer = 0f;

                        //  Basically this get the cooked -> burned recipe, should have followed the tutorial but nope
                        fryingRecipeSO = this.GetFryingRecipeSOWithInput(this.GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                    }

                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalised = burningTimer / fryingRecipeSO.fryingTimerMax > 1f ? 1f : burningTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (burningTimer >= fryingRecipeSO.fryingTimerMax) {
                        //  Oh fvck

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalised = 0f
                        });
                    }

                    break;
                case State.Burned:

                    break;
            }

        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //  Theres no kitchen object here

            if (player.HasKitchenObject()) {
                //  Player is holding something

                if (this.HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //  The item player is holding is in a recipe

                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingTimer = 0;
                    fryingRecipeSO = this.GetFryingRecipeSOWithInput(this.GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalised = fryingTimer / fryingRecipeSO.fryingTimerMax > 1f ? 1f : fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
            //  else: Player is not holding anything
        }
        else {
            //  Theres a kitchen object here

            if (!player.HasKitchenObject()) {
                //  Player pick the kitchen object up

                this.GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalised = 0f
                });
            }
            else {
                //  else: Player is holding something

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObjectPlayer)) {
                    //  Player is holding a plate and player put food from the counter on it

                    if (plateKitchenObjectPlayer.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSO())) {

                        this.GetKitchenObject().DestroySelf();
                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        }) ;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalised = 0f
                        });
                    }
                }

                //  NOTE: you can not add a plate on the stoveCounter so the other else is removed
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        else {
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        return fryingRecipeSO != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    public State GetState() {
        return state;
    }

}
