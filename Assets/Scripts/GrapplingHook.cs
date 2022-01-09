using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    private InputAction grappleAction;



    [SerializeField]
    private CharacterController controllerChar;
    [SerializeField]
    private Transform grapplingHook;
    [SerializeField]
    private Transform handPos;
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private LayerMask grappleLayer;
    [SerializeField]
    private float maxGrappleDistance;
    [SerializeField]
    private float hookSpeed;
    [SerializeField]
    private Vector3 offset;


    private bool isShooting, isGrappling;
    private Vector3 hookPoint;

    private void Awake()
    {

        grappleAction = playerInput.actions["Grappling"];
        Debug.Log("Awake!");


    }

    private void OnEnable()
    {

        grappleAction.performed += _ => ShootHook();
        Debug.Log("Enable!");


    }


    private void OnDisable()
    {
        grappleAction.performed -= _ => ShootHook();
        Debug.Log("Disable!");

    }





    private void Start()
    {
        isShooting = false;
        isGrappling = false;
        Debug.Log("Start!");

    }

    private void Update()
    {
        if (grapplingHook.parent == handPos)
        {
           // grapplingHook.localPosition = Vector3.zero;
           // grapplingHook.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        }
        if (isGrappling)
        {
            grapplingHook.position = Vector3.Lerp(grapplingHook.position, hookPoint, hookSpeed * Time.deltaTime);
            if(Vector3.Distance (grapplingHook.position, hookPoint) < 0.5f)
            {
                Debug.Log("MOVE!");

                controllerChar.enabled = false;
                playerBody.position = Vector3.Lerp(playerBody.position, hookPoint - offset, hookSpeed * Time.deltaTime);
                if (Vector3.Distance(playerBody.position, hookPoint - offset) < 0.5f)
                {
                    Debug.Log("STOP!");

                    controllerChar.enabled = true;
                    isGrappling = false;
                    grapplingHook.SetParent(handPos);
                }
            }
        }
    }

    private void ShootHook()
    {

        if (isShooting || isGrappling) return;

        isShooting = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxGrappleDistance, grappleLayer))
        {
            hookPoint = hit.point;
            isGrappling = true;
            grapplingHook.parent = null;
            grapplingHook.LookAt(hookPoint);
            Debug.Log("HIT!");
        }

        isShooting = false;
    }
}
