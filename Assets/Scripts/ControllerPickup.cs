using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPickup : MonoBehaviour
{
    public Transform SnapPoint;
    public SkinnedMeshRenderer handRenderer;

    public float SnapTimeTotal = 0.5f;


    private Transform pickupTransform;
    private Vector3 snapStartPosition;
    private Quaternion snapStartRotation;
    private float snapTime;

    public GameObject trialFinishedFlag;

    public void PickupObject(Transform otherTransform)
    {
        Debug.Log("Enter object");
        pickupTransform = otherTransform;

        pickupTransform.SetParent(SnapPoint);

        snapStartPosition = pickupTransform.localPosition;
        snapStartRotation = pickupTransform.localRotation;
        snapTime = 0;

        handRenderer.forceRenderingOff = true;

        StartCoroutine(SnapInPickup());
    }

    private IEnumerator SnapInPickup()
    {
        while (snapTime < SnapTimeTotal)
        {
            snapTime += Time.deltaTime;

            float lerpValue = Mathf.Clamp01(snapTime / SnapTimeTotal);

            pickupTransform.localPosition = Vector3.Lerp(snapStartPosition, Vector3.zero, lerpValue);
            pickupTransform.localRotation = Quaternion.Lerp(snapStartRotation, Quaternion.identity, lerpValue);

            yield return null;
        }

        pickupTransform.localPosition = Vector3.zero;
        pickupTransform.localRotation = Quaternion.identity;


        yield return null;
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("End Zone");
        if (col.gameObject.tag == "ReturnZone")
        {
            if (pickupTransform != null)
            {
                pickupTransform.GetComponent<CoffeeCupAutoPickup>().ResetPositionRotation();

                pickupTransform = null;

                handRenderer.forceRenderingOff = false;

                trialFinishedFlag.SetActive(true);
            }
        }
    }
}