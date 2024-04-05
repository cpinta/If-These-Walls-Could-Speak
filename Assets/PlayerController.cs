using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : Entity
{
    //Components
    Rigidbody rb;
    CapsuleCollider col;
    [SerializeField] Camera cam;
    InputAction action;
    public TMP_Text centerText;

    Vector2 movementVector = Vector2.zero;
    //Camera Variables
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] float mouseSensitivity = 6;
    float cameraPitch = 0.0f;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    float baseCamHeight = 0.6f;

    //Player Variables
    float currentSpeed = 0;
    [SerializeField] float playerSpeed = 10;
    [SerializeField] float playerJauntMovementMultiplier = 1.2f;
    [SerializeField] float playerCrouchMovementMultiplier = 0.75f;
    [SerializeField] float playerCrouchLerp = 0.8f;
    [SerializeField] float interactDistance = 10;
    [SerializeField] float basePlayerHeight = 1;
    [SerializeField] float crouchedPlayerHeight = 0.7f;
    float crouchedStandDiff;

    bool isCrouching = false;
    bool isJaunting = false;

    RaycastHit hit;
    Interactable currentInteractable;

    List<Item> items;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<MeshRenderer>().enabled = false;
        crouchedStandDiff = basePlayerHeight - crouchedPlayerHeight;

        controlState = ControlState.FullControl;
        customHide = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHiding)
        {
            if (hidingStatus == HidingStatus.Entering)
            {
                if (Vector3.Distance(transform.position, hidingSpot.position - (Vector3.up * (transform.position.y + (cam.transform.position.y - transform.position.y)))) > GameManager.I.distanceToDestination)
                {
                    transform.position = Vector3.Lerp(transform.position, hidingSpot.position - (Vector3.up * (transform.position.y + (cam.transform.position.y - transform.position.y))), hideLerp);
                    transform.rotation = Quaternion.Lerp(transform.rotation, hidingSpot.rotation, hideLerp);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, hidingSpot.position) > GameManager.I.distanceToDestination)
                {
                    transform.position = Vector3.Lerp(transform.position, preHidePosition, hideLerp);
                    //cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, hidingSpot.rotation, hideLerp);
                }
                else
                {
                    UnHide();
                }
            }
        }

        if((controlState == ControlState.CantInteract || controlState == ControlState.FullControl) && !isHiding)
        {
            Vector2 targetMouseDelta = Mouse.current.delta.ReadValue() * Time.smoothDeltaTime;
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
            cameraPitch -= currentMouseDelta.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
            cam.transform.localEulerAngles = Vector3.right * cameraPitch;
            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

            Vector3 cameraForward = movementVector.y * transform.forward;
            Vector3 cameraRight = movementVector.x * transform.right;

            currentSpeed = playerSpeed;
            if (isCrouching)
            {
                currentSpeed *= playerCrouchMovementMultiplier;
                col.height = Mathf.Lerp(col.height, crouchedPlayerHeight * 2, playerCrouchLerp);
            }
            else
            {
                CrouchRaycast();
            }

            if (isJaunting)
            {
                currentSpeed *= playerJauntMovementMultiplier;
            }
            rb.velocity = ((cameraForward + cameraRight).normalized * currentSpeed) + (Vector3.up * rb.velocity.y);
        }

        col.center = Vector3.zero - (Vector3.up * ((2 - col.height) / 2));
        cam.transform.localPosition = Vector3.up * baseCamHeight * (col.height / (basePlayerHeight * 2));

        if(controlState == ControlState.CantMove || controlState == ControlState.FullControl)
        {
            InteractRaycast();
        }
    }

    void InteractRaycast()
    {
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance);
        if (hit.collider != null)
        {
            if(hit.collider.tag == "Interactable")
            {
                Debug.DrawRay(cam.transform.position, cam.transform.position + (cam.transform.forward * interactDistance), Color.green);
                if(hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    currentInteractable = interactable;
                    if (interactable.interactText != null && interactable.interactText != "")
                    {
                        centerText.text = interactable.interactText + ": ";
                    }
                    else
                    {
                        centerText.text = "";
                    }
                }
                else
                {
                    currentInteractable = null;
                }
            }
            else
            {
                currentInteractable = null;
            }
        }
        else
        {
            Debug.DrawRay(cam.transform.position, cam.transform.position + (cam.transform.forward * interactDistance), Color.red);
            currentInteractable = null;
        }

        if (currentInteractable != null)
        {
            centerText.gameObject.SetActive(true);
        }
        else
        {
            centerText.gameObject.SetActive(false);
        }
    }

    void CrouchRaycast()
    {
        col.height = Mathf.Lerp(col.height, basePlayerHeight * 2, playerCrouchLerp);
        Physics.Raycast(cam.transform.position, transform.up, out hit, crouchedStandDiff);
        if (hit.collider != null)
        {
            Debug.DrawRay(cam.transform.position, cam.transform.position + (transform.up * crouchedStandDiff), Color.red);
            col.height = Mathf.Lerp(col.height, (crouchedPlayerHeight * 2) + hit.distance, playerCrouchLerp);
        }
        else
        {
            Debug.DrawRay(cam.transform.position, cam.transform.position + (transform.up * crouchedStandDiff), Color.green);
            col.height = Mathf.Lerp(col.height, basePlayerHeight * 2, playerCrouchLerp);
        }
    }

    public override void Hide(Transform spot)
    {
        base.Hide(spot);
    }

    void UnHide()
    {
        isHiding = false;
        controlState = ControlState.FullControl;
    }

    #region Input Handling Methods
    public void Movement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            Debug.Log("Interact");
            if(currentInteractable != null)
            {
                currentInteractable.Interact(this);
            }
        }
    }

    public void UseItem(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            Debug.Log("Use Item");
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = true;
            if(isJaunting)
            {
                isJaunting = false;
            }
        }
        if (context.canceled)
        {
            isCrouching = false;
        }
    }

    public void Jaunt(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJaunting = true;
            if (isCrouching)
            {
                isCrouching = false;
            }
        }
        if (context.canceled)
        {
            isJaunting = false;
        }
    }
    #endregion
}
