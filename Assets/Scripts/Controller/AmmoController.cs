using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AmmoController : MonoBehaviour
{
    // List for guns and shots fired
    public List<GameObject> guns;
    public List<FirePoint> reloadAll;
    // public List<bool> shotFired;
    
    // Reload guns
    private bool isReloading;

    private void Start()
    {
        StartCoroutine(WaitForSpawn());
    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(2);
        
        GetAllGuns();
    }

    private void FixedUpdate()
    {
        if (!isReloading && AllShotsFired())
        {
            isReloading = true;
            StartCoroutine(Reload());
        }
        APlayerHasDied();
    }
    
    private void GetAllGuns()
    {
        // Finds all guns in the game
        GameObject[] gunsInGame = GameObject.FindGameObjectsWithTag("Gun");

        foreach (GameObject gun in gunsInGame)
        {
            guns.Add(gun);
        }

        foreach (GameObject gun in guns)
        {
            FirePoint firePoint = gun.GetComponent<FirePoint>();

            if (firePoint != null)
            {
                reloadAll.Add(firePoint);

                // shotFired.Add(firePoint.GetHasFired());
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

        Debug.Log("reloadInvoked");
    }

    private bool AllShotsFired()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (guns[i] != null && !guns[i].GetComponent<FirePoint>().GetHasFired())
            {
                return false;
            }
        }

        return true;
    }

    private void APlayerHasDied()
    {
        // use the list of guns, delete guns if players have died
        for (int i = 0; i < guns.Count; i++)
        {
            if (guns[i] != null && !guns[i].transform.parent.gameObject.activeSelf && guns.Count > 1)
            {
                guns.Remove(guns[i]);
            }
        }
    }
}
