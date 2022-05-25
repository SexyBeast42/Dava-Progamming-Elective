using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Logic : MonoBehaviour
{
    // For moving the AI 
    public Transform target;
    private AI_Movement SetTarget;
    
    // For shooting
    private FirePoint firePoint;
    
    void Awake()
    {
        SetTarget = GetComponent<AI_Movement> ();
        firePoint = GetComponentInChildren<FirePoint>();
    }

    void Update()
    {
        SetTarget.SetTarget(target);

        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
}
