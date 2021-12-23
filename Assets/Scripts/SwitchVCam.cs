using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;
    [SerializeField]
    private float rotationSpeed = 10f;


    private CinemachineVirtualCamera virutalCamera;
    private InputAction aimAction;
    private Transform cameraTransform;

    private void Awake()
    {
        virutalCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
        cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        if(gameObject.name == "Player1"){
            aimAction.performed += _ => StartAim();
            aimAction.canceled += _ => CancelAim();
        }
    }

    private void OnDisable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void StartAim()
    {
        virutalCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
        Debug.Log("Aim");
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private void CancelAim()
    {
        virutalCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
