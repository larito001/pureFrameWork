using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieColliderCtrl : MonoBehaviour
{
    private BoxCollider collider;
    private void Start()
    {
        collider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
