using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyHookableObjects : MonoBehaviour
{
    [HideInInspector] public Vector3 attachPoint;

    [Space]
    [Header("Public References")]
    public Image attachPointCursos;
    public RectTransform canvasRect;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        attachPointCursos.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
