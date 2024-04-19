using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        //  The function is bool to avoid adding duplicates


        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) {
            //  The ingredient is not valid!

            //Debug.Log("This ingredient does not belong on the plate!");

            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            //  Already has this type, so no

            //Debug.Log("The plate already has this ingredient!");

            return false;
        }
        else {
            //  It does not have this type yet!

            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = kitchenObjectSO
            });

            //Debug.Log(kitchenObjectSO);

            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOList;
    }

}
