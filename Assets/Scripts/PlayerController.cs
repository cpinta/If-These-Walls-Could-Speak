using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;


public class PlayerController : Entity
{
    //Components
    Rigidbody rb;
    CapsuleCollider col;
    [SerializeField] Camera cam;
    InputAction action;
    public TMP_Text centerText;
    [SerializeField] Light flashlight;

    Vector2 movementVector = Vector2.zero;
    //Camera Variables
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] float mouseSensitivity = 6;
    float cameraPitch = 0.0f;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    [SerializeField] int hidingFreeCameraAngle = 15;
    bool camWasMovedLastUpdate = false;
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
    bool interactableIsCollectable = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<MeshRenderer>().enabled = false;
        crouchedStandDiff = basePlayerHeight - crouchedPlayerHeight;

        customHide = true;

        //if (GameManager.I.debug)
        //{
        //    hand.gameObject.GetComponent<MeshRenderer>().enabled = true;
        //}
        //else
        //{
        //    hand.gameObject.GetComponent<MeshRenderer>().enabled = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (hidingSpot != null)
        {
            if (moveCamera)
            {
                if (hidingState == HidingState.Entering)
                {
                    Vector3 vec = hidingSpot.location.position - (Vector3.up * (transform.position.y + (cam.transform.position.y - transform.position.y)));
                    float distance = Vector3.Distance(transform.position, hidingSpot.location.position - (Vector3.up * (transform.position.y + (cam.transform.position.y - transform.position.y))));
                    if (distance > GM.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, hidingSpot.location.position - (Vector3.up * (transform.position.y + (cam.transform.position.y - transform.position.y))), hideLerp * Time.deltaTime);

                        if (hidingSpot.syncRotation)
                        {
                            if (Quaternion.Angle(transform.rotation, hidingSpot.location.rotation)! > hidingFreeCameraAngle && camWasMovedLastUpdate)
                            {
                                if (!hidingSpot.lockCamera)
                                {
                                    canMoveCamera = true;
                                }
                            }
                            if (!canMoveCamera)
                            {
                                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, hidingSpot.location.rotation.eulerAngles, hideLerp * Time.deltaTime);
                                cam.transform.localEulerAngles = Vector3.zero;
                            }
                        }
                    }
                    else
                    {
                        transform.position = hidingSpot.location.position - (Vector3.up * (transform.position.y + (cam.transform.position.y - transform.position.y)));
                        hidingState = HidingState.In;
                        if (!hidingSpot.lockCamera)
                        {
                            canMoveCamera = true;
                        }
                        else
                        {
                            transform.eulerAngles = hidingSpot.location.rotation.eulerAngles;
                        }
                    }
                }
                else if (hidingState == HidingState.Exiting)
                {
                    if (Cursor.lockState != CursorLockMode.Locked)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                    }

                    int i = 0;
                    if (Vector3.Distance(transform.position, preHidePosition) > GM.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, preHidePosition, hideLerp * Time.deltaTime);
                    }
                    else
                    {
                        i++;
                    }

                    transform.LookAt(transform.position - (preHidePosition - transform.position), Vector3.up);
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, Vector3.right * cameraPitch, hideLerp * Time.deltaTime);
                    i++;

                    if(i == 2)
                    {
                        UnHide();
                    }
                }
                else
                {
                    if (movementVector != Vector2.zero)
                    {
                        hidingState = HidingState.Exiting;
                    }
                }
            }
        }
        else
        {
            isHiding = false;
        }

        Vector2 targetMouseDelta = Mouse.current.delta.ReadValue() * Time.smoothDeltaTime;
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        camWasMovedLastUpdate = currentMouseDelta != Vector2.zero;
        if (canMoveCamera)
        {
            cameraPitch -= currentMouseDelta.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
            cam.transform.localEulerAngles = Vector3.right * cameraPitch;
            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

            Vector3 cameraForward = movementVector.y * transform.forward;
            Vector3 cameraRight = movementVector.x * transform.right;

            if (canMoveBody)
            {
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
            }
            else
            {
                currentSpeed = 0;
            }

            if (!rb.isKinematic)
            {
                rb.velocity = ((cameraForward + cameraRight).normalized * currentSpeed) + (Vector3.up * rb.velocity.y);
            }
        }

        col.center = Vector3.zero - (Vector3.up * ((2 - col.height) / 2));
        cam.transform.localPosition = Vector3.up * baseCamHeight * (col.height / (basePlayerHeight * 2));

        
        if (canInteract && Cursor.lockState == CursorLockMode.Locked)
        {
            InteractRaycast();
        }
        else if (canInteract)
        {
            CursorRaycast();
        }
    }

    void InteractRaycast()
    {
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Interactable")
            {
                Debug.DrawRay(cam.transform.position, cam.transform.position + (cam.transform.forward * interactDistance), Color.green);
                if (hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    currentInteractable = interactable;
                    interactable.IsHovering(true, this);
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
            currentInteractable.IsHovering(true, this);
        }
        else
        {
            centerText.gameObject.SetActive(false);
        }
    }

    void CursorRaycast()
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, interactDistance); 
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Clickable")
                {
                    Debug.DrawRay(ray.origin, ray.direction, Color.green);
                    if (hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                    {
                        currentInteractable = interactable;
                        interactable.IsHovering(true, this);
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
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
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
            currentInteractable.IsHovering(true, this);
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

    public override void Hide(HidingSpot spot, bool isHiding)
    {
        base.Hide(spot, isHiding);
        rb.isKinematic = true;
        col.enabled = false;
        canMoveCamera = !spot.lockCamera;
        centerText.text = "";
        cam.transform.eulerAngles = Vector3.zero;
    }

    public override void UnHide()
    {
        base.UnHide();
        rb.isKinematic = false;
        canMoveCamera = true;
        canMoveBody = true;
        col.enabled = true;
        hidingSpot = null;
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
            if (currentInteractable != null)
            {
                if(currentInteractable is Collectable)
                {
                    Collect((Collectable)currentInteractable);
                    currentInteractable = null;
                }
                else
                {
                    currentInteractable.Interact(this);
                    currentInteractable = null;
                }
            }
        }
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Use Item");

            if (Cursor.lockState == CursorLockMode.None)
            {
                if(currentInteractable != null)
                {
                    currentInteractable.Interact(this);
                }
            }
            else
            {
                if(currentCollectable != null)
                {
                    currentCollectable.Use(this, true);
                }
            }
        }
        else if(context.canceled) 
        {
            if (currentCollectable != null)
            {
                currentCollectable.Use(this, false);
            }
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = true;
            if (isJaunting)
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

    public void ToggleFlashlight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FlashlightToggle();
        }
    }
    #endregion

    #region Flashlight
    public void FlashlightToggle()
    {
        if(flashlight.enabled)
        {
            flashlight.enabled = false;
        }
        else
        {
            flashlight.enabled = true;
        }
    }

    public void FlashlightOn()
    {
        flashlight.enabled = true;
    }

    public void FlashlightOff()
    {
        flashlight.enabled = false;
    }
    #endregion
}