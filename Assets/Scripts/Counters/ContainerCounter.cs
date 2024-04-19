using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter {

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            //  Player is not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

            //Debug.Log(kitchenObject);
        }

    }

}