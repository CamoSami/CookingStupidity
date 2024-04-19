using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress {

    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData() {
        OnAnyCut = null;
    }

    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    private int cuttingProgressMax;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //  Theres no kitchen object here

            if (player.HasKitchenObject()) {
                //  Player is holding something

                if (this.HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //  The item player is holding is in a recipe

                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    cuttingProgress = 0;
                    cuttingProgressMax = this.GetCuttingRecipeSOWithInput(this.GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalised = (float)cuttingProgress / cuttingProgressMax
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
            }
            else {
                //  else: Player is holding something

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObjectPlayer)) {
                    //  Player is holding a plate and player put food from the counter on it

                    if (plateKitchenObjectPlayer.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSO())) {

                        this.GetKitchenObject().DestroySelf();
                    }
                }

                //  NOTE: you can not add a plate on the cuttingCounter so the other else is removed
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            //  Theres a kitchen object here AND it is in the recipe

            cuttingProgress++;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalised = (float)cuttingProgress / cuttingProgressMax
            });
            
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            if (cuttingProgress >= cuttingProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = this.GetOutputForInput(this.GetKitchenObject().GetKitchenObjectSO());

                this.GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }
        else {
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}
