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
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform grapplingHookEndPoint;
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

    private bool isShootingGrap;
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
        Global.ISgrappling = false;
        Debug.Log("Start!");
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (grapplingHook.parent == handPos)
        {
           grapplingHook.localPosition = Vector3.zero;
            grapplingHook.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            grapplingHook.localScale = new Vector3(1, 1, 1);
        }

        if (Global.ISgrappling)
        {
            grapplingHook.position = Vector3.Lerp(grapplingHook.position, hookPoint, hookSpeed * Time.deltaTime);
            if(Vector3.Distance (grapplingHook.position, hookPoint) < 0.5f)
            {

                controllerChar.enabled = false;
                playerBody.position = Vector3.Lerp(playerBody.position, hookPoint - offset, hookSpeed * Time.deltaTime);
                if (Vector3.Distance(playerBody.position, hookPoint - offset) < 0.5f)
                {

                    controllerChar.enabled = true;
                    Global.ISgrappling = false;
                    grapplingHook.SetParent(handPos);
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, grapplingHookEndPoint.position);
            lineRenderer.SetPosition(1, handPos.position);

        }
    }

    private void ShootHook()
    {
        if (Global.witchAvatarIsOn == 2 ) {


            if (isShootingGrap || Global.ISgrappling) return;

            isShootingGrap = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, maxGrappleDistance, grappleLayer))
            {
                hookPoint = hit.point;
                Global.ISgrappling = true;
                grapplingHook.parent = null;
                grapplingHook.LookAt(hookPoint);
                Debug.Log("HIT!");
                lineRenderer.enabled = true;
            }

            isShootingGrap = false;
        }
    }
}
