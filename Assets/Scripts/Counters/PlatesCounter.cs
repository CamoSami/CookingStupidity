using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter {
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private float spawnPlateTimerMax = 4f;
    [SerializeField] private int platesSpawnedAmountMax = 4;

    private float spawnPlateTimer;
    private int platesSpawnedAmount;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;

        if (KitchenGameManager.Instance.GetState() == KitchenGameManager.State.GamePlaying && spawnPlateTimer >= spawnPlateTimerMax) {
            //  Spawn da plates!

            spawnPlateTimer = 0f;
            if (platesSpawnedAmount < platesSpawnedAmountMax) {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            //  Player is empty handed!

            if (platesSpawnedAmount >= 0) {
                //  There're plates!
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
