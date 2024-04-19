using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float spawnRecipeTimerMax = 4f;
    [SerializeField] private int waitingRecipeMax = 4;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;

    private void Awake() {
        Instance = this;    

        waitingRecipeSOList = new List<RecipeSO>();

        //Debug.Log(recipeListSO.recipeSOList.Count);
    }

    private void Update() {
        spawnRecipeTimer += Time.deltaTime;

        if (spawnRecipeTimer >= spawnRecipeTimerMax) {
            //  Times up, the customers are hungry

            spawnRecipeTimer = 0f;

            if (KitchenGameManager.Instance.GetState() == KitchenGameManager.State.GamePlaying && waitingRecipeSOList.Count < waitingRecipeMax) {
                //  Check if the store is full or not

                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                
                Debug.Log(waitingRecipeSO.RecipeName);

                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count != plateKitchenObject.GetKitchenObjectSOList().Count) {
                //  Does not have the same number of ingredient

                continue;
            }

            //  Check ingredient
            if (this.CheckIngredientFromPlateToRecipe(plateKitchenObject, waitingRecipeSO)) {
                //Debug.Log("The recipe is correct!");

                waitingRecipeSOList.RemoveAt(i);

                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                //  Add 1 to the successful delivery count
                Player.Instance.DeliverySuccessfulCountAdd();

                return;
            }
        }
        //  No matches found => the player got it wrong!

        //Debug.Log("Bruh");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public bool CheckIngredientFromPlateToRecipe(PlateKitchenObject plateKitchenObject, RecipeSO recipeSO) {
        bool recipeIsCorrect = true;

        foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            bool ingridientIsCorrect = false;

            foreach (KitchenObjectSO recipeKitchenObjectSO in recipeSO.KitchenObjectSOList) {
                if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                    //  Correct ingredient!
                    ingridientIsCorrect = true;

                    break;
                }
            }

            if (!ingridientIsCorrect) {
                recipeIsCorrect = false;

                break;
            }
        }

        return recipeIsCorrect;
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }

}
