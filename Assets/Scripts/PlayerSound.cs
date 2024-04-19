using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    [SerializeField] private float footstepTimerMax = 0.2f;
    [SerializeField] private float volume = 1f;

    private Player player;
    private float footstepTimer;

    private void Awake() {
        player = this.GetComponent<Player>();

        footstepTimer = 0f;
    }
    
    private void Update() {
        footstepTimer += Time.deltaTime;

        if (footstepTimer >= footstepTimerMax) {
            footstepTimer = 0f;

            if (player.IsWalking()) {
                //Debug.Log("Walking Sound Played!");

                SoundManager.Instance.PlayFootstep(player.transform.position, volume);
            }
        }
    }

}
