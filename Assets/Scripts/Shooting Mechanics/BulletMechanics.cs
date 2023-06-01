using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletMechanics : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 100;

    private int maxBounces = 4;
    private int currentBounces = 0;

    private Rigidbody rb;

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Destroy bullets after 4 bounces;
        if (currentBounces == maxBounces)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        // Needs to be able to Damage players
        if (col.gameObject.CompareTag("Player"))
        {
            // print("col");
            col.gameObject.GetComponent<AI_Logic>().Die();

            currentBounces++;
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            currentBounces++;
        }
    }
}
