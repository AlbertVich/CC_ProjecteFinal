using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{
    public GameObject ObjectToPickUp;

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
        pickUpObj();
    }

    public void pickUpObj()
    {
        if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<PickableObject>().isPickable == true && Global.PickedObject == null)
        {

            Global.PickedObject = ObjectToPickUp;
            Global.PickedObject.GetComponent<PickableObject>().isPickable = false;
            Global.PickedObject.transform.SetParent(interactionZone);
            Global.PickedObject.transform.position = interactionZone.position;
            Global.PickedObject.GetComponent<Rigidbody>().useGravity = false;
            Global.PickedObject.GetComponent<Rigidbody>().isKinematic = true;

        }

        else if (Global.PickedObject != null)
        {

            Global.PickedObject.GetComponent<PickableObject>().isPickable = true;
            Global.PickedObject.transform.SetParent(null);
            Global.PickedObject.GetComponent<Rigidbody>().useGravity = true;
            Global.PickedObject.GetComponent<Rigidbody>().isKinematic = false;
            Global.PickedObject = null;


        }
    }
}
