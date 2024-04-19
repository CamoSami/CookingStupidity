using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  TODO: Learn more about interfaces
public interface IKitchenObjectParent {
    public Transform GetKitchenObjectFollowTransform();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public bool HasKitchenObject();
}
