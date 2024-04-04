using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    public GroupFormation groupFormation;

    private void Start()
    {
        groupFormation = FindAnyObjectByType<GroupFormation>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("End Zone");
        if (col.gameObject.tag == "Pspace")
        {
            groupFormation.EnteredPspace();
        }
    }
}
