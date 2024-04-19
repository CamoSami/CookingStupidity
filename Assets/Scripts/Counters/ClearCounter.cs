using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //  There's no kitchen object here

            if (player.HasKitchenObject()) {
                //  Player is holding something

                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                //  Player is holding something

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObjectPlayer)) {
                    //  Player is holding a plate and player put food from the counter on it

                    if (plateKitchenObjectPlayer.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSO())) {

                        this.GetKitchenObject().DestroySelf();
                    }
                }
                else if (this.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObjectCounter)) {
                    //  The counter has a plate and player put food they are holding to it

                    if (plateKitchenObjectCounter.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {

                        player.GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }

}
