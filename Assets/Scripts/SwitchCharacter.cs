using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCharacter : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    private InputAction changeAction;

    public GameObject character1, character2;

    private bool groundedPlayer;




    private void Awake()
    {

        changeAction = playerInput.actions["Change"];

    }

    private void OnEnable()
    {

            changeAction.performed += _ => ChangeCharacter();
        
    }


    private void OnDisable()
    {
        changeAction.performed -= _ => ChangeCharacter();
    }


    // Start is called before the first frame update
    void Start()
    {
        character1.gameObject.SetActive (true);
        character2.gameObject.SetActive (false);

    }

    // Update is called once per frame

    public void ChangeCharacter()
    {
        if (Global.groundedPlayer == true)
        {
            switch (Global.witchAvatarIsOn)
            {
                case 1:
                    Global.witchAvatarIsOn = 2;

                    character1.gameObject.SetActive(false);
                    character2.gameObject.SetActive(true);
                    break;

                case 2:
                    Global.witchAvatarIsOn = 1;

                    character1.gameObject.SetActive(true);
                    character2.gameObject.SetActive(false);
                    break;
            }
        }
    }
    
}
