using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{
    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform interactionZone;


    [SerializeField]
    private PlayerInput playerInput;

    private InputAction grabAction;



    private void Awake()
    {
        grabAction = playerInput.actions["Grab"];

    }


    private void OnEnable()
    {
        grabAction.performed += _ => StartGrab();


    }

    private void OnDisable()
    {
        grabAction.performed += _ => StartGrab();

    }




    void StartGrab()
    {
        if(ObjectToPickUp != null && ObjectToPickUp.GetComponent<PickableObject>().isPickable == true && PickedObject == null)
        {

                PickedObject = ObjectToPickUp;
                PickedObject.GetComponent<PickableObject>().isPickable = false;
                PickedObject.transform.SetParent(interactionZone);
                PickedObject.transform.position = interactionZone.position;
                PickedObject.GetComponent<Rigidbody>().useGravity = false;
                PickedObject.GetComponent<Rigidbody>().isKinematic = true;
          
        }

        else if (PickedObject != null)
        {

                PickedObject.GetComponent<PickableObject>().isPickable = true;
                PickedObject.transform.SetParent(null);
                PickedObject.GetComponent<Rigidbody>().useGravity = true;
                PickedObject.GetComponent<Rigidbody>().isKinematic = false;
                PickedObject = null;

            
        }
    }
}
