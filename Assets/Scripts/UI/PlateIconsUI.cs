using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour {

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        //  transform is the children of the current GameObject
        foreach (Transform child in transform) {
            //  Do not delete the original thing we want to duplicate
            if (child == iconTemplate) continue;

            // Destroy the other child (I am sorry my child)
            Destroy(child.gameObject); 
        }

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);

            iconTransform.gameObject.SetActive(true);

            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSOIcon(kitchenObjectSO);
        }
    }

}
