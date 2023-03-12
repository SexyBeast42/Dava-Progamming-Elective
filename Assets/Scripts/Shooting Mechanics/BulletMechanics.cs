using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletMechanics : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 100;

    private Rigidbody rb;

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision col)
    {
        // Needs to be able to Damage players
        if (col.gameObject.CompareTag("Player"))
        {
            // print("col");
            col.gameObject.GetComponent<AI_Logic>().Die();
        }
    }
}
