using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform ingredientImage;

    private void Awake() {
        ingredientImage.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO) {
        recipeNameText.text = recipeSO.RecipeName;

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.KitchenObjectSOList) {
            Transform ingredientTransform = Instantiate(ingredientImage, iconContainer);

            ingredientTransform.gameObject.SetActive(true);

            ingredientTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }

}