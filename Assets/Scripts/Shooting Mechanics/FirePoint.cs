using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirePoint : MonoBehaviour
{
    public GameObject bulletObj;

    public bool hasFired;

    // Tell logic has reloaded
    private UnityEvent reloaded;

    private void Start()
    {
        reloaded = new UnityEvent();
        reloaded.AddListener(GetComponentInParent<AI_Logic>().HasReloaded);
    }
    
    public bool GetHasFired()
    {
        return hasFired;
    }

    public void Shoot()
    {
        // Instatiate a bullet prefab then gives it a velocity forward
        if (!hasFired)
        {
            GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>()
                .AddForce(transform.forward.normalized * bullet.GetComponent<BulletMechanics>().GetMoveSpeed(),
                    ForceMode.Impulse);
            hasFired = true;
        }
    }

    public void Reload()
    {
        reloaded.Invoke();
        hasFired = false;
        // print("reloaded2 " + hasFired);
    }
    
    //Coroutine for reload cooldown (potential if too fast)
}
