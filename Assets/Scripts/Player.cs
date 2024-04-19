using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; }

    public event EventHandler OnPickedUpSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private GameInput gameInput;
    //  TODO: learn bitmask
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private Vector3 lastInteractDir;
    private bool isWalking = false;
    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private float playerReach = 2f;
    private int deliverySuccessCount = 0;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one Player instance!");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        //  The game is no longer playing
        if (KitchenGameManager.Instance.GetState() != KitchenGameManager.State.GamePlaying) return;

        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        //  The game is no longer playing
        if (KitchenGameManager.Instance.GetState() != KitchenGameManager.State.GamePlaying) return;

        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update() {
        HandleMovement();

        HandleInteractions();
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        //  RaycastAll return the whole array of what it hits
        //  out is for output of the algorithm, here the out value is raycastHit, type RaycastHit
        bool interactable = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, playerReach, countersLayerMask);

        if (interactable) {
            // Debug.Log(raycastHit.transform);

            //  TryGet is to try getting it, if it is not null then it will return true
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                //  then clearCounter has the counter
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }

            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }

        //Debug.Log(selectedCounter);
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //  moveSpeed * Time.deltaTime is the moveDistance
        //  CasuleCast is casting a Capsule sized vector to a direction and mention if it hits something, this is for CollisionDetection
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveSpeed * Time.deltaTime);
        isWalking = moveDir != Vector3.zero;

        //  Slerp is for slow transition from one direction to another, should look more into this
        //  Time.deltaTime is to synchronize movementSpeed no matter the FPS difference
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        if (!canMove) {
            //  Cannot move towards the Dir xd

            //  Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveSpeed * Time.deltaTime);

            if (canMove) {
                // Move only in the X direction
                moveDir = moveDirX;
            }
            else {
                //  Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveSpeed * Time.deltaTime);

                if (canMove) {
                    // Move only in the Z direction
                    moveDir = moveDirZ;
                }
            }

        }

        if (canMove) {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            //Debug.Log(Time.deltaTime);
        }
    } 

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    public bool IsWalking() {
        return isWalking;
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnPickedUpSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

    public void DeliverySuccessfulCountAdd() {
        deliverySuccessCount++;
    }

    public int GetDeliverySuccessfulCount() {
        return deliverySuccessCount;
    }
}
