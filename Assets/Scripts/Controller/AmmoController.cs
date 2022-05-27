using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoController : MonoBehaviour
{
    // List for guns and shots fired
    public GameObject[] guns;
    public List<FirePoint> reloadAll;
    public List<bool> shotFired;
    
    // Reload guns
    private bool isReloading;

    private void Start()
    {
        GetAllGuns();
    }

    private void FixedUpdate()
    {
        if (!isReloading && AllShotsFired())
        {
            isReloading = true;
            StartCoroutine(Reload());
        }
    }
    
    private void GetAllGuns()
    {
        // Finds all guns in the game
        guns = GameObject.FindGameObjectsWithTag("Gun");
        
        foreach (GameObject gun in guns)
        {
            FirePoint firePoint = gun.GetComponent<FirePoint>();

            if (firePoint != null)
            {
                reloadAll.Add(firePoint);

                shotFired.Add(firePoint.GetHasFired());
            }
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1);
        foreach (FirePoint firePoint in reloadAll)
        {
            firePoint.Reload();
        }
        
        isReloading = false;

        // Debug.Log("reloadInvoked");
    }

    private bool AllShotsFired()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null && !guns[i].GetComponent<FirePoint>().GetHasFired())
            {
                return false;
            }
        }

        return true;
    }
}
