using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI_Logic : MonoBehaviour
{
    // For moving the AI 
    public Transform target;
    private AI_Movement movePlayer;
    
    // For shooting
    private FirePoint firePoint;

    // For States
    private enum State
    {
        wander,
        attack,
        evade,
    }

    private State state;
    
    void Awake()
    {
        movePlayer = GetComponent<AI_Movement> ();
        firePoint = GetComponentInChildren<FirePoint>();
    }

    void Update()
    {
        movePlayer.SetTarget(target);
    }
    
    // if hasBullet in firePoint true => attack

    // Wander State
    private void Wander()
    {
        
    }
    
    // Attack State
    private void Attack()
    {
        firePoint.Shoot();
        state = State.evade;
    }
    
    // Evade State
    private void Evade()
    {
        
    }

    // Kill AI
    public void Die()
    {
        print("hit");
        Destroy(gameObject);
    }
}
