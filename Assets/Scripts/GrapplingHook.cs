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

    private bool isShootingGrap, isGrappling;
    private Vector3 hookPoint;

    private void Awake()
    {
        grappleAction = playerInput.actions["Grappling"];
    }

    private void OnEnable()
    {
        grappleAction.performed += _ => ShootHook();
    }


    private void OnDisable()
    {
        grappleAction.performed -= _ => ShootHook();
    }



    private void Start()
    {
        isShootingGrap = false;
        isGrappling = false;
        Debug.Log("Start!");

    }

    private void Update()
    {
        if (grapplingHook.parent == handPos)
        {
           grapplingHook.localPosition = Vector3.zero;
           // grapplingHook.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        }

        if (isGrappling)
        {
            grapplingHook.position = Vector3.Lerp(grapplingHook.position, hookPoint, hookSpeed * Time.deltaTime);
            if(Vector3.Distance (grapplingHook.position, hookPoint) < 0.5f)
            {

                controllerChar.enabled = false;
                playerBody.position = Vector3.Lerp(playerBody.position, hookPoint - offset, hookSpeed * Time.deltaTime);
                if (Vector3.Distance(playerBody.position, hookPoint - offset) < 0.5f)
                {

                    controllerChar.enabled = true;
                    isGrappling = false;
                    grapplingHook.SetParent(handPos);
                }
            }
        }
    }

    private void ShootHook()
    {
        if (Global.witchAvatarIsOn == 2 && Global.ISaim == true) {


            if (isShootingGrap || isGrappling) return;

            isShootingGrap = true;
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

            isShootingGrap = false;
        }
    }
}
