using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;



[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 100f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;
    [SerializeField]
    private GameObject Player1;
    [SerializeField]
    private GameObject Player2;
    [SerializeField]
    private bool saltfunciona = false;



    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction changeAction;
    private InputAction realoadActio;

    private int bulletscount;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        changeAction = playerInput.actions["Change"];
        realoadActio = playerInput.actions["Reload"];

        bulletscount = 0;
        Global.reloading = false;


        //Bloquejar corsor
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
        jumpAction.performed += _ => JumpUp();
        realoadActio.performed += _ => Reload();

    }


    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
        jumpAction.performed -= _ => JumpUp();
        realoadActio.performed -= _ => Reload();

    }


    private void ShootGun()
    {
        

        if (Global.ISaim == true && Global.witchAvatarIsOn == 1 && Global.reloading == false)
        {
            

            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
                      
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
                bulletController.hit = false;
            }
            bulletscount ++;
            Debug.Log(bulletscount);
            if (bulletscount == 7){
                Global.reloading = true;
                StartCoroutine(ReloadWait());
            }
        
        }

    }

    void Update()
    {
        Global.groundedPlayer = controller.isGrounded;
        if (Global.groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..

        playerVelocity.y += gravityValue * Time.deltaTime;       
        controller.Move(playerVelocity * Time.deltaTime);


        //Rotacio camera direcio
        //comprobar que no hi ha input de moviment
        if(input != Vector2.zero || Global.ISaim == true){
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        



        if (Global.totalJump == 2)
        {
            Global.totalJump = 0;

        }
        if (Global.totalJump <= 2 && Global.witchAvatarIsOn == 2)
        {
            Global.totalJump = 0;
        }



    }


    public void Reload(){
        Global.reloading = true;
        StartCoroutine(ReloadWait());
    }

    private void JumpUp()
    {
        if (Global.totalJump == 1)
        {
            playerVelocity.y = 0;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            Global.totalJump++;
            Debug.Log("Salts Despres1: " + Global.totalJump);

        }

        if (Global.groundedPlayer == true && Global.totalJump == 0)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            Global.totalJump++;
        }
    }


     IEnumerator ReloadWait()
    {
        Debug.Log("Reloading: " + Global.reloading);
        Debug.Log("PreReaload");
        yield return new WaitForSeconds(3);
        Debug.Log("PosReaload");
        bulletscount=0;
        Global.reloading = false;
    }


}
