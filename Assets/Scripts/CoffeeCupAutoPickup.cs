using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCupAutoPickup : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Transform startParent;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startParent = transform.parent;
    }

    public void ResetPositionRotation()
    { 
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.parent = startParent;
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("COF");
        if (col.gameObject.tag == "GameController")
        {
            ControllerPickup controller = col.gameObject.GetComponent<ControllerPickup>();
            controller.PickupObject(transform);
        }
    }
}