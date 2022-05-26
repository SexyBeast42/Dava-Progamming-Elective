using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public GameObject bulletObj;

    public bool hasBullet = true;

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Shoot();
    //     }
    // }
    
    public void Shoot()
    {
        // print("Shoot");
        
        // Instatiate a bullet prefab then gives it a velocity forward
        if (hasBullet)
        {
            GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>()
                .AddForce(transform.forward.normalized * bullet.GetComponent<BulletMechanics>().GetMoveSpeed(),
                    ForceMode.Impulse);
            hasBullet = false;
        }
    }

    public void Reload()
    {
        hasBullet = true;
    }
    
    //Coroutine for reload cooldown (potential if too fast)
}
