using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoController : MonoBehaviour
{
    // List for guns and shots fired
    public GameObject[] guns;
    public List<bool> shotFired;
    
    // Reload guns
    private UnityEvent reloadAll;
    private bool reloading;

    public void Start()
    {
        GetAllGuns();
    }

    public void FixedUpdate()
    {
        if (!reloading && AllShotsFired())
        {
            reloading = true;
            Reload();
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
                reloadAll = new UnityEvent();
                reloadAll.AddListener(firePoint.Reload);
                
                shotFired.Add(firePoint.GetHasFired());
            }
        }
    }

    private void Reload()
    {
        reloadAll.Invoke();
        reloading = false;

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
